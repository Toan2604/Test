using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Handlers;
using DMS.Helpers;
using DMS.Repositories;
using DMS.Rpc.product;
using DMS.Services.MImage;
using DMS.Services.MNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;
using DMS.Handlers.Configuration;
using DMS.Models;
using Microsoft.EntityFrameworkCore;

namespace DMS.Services.MProduct
{
    public interface IProductService : IServiceScoped
    {
        Task<int> Count(ProductFilter ProductFilter);
        Task<int> MCount(ProductFilter ProductFilter);
        Task<List<Product>> List(ProductFilter ProductFilter);
        Task<List<Product>> MobileList(ProductFilter ProductFilter);
        Task<Product> Get(long Id);
        Task<Product> MobileGet(long Id);
        ProductFilter ToFilter(ProductFilter ProductFilter);
        Task<List<Product>> BulkInsertNewProduct(List<Product> Products);
        Task<List<Product>> BulkDeleteNewProduct(List<Product> Products);
        Task<bool> SalesOrderCalculator(List<long> ProductIds);
    }

    public class ProductService : BaseService, IProductService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private INotificationService NotificationService;
        private IProductValidator ProductValidator;
        private IProductTemplate ProductTemplate;
        private IImageService ImageService;
        private IRabbitManager RabbitManager;
        DataContext DataContext;

