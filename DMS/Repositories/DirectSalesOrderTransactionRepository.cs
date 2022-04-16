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
    public interface IDirectSalesOrderTransactionRepository
    {
        Task<int> Count(DirectSalesOrderTransactionFilter DirectSalesOrderTransactionFilter);
        Task<int> CountAll(DirectSalesOrderTransactionFilter DirectSalesOrderTransactionFilter);
        Task<List<DirectSalesOrderTransaction>> List(DirectSalesOrderTransactionFilter DirectSalesOrderTransactionFilter);
        Task<DirectSalesOrderTransaction> Get(long Id);
        Task<bool> Create(DirectSalesOrderTransaction DirectSalesOrderTransaction);
        Task<bool> Update(DirectSalesOrderTransaction DirectSalesOrderTransaction);
        Task<bool> Delete(DirectSalesOrderTransaction DirectSalesOrderTransaction);
        Task<bool> BulkMerge(List<DirectSalesOrderTransaction> DirectSalesOrderTransactions);
        Task<bool> BulkDelete(List<DirectSalesOrderTransaction> DirectSalesOrderTransactions);
    }
    public class DirectSalesOrderTransactionRepository : IDirectSalesOrderTransactionRepository
    {
        private DataContext DataContext;
        public DirectSalesOrderTransactionRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<DirectSalesOrderTransactionDAO> DynamicFilter(IQueryable<DirectSalesOrderTransactionDAO> query, DirectSalesOrderTransactionFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.DirectSalesOrderId, filter.DirectSalesOrderId);
            query = query.Where(q => q.OrganizationId, filter.OrganizationId);
            query = query.Where(q => q.ItemId, filter.ItemId);
            query = query.Where(q => q.UnitOfMeasureId, filter.UnitOfMeasureId);
            query = query.Where(q => q.Quantity, filter.Quantity);
            query = query.Where(q => q.Discount, filter.Discount);
            query = query.Where(q => q.Revenue, filter.Revenue);
            query = query.Where(q => q.TypeId, filter.TypeId);
            return query;
        }

        private IQueryable<DirectSalesOrderTransactionDAO> OrFilter(IQueryable<DirectSalesOrderTransactionDAO> query, DirectSalesOrderTransactionFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<DirectSalesOrderTransactionDAO> initQuery = query.Where(q => false);
            foreach (DirectSalesOrderTransactionFilter DirectSalesOrderTransactionFilter in filter.OrFilter)
            {
                IQueryable<DirectSalesOrderTransactionDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, DirectSalesOrderTransactionFilter.Id);
                queryable = queryable.Where(q => q.DirectSalesOrderId, DirectSalesOrderTransactionFilter.DirectSalesOrderId);
                queryable = queryable.Where(q => q.OrganizationId, DirectSalesOrderTransactionFilter.OrganizationId);
                queryable = queryable.Where(q => q.ItemId, DirectSalesOrderTransactionFilter.ItemId);
                queryable = queryable.Where(q => q.UnitOfMeasureId, DirectSalesOrderTransactionFilter.UnitOfMeasureId);
                queryable = queryable.Where(q => q.Quantity, DirectSalesOrderTransactionFilter.Quantity);
                queryable = queryable.Where(q => q.Discount, DirectSalesOrderTransactionFilter.Discount);
                queryable = queryable.Where(q => q.Revenue, DirectSalesOrderTransactionFilter.Revenue);
                queryable = queryable.Where(q => q.TypeId, DirectSalesOrderTransactionFilter.TypeId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<DirectSalesOrderTransactionDAO> DynamicOrder(IQueryable<DirectSalesOrderTransactionDAO> query, DirectSalesOrderTransactionFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case DirectSalesOrderTransactionOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case DirectSalesOrderTransactionOrder.DirectSalesOrder:
                            query = query.OrderBy(q => q.DirectSalesOrderId);
                            break;
                        case DirectSalesOrderTransactionOrder.Organization:
                            query = query.OrderBy(q => q.OrganizationId);
                            break;
                        case DirectSalesOrderTransactionOrder.Item:
                            query = query.OrderBy(q => q.ItemId);
                            break;
                        case DirectSalesOrderTransactionOrder.UnitOfMeasure:
                            query = query.OrderBy(q => q.UnitOfMeasureId);
                            break;
                        case DirectSalesOrderTransactionOrder.Quantity:
                            query = query.OrderBy(q => q.Quantity);
                            break;
                        case DirectSalesOrderTransactionOrder.Discount:
                            query = query.OrderBy(q => q.Discount);
                            break;
                        case DirectSalesOrderTransactionOrder.Revenue:
                            query = query.OrderBy(q => q.Revenue);
                            break;
                        case DirectSalesOrderTransactionOrder.Type:
                            query = query.OrderBy(q => q.TypeId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case DirectSalesOrderTransactionOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case DirectSalesOrderTransactionOrder.DirectSalesOrder:
                            query = query.OrderByDescending(q => q.DirectSalesOrderId);
                            break;
                        case DirectSalesOrderTransactionOrder.Organization:
                            query = query.OrderByDescending(q => q.OrganizationId);
                            break;
                        case DirectSalesOrderTransactionOrder.Item:
                            query = query.OrderByDescending(q => q.ItemId);
                            break;
                        case DirectSalesOrderTransactionOrder.UnitOfMeasure:
                            query = query.OrderByDescending(q => q.UnitOfMeasureId);
                            break;
                        case DirectSalesOrderTransactionOrder.Quantity:
                            query = query.OrderByDescending(q => q.Quantity);
                            break;
                        case DirectSalesOrderTransactionOrder.Discount:
                            query = query.OrderByDescending(q => q.Discount);
                            break;
                        case DirectSalesOrderTransactionOrder.Revenue:
                            query = query.OrderByDescending(q => q.Revenue);
                            break;
                        case DirectSalesOrderTransactionOrder.Type:
                            query = query.OrderByDescending(q => q.TypeId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<DirectSalesOrderTransaction>> DynamicSelect(IQueryable<DirectSalesOrderTransactionDAO> query, DirectSalesOrderTransactionFilter filter)
        {
            List<DirectSalesOrderTransaction> DirectSalesOrderTransactions = await query.Select(q => new DirectSalesOrderTransaction()
            {
                Id = filter.Selects.Contains(DirectSalesOrderTransactionSelect.Id) ? q.Id : default(long),
                DirectSalesOrderId = filter.Selects.Contains(DirectSalesOrderTransactionSelect.DirectSalesOrder) ? q.DirectSalesOrderId : default(long),
                OrganizationId = filter.Selects.Contains(DirectSalesOrderTransactionSelect.Organization) ? q.OrganizationId : default(long),
                ItemId = filter.Selects.Contains(DirectSalesOrderTransactionSelect.Item) ? q.ItemId : default(long),
                UnitOfMeasureId = filter.Selects.Contains(DirectSalesOrderTransactionSelect.UnitOfMeasure) ? q.UnitOfMeasureId : default(long),
                Quantity = filter.Selects.Contains(DirectSalesOrderTransactionSelect.Quantity) ? q.Quantity : default(long),
                Discount = filter.Selects.Contains(DirectSalesOrderTransactionSelect.Discount) ? q.Discount : default(decimal?),
                Revenue = filter.Selects.Contains(DirectSalesOrderTransactionSelect.Revenue) ? q.Revenue : default(decimal?),
                TypeId = filter.Selects.Contains(DirectSalesOrderTransactionSelect.Type) ? q.TypeId : default(long),
                DirectSalesOrder = filter.Selects.Contains(DirectSalesOrderTransactionSelect.DirectSalesOrder) && q.DirectSalesOrder != null ? new DirectSalesOrder
                {
                    Id = q.DirectSalesOrder.Id,
                    Code = q.DirectSalesOrder.Code,
                    OrganizationId = q.DirectSalesOrder.OrganizationId,
                    BuyerStoreId = q.DirectSalesOrder.BuyerStoreId,
                    PhoneNumber = q.DirectSalesOrder.PhoneNumber,
                    StoreAddress = q.DirectSalesOrder.StoreAddress,
                    DeliveryAddress = q.DirectSalesOrder.DeliveryAddress,
                    SaleEmployeeId = q.DirectSalesOrder.SaleEmployeeId,
                    OrderDate = q.DirectSalesOrder.OrderDate,
                    DeliveryDate = q.DirectSalesOrder.DeliveryDate,
                    RequestStateId = q.DirectSalesOrder.RequestStateId,
                    EditedPriceStatusId = q.DirectSalesOrder.EditedPriceStatusId,
                    Note = q.DirectSalesOrder.Note,
                    SubTotal = q.DirectSalesOrder.SubTotal,
                    GeneralDiscountPercentage = q.DirectSalesOrder.GeneralDiscountPercentage,
                    GeneralDiscountAmount = q.DirectSalesOrder.GeneralDiscountAmount,
                    TotalTaxAmount = q.DirectSalesOrder.TotalTaxAmount,
                    Total = q.DirectSalesOrder.Total,
                    StoreCheckingId = q.DirectSalesOrder.StoreCheckingId,
                } : null,
                Item = filter.Selects.Contains(DirectSalesOrderTransactionSelect.Item) && q.Item != null ? new Item
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
                Organization = filter.Selects.Contains(DirectSalesOrderTransactionSelect.Organization) && q.Organization != null ? new Organization
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
                UnitOfMeasure = filter.Selects.Contains(DirectSalesOrderTransactionSelect.UnitOfMeasure) && q.UnitOfMeasure != null ? new UnitOfMeasure
                {
                    Id = q.UnitOfMeasure.Id,
                    Code = q.UnitOfMeasure.Code,
                    Name = q.UnitOfMeasure.Name,
                    Description = q.UnitOfMeasure.Description,
                    StatusId = q.UnitOfMeasure.StatusId,
                    Used = q.UnitOfMeasure.Used,
                } : null,
            }).ToListWithNoLockAsync();
            return DirectSalesOrderTransactions;
        }

        public async Task<int> Count(DirectSalesOrderTransactionFilter filter)
        {
            IQueryable<DirectSalesOrderTransactionDAO> DirectSalesOrderTransactions = DataContext.DirectSalesOrderTransaction.AsNoTracking();
            DirectSalesOrderTransactions = DynamicFilter(DirectSalesOrderTransactions, filter);
            DirectSalesOrderTransactions = OrFilter(DirectSalesOrderTransactions, filter);
            return await DirectSalesOrderTransactions.CountWithNoLockAsync();
        }

        public async Task<int> CountAll(DirectSalesOrderTransactionFilter filter)
        {
            IQueryable<DirectSalesOrderTransactionDAO> DirectSalesOrderTransactions = DataContext.DirectSalesOrderTransaction.AsNoTracking();
            DirectSalesOrderTransactions = DynamicFilter(DirectSalesOrderTransactions, filter);
            return await DirectSalesOrderTransactions.CountWithNoLockAsync();
        }
        public async Task<List<DirectSalesOrderTransaction>> List(DirectSalesOrderTransactionFilter filter)
        {
            if (filter == null) return new List<DirectSalesOrderTransaction>();
            IQueryable<DirectSalesOrderTransactionDAO> DirectSalesOrderTransactionDAOs = DataContext.DirectSalesOrderTransaction.AsNoTracking();
            DirectSalesOrderTransactionDAOs = DynamicFilter(DirectSalesOrderTransactionDAOs, filter);
            DirectSalesOrderTransactionDAOs = OrFilter(DirectSalesOrderTransactionDAOs, filter);
            DirectSalesOrderTransactionDAOs = DynamicOrder(DirectSalesOrderTransactionDAOs, filter);
            List<DirectSalesOrderTransaction> DirectSalesOrderTransactions = await DynamicSelect(DirectSalesOrderTransactionDAOs, filter);
            return DirectSalesOrderTransactions;
        }

        public async Task<DirectSalesOrderTransaction> Get(long Id)
        {
            DirectSalesOrderTransaction DirectSalesOrderTransaction = await DataContext.DirectSalesOrderTransaction.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new DirectSalesOrderTransaction()
            {
                Id = x.Id,
                DirectSalesOrderId = x.DirectSalesOrderId,
                OrganizationId = x.OrganizationId,
                ItemId = x.ItemId,
                UnitOfMeasureId = x.UnitOfMeasureId,
                Quantity = x.Quantity,
                Discount = x.Discount,
                Revenue = x.Revenue,
                TypeId = x.TypeId,
                DirectSalesOrder = x.DirectSalesOrder == null ? null : new DirectSalesOrder
                {
                    Id = x.DirectSalesOrder.Id,
                    Code = x.DirectSalesOrder.Code,
                    OrganizationId = x.DirectSalesOrder.OrganizationId,
                    BuyerStoreId = x.DirectSalesOrder.BuyerStoreId,
                    PhoneNumber = x.DirectSalesOrder.PhoneNumber,
                    StoreAddress = x.DirectSalesOrder.StoreAddress,
                    DeliveryAddress = x.DirectSalesOrder.DeliveryAddress,
                    SaleEmployeeId = x.DirectSalesOrder.SaleEmployeeId,
                    OrderDate = x.DirectSalesOrder.OrderDate,
                    DeliveryDate = x.DirectSalesOrder.DeliveryDate,
                    RequestStateId = x.DirectSalesOrder.RequestStateId,
                    EditedPriceStatusId = x.DirectSalesOrder.EditedPriceStatusId,
                    Note = x.DirectSalesOrder.Note,
                    SubTotal = x.DirectSalesOrder.SubTotal,
                    GeneralDiscountPercentage = x.DirectSalesOrder.GeneralDiscountPercentage,
                    GeneralDiscountAmount = x.DirectSalesOrder.GeneralDiscountAmount,
                    TotalTaxAmount = x.DirectSalesOrder.TotalTaxAmount,
                    Total = x.DirectSalesOrder.Total,
                    StoreCheckingId = x.DirectSalesOrder.StoreCheckingId,
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

            if (DirectSalesOrderTransaction == null)
                return null;

            return DirectSalesOrderTransaction;
        }
        public async Task<bool> Create(DirectSalesOrderTransaction DirectSalesOrderTransaction)
        {
            DirectSalesOrderTransactionDAO DirectSalesOrderTransactionDAO = new DirectSalesOrderTransactionDAO();
            DirectSalesOrderTransactionDAO.Id = DirectSalesOrderTransaction.Id;
            DirectSalesOrderTransactionDAO.DirectSalesOrderId = DirectSalesOrderTransaction.DirectSalesOrderId;
            DirectSalesOrderTransactionDAO.OrganizationId = DirectSalesOrderTransaction.OrganizationId;
            DirectSalesOrderTransactionDAO.ItemId = DirectSalesOrderTransaction.ItemId;
            DirectSalesOrderTransactionDAO.UnitOfMeasureId = DirectSalesOrderTransaction.UnitOfMeasureId;
            DirectSalesOrderTransactionDAO.Quantity = DirectSalesOrderTransaction.Quantity;
            DirectSalesOrderTransactionDAO.Discount = DirectSalesOrderTransaction.Discount;
            DirectSalesOrderTransactionDAO.Revenue = DirectSalesOrderTransaction.Revenue;
            DirectSalesOrderTransactionDAO.TypeId = DirectSalesOrderTransaction.TypeId;
            DataContext.DirectSalesOrderTransaction.Add(DirectSalesOrderTransactionDAO);
            await DataContext.SaveChangesAsync();
            DirectSalesOrderTransaction.Id = DirectSalesOrderTransactionDAO.Id;
            await SaveReference(DirectSalesOrderTransaction);
            return true;
        }

        public async Task<bool> Update(DirectSalesOrderTransaction DirectSalesOrderTransaction)
        {
            DirectSalesOrderTransactionDAO DirectSalesOrderTransactionDAO = DataContext.DirectSalesOrderTransaction.Where(x => x.Id == DirectSalesOrderTransaction.Id).FirstOrDefault();
            if (DirectSalesOrderTransactionDAO == null)
                return false;
            DirectSalesOrderTransactionDAO.Id = DirectSalesOrderTransaction.Id;
            DirectSalesOrderTransactionDAO.DirectSalesOrderId = DirectSalesOrderTransaction.DirectSalesOrderId;
            DirectSalesOrderTransactionDAO.OrganizationId = DirectSalesOrderTransaction.OrganizationId;
            DirectSalesOrderTransactionDAO.ItemId = DirectSalesOrderTransaction.ItemId;
            DirectSalesOrderTransactionDAO.UnitOfMeasureId = DirectSalesOrderTransaction.UnitOfMeasureId;
            DirectSalesOrderTransactionDAO.Quantity = DirectSalesOrderTransaction.Quantity;
            DirectSalesOrderTransactionDAO.Discount = DirectSalesOrderTransaction.Discount;
            DirectSalesOrderTransactionDAO.Revenue = DirectSalesOrderTransaction.Revenue;
            DirectSalesOrderTransactionDAO.TypeId = DirectSalesOrderTransaction.TypeId;
            await DataContext.SaveChangesAsync();
            await SaveReference(DirectSalesOrderTransaction);
            return true;
        }

        public async Task<bool> Delete(DirectSalesOrderTransaction DirectSalesOrderTransaction)
        {
            await DataContext.DirectSalesOrderTransaction.Where(x => x.Id == DirectSalesOrderTransaction.Id).DeleteFromQueryAsync();
            return true;
        }

        public async Task<bool> BulkMerge(List<DirectSalesOrderTransaction> DirectSalesOrderTransactions)
        {
            List<DirectSalesOrderTransactionDAO> DirectSalesOrderTransactionDAOs = new List<DirectSalesOrderTransactionDAO>();
            foreach (DirectSalesOrderTransaction DirectSalesOrderTransaction in DirectSalesOrderTransactions)
            {
                DirectSalesOrderTransactionDAO DirectSalesOrderTransactionDAO = new DirectSalesOrderTransactionDAO();
                DirectSalesOrderTransactionDAO.Id = DirectSalesOrderTransaction.Id;
                DirectSalesOrderTransactionDAO.DirectSalesOrderId = DirectSalesOrderTransaction.DirectSalesOrderId;
                DirectSalesOrderTransactionDAO.OrganizationId = DirectSalesOrderTransaction.OrganizationId;
                DirectSalesOrderTransactionDAO.ItemId = DirectSalesOrderTransaction.ItemId;
                DirectSalesOrderTransactionDAO.UnitOfMeasureId = DirectSalesOrderTransaction.UnitOfMeasureId;
                DirectSalesOrderTransactionDAO.Quantity = DirectSalesOrderTransaction.Quantity;
                DirectSalesOrderTransactionDAO.Discount = DirectSalesOrderTransaction.Discount;
                DirectSalesOrderTransactionDAO.Revenue = DirectSalesOrderTransaction.Revenue;
                DirectSalesOrderTransactionDAO.TypeId = DirectSalesOrderTransaction.TypeId;
                DirectSalesOrderTransactionDAOs.Add(DirectSalesOrderTransactionDAO);
            }
            await DataContext.BulkMergeAsync(DirectSalesOrderTransactionDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<DirectSalesOrderTransaction> DirectSalesOrderTransactions)
        {
            List<long> Ids = DirectSalesOrderTransactions.Select(x => x.Id).ToList();
            await DataContext.DirectSalesOrderTransaction
                .Where(x => Ids.Contains(x.Id))
                .DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(DirectSalesOrderTransaction DirectSalesOrderTransaction)
        {
        }

    }
}
