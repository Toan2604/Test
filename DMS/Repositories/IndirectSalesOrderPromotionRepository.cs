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
    public interface IIndirectSalesOrderPromotionRepository
    {
        Task<int> Count(IndirectSalesOrderPromotionFilter IndirectSalesOrderPromotionFilter);
        Task<int> CountAll(IndirectSalesOrderPromotionFilter IndirectSalesOrderPromotionFilter);
        Task<List<IndirectSalesOrderPromotion>> List(IndirectSalesOrderPromotionFilter IndirectSalesOrderPromotionFilter);
        Task<IndirectSalesOrderPromotion> Get(long Id);
        Task<bool> Create(IndirectSalesOrderPromotion IndirectSalesOrderPromotion);
        Task<bool> Update(IndirectSalesOrderPromotion IndirectSalesOrderPromotion);
        Task<bool> Delete(IndirectSalesOrderPromotion IndirectSalesOrderPromotion);
        Task<bool> BulkMerge(List<IndirectSalesOrderPromotion> IndirectSalesOrderPromotions);
        Task<bool> BulkDelete(List<IndirectSalesOrderPromotion> IndirectSalesOrderPromotions);
    }
    public class IndirectSalesOrderPromotionRepository : IIndirectSalesOrderPromotionRepository
    {
        private DataContext DataContext;
        public IndirectSalesOrderPromotionRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<IndirectSalesOrderPromotionDAO> DynamicFilter(IQueryable<IndirectSalesOrderPromotionDAO> query, IndirectSalesOrderPromotionFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.IndirectSalesOrderId, filter.IndirectSalesOrderId);
            query = query.Where(q => q.Item.ProductId, filter.ProductId);
            query = query.Where(q => q.ItemId, filter.ItemId);
            query = query.Where(q => q.UnitOfMeasureId, filter.UnitOfMeasureId);
            query = query.Where(q => q.Quantity, filter.Quantity);
            query = query.Where(q => q.PrimaryUnitOfMeasureId, filter.PrimaryUnitOfMeasureId);
            query = query.Where(q => q.RequestedQuantity, filter.RequestedQuantity);
            query = query.Where(q => q.Note, filter.Note);
            return query;
        }

        private IQueryable<IndirectSalesOrderPromotionDAO> OrFilter(IQueryable<IndirectSalesOrderPromotionDAO> query, IndirectSalesOrderPromotionFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<IndirectSalesOrderPromotionDAO> initQuery = query.Where(q => false);
            foreach (IndirectSalesOrderPromotionFilter IndirectSalesOrderPromotionFilter in filter.OrFilter)
            {
                IQueryable<IndirectSalesOrderPromotionDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, IndirectSalesOrderPromotionFilter.Id);
                queryable = queryable.Where(q => q.IndirectSalesOrderId, IndirectSalesOrderPromotionFilter.IndirectSalesOrderId);
                queryable = queryable.Where(q => q.ItemId, IndirectSalesOrderPromotionFilter.ItemId);
                queryable = queryable.Where(q => q.UnitOfMeasureId, IndirectSalesOrderPromotionFilter.UnitOfMeasureId);
                queryable = queryable.Where(q => q.Quantity, IndirectSalesOrderPromotionFilter.Quantity);
                queryable = queryable.Where(q => q.PrimaryUnitOfMeasureId, IndirectSalesOrderPromotionFilter.PrimaryUnitOfMeasureId);
                queryable = queryable.Where(q => q.RequestedQuantity, IndirectSalesOrderPromotionFilter.RequestedQuantity);
                queryable = queryable.Where(q => q.Note, IndirectSalesOrderPromotionFilter.Note);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<IndirectSalesOrderPromotionDAO> DynamicOrder(IQueryable<IndirectSalesOrderPromotionDAO> query, IndirectSalesOrderPromotionFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case IndirectSalesOrderPromotionOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case IndirectSalesOrderPromotionOrder.IndirectSalesOrder:
                            query = query.OrderBy(q => q.IndirectSalesOrderId);
                            break;
                        case IndirectSalesOrderPromotionOrder.Item:
                            query = query.OrderBy(q => q.ItemId);
                            break;
                        case IndirectSalesOrderPromotionOrder.UnitOfMeasure:
                            query = query.OrderBy(q => q.UnitOfMeasureId);
                            break;
                        case IndirectSalesOrderPromotionOrder.Quantity:
                            query = query.OrderBy(q => q.Quantity);
                            break;
                        case IndirectSalesOrderPromotionOrder.PrimaryUnitOfMeasure:
                            query = query.OrderBy(q => q.PrimaryUnitOfMeasureId);
                            break;
                        case IndirectSalesOrderPromotionOrder.RequestedQuantity:
                            query = query.OrderBy(q => q.RequestedQuantity);
                            break;
                        case IndirectSalesOrderPromotionOrder.Note:
                            query = query.OrderBy(q => q.Note);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case IndirectSalesOrderPromotionOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case IndirectSalesOrderPromotionOrder.IndirectSalesOrder:
                            query = query.OrderByDescending(q => q.IndirectSalesOrderId);
                            break;
                        case IndirectSalesOrderPromotionOrder.Item:
                            query = query.OrderByDescending(q => q.ItemId);
                            break;
                        case IndirectSalesOrderPromotionOrder.UnitOfMeasure:
                            query = query.OrderByDescending(q => q.UnitOfMeasureId);
                            break;
                        case IndirectSalesOrderPromotionOrder.Quantity:
                            query = query.OrderByDescending(q => q.Quantity);
                            break;
                        case IndirectSalesOrderPromotionOrder.PrimaryUnitOfMeasure:
                            query = query.OrderByDescending(q => q.PrimaryUnitOfMeasureId);
                            break;
                        case IndirectSalesOrderPromotionOrder.RequestedQuantity:
                            query = query.OrderByDescending(q => q.RequestedQuantity);
                            break;
                        case IndirectSalesOrderPromotionOrder.Note:
                            query = query.OrderByDescending(q => q.Note);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<IndirectSalesOrderPromotion>> DynamicSelect(IQueryable<IndirectSalesOrderPromotionDAO> query, IndirectSalesOrderPromotionFilter filter)
        {
            List<IndirectSalesOrderPromotion> IndirectSalesOrderPromotions = await query.Select(q => new IndirectSalesOrderPromotion()
            {
                Id = filter.Selects.Contains(IndirectSalesOrderPromotionSelect.Id) ? q.Id : default(long),
                IndirectSalesOrderId = filter.Selects.Contains(IndirectSalesOrderPromotionSelect.IndirectSalesOrder) ? q.IndirectSalesOrderId : default(long),
                ItemId = filter.Selects.Contains(IndirectSalesOrderPromotionSelect.Item) ? q.ItemId : default(long),
                UnitOfMeasureId = filter.Selects.Contains(IndirectSalesOrderPromotionSelect.UnitOfMeasure) ? q.UnitOfMeasureId : default(long),
                Quantity = filter.Selects.Contains(IndirectSalesOrderPromotionSelect.Quantity) ? q.Quantity : default(long),
                PrimaryUnitOfMeasureId = filter.Selects.Contains(IndirectSalesOrderPromotionSelect.PrimaryUnitOfMeasure) ? q.PrimaryUnitOfMeasureId : default(long),
                RequestedQuantity = filter.Selects.Contains(IndirectSalesOrderPromotionSelect.RequestedQuantity) ? q.RequestedQuantity : default(long),
                Note = filter.Selects.Contains(IndirectSalesOrderPromotionSelect.Note) ? q.Note : default(string),
                Factor = filter.Selects.Contains(IndirectSalesOrderPromotionSelect.Factor) ? q.Factor : default(long?),
                IndirectSalesOrder = filter.Selects.Contains(IndirectSalesOrderPromotionSelect.IndirectSalesOrder) && q.IndirectSalesOrder != null ? new IndirectSalesOrder
                {
                    Id = q.IndirectSalesOrder.Id,
                    Code = q.IndirectSalesOrder.Code,
                    BuyerStoreId = q.IndirectSalesOrder.BuyerStoreId,
                    PhoneNumber = q.IndirectSalesOrder.PhoneNumber,
                    StoreAddress = q.IndirectSalesOrder.StoreAddress,
                    DeliveryAddress = q.IndirectSalesOrder.DeliveryAddress,
                    SellerStoreId = q.IndirectSalesOrder.SellerStoreId,
                    SaleEmployeeId = q.IndirectSalesOrder.SaleEmployeeId,
                    OrderDate = q.IndirectSalesOrder.OrderDate,
                    DeliveryDate = q.IndirectSalesOrder.DeliveryDate,
                    EditedPriceStatusId = q.IndirectSalesOrder.EditedPriceStatusId,
                    Note = q.IndirectSalesOrder.Note,
                    SubTotal = q.IndirectSalesOrder.SubTotal,
                    GeneralDiscountPercentage = q.IndirectSalesOrder.GeneralDiscountPercentage,
                    GeneralDiscountAmount = q.IndirectSalesOrder.GeneralDiscountAmount,
                    Total = q.IndirectSalesOrder.Total,
                } : null,
                Item = filter.Selects.Contains(IndirectSalesOrderPromotionSelect.Item) && q.Item != null ? new Item
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
                PrimaryUnitOfMeasure = filter.Selects.Contains(IndirectSalesOrderPromotionSelect.PrimaryUnitOfMeasure) && q.PrimaryUnitOfMeasure != null ? new UnitOfMeasure
                {
                    Id = q.PrimaryUnitOfMeasure.Id,
                    Code = q.PrimaryUnitOfMeasure.Code,
                    Name = q.PrimaryUnitOfMeasure.Name,
                    Description = q.PrimaryUnitOfMeasure.Description,
                    StatusId = q.PrimaryUnitOfMeasure.StatusId,
                } : null,
                UnitOfMeasure = filter.Selects.Contains(IndirectSalesOrderPromotionSelect.UnitOfMeasure) && q.UnitOfMeasure != null ? new UnitOfMeasure
                {
                    Id = q.UnitOfMeasure.Id,
                    Code = q.UnitOfMeasure.Code,
                    Name = q.UnitOfMeasure.Name,
                    Description = q.UnitOfMeasure.Description,
                    StatusId = q.UnitOfMeasure.StatusId,
                } : null,
            }).ToListWithNoLockAsync();
            return IndirectSalesOrderPromotions;
        }

        public async Task<int> Count(IndirectSalesOrderPromotionFilter filter)
        {
            IQueryable<IndirectSalesOrderPromotionDAO> IndirectSalesOrderPromotions = DataContext.IndirectSalesOrderPromotion.AsNoTracking();
            IndirectSalesOrderPromotions = DynamicFilter(IndirectSalesOrderPromotions, filter);
            IndirectSalesOrderPromotions = OrFilter(IndirectSalesOrderPromotions, filter);
            return await IndirectSalesOrderPromotions.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(IndirectSalesOrderPromotionFilter filter)
        {
            IQueryable<IndirectSalesOrderPromotionDAO> IndirectSalesOrderPromotions = DataContext.IndirectSalesOrderPromotion.AsNoTracking();
            IndirectSalesOrderPromotions = DynamicFilter(IndirectSalesOrderPromotions, filter);
            return await IndirectSalesOrderPromotions.CountWithNoLockAsync();
        }

        public async Task<List<IndirectSalesOrderPromotion>> List(IndirectSalesOrderPromotionFilter filter)
        {
            if (filter == null) return new List<IndirectSalesOrderPromotion>();
            IQueryable<IndirectSalesOrderPromotionDAO> IndirectSalesOrderPromotionDAOs = DataContext.IndirectSalesOrderPromotion.AsNoTracking();
            IndirectSalesOrderPromotionDAOs = DynamicFilter(IndirectSalesOrderPromotionDAOs, filter);
            IndirectSalesOrderPromotionDAOs = OrFilter(IndirectSalesOrderPromotionDAOs, filter);
            IndirectSalesOrderPromotionDAOs = DynamicOrder(IndirectSalesOrderPromotionDAOs, filter);
            List<IndirectSalesOrderPromotion> IndirectSalesOrderPromotions = await DynamicSelect(IndirectSalesOrderPromotionDAOs, filter);
            return IndirectSalesOrderPromotions;
        }

        public async Task<IndirectSalesOrderPromotion> Get(long Id)
        {
            IndirectSalesOrderPromotion IndirectSalesOrderPromotion = await DataContext.IndirectSalesOrderPromotion.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new IndirectSalesOrderPromotion()
            {
                Id = x.Id,
                IndirectSalesOrderId = x.IndirectSalesOrderId,
                ItemId = x.ItemId,
                UnitOfMeasureId = x.UnitOfMeasureId,
                Quantity = x.Quantity,
                PrimaryUnitOfMeasureId = x.PrimaryUnitOfMeasureId,
                RequestedQuantity = x.RequestedQuantity,
                Note = x.Note,
                Factor = x.Factor,
                IndirectSalesOrder = x.IndirectSalesOrder == null ? null : new IndirectSalesOrder
                {
                    Id = x.IndirectSalesOrder.Id,
                    Code = x.IndirectSalesOrder.Code,
                    BuyerStoreId = x.IndirectSalesOrder.BuyerStoreId,
                    PhoneNumber = x.IndirectSalesOrder.PhoneNumber,
                    StoreAddress = x.IndirectSalesOrder.StoreAddress,
                    DeliveryAddress = x.IndirectSalesOrder.DeliveryAddress,
                    SellerStoreId = x.IndirectSalesOrder.SellerStoreId,
                    SaleEmployeeId = x.IndirectSalesOrder.SaleEmployeeId,
                    OrderDate = x.IndirectSalesOrder.OrderDate,
                    DeliveryDate = x.IndirectSalesOrder.DeliveryDate,
                    EditedPriceStatusId = x.IndirectSalesOrder.EditedPriceStatusId,
                    Note = x.IndirectSalesOrder.Note,
                    SubTotal = x.IndirectSalesOrder.SubTotal,
                    GeneralDiscountPercentage = x.IndirectSalesOrder.GeneralDiscountPercentage,
                    GeneralDiscountAmount = x.IndirectSalesOrder.GeneralDiscountAmount,
                    Total = x.IndirectSalesOrder.Total,
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

            if (IndirectSalesOrderPromotion == null)
                return null;

            return IndirectSalesOrderPromotion;
        }
        public async Task<bool> Create(IndirectSalesOrderPromotion IndirectSalesOrderPromotion)
        {
            IndirectSalesOrderPromotionDAO IndirectSalesOrderPromotionDAO = new IndirectSalesOrderPromotionDAO();
            IndirectSalesOrderPromotionDAO.Id = IndirectSalesOrderPromotion.Id;
            IndirectSalesOrderPromotionDAO.IndirectSalesOrderId = IndirectSalesOrderPromotion.IndirectSalesOrderId;
            IndirectSalesOrderPromotionDAO.ItemId = IndirectSalesOrderPromotion.ItemId;
            IndirectSalesOrderPromotionDAO.UnitOfMeasureId = IndirectSalesOrderPromotion.UnitOfMeasureId;
            IndirectSalesOrderPromotionDAO.Quantity = IndirectSalesOrderPromotion.Quantity;
            IndirectSalesOrderPromotionDAO.PrimaryUnitOfMeasureId = IndirectSalesOrderPromotion.PrimaryUnitOfMeasureId;
            IndirectSalesOrderPromotionDAO.RequestedQuantity = IndirectSalesOrderPromotion.RequestedQuantity;
            IndirectSalesOrderPromotionDAO.Note = IndirectSalesOrderPromotion.Note;
            IndirectSalesOrderPromotionDAO.Factor = IndirectSalesOrderPromotion.Factor;
            DataContext.IndirectSalesOrderPromotion.Add(IndirectSalesOrderPromotionDAO);
            await DataContext.SaveChangesAsync();
            IndirectSalesOrderPromotion.Id = IndirectSalesOrderPromotionDAO.Id;
            await SaveReference(IndirectSalesOrderPromotion);
            return true;
        }

        public async Task<bool> Update(IndirectSalesOrderPromotion IndirectSalesOrderPromotion)
        {
            IndirectSalesOrderPromotionDAO IndirectSalesOrderPromotionDAO = DataContext.IndirectSalesOrderPromotion.Where(x => x.Id == IndirectSalesOrderPromotion.Id).FirstOrDefault();
            if (IndirectSalesOrderPromotionDAO == null)
                return false;
            IndirectSalesOrderPromotionDAO.Id = IndirectSalesOrderPromotion.Id;
            IndirectSalesOrderPromotionDAO.IndirectSalesOrderId = IndirectSalesOrderPromotion.IndirectSalesOrderId;
            IndirectSalesOrderPromotionDAO.ItemId = IndirectSalesOrderPromotion.ItemId;
            IndirectSalesOrderPromotionDAO.UnitOfMeasureId = IndirectSalesOrderPromotion.UnitOfMeasureId;
            IndirectSalesOrderPromotionDAO.Quantity = IndirectSalesOrderPromotion.Quantity;
            IndirectSalesOrderPromotionDAO.PrimaryUnitOfMeasureId = IndirectSalesOrderPromotion.PrimaryUnitOfMeasureId;
            IndirectSalesOrderPromotionDAO.RequestedQuantity = IndirectSalesOrderPromotion.RequestedQuantity;
            IndirectSalesOrderPromotionDAO.Note = IndirectSalesOrderPromotion.Note;
            IndirectSalesOrderPromotionDAO.Factor = IndirectSalesOrderPromotion.Factor;
            await DataContext.SaveChangesAsync();
            await SaveReference(IndirectSalesOrderPromotion);
            return true;
        }

        public async Task<bool> Delete(IndirectSalesOrderPromotion IndirectSalesOrderPromotion)
        {
            await DataContext.IndirectSalesOrderPromotion.Where(x => x.Id == IndirectSalesOrderPromotion.Id).DeleteFromQueryAsync();
            return true;
        }

        public async Task<bool> BulkMerge(List<IndirectSalesOrderPromotion> IndirectSalesOrderPromotions)
        {
            List<IndirectSalesOrderPromotionDAO> IndirectSalesOrderPromotionDAOs = new List<IndirectSalesOrderPromotionDAO>();
            foreach (IndirectSalesOrderPromotion IndirectSalesOrderPromotion in IndirectSalesOrderPromotions)
            {
                IndirectSalesOrderPromotionDAO IndirectSalesOrderPromotionDAO = new IndirectSalesOrderPromotionDAO();
                IndirectSalesOrderPromotionDAO.Id = IndirectSalesOrderPromotion.Id;
                IndirectSalesOrderPromotionDAO.IndirectSalesOrderId = IndirectSalesOrderPromotion.IndirectSalesOrderId;
                IndirectSalesOrderPromotionDAO.ItemId = IndirectSalesOrderPromotion.ItemId;
                IndirectSalesOrderPromotionDAO.UnitOfMeasureId = IndirectSalesOrderPromotion.UnitOfMeasureId;
                IndirectSalesOrderPromotionDAO.Quantity = IndirectSalesOrderPromotion.Quantity;
                IndirectSalesOrderPromotionDAO.PrimaryUnitOfMeasureId = IndirectSalesOrderPromotion.PrimaryUnitOfMeasureId;
                IndirectSalesOrderPromotionDAO.RequestedQuantity = IndirectSalesOrderPromotion.RequestedQuantity;
                IndirectSalesOrderPromotionDAO.Note = IndirectSalesOrderPromotion.Note;
                IndirectSalesOrderPromotionDAO.Factor = IndirectSalesOrderPromotion.Factor;
                IndirectSalesOrderPromotionDAOs.Add(IndirectSalesOrderPromotionDAO);
            }
            await DataContext.BulkMergeAsync(IndirectSalesOrderPromotionDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<IndirectSalesOrderPromotion> IndirectSalesOrderPromotions)
        {
            List<long> Ids = IndirectSalesOrderPromotions.Select(x => x.Id).ToList();
            await DataContext.IndirectSalesOrderPromotion
                .Where(x => Ids.Contains(x.Id))
                .DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(IndirectSalesOrderPromotion IndirectSalesOrderPromotion)
        {
        }

    }
}
