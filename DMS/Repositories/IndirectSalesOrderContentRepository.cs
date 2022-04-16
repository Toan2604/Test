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
    public interface IIndirectSalesOrderContentRepository
    {
        Task<int> Count(IndirectSalesOrderContentFilter IndirectSalesOrderContentFilter);
        Task<int> CountAll(IndirectSalesOrderContentFilter IndirectSalesOrderContentFilter);
        Task<List<IndirectSalesOrderContent>> List(IndirectSalesOrderContentFilter IndirectSalesOrderContentFilter);
        Task<IndirectSalesOrderContent> Get(long Id);
        Task<bool> Create(IndirectSalesOrderContent IndirectSalesOrderContent);
        Task<bool> Update(IndirectSalesOrderContent IndirectSalesOrderContent);
        Task<bool> Delete(IndirectSalesOrderContent IndirectSalesOrderContent);
        Task<bool> BulkMerge(List<IndirectSalesOrderContent> IndirectSalesOrderContents);
        Task<bool> BulkDelete(List<IndirectSalesOrderContent> IndirectSalesOrderContents);
    }
    public class IndirectSalesOrderContentRepository : IIndirectSalesOrderContentRepository
    {
        private DataContext DataContext;
        public IndirectSalesOrderContentRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<IndirectSalesOrderContentDAO> DynamicFilter(IQueryable<IndirectSalesOrderContentDAO> query, IndirectSalesOrderContentFilter filter)
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

        private IQueryable<IndirectSalesOrderContentDAO> OrFilter(IQueryable<IndirectSalesOrderContentDAO> query, IndirectSalesOrderContentFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<IndirectSalesOrderContentDAO> initQuery = query.Where(q => false);
            foreach (IndirectSalesOrderContentFilter IndirectSalesOrderContentFilter in filter.OrFilter)
            {
                IQueryable<IndirectSalesOrderContentDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, IndirectSalesOrderContentFilter.Id);
                queryable = queryable.Where(q => q.IndirectSalesOrderId, IndirectSalesOrderContentFilter.IndirectSalesOrderId);
                queryable = queryable.Where(q => q.ItemId, IndirectSalesOrderContentFilter.ItemId);
                queryable = queryable.Where(q => q.UnitOfMeasureId, IndirectSalesOrderContentFilter.UnitOfMeasureId);
                queryable = queryable.Where(q => q.Quantity, IndirectSalesOrderContentFilter.Quantity);
                queryable = queryable.Where(q => q.PrimaryUnitOfMeasureId, IndirectSalesOrderContentFilter.PrimaryUnitOfMeasureId);
                queryable = queryable.Where(q => q.RequestedQuantity, IndirectSalesOrderContentFilter.RequestedQuantity);
                queryable = queryable.Where(q => q.PrimaryPrice, IndirectSalesOrderContentFilter.PrimaryPrice);
                queryable = queryable.Where(q => q.SalePrice, IndirectSalesOrderContentFilter.SalePrice);
                queryable = queryable.Where(q => q.DiscountPercentage, IndirectSalesOrderContentFilter.DiscountPercentage);
                queryable = queryable.Where(q => q.DiscountAmount, IndirectSalesOrderContentFilter.DiscountAmount);
                queryable = queryable.Where(q => q.GeneralDiscountPercentage, IndirectSalesOrderContentFilter.GeneralDiscountPercentage);
                queryable = queryable.Where(q => q.GeneralDiscountAmount, IndirectSalesOrderContentFilter.GeneralDiscountAmount);
                queryable = queryable.Where(q => q.Amount, IndirectSalesOrderContentFilter.Amount);
                queryable = queryable.Where(q => q.TaxPercentage, IndirectSalesOrderContentFilter.TaxPercentage);
                queryable = queryable.Where(q => q.TaxAmount, IndirectSalesOrderContentFilter.TaxAmount);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<IndirectSalesOrderContentDAO> DynamicOrder(IQueryable<IndirectSalesOrderContentDAO> query, IndirectSalesOrderContentFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case IndirectSalesOrderContentOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case IndirectSalesOrderContentOrder.IndirectSalesOrder:
                            query = query.OrderBy(q => q.IndirectSalesOrderId);
                            break;
                        case IndirectSalesOrderContentOrder.Item:
                            query = query.OrderBy(q => q.ItemId);
                            break;
                        case IndirectSalesOrderContentOrder.UnitOfMeasure:
                            query = query.OrderBy(q => q.UnitOfMeasureId);
                            break;
                        case IndirectSalesOrderContentOrder.Quantity:
                            query = query.OrderBy(q => q.Quantity);
                            break;
                        case IndirectSalesOrderContentOrder.PrimaryUnitOfMeasure:
                            query = query.OrderBy(q => q.PrimaryUnitOfMeasureId);
                            break;
                        case IndirectSalesOrderContentOrder.RequestedQuantity:
                            query = query.OrderBy(q => q.RequestedQuantity);
                            break;
                        case IndirectSalesOrderContentOrder.PrimaryPrice:
                            query = query.OrderBy(q => q.PrimaryPrice);
                            break;
                        case IndirectSalesOrderContentOrder.SalePrice:
                            query = query.OrderBy(q => q.SalePrice);
                            break;
                        case IndirectSalesOrderContentOrder.DiscountPercentage:
                            query = query.OrderBy(q => q.DiscountPercentage);
                            break;
                        case IndirectSalesOrderContentOrder.DiscountAmount:
                            query = query.OrderBy(q => q.DiscountAmount);
                            break;
                        case IndirectSalesOrderContentOrder.GeneralDiscountPercentage:
                            query = query.OrderBy(q => q.GeneralDiscountPercentage);
                            break;
                        case IndirectSalesOrderContentOrder.GeneralDiscountAmount:
                            query = query.OrderBy(q => q.GeneralDiscountAmount);
                            break;
                        case IndirectSalesOrderContentOrder.Amount:
                            query = query.OrderBy(q => q.Amount);
                            break;
                        case IndirectSalesOrderContentOrder.TaxPercentage:
                            query = query.OrderBy(q => q.TaxPercentage);
                            break;
                        case IndirectSalesOrderContentOrder.TaxAmount:
                            query = query.OrderBy(q => q.TaxAmount);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case IndirectSalesOrderContentOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case IndirectSalesOrderContentOrder.IndirectSalesOrder:
                            query = query.OrderByDescending(q => q.IndirectSalesOrderId);
                            break;
                        case IndirectSalesOrderContentOrder.Item:
                            query = query.OrderByDescending(q => q.ItemId);
                            break;
                        case IndirectSalesOrderContentOrder.UnitOfMeasure:
                            query = query.OrderByDescending(q => q.UnitOfMeasureId);
                            break;
                        case IndirectSalesOrderContentOrder.Quantity:
                            query = query.OrderByDescending(q => q.Quantity);
                            break;
                        case IndirectSalesOrderContentOrder.PrimaryUnitOfMeasure:
                            query = query.OrderByDescending(q => q.PrimaryUnitOfMeasureId);
                            break;
                        case IndirectSalesOrderContentOrder.RequestedQuantity:
                            query = query.OrderByDescending(q => q.RequestedQuantity);
                            break;
                        case IndirectSalesOrderContentOrder.PrimaryPrice:
                            query = query.OrderByDescending(q => q.PrimaryPrice);
                            break;
                        case IndirectSalesOrderContentOrder.SalePrice:
                            query = query.OrderByDescending(q => q.SalePrice);
                            break;
                        case IndirectSalesOrderContentOrder.DiscountPercentage:
                            query = query.OrderByDescending(q => q.DiscountPercentage);
                            break;
                        case IndirectSalesOrderContentOrder.DiscountAmount:
                            query = query.OrderByDescending(q => q.DiscountAmount);
                            break;
                        case IndirectSalesOrderContentOrder.GeneralDiscountPercentage:
                            query = query.OrderByDescending(q => q.GeneralDiscountPercentage);
                            break;
                        case IndirectSalesOrderContentOrder.GeneralDiscountAmount:
                            query = query.OrderByDescending(q => q.GeneralDiscountAmount);
                            break;
                        case IndirectSalesOrderContentOrder.Amount:
                            query = query.OrderByDescending(q => q.Amount);
                            break;
                        case IndirectSalesOrderContentOrder.TaxPercentage:
                            query = query.OrderByDescending(q => q.TaxPercentage);
                            break;
                        case IndirectSalesOrderContentOrder.TaxAmount:
                            query = query.OrderByDescending(q => q.TaxAmount);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<IndirectSalesOrderContent>> DynamicSelect(IQueryable<IndirectSalesOrderContentDAO> query, IndirectSalesOrderContentFilter filter)
        {
            List<IndirectSalesOrderContent> IndirectSalesOrderContents = await query.Select(q => new IndirectSalesOrderContent()
            {
                Id = filter.Selects.Contains(IndirectSalesOrderContentSelect.Id) ? q.Id : default(long),
                IndirectSalesOrderId = filter.Selects.Contains(IndirectSalesOrderContentSelect.IndirectSalesOrder) ? q.IndirectSalesOrderId : default(long),
                ItemId = filter.Selects.Contains(IndirectSalesOrderContentSelect.Item) ? q.ItemId : default(long),
                UnitOfMeasureId = filter.Selects.Contains(IndirectSalesOrderContentSelect.UnitOfMeasure) ? q.UnitOfMeasureId : default(long),
                Quantity = filter.Selects.Contains(IndirectSalesOrderContentSelect.Quantity) ? q.Quantity : default(long),
                PrimaryUnitOfMeasureId = filter.Selects.Contains(IndirectSalesOrderContentSelect.PrimaryUnitOfMeasure) ? q.PrimaryUnitOfMeasureId : default(long),
                RequestedQuantity = filter.Selects.Contains(IndirectSalesOrderContentSelect.RequestedQuantity) ? q.RequestedQuantity : default(long),
                PrimaryPrice = filter.Selects.Contains(IndirectSalesOrderContentSelect.PrimaryPrice) ? q.PrimaryPrice : default(long),
                SalePrice = filter.Selects.Contains(IndirectSalesOrderContentSelect.SalePrice) ? q.SalePrice : default(long),
                DiscountPercentage = filter.Selects.Contains(IndirectSalesOrderContentSelect.DiscountPercentage) ? q.DiscountPercentage : default(decimal?),
                DiscountAmount = filter.Selects.Contains(IndirectSalesOrderContentSelect.DiscountAmount) ? q.DiscountAmount : default(long?),
                GeneralDiscountPercentage = filter.Selects.Contains(IndirectSalesOrderContentSelect.GeneralDiscountPercentage) ? q.GeneralDiscountPercentage : default(decimal?),
                GeneralDiscountAmount = filter.Selects.Contains(IndirectSalesOrderContentSelect.GeneralDiscountAmount) ? q.GeneralDiscountAmount : default(long?),
                Amount = filter.Selects.Contains(IndirectSalesOrderContentSelect.Amount) ? q.Amount : default(long),
                TaxPercentage = filter.Selects.Contains(IndirectSalesOrderContentSelect.TaxPercentage) ? q.TaxPercentage : default(decimal?),
                TaxAmount = filter.Selects.Contains(IndirectSalesOrderContentSelect.TaxAmount) ? q.TaxAmount : default(long?),
                Factor = filter.Selects.Contains(IndirectSalesOrderContentSelect.Factor) ? q.Factor : default(long?),
                IndirectSalesOrder = filter.Selects.Contains(IndirectSalesOrderContentSelect.IndirectSalesOrder) && q.IndirectSalesOrder != null ? new IndirectSalesOrder
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
                PrimaryUnitOfMeasure = filter.Selects.Contains(IndirectSalesOrderContentSelect.PrimaryUnitOfMeasure) && q.PrimaryUnitOfMeasure != null ? new UnitOfMeasure
                {
                    Id = q.PrimaryUnitOfMeasure.Id,
                    Code = q.PrimaryUnitOfMeasure.Code,
                    Name = q.PrimaryUnitOfMeasure.Name,
                    Description = q.PrimaryUnitOfMeasure.Description,
                    StatusId = q.PrimaryUnitOfMeasure.StatusId,
                } : null,
                UnitOfMeasure = filter.Selects.Contains(IndirectSalesOrderContentSelect.UnitOfMeasure) && q.UnitOfMeasure != null ? new UnitOfMeasure
                {
                    Id = q.UnitOfMeasure.Id,
                    Code = q.UnitOfMeasure.Code,
                    Name = q.UnitOfMeasure.Name,
                    Description = q.UnitOfMeasure.Description,
                    StatusId = q.UnitOfMeasure.StatusId,
                } : null,
            }).ToListWithNoLockAsync();
            return IndirectSalesOrderContents;
        }

        public async Task<int> Count(IndirectSalesOrderContentFilter filter)
        {
            IQueryable<IndirectSalesOrderContentDAO> IndirectSalesOrderContents = DataContext.IndirectSalesOrderContent.AsNoTracking();
            IndirectSalesOrderContents = DynamicFilter(IndirectSalesOrderContents, filter);
            IndirectSalesOrderContents = OrFilter(IndirectSalesOrderContents, filter);
            return await IndirectSalesOrderContents.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(IndirectSalesOrderContentFilter filter)
        {
            IQueryable<IndirectSalesOrderContentDAO> IndirectSalesOrderContents = DataContext.IndirectSalesOrderContent.AsNoTracking();
            IndirectSalesOrderContents = DynamicFilter(IndirectSalesOrderContents, filter);
            return await IndirectSalesOrderContents.CountWithNoLockAsync();
        }

        public async Task<List<IndirectSalesOrderContent>> List(IndirectSalesOrderContentFilter filter)
        {
            if (filter == null) return new List<IndirectSalesOrderContent>();
            IQueryable<IndirectSalesOrderContentDAO> IndirectSalesOrderContentDAOs = DataContext.IndirectSalesOrderContent.AsNoTracking();
            IndirectSalesOrderContentDAOs = DynamicFilter(IndirectSalesOrderContentDAOs, filter);
            IndirectSalesOrderContentDAOs = OrFilter(IndirectSalesOrderContentDAOs, filter);
            IndirectSalesOrderContentDAOs = DynamicOrder(IndirectSalesOrderContentDAOs, filter);
            List<IndirectSalesOrderContent> IndirectSalesOrderContents = await DynamicSelect(IndirectSalesOrderContentDAOs, filter);
            return IndirectSalesOrderContents;
        }

        public async Task<IndirectSalesOrderContent> Get(long Id)
        {
            IndirectSalesOrderContent IndirectSalesOrderContent = await DataContext.IndirectSalesOrderContent.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new IndirectSalesOrderContent()
            {
                Id = x.Id,
                IndirectSalesOrderId = x.IndirectSalesOrderId,
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

            if (IndirectSalesOrderContent == null)
                return null;

            return IndirectSalesOrderContent;
        }
        public async Task<bool> Create(IndirectSalesOrderContent IndirectSalesOrderContent)
        {
            IndirectSalesOrderContentDAO IndirectSalesOrderContentDAO = new IndirectSalesOrderContentDAO();
            IndirectSalesOrderContentDAO.Id = IndirectSalesOrderContent.Id;
            IndirectSalesOrderContentDAO.IndirectSalesOrderId = IndirectSalesOrderContent.IndirectSalesOrderId;
            IndirectSalesOrderContentDAO.ItemId = IndirectSalesOrderContent.ItemId;
            IndirectSalesOrderContentDAO.UnitOfMeasureId = IndirectSalesOrderContent.UnitOfMeasureId;
            IndirectSalesOrderContentDAO.Quantity = IndirectSalesOrderContent.Quantity;
            IndirectSalesOrderContentDAO.PrimaryUnitOfMeasureId = IndirectSalesOrderContent.PrimaryUnitOfMeasureId;
            IndirectSalesOrderContentDAO.RequestedQuantity = IndirectSalesOrderContent.RequestedQuantity;
            IndirectSalesOrderContentDAO.PrimaryPrice = IndirectSalesOrderContent.PrimaryPrice;
            IndirectSalesOrderContentDAO.SalePrice = IndirectSalesOrderContent.SalePrice;
            IndirectSalesOrderContentDAO.DiscountPercentage = IndirectSalesOrderContent.DiscountPercentage;
            IndirectSalesOrderContentDAO.DiscountAmount = IndirectSalesOrderContent.DiscountAmount;
            IndirectSalesOrderContentDAO.GeneralDiscountPercentage = IndirectSalesOrderContent.GeneralDiscountPercentage;
            IndirectSalesOrderContentDAO.GeneralDiscountAmount = IndirectSalesOrderContent.GeneralDiscountAmount;
            IndirectSalesOrderContentDAO.Amount = IndirectSalesOrderContent.Amount;
            IndirectSalesOrderContentDAO.TaxPercentage = IndirectSalesOrderContent.TaxPercentage;
            IndirectSalesOrderContentDAO.TaxAmount = IndirectSalesOrderContent.TaxAmount;
            IndirectSalesOrderContentDAO.Factor = IndirectSalesOrderContent.Factor;
            DataContext.IndirectSalesOrderContent.Add(IndirectSalesOrderContentDAO);
            await DataContext.SaveChangesAsync();
            IndirectSalesOrderContent.Id = IndirectSalesOrderContentDAO.Id;
            await SaveReference(IndirectSalesOrderContent);
            return true;
        }

        public async Task<bool> Update(IndirectSalesOrderContent IndirectSalesOrderContent)
        {
            IndirectSalesOrderContentDAO IndirectSalesOrderContentDAO = DataContext.IndirectSalesOrderContent.Where(x => x.Id == IndirectSalesOrderContent.Id).FirstOrDefault();
            if (IndirectSalesOrderContentDAO == null)
                return false;
            IndirectSalesOrderContentDAO.Id = IndirectSalesOrderContent.Id;
            IndirectSalesOrderContentDAO.IndirectSalesOrderId = IndirectSalesOrderContent.IndirectSalesOrderId;
            IndirectSalesOrderContentDAO.ItemId = IndirectSalesOrderContent.ItemId;
            IndirectSalesOrderContentDAO.UnitOfMeasureId = IndirectSalesOrderContent.UnitOfMeasureId;
            IndirectSalesOrderContentDAO.Quantity = IndirectSalesOrderContent.Quantity;
            IndirectSalesOrderContentDAO.PrimaryUnitOfMeasureId = IndirectSalesOrderContent.PrimaryUnitOfMeasureId;
            IndirectSalesOrderContentDAO.RequestedQuantity = IndirectSalesOrderContent.RequestedQuantity;
            IndirectSalesOrderContentDAO.PrimaryPrice = IndirectSalesOrderContent.PrimaryPrice;
            IndirectSalesOrderContentDAO.SalePrice = IndirectSalesOrderContent.SalePrice;
            IndirectSalesOrderContentDAO.DiscountPercentage = IndirectSalesOrderContent.DiscountPercentage;
            IndirectSalesOrderContentDAO.DiscountAmount = IndirectSalesOrderContent.DiscountAmount;
            IndirectSalesOrderContentDAO.GeneralDiscountPercentage = IndirectSalesOrderContent.GeneralDiscountPercentage;
            IndirectSalesOrderContentDAO.GeneralDiscountAmount = IndirectSalesOrderContent.GeneralDiscountAmount;
            IndirectSalesOrderContentDAO.Amount = IndirectSalesOrderContent.Amount;
            IndirectSalesOrderContentDAO.TaxPercentage = IndirectSalesOrderContent.TaxPercentage;
            IndirectSalesOrderContentDAO.TaxAmount = IndirectSalesOrderContent.TaxAmount;
            IndirectSalesOrderContentDAO.Factor = IndirectSalesOrderContent.Factor;
            await DataContext.SaveChangesAsync();
            await SaveReference(IndirectSalesOrderContent);
            return true;
        }

        public async Task<bool> Delete(IndirectSalesOrderContent IndirectSalesOrderContent)
        {
            await DataContext.IndirectSalesOrderContent.Where(x => x.Id == IndirectSalesOrderContent.Id).DeleteFromQueryAsync();
            return true;
        }

        public async Task<bool> BulkMerge(List<IndirectSalesOrderContent> IndirectSalesOrderContents)
        {
            List<IndirectSalesOrderContentDAO> IndirectSalesOrderContentDAOs = new List<IndirectSalesOrderContentDAO>();
            foreach (IndirectSalesOrderContent IndirectSalesOrderContent in IndirectSalesOrderContents)
            {
                IndirectSalesOrderContentDAO IndirectSalesOrderContentDAO = new IndirectSalesOrderContentDAO();
                IndirectSalesOrderContentDAO.Id = IndirectSalesOrderContent.Id;
                IndirectSalesOrderContentDAO.IndirectSalesOrderId = IndirectSalesOrderContent.IndirectSalesOrderId;
                IndirectSalesOrderContentDAO.ItemId = IndirectSalesOrderContent.ItemId;
                IndirectSalesOrderContentDAO.UnitOfMeasureId = IndirectSalesOrderContent.UnitOfMeasureId;
                IndirectSalesOrderContentDAO.Quantity = IndirectSalesOrderContent.Quantity;
                IndirectSalesOrderContentDAO.PrimaryUnitOfMeasureId = IndirectSalesOrderContent.PrimaryUnitOfMeasureId;
                IndirectSalesOrderContentDAO.RequestedQuantity = IndirectSalesOrderContent.RequestedQuantity;
                IndirectSalesOrderContentDAO.PrimaryPrice = IndirectSalesOrderContent.PrimaryPrice;
                IndirectSalesOrderContentDAO.SalePrice = IndirectSalesOrderContent.SalePrice;
                IndirectSalesOrderContentDAO.DiscountPercentage = IndirectSalesOrderContent.DiscountPercentage;
                IndirectSalesOrderContentDAO.DiscountAmount = IndirectSalesOrderContent.DiscountAmount;
                IndirectSalesOrderContentDAO.GeneralDiscountPercentage = IndirectSalesOrderContent.GeneralDiscountPercentage;
                IndirectSalesOrderContentDAO.GeneralDiscountAmount = IndirectSalesOrderContent.GeneralDiscountAmount;
                IndirectSalesOrderContentDAO.Amount = IndirectSalesOrderContent.Amount;
                IndirectSalesOrderContentDAO.TaxPercentage = IndirectSalesOrderContent.TaxPercentage;
                IndirectSalesOrderContentDAO.TaxAmount = IndirectSalesOrderContent.TaxAmount;
                IndirectSalesOrderContentDAO.Factor = IndirectSalesOrderContent.Factor;
                IndirectSalesOrderContentDAOs.Add(IndirectSalesOrderContentDAO);
            }
            await DataContext.BulkMergeAsync(IndirectSalesOrderContentDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<IndirectSalesOrderContent> IndirectSalesOrderContents)
        {
            List<long> Ids = IndirectSalesOrderContents.Select(x => x.Id).ToList();

            await DataContext.IndirectSalesOrderContent
                .Where(x => Ids.Contains(x.Id))
                .DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(IndirectSalesOrderContent IndirectSalesOrderContent)
        {
        }

    }
}
