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
    public interface IIndirectSalesOrderTransactionRepository
    {
        Task<int> Count(IndirectSalesOrderTransactionFilter IndirectSalesOrderTransactionFilter);
        Task<int> CountAll(IndirectSalesOrderTransactionFilter IndirectSalesOrderTransactionFilter);
        Task<List<IndirectSalesOrderTransaction>> List(IndirectSalesOrderTransactionFilter IndirectSalesOrderTransactionFilter);
        Task<IndirectSalesOrderTransaction> Get(long Id);
        Task<bool> Create(IndirectSalesOrderTransaction IndirectSalesOrderTransaction);
        Task<bool> Update(IndirectSalesOrderTransaction IndirectSalesOrderTransaction);
        Task<bool> Delete(IndirectSalesOrderTransaction IndirectSalesOrderTransaction);
        Task<bool> BulkMerge(List<IndirectSalesOrderTransaction> IndirectSalesOrderTransactions);
        Task<bool> BulkDelete(List<IndirectSalesOrderTransaction> IndirectSalesOrderTransactions);
    }
    public class IndirectSalesOrderTransactionRepository : IIndirectSalesOrderTransactionRepository
    {
        private DataContext DataContext;
        public IndirectSalesOrderTransactionRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<IndirectSalesOrderTransactionDAO> DynamicFilter(IQueryable<IndirectSalesOrderTransactionDAO> query, IndirectSalesOrderTransactionFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.IndirectSalesOrderId, filter.IndirectSalesOrderId);
            query = query.Where(q => q.OrganizationId, filter.OrganizationId);
            query = query.Where(q => q.ItemId, filter.ItemId);
            query = query.Where(q => q.UnitOfMeasureId, filter.UnitOfMeasureId);
            query = query.Where(q => q.Quantity, filter.Quantity);
            query = query.Where(q => q.Discount, filter.Discount);
            query = query.Where(q => q.Revenue, filter.Revenue);
            query = query.Where(q => q.TypeId, filter.TypeId);
            return query;
        }

        private IQueryable<IndirectSalesOrderTransactionDAO> OrFilter(IQueryable<IndirectSalesOrderTransactionDAO> query, IndirectSalesOrderTransactionFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<IndirectSalesOrderTransactionDAO> initQuery = query.Where(q => false);
            foreach (IndirectSalesOrderTransactionFilter IndirectSalesOrderTransactionFilter in filter.OrFilter)
            {
                IQueryable<IndirectSalesOrderTransactionDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, IndirectSalesOrderTransactionFilter.Id);
                queryable = queryable.Where(q => q.IndirectSalesOrderId, IndirectSalesOrderTransactionFilter.IndirectSalesOrderId);
                queryable = queryable.Where(q => q.OrganizationId, IndirectSalesOrderTransactionFilter.OrganizationId);
                queryable = queryable.Where(q => q.ItemId, IndirectSalesOrderTransactionFilter.ItemId);
                queryable = queryable.Where(q => q.UnitOfMeasureId, IndirectSalesOrderTransactionFilter.UnitOfMeasureId);
                queryable = queryable.Where(q => q.Quantity, IndirectSalesOrderTransactionFilter.Quantity);
                queryable = queryable.Where(q => q.Discount, IndirectSalesOrderTransactionFilter.Discount);
                queryable = queryable.Where(q => q.Revenue, IndirectSalesOrderTransactionFilter.Revenue);
                if (IndirectSalesOrderTransactionFilter.TypeId != null)
                    queryable = queryable.Where(q => q.TypeId, IndirectSalesOrderTransactionFilter.TypeId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<IndirectSalesOrderTransactionDAO> DynamicOrder(IQueryable<IndirectSalesOrderTransactionDAO> query, IndirectSalesOrderTransactionFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case IndirectSalesOrderTransactionOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case IndirectSalesOrderTransactionOrder.IndirectSalesOrder:
                            query = query.OrderBy(q => q.IndirectSalesOrderId);
                            break;
                        case IndirectSalesOrderTransactionOrder.Organization:
                            query = query.OrderBy(q => q.OrganizationId);
                            break;
                        case IndirectSalesOrderTransactionOrder.Item:
                            query = query.OrderBy(q => q.ItemId);
                            break;
                        case IndirectSalesOrderTransactionOrder.UnitOfMeasure:
                            query = query.OrderBy(q => q.UnitOfMeasureId);
                            break;
                        case IndirectSalesOrderTransactionOrder.Quantity:
                            query = query.OrderBy(q => q.Quantity);
                            break;
                        case IndirectSalesOrderTransactionOrder.Discount:
                            query = query.OrderBy(q => q.Discount);
                            break;
                        case IndirectSalesOrderTransactionOrder.Revenue:
                            query = query.OrderBy(q => q.Revenue);
                            break;
                        case IndirectSalesOrderTransactionOrder.Type:
                            query = query.OrderBy(q => q.TypeId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case IndirectSalesOrderTransactionOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case IndirectSalesOrderTransactionOrder.IndirectSalesOrder:
                            query = query.OrderByDescending(q => q.IndirectSalesOrderId);
                            break;
                        case IndirectSalesOrderTransactionOrder.Organization:
                            query = query.OrderByDescending(q => q.OrganizationId);
                            break;
                        case IndirectSalesOrderTransactionOrder.Item:
                            query = query.OrderByDescending(q => q.ItemId);
                            break;
                        case IndirectSalesOrderTransactionOrder.UnitOfMeasure:
                            query = query.OrderByDescending(q => q.UnitOfMeasureId);
                            break;
                        case IndirectSalesOrderTransactionOrder.Quantity:
                            query = query.OrderByDescending(q => q.Quantity);
                            break;
                        case IndirectSalesOrderTransactionOrder.Discount:
                            query = query.OrderByDescending(q => q.Discount);
                            break;
                        case IndirectSalesOrderTransactionOrder.Revenue:
                            query = query.OrderByDescending(q => q.Revenue);
                            break;
                        case IndirectSalesOrderTransactionOrder.Type:
                            query = query.OrderByDescending(q => q.TypeId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<IndirectSalesOrderTransaction>> DynamicSelect(IQueryable<IndirectSalesOrderTransactionDAO> query, IndirectSalesOrderTransactionFilter filter)
        {
            List<IndirectSalesOrderTransaction> IndirectSalesOrderTransactions = await query.Select(q => new IndirectSalesOrderTransaction()
            {
                Id = filter.Selects.Contains(IndirectSalesOrderTransactionSelect.Id) ? q.Id : default(long),
                IndirectSalesOrderId = filter.Selects.Contains(IndirectSalesOrderTransactionSelect.IndirectSalesOrder) ? q.IndirectSalesOrderId : default(long),
                OrganizationId = filter.Selects.Contains(IndirectSalesOrderTransactionSelect.Organization) ? q.OrganizationId : default(long),
                ItemId = filter.Selects.Contains(IndirectSalesOrderTransactionSelect.Item) ? q.ItemId : default(long),
                UnitOfMeasureId = filter.Selects.Contains(IndirectSalesOrderTransactionSelect.UnitOfMeasure) ? q.UnitOfMeasureId : default(long),
                Quantity = filter.Selects.Contains(IndirectSalesOrderTransactionSelect.Quantity) ? q.Quantity : default(long),
                Discount = filter.Selects.Contains(IndirectSalesOrderTransactionSelect.Discount) ? q.Discount : default(decimal?),
                Revenue = filter.Selects.Contains(IndirectSalesOrderTransactionSelect.Revenue) ? q.Revenue : default(decimal?),
                TypeId = filter.Selects.Contains(IndirectSalesOrderTransactionSelect.Type) ? q.TypeId : default(long),
                IndirectSalesOrder = filter.Selects.Contains(IndirectSalesOrderTransactionSelect.IndirectSalesOrder) && q.IndirectSalesOrder != null ? new IndirectSalesOrder
                {
                    Id = q.IndirectSalesOrder.Id,
                    Code = q.IndirectSalesOrder.Code,
                    OrganizationId = q.IndirectSalesOrder.OrganizationId,
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
                    StoreCheckingId = q.IndirectSalesOrder.StoreCheckingId,
                } : null,
                Item = filter.Selects.Contains(IndirectSalesOrderTransactionSelect.Item) && q.Item != null ? new Item
                {
                    Id = q.Item.Id,
                    ProductId = q.Item.ProductId,
                    Code = q.Item.Code,
                    Name = q.Item.Name,
                    ScanCode = q.Item.ScanCode,
                    SalePrice = q.Item.SalePrice,
                    RetailPrice = q.Item.RetailPrice,
                    StatusId = q.Item.StatusId,
                    Used = q.Item.Used,
                } : null,
                Organization = filter.Selects.Contains(IndirectSalesOrderTransactionSelect.Organization) && q.Organization != null ? new Organization
                {
                    Id = q.Organization.Id,
                    Code = q.Organization.Code,
                    Name = q.Organization.Name,
                    ParentId = q.Organization.ParentId,
                    Path = q.Organization.Path,
                    Level = q.Organization.Level,
                    StatusId = q.Organization.StatusId,
                    Phone = q.Organization.Phone,
                    Email = q.Organization.Email,
                    Address = q.Organization.Address,
                } : null,
                UnitOfMeasure = filter.Selects.Contains(IndirectSalesOrderTransactionSelect.UnitOfMeasure) && q.UnitOfMeasure != null ? new UnitOfMeasure
                {
                    Id = q.UnitOfMeasure.Id,
                    Code = q.UnitOfMeasure.Code,
                    Name = q.UnitOfMeasure.Name,
                    Description = q.UnitOfMeasure.Description,
                    StatusId = q.UnitOfMeasure.StatusId,
                    Used = q.UnitOfMeasure.Used,
                } : null,
            }).ToListWithNoLockAsync();
            return IndirectSalesOrderTransactions;
        }

        public async Task<int> Count(IndirectSalesOrderTransactionFilter filter)
        {
            IQueryable<IndirectSalesOrderTransactionDAO> IndirectSalesOrderTransactions = DataContext.IndirectSalesOrderTransaction.AsNoTracking();
            IndirectSalesOrderTransactions = DynamicFilter(IndirectSalesOrderTransactions, filter);
            IndirectSalesOrderTransactions = OrFilter(IndirectSalesOrderTransactions, filter);
            return await IndirectSalesOrderTransactions.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(IndirectSalesOrderTransactionFilter filter)
        {
            IQueryable<IndirectSalesOrderTransactionDAO> IndirectSalesOrderTransactions = DataContext.IndirectSalesOrderTransaction.AsNoTracking();
            IndirectSalesOrderTransactions = DynamicFilter(IndirectSalesOrderTransactions, filter);
            return await IndirectSalesOrderTransactions.CountWithNoLockAsync();
        }

        public async Task<List<IndirectSalesOrderTransaction>> List(IndirectSalesOrderTransactionFilter filter)
        {
            if (filter == null) return new List<IndirectSalesOrderTransaction>();
            IQueryable<IndirectSalesOrderTransactionDAO> IndirectSalesOrderTransactionDAOs = DataContext.IndirectSalesOrderTransaction.AsNoTracking();
            IndirectSalesOrderTransactionDAOs = DynamicFilter(IndirectSalesOrderTransactionDAOs, filter);
            IndirectSalesOrderTransactionDAOs = OrFilter(IndirectSalesOrderTransactionDAOs, filter);
            IndirectSalesOrderTransactionDAOs = DynamicOrder(IndirectSalesOrderTransactionDAOs, filter);
            List<IndirectSalesOrderTransaction> IndirectSalesOrderTransactions = await DynamicSelect(IndirectSalesOrderTransactionDAOs, filter);
            return IndirectSalesOrderTransactions;
        }

        public async Task<IndirectSalesOrderTransaction> Get(long Id)
        {
            IndirectSalesOrderTransaction IndirectSalesOrderTransaction = await DataContext.IndirectSalesOrderTransaction.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new IndirectSalesOrderTransaction()
            {
                Id = x.Id,
                IndirectSalesOrderId = x.IndirectSalesOrderId,
                OrganizationId = x.OrganizationId,
                ItemId = x.ItemId,
                UnitOfMeasureId = x.UnitOfMeasureId,
                Quantity = x.Quantity,
                Discount = x.Discount,
                Revenue = x.Revenue,
                TypeId = x.TypeId,
                IndirectSalesOrder = x.IndirectSalesOrder == null ? null : new IndirectSalesOrder
                {
                    Id = x.IndirectSalesOrder.Id,
                    Code = x.IndirectSalesOrder.Code,
                    OrganizationId = x.IndirectSalesOrder.OrganizationId,
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
                    StoreCheckingId = x.IndirectSalesOrder.StoreCheckingId,
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
                    Used = x.Item.Used,
                },
                Organization = x.Organization == null ? null : new Organization
                {
                    Id = x.Organization.Id,
                    Code = x.Organization.Code,
                    Name = x.Organization.Name,
                    ParentId = x.Organization.ParentId,
                    Path = x.Organization.Path,
                    Level = x.Organization.Level,
                    StatusId = x.Organization.StatusId,
                    Phone = x.Organization.Phone,
                    Email = x.Organization.Email,
                    Address = x.Organization.Address,
                },
                UnitOfMeasure = x.UnitOfMeasure == null ? null : new UnitOfMeasure
                {
                    Id = x.UnitOfMeasure.Id,
                    Code = x.UnitOfMeasure.Code,
                    Name = x.UnitOfMeasure.Name,
                    Description = x.UnitOfMeasure.Description,
                    StatusId = x.UnitOfMeasure.StatusId,
                    Used = x.UnitOfMeasure.Used,
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (IndirectSalesOrderTransaction == null)
                return null;

            return IndirectSalesOrderTransaction;
        }
        public async Task<bool> Create(IndirectSalesOrderTransaction IndirectSalesOrderTransaction)
        {
            IndirectSalesOrderTransactionDAO IndirectSalesOrderTransactionDAO = new IndirectSalesOrderTransactionDAO();
            IndirectSalesOrderTransactionDAO.Id = IndirectSalesOrderTransaction.Id;
            IndirectSalesOrderTransactionDAO.IndirectSalesOrderId = IndirectSalesOrderTransaction.IndirectSalesOrderId;
            IndirectSalesOrderTransactionDAO.OrganizationId = IndirectSalesOrderTransaction.OrganizationId;
            IndirectSalesOrderTransactionDAO.ItemId = IndirectSalesOrderTransaction.ItemId;
            IndirectSalesOrderTransactionDAO.UnitOfMeasureId = IndirectSalesOrderTransaction.UnitOfMeasureId;
            IndirectSalesOrderTransactionDAO.Quantity = IndirectSalesOrderTransaction.Quantity;
            IndirectSalesOrderTransactionDAO.Discount = IndirectSalesOrderTransaction.Discount;
            IndirectSalesOrderTransactionDAO.Revenue = IndirectSalesOrderTransaction.Revenue;
            IndirectSalesOrderTransactionDAO.TypeId = IndirectSalesOrderTransaction.TypeId;
            DataContext.IndirectSalesOrderTransaction.Add(IndirectSalesOrderTransactionDAO);
            await DataContext.SaveChangesAsync();
            IndirectSalesOrderTransaction.Id = IndirectSalesOrderTransactionDAO.Id;
            await SaveReference(IndirectSalesOrderTransaction);
            return true;
        }

        public async Task<bool> Update(IndirectSalesOrderTransaction IndirectSalesOrderTransaction)
        {
            IndirectSalesOrderTransactionDAO IndirectSalesOrderTransactionDAO = DataContext.IndirectSalesOrderTransaction.Where(x => x.Id == IndirectSalesOrderTransaction.Id).FirstOrDefault();
            if (IndirectSalesOrderTransactionDAO == null)
                return false;
            IndirectSalesOrderTransactionDAO.Id = IndirectSalesOrderTransaction.Id;
            IndirectSalesOrderTransactionDAO.IndirectSalesOrderId = IndirectSalesOrderTransaction.IndirectSalesOrderId;
            IndirectSalesOrderTransactionDAO.OrganizationId = IndirectSalesOrderTransaction.OrganizationId;
            IndirectSalesOrderTransactionDAO.ItemId = IndirectSalesOrderTransaction.ItemId;
            IndirectSalesOrderTransactionDAO.UnitOfMeasureId = IndirectSalesOrderTransaction.UnitOfMeasureId;
            IndirectSalesOrderTransactionDAO.Quantity = IndirectSalesOrderTransaction.Quantity;
            IndirectSalesOrderTransactionDAO.Discount = IndirectSalesOrderTransaction.Discount;
            IndirectSalesOrderTransactionDAO.Revenue = IndirectSalesOrderTransaction.Revenue;
            IndirectSalesOrderTransactionDAO.TypeId = IndirectSalesOrderTransaction.TypeId;
            await DataContext.SaveChangesAsync();
            await SaveReference(IndirectSalesOrderTransaction);
            return true;
        }

        public async Task<bool> Delete(IndirectSalesOrderTransaction IndirectSalesOrderTransaction)
        {
            await DataContext.IndirectSalesOrderTransaction.Where(x => x.Id == IndirectSalesOrderTransaction.Id).DeleteFromQueryAsync();
            return true;
        }

        public async Task<bool> BulkMerge(List<IndirectSalesOrderTransaction> IndirectSalesOrderTransactions)
        {
            List<IndirectSalesOrderTransactionDAO> IndirectSalesOrderTransactionDAOs = new List<IndirectSalesOrderTransactionDAO>();
            foreach (IndirectSalesOrderTransaction IndirectSalesOrderTransaction in IndirectSalesOrderTransactions)
            {
                IndirectSalesOrderTransactionDAO IndirectSalesOrderTransactionDAO = new IndirectSalesOrderTransactionDAO();
                IndirectSalesOrderTransactionDAO.Id = IndirectSalesOrderTransaction.Id;
                IndirectSalesOrderTransactionDAO.IndirectSalesOrderId = IndirectSalesOrderTransaction.IndirectSalesOrderId;
                IndirectSalesOrderTransactionDAO.OrganizationId = IndirectSalesOrderTransaction.OrganizationId;
                IndirectSalesOrderTransactionDAO.ItemId = IndirectSalesOrderTransaction.ItemId;
                IndirectSalesOrderTransactionDAO.UnitOfMeasureId = IndirectSalesOrderTransaction.UnitOfMeasureId;
                IndirectSalesOrderTransactionDAO.Quantity = IndirectSalesOrderTransaction.Quantity;
                IndirectSalesOrderTransactionDAO.Discount = IndirectSalesOrderTransaction.Discount;
                IndirectSalesOrderTransactionDAO.Revenue = IndirectSalesOrderTransaction.Revenue;
                IndirectSalesOrderTransactionDAO.TypeId = IndirectSalesOrderTransaction.TypeId;
                IndirectSalesOrderTransactionDAOs.Add(IndirectSalesOrderTransactionDAO);
            }
            await DataContext.BulkMergeAsync(IndirectSalesOrderTransactionDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<IndirectSalesOrderTransaction> IndirectSalesOrderTransactions)
        {
            List<long> Ids = IndirectSalesOrderTransactions.Select(x => x.Id).ToList();
            await DataContext.IndirectSalesOrderTransaction
                .Where(x => Ids.Contains(x.Id))
                .DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(IndirectSalesOrderTransaction IndirectSalesOrderTransaction)
        {
        }

    }
}