        public ProductService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            INotificationService NotificationService,
            IProductValidator ProductValidator,
            IProductTemplate ProductTemplate,
            IImageService ImageService,
            IRabbitManager RabbitManager,
            DataContext DataContext
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.NotificationService = NotificationService;
            this.ProductValidator = ProductValidator;
            this.ProductTemplate = ProductTemplate;
            this.ImageService = ImageService;
            this.RabbitManager = RabbitManager;
            this.DataContext = DataContext;
        }
        public async Task<int> Count(ProductFilter ProductFilter)
        {
            try
            {
                int result = await UOW.ProductRepository.Count(ProductFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ProductService));
            }
            return 0;
        }

        public async Task<int> MCount(ProductFilter ProductFilter)
        {
            try
            {
                int result;
                var SystemConfig = await UOW.SystemConfigurationRepository.Get();
                if (SystemConfig.USE_ELASTICSEARCH)
                    result = await UOW.EsProductRepository.Count(ProductFilter);
                else
                    result = await UOW.ProductRepository.Count(ProductFilter);

                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ProductService));
            }
            return 0;
        }

        public async Task<List<Product>> List(ProductFilter ProductFilter)
        {
            try
            {
                List<Product> Products = await UOW.ProductRepository.List(ProductFilter);
                List<long> ProductIds = Products.Select(p => p.Id).ToList();
                ItemFilter ItemFilter = new ItemFilter
                {
                    ProductId = new IdFilter { In = ProductIds },
                    StatusId = null,
                    Selects = ItemSelect.Id | ItemSelect.ProductId,
                    Skip = 0,
                    Take = int.MaxValue,
                };
                List<Item> Items = await UOW.ItemRepository.List(ItemFilter);
                foreach (Product Product in Products)
                {
                    Product.VariationCounter = Items.Where(i => i.ProductId == Product.Id).Count();
                }
                return Products;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ProductService));
            }
            return null;
        }

        public async Task<List<Product>> MobileList(ProductFilter ProductFilter)
        {
            try
            {
                List<Product> Products;
                var SystemConfig = await UOW.SystemConfigurationRepository.Get();
                if (SystemConfig.USE_ELASTICSEARCH)
                    Products = await UOW.EsProductRepository.List(ProductFilter);
                else
                    Products = await UOW.ProductRepository.List(ProductFilter);

                List<long> ProductIds = Products.Select(p => p.Id).ToList();
                ItemFilter ItemFilter = new ItemFilter
                {
                    ProductId = new IdFilter { In = ProductIds },
                    StatusId = null,
                    Selects = ItemSelect.Id | ItemSelect.ProductId,
                    Skip = 0,
                    Take = int.MaxValue,
                };

                List<Item> Items = new List<Item>();
                if (SystemConfig.USE_ELASTICSEARCH)
                    await UOW.EsItemRepository.List(ItemFilter);
                else
                    await UOW.ItemRepository.List(ItemFilter);

                foreach (Product Product in Products)
                {
                    Product.VariationCounter = Items.Where(i => i.ProductId == Product.Id).Count();
                }
                return Products;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ProductService));
            }
            return null;
        }

        public async Task<Product> Get(long Id)
        {
            Product Product = await UOW.ProductRepository.Get(Id);
            if (Product == null)
                return null;
            if (Product.Items != null && Product.Items.Any())
            {
                Product.VariationCounter = Product.Items.Count;
            }
            return Product;
        }

        public async Task<Product> MobileGet(long Id)
        {
            Product Product;
            Product = await UOW.ProductRepository.Get(Id);

            if (Product == null)
                return null;
            if (Product.Items != null && Product.Items.Any())
            {
                Product.VariationCounter = Product.Items.Count;
            }
            return Product;
        }


        public async Task<List<Product>> BulkInsertNewProduct(List<Product> Products)
        {
            if (!await ProductValidator.BulkMergeNewProduct(Products))
                return Products;

            try
            {
                var OldProducts = Products.Where(x => x.IsNew == false).ToList();
                await UOW.ProductRepository.Clean(); //xóa hết product cũ trong list rồi mới thêm list mới
                var CurrentUser = await UOW.AppUserRepository.GetSimple(CurrentContext.UserId);
                Products.ForEach(x => x.IsNew = true);
                await UOW.ProductRepository.BulkInsertNewProduct(Products);
                List<long> ProductIds = Products.Select(x => x.Id).ToList();
                Products = await UOW.ProductRepository.List(ProductIds);
                Sync(Products);
                List<GlobalUserNotification> GlobalUserNotifications = new List<GlobalUserNotification>();
                var RecipientRowIds = (await UOW.AppUserRepository.List(new AppUserFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = AppUserSelect.RowId,
                    OrganizationId = new IdFilter { }
                })).Select(x => x.RowId).ToList();
                var StoreUserRowIds = (await UOW.StoreUserRepository.List(new StoreUserFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = StoreUserSelect.RowId,
                })).Select(x => x.RowId).ToList();
                foreach (var Product in OldProducts)
                {
                    foreach (var RowId in RecipientRowIds)
                    {
                        GlobalUserNotification GlobalUserNotification = ProductTemplate.CreateAppUserNotification(CurrentUser.RowId, RowId, Product, CurrentUser, NotificationType.CREATE);
                        GlobalUserNotifications.Add(GlobalUserNotification);
                    }
                    foreach (var RowId in StoreUserRowIds)
                    {
                        GlobalUserNotification GlobalUserNotification = ProductTemplate.CreateStoreUserNotification(CurrentUser.RowId, RowId, Product, CurrentUser, NotificationType.CREATE);
                        GlobalUserNotifications.Add(GlobalUserNotification);
                    }
                }
                RabbitManager.PublishList(GlobalUserNotifications, RoutingKeyEnum.GlobalUserNotificationSend.Code);
                Logging.CreateAuditLog(new { }, Products, nameof(ProductService));
                return Products;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ProductService));
            }
            return null;
        }

        public async Task<List<Product>> BulkDeleteNewProduct(List<Product> Products)
        {
            if (!await ProductValidator.BulkMergeNewProduct(Products))
                return Products;

            try
            {
                Products.ForEach(x => x.IsNew = false);
                await UOW.ProductRepository.BulkDeleteNewProduct(Products);
                List<long> ProductIds = Products.Select(x => x.Id).ToList();
                Products = await UOW.ProductRepository.List(ProductIds);
                Sync(Products);
                Logging.CreateAuditLog(new { }, Products, nameof(ProductService));
                return Products;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ProductService));
            }
            return null;
        }

        public ProductFilter ToFilter(ProductFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<ProductFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                ProductFilter subFilter = new ProductFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Id))
                        subFilter.Id = FilterPermissionDefinition.IdFilter;
                    if (FilterPermissionDefinition.Name == nameof(subFilter.ProductTypeId))
                        subFilter.ProductTypeId = FilterPermissionDefinition.IdFilter;
                    if (FilterPermissionDefinition.Name == nameof(subFilter.ProductGroupingId))
                        subFilter.ProductGroupingId = FilterPermissionDefinition.IdFilter;
                }
            }
            return filter;
        }

        public async Task<bool> SalesOrderCalculator(List<long> ProductIds)
        {
            try
            {
                var Products = ProductIds.Select(x => new Product { Id = x }).ToList();
                IdFilter ProductIdFilter = new IdFilter { In = ProductIds };
                DateFilter OrderDateFilter = new DateFilter { GreaterEqual = DateTime.Today.AddDays(-30).AddHours(17)};

                // DirectSalesOrderTransaction
                var direct_sales_order_query = DataContext.DirectSalesOrder.AsNoTracking();
                direct_sales_order_query = direct_sales_order_query.Where(q => q.OrderDate, OrderDateFilter);
                direct_sales_order_query = direct_sales_order_query.Where(q => q.GeneralApprovalStateId, new IdFilter()
                {
                    In = new List<long> { GeneralApprovalStateEnum.APPROVED.Id, GeneralApprovalStateEnum.STORE_APPROVED.Id }
                });
                var DirectSalesOrderIds = await direct_sales_order_query.Select(x => x.Id).ToListWithNoLockAsync();

                var direct_sales_order_transaction_query = DataContext.DirectSalesOrderTransaction.AsNoTracking();
                direct_sales_order_transaction_query = direct_sales_order_transaction_query.Where(x => x.DirectSalesOrderId, new IdFilter { In = DirectSalesOrderIds });
                direct_sales_order_transaction_query = direct_sales_order_transaction_query.Where(x => x.Item.ProductId, ProductIdFilter);
                var SalesOrderTransaction1 = await direct_sales_order_transaction_query.GroupBy(x => x.Item.ProductId)
                    .Select(x => new {
                        Id = x.Key,
                        SalesOrderCounter = x.Sum(y => y.Quantity),
                    }).ToListWithNoLockAsync();

                // IndirectSalesOrderTransaction
                var indirect_sales_order_query = DataContext.IndirectSalesOrder.AsNoTracking();
                indirect_sales_order_query = indirect_sales_order_query.Where(q => q.OrderDate, OrderDateFilter);
                indirect_sales_order_query = indirect_sales_order_query.Where(q => q.RequestStateId, new IdFilter() { Equal = RequestStateEnum.APPROVED.Id });
                var IndirectSalesOrderIds = await indirect_sales_order_query.Select(x => x.Id).ToListWithNoLockAsync();

                var indirect_sales_order_transaction_query = DataContext.IndirectSalesOrderTransaction.AsNoTracking();
                indirect_sales_order_transaction_query = indirect_sales_order_transaction_query.Where(x => x.IndirectSalesOrderId, new IdFilter { In = IndirectSalesOrderIds });
                indirect_sales_order_transaction_query = indirect_sales_order_transaction_query.Where(x => x.Item.ProductId, ProductIdFilter);

                var SalesOrderTransaction2 = await indirect_sales_order_transaction_query.GroupBy(x => x.Item.ProductId)
                    .Select(x => new {
                        Id = x.Key,
                        SalesOrderCounter = x.Sum(y => y.Quantity),
                    }).ToListWithNoLockAsync();
                SalesOrderTransaction1.AddRange(SalesOrderTransaction2);
                foreach (var product in Products)
                {
                    product.SalesOrderCounter = SalesOrderTransaction1.Where(x => x.Id == product.Id).Sum(x => x.SalesOrderCounter);
                }

                await UOW.ProductRepository.BulkMergeSalesOrderCounter(Products);
                return true;

            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ProductService));
            }
            return false;
        }
        private void Sync(List<Product> Products)
        {
            RabbitManager.PublishList(Products, RoutingKeyEnum.ProductNew.Code);
        }
    }
}
