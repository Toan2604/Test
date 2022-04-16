using DMS.ABE.Common;
using TrueSight.Common;
using DMS.ABE.Entities;
using DMS.ABE.Enums;
using DMS.ABE.Handlers.Configuration;
using DMS.ABE.Helpers;
using DMS.ABE.Repositories;
using DMS.ABE.Services.MImage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.ABE.Services.MProduct
{
    public interface IProductService : IServiceScoped
    {
        Task<int> Count(ProductFilter ProductFilter);
        Task<int> ESCount(ProductFilter ProductFilter);
        Task<List<Product>> List(ProductFilter ProductFilter);
        Task<List<Product>> ESList(ProductFilter ProductFilter);
        Task<Product> Get(long Id);
        Task<Image> SaveImage(Image Image);
        ProductFilter ToFilter(ProductFilter ProductFilter);
    }

    public class ProductService : BaseService, IProductService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IImageService ImageService;
        private IRabbitManager RabbitManager;

        public ProductService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IImageService ImageService,
            IRabbitManager RabbitManager
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.ImageService = ImageService;
            this.RabbitManager = RabbitManager;
        }
        public async Task<int> Count(ProductFilter ProductFilter)
        {
            try
            {
                Store Store = await GetStore();
                List<StoreUserFavoriteProductMapping> StoreUserFavoriteProductMappings = await UOW.StoreUserFavoriteProductMappingRepository.List(
                    new StoreUserFavoriteProductMappingFilter
                    {
                        StoreUserId = new IdFilter { Equal = CurrentContext.StoreUserId },
                        Selects = StoreUserFavoriteProductMappingSelect.FavoriteProduct | StoreUserFavoriteProductMappingSelect.StoreUser
                    }
                ); // lay ra tat ca cac mapping cua storeUser
                List<long> ProductIds = StoreUserFavoriteProductMappings
                    .Select(x => x.FavoriteProductId)
                    .ToList(); // lay ra list cac product duoc like

                ProductFilter = await SetProductFilter(ProductFilter, ProductIds, Store); // tính lại productFilter

                int result;
                result = await UOW.ProductRepository.Count(ProductFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ProductService));
            }
            return 0;
        }
        public async Task<int> ESCount(ProductFilter ProductFilter)
        {
            try
            {
                Store Store = await GetStore();
                List<StoreUserFavoriteProductMapping> StoreUserFavoriteProductMappings = await UOW.StoreUserFavoriteProductMappingRepository.List(
                    new StoreUserFavoriteProductMappingFilter
                    {
                        StoreUserId = new IdFilter { Equal = CurrentContext.StoreUserId },
                        Selects = StoreUserFavoriteProductMappingSelect.FavoriteProduct | StoreUserFavoriteProductMappingSelect.StoreUser
                    }
                ); // lay ra tat ca cac mapping cua storeUser
                List<long> ProductIds = StoreUserFavoriteProductMappings
                    .Select(x => x.FavoriteProductId)
                    .ToList(); // lay ra list cac product duoc like

                ProductFilter = await SetProductFilter(ProductFilter, ProductIds, Store); // tính lại productFilter

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
        public async Task<List<Product>> ESList(ProductFilter ProductFilter)
        {
            try
            {
                Store Store = await GetStore();
                List<StoreUserFavoriteProductMapping> StoreUserFavoriteProductMappings = await UOW.StoreUserFavoriteProductMappingRepository.List(
                    new StoreUserFavoriteProductMappingFilter
                    {
                        StoreUserId = new IdFilter { Equal = CurrentContext.StoreUserId },
                        Selects = StoreUserFavoriteProductMappingSelect.FavoriteProduct | StoreUserFavoriteProductMappingSelect.StoreUser
                    }
                ); // lay ra tat ca cac mapping cua storeUser
                List<long> ProductIds = StoreUserFavoriteProductMappings
                    .Select(x => x.FavoriteProductId)
                    .ToList(); // lay ra list cac product duoc like

                ProductFilter = await SetProductFilter(ProductFilter, ProductIds, Store); // tính lại productFilter

                List<Product> Products;

                var SystemConfig = await UOW.SystemConfigurationRepository.Get();
                if (SystemConfig.USE_ELASTICSEARCH)
                    Products = await UOW.EsProductRepository.List(ProductFilter);
                else
                    Products = await UOW.ProductRepository.List(ProductFilter);

                ProductIds = Products.Select(x => x.Id).ToList();
                ItemFilter ItemFilter = new ItemFilter
                {
                    ProductId = new IdFilter { In = ProductIds },
                    Selects = ItemSelect.ALL,
                    OrderBy = ItemOrder.Id,
                    OrderType = OrderType.ASC,
                    Skip = 0,
                    Take = int.MaxValue,
                };

                List<Item> Items = await UOW.ItemRepository.List(ItemFilter);
                Items = await ApplyPrice(Items, Store.Id); // ap gia theo priceList

                var VariationGroupings = Products.Where(x => x.UsedVariationId == UsedVariationEnum.USED.Id).SelectMany(x => x.VariationGroupings).ToList();
                var VariationGroupingIds = VariationGroupings?.Select(x => x.Id).ToList();
                VariationFilter VariationFilter = new VariationFilter
                {
                    VariationGroupingId = new IdFilter { In = VariationGroupingIds },
                    Selects = VariationSelect.ALL,
                    OrderBy = VariationOrder.Id,
                    OrderType = OrderType.ASC,
                    Skip = 0,
                    Take = int.MaxValue,
                };
                List<Variation> VariationsInDb = await UOW.VariationRepository.List(VariationFilter);

                foreach (Product Product in Products)
                {
                    List<Item> items = new List<Item>();
                    if (Product.VariationGroupings != null && Product.VariationGroupings.Count > 0)
                    {
                        List<Variation> Variations = VariationsInDb.Where(x => x.VariationGroupingId == Product.VariationGroupings[0].Id).ToList();
                        foreach (Variation Variation in Variations)
                        {
                            Item Item = Items.Where(x => x.ProductId == Product.Id && x.Code.Split("-").Contains(Variation.Code)).FirstOrDefault();
                            if (Item != null)
                                items.Add(Item);
                        }
                    }
                    List<Item> NonVariationsItems = Items.Where(x => x.ProductId == Product.Id).ToList();
                    Product.Items = items.Count > 0 ? items : NonVariationsItems;
                    Product.VariationCounter = Items.Where(i => i.ProductId == Product.Id).Count();
                    Product.IsFavorite = false;
                    int LikeCount = StoreUserFavoriteProductMappings.Where(x => x.FavoriteProductId == Product.Id).Count();
                    if (LikeCount > 0) Product.IsFavorite = true;
                }
                return Products;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ProductService));
            }
            return null;
        }
        public async Task<List<Product>> List(ProductFilter ProductFilter)
        {
            try
            {
                Store Store = await GetStore();
                List<StoreUserFavoriteProductMapping> StoreUserFavoriteProductMappings = await UOW.StoreUserFavoriteProductMappingRepository.List(
                    new StoreUserFavoriteProductMappingFilter
                    {
                        StoreUserId = new IdFilter { Equal = CurrentContext.StoreUserId },
                        Selects = StoreUserFavoriteProductMappingSelect.FavoriteProduct | StoreUserFavoriteProductMappingSelect.StoreUser
                    }
                ); // lay ra tat ca cac mapping cua storeUser
                List<long> ProductIds = StoreUserFavoriteProductMappings
                    .Select(x => x.FavoriteProductId)
                    .ToList(); // lay ra list cac product duoc like

                ProductFilter = await SetProductFilter(ProductFilter, ProductIds, Store); // tính lại productFilter

                List<Product> Products;

                Products = await UOW.ProductRepository.List(ProductFilter);

                ProductIds = Products.Select(x => x.Id).ToList();
                ItemFilter ItemFilter = new ItemFilter
                {
                    ProductId = new IdFilter { In = ProductIds },
                    Selects = ItemSelect.ALL,
                    OrderBy = ItemOrder.Id,
                    OrderType = OrderType.ASC,
                    Skip = 0,
                    Take = int.MaxValue,
                };

                List<Item> Items = await UOW.ItemRepository.List(ItemFilter); ;
                var SystemConfig = await UOW.SystemConfigurationRepository.Get();
                if (SystemConfig.USE_ELASTICSEARCH)
                    Items = await UOW.EsItemRepository.List(ItemFilter);
                else
                    Items = await UOW.ItemRepository.List(ItemFilter);

                Items = await ApplyPrice(Items, Store.Id); // ap gia theo priceList
                foreach (Product Product in Products)
                {
                    List<Item> items = new List<Item>();
                    if (Product.VariationGroupings != null && Product.VariationGroupings.Count > 0)
                    {
                        VariationFilter VariationFilter = new VariationFilter
                        {
                            VariationGroupingId = new IdFilter { Equal = Product.VariationGroupings[0].Id },
                            Selects = VariationSelect.ALL,
                            OrderBy = VariationOrder.Id,
                            OrderType = OrderType.ASC,
                            Skip = 0,
                            Take = int.MaxValue,
                        };
                        List<Variation> Variations = await UOW.VariationRepository.List(VariationFilter);
                        foreach (Variation Variation in Variations)
                        {
                            Item Item = Items.Where(x => x.ProductId == Product.Id && x.Code.Split("-").Contains(Variation.Code)).FirstOrDefault();
                            if (Item != null)
                                items.Add(Item);
                        }
                    }
                    List<Item> NonVariationsItems = Items.Where(x => x.ProductId == Product.Id).ToList();
                    Product.Items = items.Count > 0 ? items : NonVariationsItems;
                    Product.VariationCounter = Items.Where(i => i.ProductId == Product.Id).Count();
                    Product.IsFavorite = false;
                    int LikeCount = StoreUserFavoriteProductMappings.Where(x => x.FavoriteProductId == Product.Id).Count();
                    if (LikeCount > 0) Product.IsFavorite = true;
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
            Product Product;
            Product = await UOW.ProductRepository.Get(Id);
            Product.IsFavorite = false;
            int LikeCount = await UOW.StoreUserFavoriteProductMappingRepository.Count(new StoreUserFavoriteProductMappingFilter
            {
                FavoriteProductId = new IdFilter { Equal = Id },
                StoreUserId = new IdFilter { Equal = CurrentContext.StoreUserId }
            });
            if (LikeCount > 0)
            {
                Product.IsFavorite = true;
            }
            if (Product == null)
                return null;
            if (Product.Items != null && Product.Items.Any())
            {
                Product.VariationCounter = Product.Items.Count;
            }
            return Product;
        }

        public async Task<Image> SaveImage(Image Image)
        {
            FileInfo fileInfo = new FileInfo(Image.Name);
            string path = $"/product/{StaticParams.DateTimeNow.ToString("yyyyMMdd")}/{Guid.NewGuid()}{fileInfo.Extension}";
            string thumbnailPath = $"/product/{StaticParams.DateTimeNow.ToString("yyyyMMdd")}/{Guid.NewGuid()}{fileInfo.Extension}";
            Image = await ImageService.Create(Image, path, thumbnailPath, 128, 128);
            return Image;
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

        private async Task<List<Item>> ApplyPrice(List<Item> Items, long StoreId)
        {
            var Store = await UOW.StoreRepository.Get(StoreId);
            SystemConfiguration SystemConfiguration = await UOW.SystemConfigurationRepository.Get();
            OrganizationFilter OrganizationFilter = new OrganizationFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = OrganizationSelect.ALL,
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            };

            var Organizations = await UOW.OrganizationRepository.List(OrganizationFilter);
            var OrganizationIds = Organizations
                .Where(x => x.Path.StartsWith(Store.Organization.Path) || Store.Organization.Path.StartsWith(x.Path))
                .Select(x => x.Id)
                .ToList();

            var ItemIds = Items.Select(x => x.Id).Distinct().ToList();
            Dictionary<long, decimal> result = new Dictionary<long, decimal>();

            PriceListItemMappingFilter PriceListItemMappingFilter = new PriceListItemMappingFilter
            {
                ItemId = new IdFilter { In = ItemIds },
                Skip = 0,
                Take = int.MaxValue,
                Selects = PriceListItemMappingSelect.ALL,
                PriceListTypeId = new IdFilter { Equal = PriceListTypeEnum.ALLSTORE.Id },
                SalesOrderTypeId = new IdFilter { In = new List<long> { SalesOrderTypeEnum.INDIRECT.Id, SalesOrderTypeEnum.ALL.Id } },
                OrganizationId = new IdFilter { In = OrganizationIds },
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            };

            var PriceListItemMappingAllStore = await UOW.PriceListItemMappingItemMappingRepository.List(PriceListItemMappingFilter);
            List<PriceListItemMapping> PriceListItemMappings = new List<PriceListItemMapping>();
            PriceListItemMappings.AddRange(PriceListItemMappingAllStore);

            PriceListItemMappingFilter = new PriceListItemMappingFilter
            {
                ItemId = new IdFilter { In = ItemIds },
                Skip = 0,
                Take = int.MaxValue,
                Selects = PriceListItemMappingSelect.ALL,
                PriceListTypeId = new IdFilter { Equal = PriceListTypeEnum.STOREGROUPING.Id },
                SalesOrderTypeId = new IdFilter { In = new List<long> { SalesOrderTypeEnum.DIRECT.Id, SalesOrderTypeEnum.ALL.Id } },
                StoreGroupingId = new IdFilter { In = Store.StoreStoreGroupingMappings?.Select(x => x.StoreGroupingId).ToList() },
                OrganizationId = new IdFilter { In = OrganizationIds },
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            };
            var PriceListItemMappingStoreGrouping = await UOW.PriceListItemMappingItemMappingRepository.List(PriceListItemMappingFilter);

            PriceListItemMappingFilter = new PriceListItemMappingFilter
            {
                ItemId = new IdFilter { In = ItemIds },
                Skip = 0,
                Take = int.MaxValue,
                Selects = PriceListItemMappingSelect.ALL,
                PriceListTypeId = new IdFilter { Equal = PriceListTypeEnum.STORETYPE.Id },
                SalesOrderTypeId = new IdFilter { In = new List<long> { SalesOrderTypeEnum.DIRECT.Id, SalesOrderTypeEnum.ALL.Id } },
                StoreTypeId = new IdFilter { Equal = Store.StoreTypeId },
                OrganizationId = new IdFilter { In = OrganizationIds },
                StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id }
            };
            var PriceListItemMappingStoreType = await UOW.PriceListItemMappingItemMappingRepository.List(PriceListItemMappingFilter);

            PriceListItemMappingFilter = new PriceListItemMappingFilter
            {
                ItemId = new IdFilter { In = ItemIds },
                Skip = 0,
                Take = int.MaxValue,
                Selects = PriceListItemMappingSelect.ALL,
                PriceListTypeId = new IdFilter { Equal = PriceListTypeEnum.DETAILS.Id },
                SalesOrderTypeId = new IdFilter { In = new List<long> { SalesOrderTypeEnum.DIRECT.Id, SalesOrderTypeEnum.ALL.Id } },
                StoreId = new IdFilter { Equal = StoreId },
                OrganizationId = new IdFilter { In = OrganizationIds },
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            };
            var PriceListItemMappingStoreDetail = await UOW.PriceListItemMappingItemMappingRepository.List(PriceListItemMappingFilter);
            PriceListItemMappings.AddRange(PriceListItemMappingStoreGrouping);
            PriceListItemMappings.AddRange(PriceListItemMappingStoreType);
            PriceListItemMappings.AddRange(PriceListItemMappingStoreDetail);

            //Áp giá theo cấu hình
            //Ưu tiên lấy giá thấp hơn
            if (SystemConfiguration.PRIORITY_USE_PRICE_LIST == 0)
            {
                foreach (var ItemId in ItemIds)
                {
                    result.Add(ItemId, decimal.MaxValue);
                }
                foreach (var ItemId in ItemIds)
                {
                    foreach (var OrganizationId in OrganizationIds)
                    {
                        decimal targetPrice = decimal.MaxValue;
                        targetPrice = PriceListItemMappings.Where(x => x.ItemId == ItemId && x.PriceList.OrganizationId == OrganizationId)
                            .Select(x => x.Price)
                            .DefaultIfEmpty(decimal.MaxValue)
                            .Min();
                        if (targetPrice < result[ItemId])
                        {
                            result[ItemId] = targetPrice;
                        }
                    }
                }

                foreach (var ItemId in ItemIds)
                {
                    if (result[ItemId] == decimal.MaxValue)
                    {
                        result[ItemId] = Items.Where(x => x.Id == ItemId).Select(x => x.SalePrice.GetValueOrDefault(0)).FirstOrDefault();
                    }
                }
            }
            //Ưu tiên lấy giá cao hơn
            else if (SystemConfiguration.PRIORITY_USE_PRICE_LIST == 1)
            {
                foreach (var ItemId in ItemIds)
                {
                    result.Add(ItemId, decimal.MinValue);
                }
                foreach (var ItemId in ItemIds)
                {
                    foreach (var OrganizationId in OrganizationIds)
                    {
                        decimal targetPrice = decimal.MinValue;
                        targetPrice = PriceListItemMappings.Where(x => x.ItemId == ItemId && x.PriceList.OrganizationId == OrganizationId)
                            .Select(x => x.Price)
                            .DefaultIfEmpty(decimal.MinValue)
                            .Max();
                        if (targetPrice > result[ItemId])
                        {
                            result[ItemId] = targetPrice;
                        }
                    }
                }

                foreach (var ItemId in ItemIds)
                {
                    if (result[ItemId] == decimal.MinValue)
                    {
                        result[ItemId] = Items.Where(x => x.Id == ItemId).Select(x => x.SalePrice.GetValueOrDefault(0)).FirstOrDefault();
                    }
                }
            }

            foreach (var item in Items)
            {
                item.SalePrice = result[item.Id] * (1 + item.Product.TaxType.Percentage / 100);
            }
            // gia hien thi tren man hinh khong tinh thue
            return Items;
        }

        private async Task<Store> GetStore()
        {
            var StoreUserId = CurrentContext.StoreUserId;
            StoreUser StoreUser = await UOW.StoreUserRepository.Get(StoreUserId);
            if (StoreUser == null)
            {
                return null;
            } // check storeUser co ton tai khong
            Store Store = await UOW.StoreRepository.Get(StoreUser.StoreId);
            if (Store == null)
            {
                return null;
            } // check store tuong ung vs storeUser co ton tai khong
            return Store;
        }

        private async Task<ProductFilter> SetProductFilter(ProductFilter ProductFilter, List<long> ProductIds, Store Store)
        {

            //if (ProductFilter.ItemSalePrice != null)
            //{
            //    ItemFilter ItemFilter = new ItemFilter
            //    {
            //        Skip = 0,
            //        Take = int.MaxValue,
            //        Selects = ItemSelect.Id | ItemSelect.ProductId | ItemSelect.SalePrice
            //    };

            //    List<Item> ItemList;
            //    ItemList = await UOW.ItemRepository.List(ItemFilter);

            //    ItemList = await ApplyPrice(ItemList, Store.Id); // ap gia theo priceList
            //    if (ProductFilter.ItemSalePrice.GreaterEqual.HasValue)
            //    {
            //        ItemList = ItemList
            //        .Where(x => x.SalePrice >= ProductFilter.ItemSalePrice.GreaterEqual)
            //        .ToList(); // lọc giá item sau khi đã áp giá theo filter
            //    }
            //    if (ProductFilter.ItemSalePrice.LessEqual.HasValue)
            //    {
            //        ItemList = ItemList
            //        .Where(x => x.SalePrice <= ProductFilter.ItemSalePrice.LessEqual)
            //        .ToList(); // lọc giá item sau khi đã áp giá theo filter
            //    }
            //    if (ProductFilter.ItemSalePrice.GreaterEqual.HasValue && ProductFilter.ItemSalePrice.LessEqual.HasValue)
            //    {
            //        ItemList = ItemList
            //        .Where(x => x.SalePrice >= ProductFilter.ItemSalePrice.GreaterEqual && x.SalePrice <= ProductFilter.ItemSalePrice.LessEqual)
            //        .ToList(); // lọc giá item sau khi đã áp giá theo filter
            //    }
            //    var Ids = ItemList.Select(x => x.ProductId)
            //        .Distinct()
            //        .ToList();
            //    if (ProductFilter.Id.In != null)
            //    {
            //        ProductFilter.Id.In = ProductFilter.Id.In
            //            .Where(x => Ids
            //            .Contains(x))
            //            .ToList(); // tổ hợp ProductFilter Id
            //    }
            //    else
            //    {
            //        ProductFilter.Id.In = Ids;
            //    }
            //} // lay productId theo gia Item

            if (ProductFilter.Id == null)
                ProductFilter.Id = new IdFilter();
            if (ProductFilter.IsFavorite.HasValue && ProductFilter.IsFavorite.Value)
            {
                if (ProductFilter.Id.In != null)
                {
                    ProductFilter.Id.In = ProductFilter.Id.In
                        .Where(x => ProductIds.Contains(x))
                        .ToList();
                }
                else
                {
                    ProductFilter.Id.In = ProductIds; // lay het cac san pham duoc like
                }
            }
            if (ProductFilter.IsFavorite.HasValue && !ProductFilter.IsFavorite.Value)
            {
                if (ProductFilter.Id.NotIn != null)
                {
                    ProductFilter.Id.NotIn = ProductFilter.Id.NotIn
                        .Where(x => ProductIds.Contains(x))
                        .ToList();
                }
                else
                {
                    ProductFilter.Id.NotIn = ProductIds; // lay het cac san pham khong duoc thich
                }
            }
            if (ProductFilter.CategoryId != null && ProductFilter.CategoryId.Equal.HasValue)
            {
                Category Category = await UOW.CategoryRepository.Get(ProductFilter.CategoryId.Equal.Value);
                List<Category> Cateogories = await UOW.CategoryRepository.List(new CategoryFilter
                {
                    Take = int.MaxValue,
                    Skip = 0,
                    Selects = CategorySelect.Id | CategorySelect.Path,
                    Path = new StringFilter { StartWith = Category.Path }
                });
                List<long> CategoryIds = Cateogories.Select(x => x.Id).ToList();
                ProductFilter.CategoryId.In = CategoryIds;
            }
            return ProductFilter;
        } // tính lại ProductFilter theo Item SalePrice và sản phẩm được yêu thích
    }
}
