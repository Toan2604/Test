using DMS.Entities;
using DMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Repositories
{
    public interface IDirectSalesOrderContentRepository
    {
        Task<int> Count(DirectSalesOrderContentFilter DirectSalesOrderContentFilter);
        Task<int> CountAll(DirectSalesOrderContentFilter DirectSalesOrderContentFilter);
        Task<List<DirectSalesOrderContent>> List(DirectSalesOrderContentFilter DirectSalesOrderContentFilter);
        Task<DirectSalesOrderContent> Get(long Id);
        Task<bool> Create(DirectSalesOrderContent DirectSalesOrderContent);
        Task<bool> Update(DirectSalesOrderContent DirectSalesOrderContent);
        Task<bool> Delete(DirectSalesOrderContent DirectSalesOrderContent);
        Task<bool> BulkMerge(List<DirectSalesOrderContent> DirectSalesOrderContents);
        Task<bool> BulkDelete(List<DirectSalesOrderContent> DirectSalesOrderContents);
    }
    public class DirectSalesOrderContentRepository : IDirectSalesOrderContentRepository
    {
        private DataContext DataContext;
        public DirectSalesOrderContentRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<DirectSalesOrderContentDAO> DynamicFilter(IQueryable<DirectSalesOrderContentDAO> query, DirectSalesOrderContentFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.DirectSalesOrderId, filter.DirectSalesOrderId);
            query = query.Where(q => q.Item.ProductId, filter.ProductId);
            query = query.Where(q => q.ItemId, filter.ItemId);
            query = query.Where(q => q.UnitOfMeasureId, filter.UnitOfMeasureId);
            query = query.Where(q => q.Quantity, filter.Quantity);
            query = query.Where(q => q.PrimaryUnitOfMeasureId, filter.PrimaryUnitOfMeasureId);
            query = query.Where(q => q.RequestedQuantity, filter.RequestedQuantity);
            query = query.Where(q => q.PrimaryPrice, filter.PrimaryPrice);
            query = query.Where(q => q.SalePrice, filter.SalePrice);
            query = query.Where(q => q.DiscountPercentage, filter.DiscountPercentage);
            query = query.Where(q => q.DiscountAmount, filter.DiscountAmount);
            query = query.Where(q => q.GeneralDiscountPercentage, filter.GeneralDiscountPercentage);
            query = query.Where(q => q.GeneralDiscountAmount, filter.GeneralDiscountAmount);
            query = query.Where(q => q.Amount, filter.Amount);
            query = query.Where(q => q.TaxPercentage, filter.TaxPercentage);
            query = query.Where(q => q.TaxAmount, filter.TaxAmount);
            return query;
        }

        private IQueryable<DirectSalesOrderContentDAO> OrFilter(IQueryable<DirectSalesOrderContentDAO> query, DirectSalesOrderContentFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<DirectSalesOrderContentDAO> initQuery = query.Where(q => false);
            foreach (DirectSalesOrderContentFilter DirectSalesOrderContentFilter in filter.OrFilter)
            {
                IQueryable<DirectSalesOrderContentDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, DirectSalesOrderContentFilter.Id);
                queryable = queryable.Where(q => q.DirectSalesOrderId, DirectSalesOrderContentFilter.DirectSalesOrderId);
                queryable = queryable.Where(q => q.ItemId, DirectSalesOrderContentFilter.ItemId);
                queryable = queryable.Where(q => q.UnitOfMeasureId, DirectSalesOrderContentFilter.UnitOfMeasureId);
                queryable = queryable.Where(q => q.Quantity, DirectSalesOrderContentFilter.Quantity);
                queryable = queryable.Where(q => q.PrimaryUnitOfMeasureId, DirectSalesOrderContentFilter.PrimaryUnitOfMeasureId);
                queryable = queryable.Where(q => q.RequestedQuantity, DirectSalesOrderContentFilter.RequestedQuantity);
                queryable = queryable.Where(q => q.PrimaryPrice, DirectSalesOrderContentFilter.PrimaryPrice);
                queryable = queryable.Where(q => q.SalePrice, DirectSalesOrderContentFilter.SalePrice);
                queryable = queryable.Where(q => q.DiscountPercentage, DirectSalesOrderContentFilter.DiscountPercentage);
                queryable = queryable.Where(q => q.DiscountAmount, DirectSalesOrderContentFilter.DiscountAmount);
                queryable = queryable.Where(q => q.GeneralDiscountPercentage, DirectSalesOrderContentFilter.GeneralDiscountPercentage);
                queryable = queryable.Where(q => q.GeneralDiscountAmount, DirectSalesOrderContentFilter.GeneralDiscountAmount);
                queryable = queryable.Where(q => q.Amount, DirectSalesOrderContentFilter.Amount);
                queryable = queryable.Where(q => q.TaxPercentage, DirectSalesOrderContentFilter.TaxPercentage);
                queryable = queryable.Where(q => q.TaxAmount, DirectSalesOrderContentFilter.TaxAmount);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<DirectSalesOrderContentDAO> DynamicOrder(IQueryable<DirectSalesOrderContentDAO> query, DirectSalesOrderContentFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case DirectSalesOrderContentOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case DirectSalesOrderContentOrder.DirectSalesOrder:
                            query = query.OrderBy(q => q.DirectSalesOrderId);
                            break;
                        case DirectSalesOrderContentOrder.Item:
                            query = query.OrderBy(q => q.ItemId);
                            break;
                        case DirectSalesOrderContentOrder.UnitOfMeasure:
                            query = query.OrderBy(q => q.UnitOfMeasureId);
                            break;
                        case DirectSalesOrderContentOrder.Quantity:
                            query = query.OrderBy(q => q.Quantity);
                            break;
                        case DirectSalesOrderContentOrder.PrimaryUnitOfMeasure:
                            query = query.OrderBy(q => q.PrimaryUnitOfMeasureId);
                            break;
                        case DirectSalesOrderContentOrder.RequestedQuantity:
                            query = query.OrderBy(q => q.RequestedQuantity);
                            break;
                        case DirectSalesOrderContentOrder.PrimaryPrice:
                            query = query.OrderBy(q => q.PrimaryPrice);
                            break;
                        case DirectSalesOrderContentOrder.SalePrice:
                            query = query.OrderBy(q => q.SalePrice);
                            break;
                        case DirectSalesOrderContentOrder.DiscountPercentage:
                            query = query.OrderBy(q => q.DiscountPercentage);
                            break;
                        case DirectSalesOrderContentOrder.DiscountAmount:
                            query = query.OrderBy(q => q.DiscountAmount);
                            break;
                        case DirectSalesOrderContentOrder.GeneralDiscountPercentage:
                            query = query.OrderBy(q => q.GeneralDiscountPercentage);
                            break;
                        case DirectSalesOrderContentOrder.GeneralDiscountAmount:
                            query = query.OrderBy(q => q.GeneralDiscountAmount);
                            break;
                        case DirectSalesOrderContentOrder.Amount:
                            query = query.OrderBy(q => q.Amount);
                            break;
                        case DirectSalesOrderContentOrder.TaxPercentage:
                            query = query.OrderBy(q => q.TaxPercentage);
                            break;
                        case DirectSalesOrderContentOrder.TaxAmount:
                            query = query.OrderBy(q => q.TaxAmount);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case DirectSalesOrderContentOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case DirectSalesOrderContentOrder.DirectSalesOrder:
                            query = query.OrderByDescending(q => q.DirectSalesOrderId);
                            break;
                        case DirectSalesOrderContentOrder.Item:
                            query = query.OrderByDescending(q => q.ItemId);
                            break;
                        case DirectSalesOrderContentOrder.UnitOfMeasure:
                            query = query.OrderByDescending(q => q.UnitOfMeasureId);
                            break;
                        case DirectSalesOrderContentOrder.Quantity:
                            query = query.OrderByDescending(q => q.Quantity);
                            break;
                        case DirectSalesOrderContentOrder.PrimaryUnitOfMeasure:
                            query = query.OrderByDescending(q => q.PrimaryUnitOfMeasureId);
                            break;
                        case DirectSalesOrderContentOrder.RequestedQuantity:
                            query = query.OrderByDescending(q => q.RequestedQuantity);
                            break;
                        case DirectSalesOrderContentOrder.PrimaryPrice:
                            query = query.OrderByDescending(q => q.PrimaryPrice);
                            break;
                        case DirectSalesOrderContentOrder.SalePrice:
                            query = query.OrderByDescending(q => q.SalePrice);
                            break;
                        case DirectSalesOrderContentOrder.DiscountPercentage:
                            query = query.OrderByDescending(q => q.DiscountPercentage);
                            break;
                        case DirectSalesOrderContentOrder.DiscountAmount:
                            query = query.OrderByDescending(q => q.DiscountAmount);
                            break;
                        case DirectSalesOrderContentOrder.GeneralDiscountPercentage:
                            query = query.OrderByDescending(q => q.GeneralDiscountPercentage);
                            break;
                        case DirectSalesOrderContentOrder.GeneralDiscountAmount:
                            query = query.OrderByDescending(q => q.GeneralDiscountAmount);
                            break;
                        case DirectSalesOrderContentOrder.Amount:
                            query = query.OrderByDescending(q => q.Amount);
                            break;
                        case DirectSalesOrderContentOrder.TaxPercentage:
                            query = query.OrderByDescending(q => q.TaxPercentage);
                            break;
                        case DirectSalesOrderContentOrder.TaxAmount:
                            query = query.OrderByDescending(q => q.TaxAmount);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<DirectSalesOrderContent>> DynamicSelect(IQueryable<DirectSalesOrderContentDAO> query, DirectSalesOrderContentFilter filter)
        {
            List<DirectSalesOrderContent> DirectSalesOrderContents = await query.Select(q => new DirectSalesOrderContent()
            {
                Id = filter.Selects.Contains(DirectSalesOrderContentSelect.Id) ? q.Id : default(long),
                DirectSalesOrderId = filter.Selects.Contains(DirectSalesOrderContentSelect.DirectSalesOrder) ? q.DirectSalesOrderId : default(long),
                ItemId = filter.Selects.Contains(DirectSalesOrderContentSelect.Item) ? q.ItemId : default(long),
                UnitOfMeasureId = filter.Selects.Contains(DirectSalesOrderContentSelect.UnitOfMeasure) ? q.UnitOfMeasureId : default(long),
                Quantity = filter.Selects.Contains(DirectSalesOrderContentSelect.Quantity) ? q.Quantity : default(long),
                PrimaryUnitOfMeasureId = filter.Selects.Contains(DirectSalesOrderContentSelect.PrimaryUnitOfMeasure) ? q.PrimaryUnitOfMeasureId : default(long),
                RequestedQuantity = filter.Selects.Contains(DirectSalesOrderContentSelect.RequestedQuantity) ? q.RequestedQuantity : default(long),
                PrimaryPrice = filter.Selects.Contains(DirectSalesOrderContentSelect.PrimaryPrice) ? q.PrimaryPrice : default(long),
                SalePrice = filter.Selects.Contains(DirectSalesOrderContentSelect.SalePrice) ? q.SalePrice : default(long),
                DiscountPercentage = filter.Selects.Contains(DirectSalesOrderContentSelect.DiscountPercentage) ? q.DiscountPercentage : default(decimal?),
                DiscountAmount = filter.Selects.Contains(DirectSalesOrderContentSelect.DiscountAmount) ? q.DiscountAmount : default(long?),
                GeneralDiscountPercentage = filter.Selects.Contains(DirectSalesOrderContentSelect.GeneralDiscountPercentage) ? q.GeneralDiscountPercentage : default(decimal?),
                GeneralDiscountAmount = filter.Selects.Contains(DirectSalesOrderContentSelect.GeneralDiscountAmount) ? q.GeneralDiscountAmount : default(long?),
                Amount = filter.Selects.Contains(DirectSalesOrderContentSelect.Amount) ? q.Amount : default(long),
                TaxPercentage = filter.Selects.Contains(DirectSalesOrderContentSelect.TaxPercentage) ? q.TaxPercentage : default(decimal?),
                TaxAmount = filter.Selects.Contains(DirectSalesOrderContentSelect.TaxAmount) ? q.TaxAmount : default(long?),
                Factor = filter.Selects.Contains(DirectSalesOrderContentSelect.Factor) ? q.Factor : default(long?),
                DirectSalesOrder = filter.Selects.Contains(DirectSalesOrderContentSelect.DirectSalesOrder) && q.DirectSalesOrder != null ? new DirectSalesOrder
                {
                    Id = q.DirectSalesOrder.Id,
                    Code = q.DirectSalesOrder.Code,
                    BuyerStoreId = q.DirectSalesOrder.BuyerStoreId,
                    PhoneNumber = q.DirectSalesOrder.PhoneNumber,
                    StoreAddress = q.DirectSalesOrder.StoreAddress,
                    DeliveryAddress = q.DirectSalesOrder.DeliveryAddress,
                    SaleEmployeeId = q.DirectSalesOrder.SaleEmployeeId,
                    OrderDate = q.DirectSalesOrder.OrderDate,
                    DeliveryDate = q.DirectSalesOrder.DeliveryDate,
                    EditedPriceStatusId = q.DirectSalesOrder.EditedPriceStatusId,
                    Note = q.DirectSalesOrder.Note,
                    SubTotal = q.DirectSalesOrder.SubTotal,
                    GeneralDiscountPercentage = q.DirectSalesOrder.GeneralDiscountPercentage,
                    GeneralDiscountAmount = q.DirectSalesOrder.GeneralDiscountAmount,
                    TotalTaxAmount = q.DirectSalesOrder.TotalTaxAmount,
                    Total = q.DirectSalesOrder.Total,
                } : null,
                PrimaryUnitOfMeasure = filter.Selects.Contains(DirectSalesOrderContentSelect.PrimaryUnitOfMeasure) && q.PrimaryUnitOfMeasure != null ? new UnitOfMeasure
                {
                    Id = q.PrimaryUnitOfMeasure.Id,
                    Code = q.PrimaryUnitOfMeasure.Code,
                    Name = q.PrimaryUnitOfMeasure.Name,
                    Description = q.PrimaryUnitOfMeasure.Description,
                    StatusId = q.PrimaryUnitOfMeasure.StatusId,
                } : null,
                UnitOfMeasure = filter.Selects.Contains(DirectSalesOrderContentSelect.UnitOfMeasure) && q.UnitOfMeasure != null ? new UnitOfMeasure
                {
                    Id = q.UnitOfMeasure.Id,
                    Code = q.UnitOfMeasure.Code,
                    Name = q.UnitOfMeasure.Name,
                    Description = q.UnitOfMeasure.Description,
                    StatusId = q.UnitOfMeasure.StatusId,
                } : null,
            }).ToListWithNoLockAsync();
            return DirectSalesOrderContents;
        }

        public async Task<int> Count(DirectSalesOrderContentFilter filter)
        {
            IQueryable<DirectSalesOrderContentDAO> DirectSalesOrderContents = DataContext.DirectSalesOrderContent.AsNoTracking();
            DirectSalesOrderContents = DynamicFilter(DirectSalesOrderContents, filter);
            DirectSalesOrderContents = OrFilter(DirectSalesOrderContents, filter);
            return await DirectSalesOrderContents.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(DirectSalesOrderContentFilter filter)
        {
            IQueryable<DirectSalesOrderContentDAO> DirectSalesOrderContents = DataContext.DirectSalesOrderContent.AsNoTracking();
            DirectSalesOrderContents = DynamicFilter(DirectSalesOrderContents, filter);
            return await DirectSalesOrderContents.CountWithNoLockAsync();
        }
        public async Task<List<DirectSalesOrderContent>> List(DirectSalesOrderContentFilter filter)
        {
            if (filter == null) return new List<DirectSalesOrderContent>();
            IQueryable<DirectSalesOrderContentDAO> DirectSalesOrderContentDAOs = DataContext.DirectSalesOrderContent.AsNoTracking();
            DirectSalesOrderContentDAOs = DynamicFilter(DirectSalesOrderContentDAOs, filter);
            DirectSalesOrderContentDAOs = OrFilter(DirectSalesOrderContentDAOs, filter);
            DirectSalesOrderContentDAOs = DynamicOrder(DirectSalesOrderContentDAOs, filter);
            List<DirectSalesOrderContent> DirectSalesOrderContents = await DynamicSelect(DirectSalesOrderContentDAOs, filter);
            return DirectSalesOrderContents;
        }

        public async Task<DirectSalesOrderContent> Get(long Id)
        {
            DirectSalesOrderContent DirectSalesOrderContent = await DataContext.DirectSalesOrderContent.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new DirectSalesOrderContent()
            {
                Id = x.Id,
                DirectSalesOrderId = x.DirectSalesOrderId,
                ItemId = x.ItemId,
                UnitOfMeasureId = x.UnitOfMeasureId,
                Quantity = x.Quantity,
                PrimaryUnitOfMeasureId = x.PrimaryUnitOfMeasureId,
                RequestedQuantity = x.RequestedQuantity,
                PrimaryPrice = x.PrimaryPrice,
                SalePrice = x.SalePrice,
                DiscountPercentage = x.DiscountPercentage,
                DiscountAmount = x.DiscountAmount,
                GeneralDiscountPercentage = x.GeneralDiscountPercentage,
                GeneralDiscountAmount = x.GeneralDiscountAmount,
                Amount = x.Amount,
                TaxPercentage = x.TaxPercentage,
                TaxAmount = x.TaxAmount,
                Factor = x.Factor,
                DirectSalesOrder = x.DirectSalesOrder == null ? null : new DirectSalesOrder
                {
                    Id = x.DirectSalesOrder.Id,
                    Code = x.DirectSalesOrder.Code,
                    BuyerStoreId = x.DirectSalesOrder.BuyerStoreId,
                    PhoneNumber = x.DirectSalesOrder.PhoneNumber,
                    StoreAddress = x.DirectSalesOrder.StoreAddress,
                    DeliveryAddress = x.DirectSalesOrder.DeliveryAddress,
                    SaleEmployeeId = x.DirectSalesOrder.SaleEmployeeId,
                    OrderDate = x.DirectSalesOrder.OrderDate,
                    DeliveryDate = x.DirectSalesOrder.DeliveryDate,
                    EditedPriceStatusId = x.DirectSalesOrder.EditedPriceStatusId,
                    Note = x.DirectSalesOrder.Note,
                    SubTotal = x.DirectSalesOrder.SubTotal,
                    GeneralDiscountPercentage = x.DirectSalesOrder.GeneralDiscountPercentage,
                    GeneralDiscountAmount = x.DirectSalesOrder.GeneralDiscountAmount,
                    TotalTaxAmount = x.DirectSalesOrder.TotalTaxAmount,
                    Total = x.DirectSalesOrder.Total,
                },
                PrimaryUnitOfMeasure = x.PrimaryUnitOfMeasure == null ? null : new UnitOfMeasure
                {
                    Id = x.PrimaryUnitOfMeasure.Id,
                    Code = x.PrimaryUnitOfMeasure.Code,
                    Name = x.PrimaryUnitOfMeasure.Name,
                    Description = x.PrimaryUnitOfMeasure.Description,
                    StatusId = x.PrimaryUnitOfMeasure.StatusId,
                },
                UnitOfMeasure = x.UnitOfMeasure == null ? null : new UnitOfMeasure
                {
                    Id = x.UnitOfMeasure.Id,
                    Code = x.UnitOfMeasure.Code,
                    Name = x.UnitOfMeasure.Name,
                    Description = x.UnitOfMeasure.Description,
                    StatusId = x.UnitOfMeasure.StatusId,
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (DirectSalesOrderContent == null)
                return null;

            return DirectSalesOrderContent;
        }
        public async Task<bool> Create(DirectSalesOrderContent DirectSalesOrderContent)
        {
            DirectSalesOrderContentDAO DirectSalesOrderContentDAO = new DirectSalesOrderContentDAO();
            DirectSalesOrderContentDAO.Id = DirectSalesOrderContent.Id;
            DirectSalesOrderContentDAO.DirectSalesOrderId = DirectSalesOrderContent.DirectSalesOrderId;
            DirectSalesOrderContentDAO.ItemId = DirectSalesOrderContent.ItemId;
            DirectSalesOrderContentDAO.UnitOfMeasureId = DirectSalesOrderContent.UnitOfMeasureId;
            DirectSalesOrderContentDAO.Quantity = DirectSalesOrderContent.Quantity;
            DirectSalesOrderContentDAO.PrimaryUnitOfMeasureId = DirectSalesOrderContent.PrimaryUnitOfMeasureId;
            DirectSalesOrderContentDAO.RequestedQuantity = DirectSalesOrderContent.RequestedQuantity;
            DirectSalesOrderContentDAO.PrimaryPrice = DirectSalesOrderContent.PrimaryPrice;
            DirectSalesOrderContentDAO.SalePrice = DirectSalesOrderContent.SalePrice;
            DirectSalesOrderContentDAO.DiscountPercentage = DirectSalesOrderContent.DiscountPercentage;
            DirectSalesOrderContentDAO.DiscountAmount = DirectSalesOrderContent.DiscountAmount;
            DirectSalesOrderContentDAO.GeneralDiscountPercentage = DirectSalesOrderContent.GeneralDiscountPercentage;
            DirectSalesOrderContentDAO.GeneralDiscountAmount = DirectSalesOrderContent.GeneralDiscountAmount;
            DirectSalesOrderContentDAO.Amount = DirectSalesOrderContent.Amount;
            DirectSalesOrderContentDAO.TaxPercentage = DirectSalesOrderContent.TaxPercentage;
            DirectSalesOrderContentDAO.TaxAmount = DirectSalesOrderContent.TaxAmount;
            DirectSalesOrderContentDAO.Factor = DirectSalesOrderContent.Factor;
            DataContext.DirectSalesOrderContent.Add(DirectSalesOrderContentDAO);
            await DataContext.SaveChangesAsync();
            DirectSalesOrderContent.Id = DirectSalesOrderContentDAO.Id;
            await SaveReference(DirectSalesOrderContent);
            return true;
        }

        public async Task<bool> Update(DirectSalesOrderContent DirectSalesOrderContent)
        {
            DirectSalesOrderContentDAO DirectSalesOrderContentDAO = DataContext.DirectSalesOrderContent.Where(x => x.Id == DirectSalesOrderContent.Id).FirstOrDefault();
            if (DirectSalesOrderContentDAO == null)
                return false;
            DirectSalesOrderContentDAO.Id = DirectSalesOrderContent.Id;
            DirectSalesOrderContentDAO.DirectSalesOrderId = DirectSalesOrderContent.DirectSalesOrderId;
            DirectSalesOrderContentDAO.ItemId = DirectSalesOrderContent.ItemId;
            DirectSalesOrderContentDAO.UnitOfMeasureId = DirectSalesOrderContent.UnitOfMeasureId;
            DirectSalesOrderContentDAO.Quantity = DirectSalesOrderContent.Quantity;
            DirectSalesOrderContentDAO.PrimaryUnitOfMeasureId = DirectSalesOrderContent.PrimaryUnitOfMeasureId;
            DirectSalesOrderContentDAO.RequestedQuantity = DirectSalesOrderContent.RequestedQuantity;
            DirectSalesOrderContentDAO.PrimaryPrice = DirectSalesOrderContent.PrimaryPrice;
            DirectSalesOrderContentDAO.SalePrice = DirectSalesOrderContent.SalePrice;
            DirectSalesOrderContentDAO.DiscountPercentage = DirectSalesOrderContent.DiscountPercentage;
            DirectSalesOrderContentDAO.DiscountAmount = DirectSalesOrderContent.DiscountAmount;
            DirectSalesOrderContentDAO.GeneralDiscountPercentage = DirectSalesOrderContent.GeneralDiscountPercentage;
            DirectSalesOrderContentDAO.GeneralDiscountAmount = DirectSalesOrderContent.GeneralDiscountAmount;
            DirectSalesOrderContentDAO.Amount = DirectSalesOrderContent.Amount;
            DirectSalesOrderContentDAO.TaxPercentage = DirectSalesOrderContent.TaxPercentage;
            DirectSalesOrderContentDAO.TaxAmount = DirectSalesOrderContent.TaxAmount;
            DirectSalesOrderContentDAO.Factor = DirectSalesOrderContent.Factor;
            await DataContext.SaveChangesAsync();
            await SaveReference(DirectSalesOrderContent);
            return true;
        }

        public async Task<bool> Delete(DirectSalesOrderContent DirectSalesOrderContent)
        {
            await DataContext.DirectSalesOrderContent.Where(x => x.Id == DirectSalesOrderContent.Id).DeleteFromQueryAsync();
            return true;
        }

        public async Task<bool> BulkMerge(List<DirectSalesOrderContent> DirectSalesOrderContents)
        {
            List<DirectSalesOrderContentDAO> DirectSalesOrderContentDAOs = new List<DirectSalesOrderContentDAO>();
            foreach (DirectSalesOrderContent DirectSalesOrderContent in DirectSalesOrderContents)
            {
                DirectSalesOrderContentDAO DirectSalesOrderContentDAO = new DirectSalesOrderContentDAO();
                DirectSalesOrderContentDAO.Id = DirectSalesOrderContent.Id;
                DirectSalesOrderContentDAO.DirectSalesOrderId = DirectSalesOrderContent.DirectSalesOrderId;
                DirectSalesOrderContentDAO.ItemId = DirectSalesOrderContent.ItemId;
                DirectSalesOrderContentDAO.UnitOfMeasureId = DirectSalesOrderContent.UnitOfMeasureId;
                DirectSalesOrderContentDAO.Quantity = DirectSalesOrderContent.Quantity;
                DirectSalesOrderContentDAO.PrimaryUnitOfMeasureId = DirectSalesOrderContent.PrimaryUnitOfMeasureId;
                DirectSalesOrderContentDAO.RequestedQuantity = DirectSalesOrderContent.RequestedQuantity;
                DirectSalesOrderContentDAO.PrimaryPrice = DirectSalesOrderContent.PrimaryPrice;
                DirectSalesOrderContentDAO.SalePrice = DirectSalesOrderContent.SalePrice;
                DirectSalesOrderContentDAO.DiscountPercentage = DirectSalesOrderContent.DiscountPercentage;
                DirectSalesOrderContentDAO.DiscountAmount = DirectSalesOrderContent.DiscountAmount;
                DirectSalesOrderContentDAO.GeneralDiscountPercentage = DirectSalesOrderContent.GeneralDiscountPercentage;
                DirectSalesOrderContentDAO.GeneralDiscountAmount = DirectSalesOrderContent.GeneralDiscountAmount;
                DirectSalesOrderContentDAO.Amount = DirectSalesOrderContent.Amount;
                DirectSalesOrderContentDAO.TaxPercentage = DirectSalesOrderContent.TaxPercentage;
                DirectSalesOrderContentDAO.TaxAmount = DirectSalesOrderContent.TaxAmount;
                DirectSalesOrderContentDAO.Factor = DirectSalesOrderContent.Factor;
                DirectSalesOrderContentDAOs.Add(DirectSalesOrderContentDAO);
            }
            await DataContext.BulkMergeAsync(DirectSalesOrderContentDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<DirectSalesOrderContent> DirectSalesOrderContents)
        {
            List<long> Ids = DirectSalesOrderContents.Select(x => x.Id).ToList();
            await DataContext.DirectSalesOrderContent
                .Where(x => Ids.Contains(x.Id))
                .DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(DirectSalesOrderContent DirectSalesOrderContent)
        {
        }

    }
}
