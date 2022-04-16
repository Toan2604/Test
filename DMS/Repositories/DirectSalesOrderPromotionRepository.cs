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
    public interface IDirectSalesOrderPromotionRepository
    {
        Task<int> Count(DirectSalesOrderPromotionFilter DirectSalesOrderPromotionFilter);
        Task<int> CountAll(DirectSalesOrderPromotionFilter DirectSalesOrderPromotionFilter);
        Task<List<DirectSalesOrderPromotion>> List(DirectSalesOrderPromotionFilter DirectSalesOrderPromotionFilter);
        Task<DirectSalesOrderPromotion> Get(long Id);
        Task<bool> Create(DirectSalesOrderPromotion DirectSalesOrderPromotion);
        Task<bool> Update(DirectSalesOrderPromotion DirectSalesOrderPromotion);
        Task<bool> Delete(DirectSalesOrderPromotion DirectSalesOrderPromotion);
        Task<bool> BulkMerge(List<DirectSalesOrderPromotion> DirectSalesOrderPromotions);
        Task<bool> BulkDelete(List<DirectSalesOrderPromotion> DirectSalesOrderPromotions);
    }
    public class DirectSalesOrderPromotionRepository : IDirectSalesOrderPromotionRepository
    {
        private DataContext DataContext;
        public DirectSalesOrderPromotionRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<DirectSalesOrderPromotionDAO> DynamicFilter(IQueryable<DirectSalesOrderPromotionDAO> query, DirectSalesOrderPromotionFilter filter)
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
            query = query.Where(q => q.Note, filter.Note);
            return query;
        }

        private IQueryable<DirectSalesOrderPromotionDAO> OrFilter(IQueryable<DirectSalesOrderPromotionDAO> query, DirectSalesOrderPromotionFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<DirectSalesOrderPromotionDAO> initQuery = query.Where(q => false);
            foreach (DirectSalesOrderPromotionFilter DirectSalesOrderPromotionFilter in filter.OrFilter)
            {
                IQueryable<DirectSalesOrderPromotionDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, DirectSalesOrderPromotionFilter.Id);
                queryable = queryable.Where(q => q.DirectSalesOrderId, DirectSalesOrderPromotionFilter.DirectSalesOrderId);
                queryable = queryable.Where(q => q.ItemId, DirectSalesOrderPromotionFilter.ItemId);
                queryable = queryable.Where(q => q.UnitOfMeasureId, DirectSalesOrderPromotionFilter.UnitOfMeasureId);
                queryable = queryable.Where(q => q.Quantity, DirectSalesOrderPromotionFilter.Quantity);
                queryable = queryable.Where(q => q.PrimaryUnitOfMeasureId, DirectSalesOrderPromotionFilter.PrimaryUnitOfMeasureId);
                queryable = queryable.Where(q => q.RequestedQuantity, DirectSalesOrderPromotionFilter.RequestedQuantity);
                queryable = queryable.Where(q => q.Note, DirectSalesOrderPromotionFilter.Note);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<DirectSalesOrderPromotionDAO> DynamicOrder(IQueryable<DirectSalesOrderPromotionDAO> query, DirectSalesOrderPromotionFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case DirectSalesOrderPromotionOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case DirectSalesOrderPromotionOrder.DirectSalesOrder:
                            query = query.OrderBy(q => q.DirectSalesOrderId);
                            break;
                        case DirectSalesOrderPromotionOrder.Item:
                            query = query.OrderBy(q => q.ItemId);
                            break;
                        case DirectSalesOrderPromotionOrder.UnitOfMeasure:
                            query = query.OrderBy(q => q.UnitOfMeasureId);
                            break;
                        case DirectSalesOrderPromotionOrder.Quantity:
                            query = query.OrderBy(q => q.Quantity);
                            break;
                        case DirectSalesOrderPromotionOrder.PrimaryUnitOfMeasure:
                            query = query.OrderBy(q => q.PrimaryUnitOfMeasureId);
                            break;
                        case DirectSalesOrderPromotionOrder.RequestedQuantity:
                            query = query.OrderBy(q => q.RequestedQuantity);
                            break;
                        case DirectSalesOrderPromotionOrder.Note:
                            query = query.OrderBy(q => q.Note);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case DirectSalesOrderPromotionOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case DirectSalesOrderPromotionOrder.DirectSalesOrder:
                            query = query.OrderByDescending(q => q.DirectSalesOrderId);
                            break;
                        case DirectSalesOrderPromotionOrder.Item:
                            query = query.OrderByDescending(q => q.ItemId);
                            break;
                        case DirectSalesOrderPromotionOrder.UnitOfMeasure:
                            query = query.OrderByDescending(q => q.UnitOfMeasureId);
                            break;
                        case DirectSalesOrderPromotionOrder.Quantity:
                            query = query.OrderByDescending(q => q.Quantity);
                            break;
                        case DirectSalesOrderPromotionOrder.PrimaryUnitOfMeasure:
                            query = query.OrderByDescending(q => q.PrimaryUnitOfMeasureId);
                            break;
                        case DirectSalesOrderPromotionOrder.RequestedQuantity:
                            query = query.OrderByDescending(q => q.RequestedQuantity);
                            break;
                        case DirectSalesOrderPromotionOrder.Note:
                            query = query.OrderByDescending(q => q.Note);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<DirectSalesOrderPromotion>> DynamicSelect(IQueryable<DirectSalesOrderPromotionDAO> query, DirectSalesOrderPromotionFilter filter)
        {
            List<DirectSalesOrderPromotion> DirectSalesOrderPromotions = await query.Select(q => new DirectSalesOrderPromotion()
            {
                Id = filter.Selects.Contains(DirectSalesOrderPromotionSelect.Id) ? q.Id : default(long),
                DirectSalesOrderId = filter.Selects.Contains(DirectSalesOrderPromotionSelect.DirectSalesOrder) ? q.DirectSalesOrderId : default(long),
                ItemId = filter.Selects.Contains(DirectSalesOrderPromotionSelect.Item) ? q.ItemId : default(long),
                UnitOfMeasureId = filter.Selects.Contains(DirectSalesOrderPromotionSelect.UnitOfMeasure) ? q.UnitOfMeasureId : default(long),
                Quantity = filter.Selects.Contains(DirectSalesOrderPromotionSelect.Quantity) ? q.Quantity : default(long),
                PrimaryUnitOfMeasureId = filter.Selects.Contains(DirectSalesOrderPromotionSelect.PrimaryUnitOfMeasure) ? q.PrimaryUnitOfMeasureId : default(long),
                RequestedQuantity = filter.Selects.Contains(DirectSalesOrderPromotionSelect.RequestedQuantity) ? q.RequestedQuantity : default(long),
                Note = filter.Selects.Contains(DirectSalesOrderPromotionSelect.Note) ? q.Note : default(string),
                Factor = filter.Selects.Contains(DirectSalesOrderPromotionSelect.Factor) ? q.Factor : default(long?),
                DirectSalesOrder = filter.Selects.Contains(DirectSalesOrderPromotionSelect.DirectSalesOrder) && q.DirectSalesOrder != null ? new DirectSalesOrder
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
                Item = filter.Selects.Contains(DirectSalesOrderPromotionSelect.Item) && q.Item != null ? new Item
                {
                    Id = q.Item.Id,
                    ProductId = q.Item.ProductId,
                    Code = q.Item.Code,
                    Name = q.Item.Name,
                    ScanCode = q.Item.ScanCode,
                    SalePrice = q.Item.SalePrice,
                    RetailPrice = q.Item.RetailPrice,
                    StatusId = q.Item.StatusId,
                } : null,
                PrimaryUnitOfMeasure = filter.Selects.Contains(DirectSalesOrderPromotionSelect.PrimaryUnitOfMeasure) && q.PrimaryUnitOfMeasure != null ? new UnitOfMeasure
                {
                    Id = q.PrimaryUnitOfMeasure.Id,
                    Code = q.PrimaryUnitOfMeasure.Code,
                    Name = q.PrimaryUnitOfMeasure.Name,
                    Description = q.PrimaryUnitOfMeasure.Description,
                    StatusId = q.PrimaryUnitOfMeasure.StatusId,
                } : null,
                UnitOfMeasure = filter.Selects.Contains(DirectSalesOrderPromotionSelect.UnitOfMeasure) && q.UnitOfMeasure != null ? new UnitOfMeasure
                {
                    Id = q.UnitOfMeasure.Id,
                    Code = q.UnitOfMeasure.Code,
                    Name = q.UnitOfMeasure.Name,
                    Description = q.UnitOfMeasure.Description,
                    StatusId = q.UnitOfMeasure.StatusId,
                } : null,
            }).ToListWithNoLockAsync();
            return DirectSalesOrderPromotions;
        }

        public async Task<int> Count(DirectSalesOrderPromotionFilter filter)
        {
            IQueryable<DirectSalesOrderPromotionDAO> DirectSalesOrderPromotions = DataContext.DirectSalesOrderPromotion.AsNoTracking();
            DirectSalesOrderPromotions = DynamicFilter(DirectSalesOrderPromotions, filter);
            DirectSalesOrderPromotions = OrFilter(DirectSalesOrderPromotions, filter);
            return await DirectSalesOrderPromotions.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(DirectSalesOrderPromotionFilter filter)
        {
            IQueryable<DirectSalesOrderPromotionDAO> DirectSalesOrderPromotions = DataContext.DirectSalesOrderPromotion.AsNoTracking();
            DirectSalesOrderPromotions = DynamicFilter(DirectSalesOrderPromotions, filter);
            return await DirectSalesOrderPromotions.CountWithNoLockAsync();
        }

        public async Task<List<DirectSalesOrderPromotion>> List(DirectSalesOrderPromotionFilter filter)
        {
            if (filter == null) return new List<DirectSalesOrderPromotion>();
            IQueryable<DirectSalesOrderPromotionDAO> DirectSalesOrderPromotionDAOs = DataContext.DirectSalesOrderPromotion.AsNoTracking();
            DirectSalesOrderPromotionDAOs = DynamicFilter(DirectSalesOrderPromotionDAOs, filter);
            DirectSalesOrderPromotionDAOs = OrFilter(DirectSalesOrderPromotionDAOs, filter);
            DirectSalesOrderPromotionDAOs = DynamicOrder(DirectSalesOrderPromotionDAOs, filter);
            List<DirectSalesOrderPromotion> DirectSalesOrderPromotions = await DynamicSelect(DirectSalesOrderPromotionDAOs, filter);
            return DirectSalesOrderPromotions;
        }

        public async Task<DirectSalesOrderPromotion> Get(long Id)
        {
            DirectSalesOrderPromotion DirectSalesOrderPromotion = await DataContext.DirectSalesOrderPromotion.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new DirectSalesOrderPromotion()
            {
                Id = x.Id,
                DirectSalesOrderId = x.DirectSalesOrderId,
                ItemId = x.ItemId,
                UnitOfMeasureId = x.UnitOfMeasureId,
                Quantity = x.Quantity,
                PrimaryUnitOfMeasureId = x.PrimaryUnitOfMeasureId,
                RequestedQuantity = x.RequestedQuantity,
                Note = x.Note,
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
                Item = x.Item == null ? null : new Item
                {
                    Id = x.Item.Id,
                    ProductId = x.Item.ProductId,
                    Code = x.Item.Code,
                    Name = x.Item.Name,
                    ScanCode = x.Item.ScanCode,
                    SalePrice = x.Item.SalePrice,
                    RetailPrice = x.Item.RetailPrice,
                    StatusId = x.Item.StatusId,
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

            if (DirectSalesOrderPromotion == null)
                return null;

            return DirectSalesOrderPromotion;
        }
        public async Task<bool> Create(DirectSalesOrderPromotion DirectSalesOrderPromotion)
        {
            DirectSalesOrderPromotionDAO DirectSalesOrderPromotionDAO = new DirectSalesOrderPromotionDAO();
            DirectSalesOrderPromotionDAO.Id = DirectSalesOrderPromotion.Id;
            DirectSalesOrderPromotionDAO.DirectSalesOrderId = DirectSalesOrderPromotion.DirectSalesOrderId;
            DirectSalesOrderPromotionDAO.ItemId = DirectSalesOrderPromotion.ItemId;
            DirectSalesOrderPromotionDAO.UnitOfMeasureId = DirectSalesOrderPromotion.UnitOfMeasureId;
            DirectSalesOrderPromotionDAO.Quantity = DirectSalesOrderPromotion.Quantity;
            DirectSalesOrderPromotionDAO.PrimaryUnitOfMeasureId = DirectSalesOrderPromotion.PrimaryUnitOfMeasureId;
            DirectSalesOrderPromotionDAO.RequestedQuantity = DirectSalesOrderPromotion.RequestedQuantity;
            DirectSalesOrderPromotionDAO.Note = DirectSalesOrderPromotion.Note;
            DirectSalesOrderPromotionDAO.Factor = DirectSalesOrderPromotion.Factor;
            DataContext.DirectSalesOrderPromotion.Add(DirectSalesOrderPromotionDAO);
            await DataContext.SaveChangesAsync();
            DirectSalesOrderPromotion.Id = DirectSalesOrderPromotionDAO.Id;
            await SaveReference(DirectSalesOrderPromotion);
            return true;
        }

        public async Task<bool> Update(DirectSalesOrderPromotion DirectSalesOrderPromotion)
        {
            DirectSalesOrderPromotionDAO DirectSalesOrderPromotionDAO = DataContext.DirectSalesOrderPromotion.Where(x => x.Id == DirectSalesOrderPromotion.Id).FirstOrDefault();
            if (DirectSalesOrderPromotionDAO == null)
                return false;
            DirectSalesOrderPromotionDAO.Id = DirectSalesOrderPromotion.Id;
            DirectSalesOrderPromotionDAO.DirectSalesOrderId = DirectSalesOrderPromotion.DirectSalesOrderId;
            DirectSalesOrderPromotionDAO.ItemId = DirectSalesOrderPromotion.ItemId;
            DirectSalesOrderPromotionDAO.UnitOfMeasureId = DirectSalesOrderPromotion.UnitOfMeasureId;
            DirectSalesOrderPromotionDAO.Quantity = DirectSalesOrderPromotion.Quantity;
            DirectSalesOrderPromotionDAO.PrimaryUnitOfMeasureId = DirectSalesOrderPromotion.PrimaryUnitOfMeasureId;
            DirectSalesOrderPromotionDAO.RequestedQuantity = DirectSalesOrderPromotion.RequestedQuantity;
            DirectSalesOrderPromotionDAO.Note = DirectSalesOrderPromotion.Note;
            DirectSalesOrderPromotionDAO.Factor = DirectSalesOrderPromotion.Factor;
            await DataContext.SaveChangesAsync();
            await SaveReference(DirectSalesOrderPromotion);
            return true;
        }

        public async Task<bool> Delete(DirectSalesOrderPromotion DirectSalesOrderPromotion)
        {
            await DataContext.DirectSalesOrderPromotion.Where(x => x.Id == DirectSalesOrderPromotion.Id).DeleteFromQueryAsync();
            return true;
        }

        public async Task<bool> BulkMerge(List<DirectSalesOrderPromotion> DirectSalesOrderPromotions)
        {
            List<DirectSalesOrderPromotionDAO> DirectSalesOrderPromotionDAOs = new List<DirectSalesOrderPromotionDAO>();
            foreach (DirectSalesOrderPromotion DirectSalesOrderPromotion in DirectSalesOrderPromotions)
            {
                DirectSalesOrderPromotionDAO DirectSalesOrderPromotionDAO = new DirectSalesOrderPromotionDAO();
                DirectSalesOrderPromotionDAO.Id = DirectSalesOrderPromotion.Id;
                DirectSalesOrderPromotionDAO.DirectSalesOrderId = DirectSalesOrderPromotion.DirectSalesOrderId;
                DirectSalesOrderPromotionDAO.ItemId = DirectSalesOrderPromotion.ItemId;
                DirectSalesOrderPromotionDAO.UnitOfMeasureId = DirectSalesOrderPromotion.UnitOfMeasureId;
                DirectSalesOrderPromotionDAO.Quantity = DirectSalesOrderPromotion.Quantity;
                DirectSalesOrderPromotionDAO.PrimaryUnitOfMeasureId = DirectSalesOrderPromotion.PrimaryUnitOfMeasureId;
                DirectSalesOrderPromotionDAO.RequestedQuantity = DirectSalesOrderPromotion.RequestedQuantity;
                DirectSalesOrderPromotionDAO.Note = DirectSalesOrderPromotion.Note;
                DirectSalesOrderPromotionDAO.Factor = DirectSalesOrderPromotion.Factor;
                DirectSalesOrderPromotionDAOs.Add(DirectSalesOrderPromotionDAO);
            }
            await DataContext.BulkMergeAsync(DirectSalesOrderPromotionDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<DirectSalesOrderPromotion> DirectSalesOrderPromotions)
        {
            List<long> Ids = DirectSalesOrderPromotions.Select(x => x.Id).ToList();
            await DataContext.DirectSalesOrderPromotion
                .Where(x => Ids.Contains(x.Id))
                .DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(DirectSalesOrderPromotion DirectSalesOrderPromotion)
        {
        }

    }
}
