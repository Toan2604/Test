using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thinktecture;
using Thinktecture.EntityFrameworkCore.TempTables;
using TrueSight.Common;

namespace DMS.Repositories
{
    public interface IDirectSalesOrderRepository
    {
        Task<int> Count(DirectSalesOrderFilter DirectSalesOrderFilter);
        Task<int> CountAll(DirectSalesOrderFilter DirectSalesOrderFilter);
        Task<List<DirectSalesOrder>> List(DirectSalesOrderFilter DirectSalesOrderFilter);
        Task<int> CountNew(DirectSalesOrderFilter DirectSalesOrderFilter);
        Task<List<DirectSalesOrder>> ListNew(DirectSalesOrderFilter DirectSalesOrderFilter);
        Task<int> CountPending(DirectSalesOrderFilter DirectSalesOrderFilter);
        Task<List<DirectSalesOrder>> ListPending(DirectSalesOrderFilter DirectSalesOrderFilter);
        Task<int> CountCompleted(DirectSalesOrderFilter DirectSalesOrderFilter);
        Task<List<DirectSalesOrder>> ListCompleted(DirectSalesOrderFilter DirectSalesOrderFilter);
        Task<int> CountInScoped(DirectSalesOrderFilter DirectSalesOrderFilter);
        Task<List<DirectSalesOrder>> ListInScoped(DirectSalesOrderFilter DirectSalesOrderFilter);
        Task<List<DirectSalesOrder>> List(List<long> Ids);
        Task<DirectSalesOrder> Get(long Id);
        Task<bool> Create(DirectSalesOrder DirectSalesOrder);
        Task<bool> Update(DirectSalesOrder DirectSalesOrder);
        Task<bool> Delete(DirectSalesOrder DirectSalesOrder);
        Task<bool> BulkMerge(List<DirectSalesOrder> DirectSalesOrders);
        Task<bool> BulkUpdateCheckState(List<DirectSalesOrder> DirectSalesOrders);
        Task<bool> BulkDelete(List<DirectSalesOrder> DirectSalesOrders);
        Task<bool> BulkUpdate(List<DirectSalesOrder> DirectSalesOrders);
        Task<bool> UpdateState(DirectSalesOrder DirectSalesOrder, SystemConfiguration SystemConfiguration);
    }
    public class DirectSalesOrderRepository : IDirectSalesOrderRepository
    {
        private DataContext DataContext;
        public DirectSalesOrderRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<DirectSalesOrderDAO> DynamicFilter(IQueryable<DirectSalesOrderDAO> query, DirectSalesOrderFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            if (filter.OrganizationId != null)
            {
                if (filter.OrganizationId.Equal != null)
                {
                    OrganizationDAO OrganizationDAO = DataContext.Organization
                        .Where(o => o.Id == filter.OrganizationId.Equal.Value).FirstOrDefault();
                    query = query.Where(q => q.Organization.Path.StartsWith(OrganizationDAO.Path));
                }
                if (filter.OrganizationId.NotEqual != null)
                {
                    OrganizationDAO OrganizationDAO = DataContext.Organization
                        .Where(o => o.Id == filter.OrganizationId.NotEqual.Value).FirstOrDefault();
                    query = query.Where(q => !q.Organization.Path.StartsWith(OrganizationDAO.Path));
                }
                if (filter.OrganizationId.In != null)
                {
                    List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                        .Where(o => o.DeletedAt == null && o.StatusId == 1).ToListWithNoLock();
                    List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => filter.OrganizationId.In.Contains(o.Id)).ToList();
                    List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> Ids = Branches.Select(o => o.Id).ToList();
                    IdFilter IdFilter = new IdFilter { In = Ids };
                    query = query.Where(x => x.OrganizationId, IdFilter);
                }
                if (filter.OrganizationId.NotIn != null)
                {
                    List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                        .Where(o => o.DeletedAt == null && o.StatusId == 1).ToListWithNoLock();
                    List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => filter.OrganizationId.NotIn.Contains(o.Id)).ToList();
                    List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> Ids = Branches.Select(o => o.Id).ToList();
                    IdFilter IdFilter = new IdFilter { NotIn = Ids };
                    query = query.Where(x => x.OrganizationId, IdFilter);
                }
            }

            query = query.Where(q => q.BuyerStore.Code, filter.BuyerStoreCode);
            query = query.Where(q => q.GeneralApprovalStateId, filter.GeneralApprovalStateId);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.BuyerStoreId, filter.BuyerStoreId);
            query = query.Where(q => q.PhoneNumber, filter.PhoneNumber);
            query = query.Where(q => q.StoreAddress, filter.StoreAddress);
            query = query.Where(q => q.DeliveryAddress, filter.DeliveryAddress);
            query = query.Where(q => q.SaleEmployeeId, filter.AppUserId);
            query = query.Where(q => q.OrderDate, filter.OrderDate);
            query = query.Where(q => q.DeliveryDate, filter.DeliveryDate);
            query = query.Where(q => q.EditedPriceStatusId, filter.EditedPriceStatusId);
            query = query.Where(q => q.Note, filter.Note);
            query = query.Where(q => q.RequestStateId, filter.RequestStateId);
            query = query.Where(q => q.StoreApprovalStateId, filter.StoreApprovalStateId);
            query = query.Where(q => q.ErpApprovalStateId, filter.ErpApprovalStateId);
            query = query.Where(q => q.SubTotal, filter.SubTotal);
            query = query.Where(q => q.GeneralDiscountPercentage, filter.GeneralDiscountPercentage);
            query = query.Where(q => q.GeneralDiscountAmount, filter.GeneralDiscountAmount);
            query = query.Where(q => q.PromotionCode, filter.PromotionCode);
            query = query.Where(q => q.TotalTaxAmount, filter.TotalTaxAmount);
            query = query.Where(q => q.Total, filter.Total);
            query = query.Where(q => q.BuyerStore.StoreStatusId, filter.StoreStatusId);
            query = query.Where(q => q.StoreBalanceCheckStateId, filter.StoreBalanceCheckStateId);
            query = query.Where(q => q.InventoryCheckStateId, filter.InventoryCheckStateId);
            query = query.Where(q => q.DirectSalesOrderSourceTypeId, filter.DirectSalesOrderSourceTypeId);
            query = query.Where(q => q.UpdatedAt, filter.UpdatedAt);
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                List<string> Tokens = filter.Search.Split(" ").Select(x => x.ToLower()).ToList();
                var queryForCode = query;
                var queryForName = query;
                var queryForUnsignName = query;
                foreach (string Token in Tokens)
                {
                    if (string.IsNullOrWhiteSpace(Token))
                        continue;
                    queryForCode = queryForCode.Where(x => x.Code.ToLower().Contains(Token));
                    queryForName = queryForName.Where(x => x.BuyerStore.Name.ToLower().Contains(Token));
                    queryForUnsignName = queryForUnsignName.Where(x => x.BuyerStore.UnsignName.ToLower().Contains(Token));
                }
                query = queryForCode.Union(queryForName).Union(queryForUnsignName);
                query = query.Distinct();
            }
            return query;
        }

        private IQueryable<DirectSalesOrderDAO> OrFilter(IQueryable<DirectSalesOrderDAO> query, DirectSalesOrderFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<DirectSalesOrderDAO> initQuery = query.Where(q => false);
            foreach (DirectSalesOrderFilter DirectSalesOrderFilter in filter.OrFilter)
            {
                IQueryable<DirectSalesOrderDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, DirectSalesOrderFilter.Id);
                if (DirectSalesOrderFilter.OrganizationId != null)
                {
                    if (DirectSalesOrderFilter.OrganizationId.Equal != null)
                    {
                        OrganizationDAO OrganizationDAO = DataContext.Organization
                            .Where(o => o.Id == DirectSalesOrderFilter.OrganizationId.Equal.Value).FirstOrDefault();
                        queryable = queryable.Where(q => q.Organization.Path.StartsWith(OrganizationDAO.Path));
                    }
                    if (DirectSalesOrderFilter.OrganizationId.NotEqual != null)
                    {
                        OrganizationDAO OrganizationDAO = DataContext.Organization
                            .Where(o => o.Id == DirectSalesOrderFilter.OrganizationId.NotEqual.Value).FirstOrDefault();
                        queryable = queryable.Where(q => !q.Organization.Path.StartsWith(OrganizationDAO.Path));
                    }
                    if (DirectSalesOrderFilter.OrganizationId.In != null)
                    {
                        List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                            .Where(o => o.DeletedAt == null && o.StatusId == 1).ToListWithNoLock();
                        List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => DirectSalesOrderFilter.OrganizationId.In.Contains(o.Id)).ToList();
                        List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> Ids = Branches.Select(o => o.Id).ToList();
                        IdFilter IdFilter = new IdFilter { In = Ids };
                        queryable = queryable.Where(x => x.OrganizationId, IdFilter);
                    }
                    if (DirectSalesOrderFilter.OrganizationId.NotIn != null)
                    {
                        List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                            .Where(o => o.DeletedAt == null && o.StatusId == 1).ToListWithNoLock();
                        List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => DirectSalesOrderFilter.OrganizationId.NotIn.Contains(o.Id)).ToList();
                        List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> Ids = Branches.Select(o => o.Id).ToList();
                        IdFilter IdFilter = new IdFilter { NotIn = Ids };
                        queryable = queryable.Where(x => x.OrganizationId, IdFilter);
                    }
                }
                queryable = queryable.Where(q => q.GeneralApprovalStateId, DirectSalesOrderFilter.GeneralApprovalStateId);
                queryable = queryable.Where(q => q.Code, DirectSalesOrderFilter.Code);
                queryable = queryable.Where(q => q.BuyerStoreId, DirectSalesOrderFilter.BuyerStoreId);
                queryable = queryable.Where(q => q.PhoneNumber, DirectSalesOrderFilter.PhoneNumber);
                queryable = queryable.Where(q => q.StoreAddress, DirectSalesOrderFilter.StoreAddress);
                queryable = queryable.Where(q => q.DeliveryAddress, DirectSalesOrderFilter.DeliveryAddress);
                //queryable = queryable.Where(q => q.SaleEmployeeId, DirectSalesOrderFilter.AppUserId);
                queryable = queryable.Where(q => q.OrderDate, DirectSalesOrderFilter.OrderDate);
                queryable = queryable.Where(q => q.DeliveryDate, DirectSalesOrderFilter.DeliveryDate);
                queryable = queryable.Where(q => q.EditedPriceStatusId, DirectSalesOrderFilter.EditedPriceStatusId);
                queryable = queryable.Where(q => q.Note, DirectSalesOrderFilter.Note);
                queryable = queryable.Where(q => q.RequestStateId, DirectSalesOrderFilter.RequestStateId);
                queryable = queryable.Where(q => q.SubTotal, DirectSalesOrderFilter.SubTotal);
                queryable = queryable.Where(q => q.GeneralDiscountPercentage, DirectSalesOrderFilter.GeneralDiscountPercentage);
                queryable = queryable.Where(q => q.GeneralDiscountAmount, DirectSalesOrderFilter.GeneralDiscountAmount);
                queryable = queryable.Where(q => q.TotalTaxAmount, DirectSalesOrderFilter.TotalTaxAmount);
                queryable = queryable.Where(q => q.Total, DirectSalesOrderFilter.Total);
                if (DirectSalesOrderFilter.AppUserId != null)
                {
                    if (DirectSalesOrderFilter.AppUserId.Equal.HasValue)
                    {
                        queryable = queryable.Where(x => x.CreatorId == DirectSalesOrderFilter.AppUserId.Equal.Value || x.SaleEmployeeId == DirectSalesOrderFilter.AppUserId.Equal.Value);
                    }
                    else if (DirectSalesOrderFilter.AppUserId.In != null && DirectSalesOrderFilter.AppUserId.In.Count > 0)
                    {
                        queryable = queryable.Where(x => DirectSalesOrderFilter.AppUserId.In.Contains(x.CreatorId.Value) || DirectSalesOrderFilter.AppUserId.In.Contains(x.SaleEmployeeId));
                    }
                }
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<DirectSalesOrderDAO> DynamicOrder(IQueryable<DirectSalesOrderDAO> query, DirectSalesOrderFilter filter)
        {
            query = query.Distinct();
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case DirectSalesOrderOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case DirectSalesOrderOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case DirectSalesOrderOrder.Organization:
                            query = query.OrderBy(q => q.OrganizationId);
                            break;
                        case DirectSalesOrderOrder.BuyerStore:
                            query = query.OrderBy(q => q.BuyerStoreId);
                            break;
                        case DirectSalesOrderOrder.PhoneNumber:
                            query = query.OrderBy(q => q.PhoneNumber);
                            break;
                        case DirectSalesOrderOrder.StoreAddress:
                            query = query.OrderBy(q => q.StoreAddress);
                            break;
                        case DirectSalesOrderOrder.DeliveryAddress:
                            query = query.OrderBy(q => q.DeliveryAddress);
                            break;
                        case DirectSalesOrderOrder.SaleEmployee:
                            query = query.OrderBy(q => q.SaleEmployeeId);
                            break;
                        case DirectSalesOrderOrder.OrderDate:
                            query = query.OrderBy(q => q.OrderDate);
                            break;
                        case DirectSalesOrderOrder.DeliveryDate:
                            query = query.OrderBy(q => q.DeliveryDate);
                            break;
                        case DirectSalesOrderOrder.EditedPriceStatus:
                            query = query.OrderBy(q => q.EditedPriceStatusId);
                            break;
                        case DirectSalesOrderOrder.Note:
                            query = query.OrderBy(q => q.Note);
                            break;
                        case DirectSalesOrderOrder.RequestState:
                            query = query.OrderBy(q => q.RequestStateId);
                            break;
                        case DirectSalesOrderOrder.StoreApprovalStateId:
                            query = query.OrderBy(q => q.StoreApprovalStateId);
                            break;
                        case DirectSalesOrderOrder.ErpApprovalStateId:
                            query = query.OrderBy(q => q.ErpApprovalStateId);
                            break;
                        case DirectSalesOrderOrder.SubTotal:
                            query = query.OrderBy(q => q.SubTotal);
                            break;
                        case DirectSalesOrderOrder.GeneralDiscountPercentage:
                            query = query.OrderBy(q => q.GeneralDiscountPercentage);
                            break;
                        case DirectSalesOrderOrder.GeneralDiscountAmount:
                            query = query.OrderBy(q => q.GeneralDiscountAmount);
                            break;
                        case DirectSalesOrderOrder.TotalTaxAmount:
                            query = query.OrderBy(q => q.TotalTaxAmount);
                            break;
                        case DirectSalesOrderOrder.Total:
                            query = query.OrderBy(q => q.Total);
                            break;
                        case DirectSalesOrderOrder.CreatedAt:
                            query = query.OrderBy(q => q.CreatedAt);
                            break;
                        case DirectSalesOrderOrder.UpdatedAt:
                            query = query.OrderBy(q => q.UpdatedAt);
                            break;
                        case DirectSalesOrderOrder.GeneralApprovalStateId:
                            query = query.OrderBy(q => q.GeneralApprovalStateId);
                            break;
                        case DirectSalesOrderOrder.InventoryCheckStateId:
                            query = query.OrderBy(q => q.InventoryCheckStateId);
                            break;
                        case DirectSalesOrderOrder.StoreBalanceCheckStateId:
                            query = query.OrderBy(q => q.StoreBalanceCheckStateId);
                            break;
                        default:
                            query = query.OrderBy(q => q.UpdatedAt);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case DirectSalesOrderOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case DirectSalesOrderOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case DirectSalesOrderOrder.Organization:
                            query = query.OrderByDescending(q => q.OrganizationId);
                            break;
                        case DirectSalesOrderOrder.BuyerStore:
                            query = query.OrderByDescending(q => q.BuyerStoreId);
                            break;
                        case DirectSalesOrderOrder.PhoneNumber:
                            query = query.OrderByDescending(q => q.PhoneNumber);
                            break;
                        case DirectSalesOrderOrder.StoreAddress:
                            query = query.OrderByDescending(q => q.StoreAddress);
                            break;
                        case DirectSalesOrderOrder.DeliveryAddress:
                            query = query.OrderByDescending(q => q.DeliveryAddress);
                            break;
                        case DirectSalesOrderOrder.SaleEmployee:
                            query = query.OrderByDescending(q => q.SaleEmployeeId);
                            break;
                        case DirectSalesOrderOrder.OrderDate:
                            query = query.OrderByDescending(q => q.OrderDate);
                            break;
                        case DirectSalesOrderOrder.DeliveryDate:
                            query = query.OrderByDescending(q => q.DeliveryDate);
                            break;
                        case DirectSalesOrderOrder.EditedPriceStatus:
                            query = query.OrderByDescending(q => q.EditedPriceStatusId);
                            break;
                        case DirectSalesOrderOrder.Note:
                            query = query.OrderByDescending(q => q.Note);
                            break;
                        case DirectSalesOrderOrder.RequestState:
                            query = query.OrderByDescending(q => q.RequestStateId);
                            break;
                        case DirectSalesOrderOrder.StoreApprovalStateId:
                            query = query.OrderByDescending(q => q.StoreApprovalStateId);
                            break;
                        case DirectSalesOrderOrder.ErpApprovalStateId:
                            query = query.OrderByDescending(q => q.ErpApprovalStateId);
                            break;
                        case DirectSalesOrderOrder.SubTotal:
                            query = query.OrderByDescending(q => q.SubTotal);
                            break;
                        case DirectSalesOrderOrder.GeneralDiscountPercentage:
                            query = query.OrderByDescending(q => q.GeneralDiscountPercentage);
                            break;
                        case DirectSalesOrderOrder.GeneralDiscountAmount:
                            query = query.OrderByDescending(q => q.GeneralDiscountAmount);
                            break;
                        case DirectSalesOrderOrder.TotalTaxAmount:
                            query = query.OrderByDescending(q => q.TotalTaxAmount);
                            break;
                        case DirectSalesOrderOrder.Total:
                            query = query.OrderByDescending(q => q.Total);
                            break;
                        case DirectSalesOrderOrder.CreatedAt:
                            query = query.OrderByDescending(q => q.CreatedAt);
                            break;
                        case DirectSalesOrderOrder.UpdatedAt:
                            query = query.OrderByDescending(q => q.UpdatedAt);
                            break;
                        case DirectSalesOrderOrder.GeneralApprovalStateId:
                            query = query.OrderByDescending(q => q.GeneralApprovalStateId);
                            break;
                        case DirectSalesOrderOrder.InventoryCheckStateId:
                            query = query.OrderByDescending(q => q.InventoryCheckStateId);
                            break;
                        case DirectSalesOrderOrder.StoreBalanceCheckStateId:
                            query = query.OrderByDescending(q => q.StoreBalanceCheckStateId);
                            break;
                        default:
                            query = query.OrderByDescending(q => q.UpdatedAt);
                            break;
                    }
                    break;
                default:
                    query = query.OrderByDescending(q => q.UpdatedAt);
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<DirectSalesOrder>> DynamicSelect(IQueryable<DirectSalesOrderDAO> query, DirectSalesOrderFilter filter)
        {
            List<DirectSalesOrder> DirectSalesOrders = await query.Select(q => new DirectSalesOrder()
            {
                Id = filter.Selects.Contains(DirectSalesOrderSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(DirectSalesOrderSelect.Code) ? q.Code : default(string),
                OrganizationId = filter.Selects.Contains(DirectSalesOrderSelect.Organization) ? q.OrganizationId : default(long),
                BuyerStoreId = filter.Selects.Contains(DirectSalesOrderSelect.BuyerStore) ? q.BuyerStoreId : default(long),
                PhoneNumber = filter.Selects.Contains(DirectSalesOrderSelect.PhoneNumber) ? q.PhoneNumber : default(string),
                StoreAddress = filter.Selects.Contains(DirectSalesOrderSelect.StoreAddress) ? q.StoreAddress : default(string),
                DeliveryAddress = filter.Selects.Contains(DirectSalesOrderSelect.DeliveryAddress) ? q.DeliveryAddress : default(string),
                SaleEmployeeId = filter.Selects.Contains(DirectSalesOrderSelect.SaleEmployee) ? q.SaleEmployeeId : default(long),
                OrderDate = filter.Selects.Contains(DirectSalesOrderSelect.OrderDate) ? q.OrderDate : default(DateTime),
                DeliveryDate = filter.Selects.Contains(DirectSalesOrderSelect.DeliveryDate) ? q.DeliveryDate : default(DateTime?),
                EditedPriceStatusId = filter.Selects.Contains(DirectSalesOrderSelect.EditedPriceStatus) ? q.EditedPriceStatusId : default(long),
                Note = filter.Selects.Contains(DirectSalesOrderSelect.Note) ? q.Note : default(string),
                RequestStateId = filter.Selects.Contains(DirectSalesOrderSelect.RequestState) ? q.RequestStateId : default(long),
                StoreApprovalStateId = filter.Selects.Contains(DirectSalesOrderSelect.StoreApprovalState) ? q.StoreApprovalStateId : default(long),
                ErpApprovalStateId = filter.Selects.Contains(DirectSalesOrderSelect.ErpApprovalState) ? q.ErpApprovalStateId : default(long),
                SubTotal = filter.Selects.Contains(DirectSalesOrderSelect.SubTotal) ? q.SubTotal : default(decimal),
                GeneralDiscountPercentage = filter.Selects.Contains(DirectSalesOrderSelect.GeneralDiscountPercentage) ? q.GeneralDiscountPercentage : default(decimal?),
                GeneralDiscountAmount = filter.Selects.Contains(DirectSalesOrderSelect.GeneralDiscountAmount) ? q.GeneralDiscountAmount : default(decimal?),
                PromotionCode = filter.Selects.Contains(DirectSalesOrderSelect.PromotionCode) ? q.PromotionCode : default(string),
                PromotionValue = filter.Selects.Contains(DirectSalesOrderSelect.PromotionValue) ? q.PromotionValue : default(decimal?),
                TotalTaxAmount = filter.Selects.Contains(DirectSalesOrderSelect.TotalTaxAmount) ? q.TotalTaxAmount : default(decimal),
                TotalAfterTax = filter.Selects.Contains(DirectSalesOrderSelect.TotalAfterTax) ? q.TotalAfterTax : default(decimal),
                Total = filter.Selects.Contains(DirectSalesOrderSelect.Total) ? q.Total : default(decimal),
                CreatedAt = filter.Selects.Contains(DirectSalesOrderSelect.CreatedAt) ? q.CreatedAt : default(DateTime),
                UpdatedAt = filter.Selects.Contains(DirectSalesOrderSelect.UpdatedAt) ? q.UpdatedAt : default(DateTime),
                GeneralApprovalStateId = filter.Selects.Contains(DirectSalesOrderSelect.GeneralApprovalState) ? q.GeneralApprovalStateId : default(long),
                StoreBalanceCheckStateId = filter.Selects.Contains(DirectSalesOrderSelect.StoreBalanceCheckState) ? q.StoreBalanceCheckStateId : default(long),
                InventoryCheckStateId = filter.Selects.Contains(DirectSalesOrderSelect.InventoryCheckState) ? q.InventoryCheckStateId : default(long),
                DirectSalesOrderSourceTypeId = filter.Selects.Contains(DirectSalesOrderSelect.DirectSalesOrderSourceType) ? q.DirectSalesOrderSourceTypeId : default(long),
                StoreCheckingId = filter.Selects.Contains(DirectSalesOrderSelect.StoreChecking) ? q.StoreCheckingId : null,
                GeneralApprovalState = filter.Selects.Contains(DirectSalesOrderSelect.GeneralApprovalState) && q.GeneralApprovalState == null ? null : new GeneralApprovalState
                {
                    Id = q.GeneralApprovalState.Id,
                    Code = q.GeneralApprovalState.Code,
                    Name = q.GeneralApprovalState.Name,
                },
                DirectSalesOrderSourceType = filter.Selects.Contains(DirectSalesOrderSelect.DirectSalesOrderSourceType) && q.DirectSalesOrderSourceType == null ? null : new DirectSalesOrderSourceType
                {
                    Id = q.DirectSalesOrderSourceType.Id,
                    Code = q.DirectSalesOrderSourceType.Code,
                    Name = q.DirectSalesOrderSourceType.Name,
                },
                StoreBalanceCheckState = filter.Selects.Contains(DirectSalesOrderSelect.StoreBalanceCheckState) && q.StoreBalanceCheckState == null ? null : new CheckState
                {
                    Id = q.StoreBalanceCheckState.Id,
                    Code = q.StoreBalanceCheckState.Code,
                    Name = q.StoreBalanceCheckState.Name,
                },
                InventoryCheckState = filter.Selects.Contains(DirectSalesOrderSelect.InventoryCheckState) && q.StoreBalanceCheckState == null ? null : new CheckState
                {
                    Id = q.InventoryCheckState.Id,
                    Code = q.InventoryCheckState.Code,
                    Name = q.InventoryCheckState.Name,
                },
                BuyerStore = filter.Selects.Contains(DirectSalesOrderSelect.BuyerStore) && q.BuyerStore != null ? new Store
                {
                    Id = q.BuyerStore.Id,
                    Code = q.BuyerStore.Code,
                    CodeDraft = q.BuyerStore.CodeDraft,
                    Name = q.BuyerStore.Name,
                    ParentStoreId = q.BuyerStore.ParentStoreId,
                    OrganizationId = q.BuyerStore.OrganizationId,
                    StoreTypeId = q.BuyerStore.StoreTypeId,
                    Telephone = q.BuyerStore.Telephone,
                    ProvinceId = q.BuyerStore.ProvinceId,
                    DistrictId = q.BuyerStore.DistrictId,
                    WardId = q.BuyerStore.WardId,
                    Address = q.BuyerStore.Address,
                    DeliveryAddress = q.BuyerStore.DeliveryAddress,
                    Latitude = q.BuyerStore.Latitude,
                    Longitude = q.BuyerStore.Longitude,
                    DeliveryLatitude = q.BuyerStore.DeliveryLatitude,
                    DeliveryLongitude = q.BuyerStore.DeliveryLongitude,
                    OwnerName = q.BuyerStore.OwnerName,
                    OwnerPhone = q.BuyerStore.OwnerPhone,
                    OwnerEmail = q.BuyerStore.OwnerEmail,
                    TaxCode = q.BuyerStore.TaxCode,
                    LegalEntity = q.BuyerStore.LegalEntity,
                    StatusId = q.BuyerStore.StatusId,
                    StoreStatus = q.BuyerStore.StoreStatus == null ? null : new StoreStatus
                    {
                        Id = q.BuyerStore.StoreStatus.Id,
                        Code = q.BuyerStore.StoreStatus.Code,
                        Name = q.BuyerStore.StoreStatus.Name,
                    },
                    Province = q.BuyerStore.Province == null ? null : new Province
                    {
                        Id = q.BuyerStore.Province.Id,
                        Code = q.BuyerStore.Province.Code,
                        Name = q.BuyerStore.Province.Name,
                    },
                    District = q.BuyerStore.District == null ? null : new District
                    {
                        Id = q.BuyerStore.District.Id,
                        Code = q.BuyerStore.District.Code,
                        Name = q.BuyerStore.District.Name,
                    },
                    StoreType = q.BuyerStore.StoreType == null ? null : new StoreType
                    {
                        Id = q.BuyerStore.StoreType.Id,
                        Code = q.BuyerStore.StoreType.Code,
                        Name = q.BuyerStore.StoreType.Name,
                    }
                } : null,
                EditedPriceStatus = filter.Selects.Contains(DirectSalesOrderSelect.EditedPriceStatus) && q.EditedPriceStatus != null ? new EditedPriceStatus
                {
                    Id = q.EditedPriceStatus.Id,
                    Code = q.EditedPriceStatus.Code,
                    Name = q.EditedPriceStatus.Name,
                } : null,
                Organization = filter.Selects.Contains(DirectSalesOrderSelect.Organization) && q.Organization != null ? new Organization
                {
                    Id = q.Organization.Id,
                    Code = q.Organization.Code,
                    Name = q.Organization.Name,
                    Address = q.Organization.Address,
                    Phone = q.Organization.Phone,
                    Path = q.Organization.Path,
                    ParentId = q.Organization.ParentId,
                    Email = q.Organization.Email,
                    StatusId = q.Organization.StatusId,
                    Level = q.Organization.Level
                } : null,
                RequestState = filter.Selects.Contains(DirectSalesOrderSelect.RequestState) && q.RequestState != null ? new RequestState
                {
                    Id = q.RequestState.Id,
                    Code = q.RequestState.Code,
                    Name = q.RequestState.Name,
                } : null,
                StoreApprovalState = filter.Selects.Contains(DirectSalesOrderSelect.StoreApprovalState) && q.StoreApprovalState != null ? new StoreApprovalState
                {
                    Id = q.StoreApprovalState.Id,
                    Code = q.StoreApprovalState.Code,
                    Name = q.StoreApprovalState.Name,
                } : null,
                ErpApprovalState = filter.Selects.Contains(DirectSalesOrderSelect.ErpApprovalState) && q.ErpApprovalState != null ? new ErpApprovalState
                {
                    Id = q.ErpApprovalState.Id,
                    Code = q.ErpApprovalState.Code,
                    Name = q.ErpApprovalState.Name,
                } : null,
                SaleEmployee = filter.Selects.Contains(DirectSalesOrderSelect.SaleEmployee) && q.SaleEmployee != null ? new AppUser
                {
                    Id = q.SaleEmployee.Id,
                    Username = q.SaleEmployee.Username,
                    DisplayName = q.SaleEmployee.DisplayName,
                    Address = q.SaleEmployee.Address,
                    Email = q.SaleEmployee.Email,
                    Phone = q.SaleEmployee.Phone,
                } : null,
                RowId = q.RowId
            }).ToListWithNoLockAsync();


            var DirectSalesOrderIds = DirectSalesOrders.Select(x => x.Id).ToList();
            IdFilter DirectSalesOrderIdFilter = new IdFilter();
            if (filter.Selects.Contains(DirectSalesOrderSelect.DirectSalesOrderContents))
            {
                var DirectSalesOrderContents = await DataContext.DirectSalesOrderContent
                    .AsNoTracking()
                    .Where(x => x.DirectSalesOrderId, DirectSalesOrderIdFilter)
                    .Select(x => new DirectSalesOrderContent
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
                        EditedPriceStatusId = x.EditedPriceStatusId,
                        DiscountPercentage = x.DiscountPercentage,
                        DiscountAmount = x.DiscountAmount,
                        GeneralDiscountAmount = x.GeneralDiscountAmount,
                        GeneralDiscountPercentage = x.GeneralDiscountPercentage,
                        TaxAmount = x.TaxAmount,
                        TaxPercentage = x.TaxPercentage,
                        Amount = x.Amount,
                        Factor = x.Factor,
                        InventoryCheckStateId = x.InventoryCheckStateId,
                        Item = x.Item == null ? null : new Item
                        {
                            Id = x.Item.Id,
                            Code = x.Item.Code,
                            Name = x.Item.Name,
                            ProductId = x.Item.ProductId,
                            ERPCode = x.Item.ERPCode
                        },
                        UnitOfMeasure = x.UnitOfMeasure == null ? null : new UnitOfMeasure
                        {
                            Id = x.UnitOfMeasure.Id,
                            Code = x.UnitOfMeasure.Code,
                            Name = x.UnitOfMeasure.Name
                        }
                    }).ToListWithNoLockAsync();
                foreach (DirectSalesOrder DirectSalesOrder in DirectSalesOrders)
                {
                    DirectSalesOrder.DirectSalesOrderContents = DirectSalesOrderContents.Where(x => x.DirectSalesOrderId == DirectSalesOrder.Id).ToList();
                }
            }
            if (filter.Selects.Contains(DirectSalesOrderSelect.DirectSalesOrderPromotions))
            {
                var DirectSalesOrderPromotions = await DataContext.DirectSalesOrderPromotion.AsNoTracking()
                    .Where(x => x.DirectSalesOrderId, DirectSalesOrderIdFilter)
                    .Select(x => new DirectSalesOrderPromotion
                    {
                        Id = x.Id,
                        DirectSalesOrderId = x.DirectSalesOrderId,
                        ItemId = x.ItemId,
                        UnitOfMeasureId = x.UnitOfMeasureId,
                        Quantity = x.Quantity,
                        PrimaryUnitOfMeasureId = x.PrimaryUnitOfMeasureId,
                        RequestedQuantity = x.RequestedQuantity,
                        Factor = x.Factor,
                        InventoryCheckStateId = x.InventoryCheckStateId,
                        Item = x.Item == null ? null : new Item
                        {
                            Id = x.Item.Id,
                            Code = x.Item.Code,
                            Name = x.Item.Name,
                            ProductId = x.Item.ProductId,
                            ERPCode = x.Item.ERPCode
                        },
                        UnitOfMeasure = x.UnitOfMeasure == null ? null : new UnitOfMeasure
                        {
                            Id = x.UnitOfMeasure.Id,
                            Code = x.UnitOfMeasure.Code,
                            Name = x.UnitOfMeasure.Name
                        }
                    }).ToListWithNoLockAsync();
                foreach (DirectSalesOrder DirectSalesOrder in DirectSalesOrders)
                {
                    DirectSalesOrder.DirectSalesOrderPromotions = DirectSalesOrderPromotions.Where(x => x.DirectSalesOrderId == DirectSalesOrder.Id).ToList();
                }
            }
            if (filter.Selects.Contains(DirectSalesOrderSelect.BuyerStore))
            {
                var BuyerStoreIds = DirectSalesOrders.Select(x => x.BuyerStoreId).ToList();
                var StoreStoreGroupingMappings = await DataContext.StoreStoreGroupingMapping.AsNoTracking()
                    .Where(x => x.StoreId, new IdFilter { In = BuyerStoreIds })
                    .Select(x => new StoreStoreGroupingMapping
                    {
                        StoreId = x.StoreId,
                        StoreGroupingId = x.StoreGroupingId,
                        Store = x.Store == null ? null : new Store
                        {
                            Id = x.Store.Id,
                            Code = x.Store.Code,
                            Name = x.Store.Name
                        },
                        StoreGrouping = x.StoreGrouping == null ? null : new StoreGrouping
                        {
                            Id = x.StoreGrouping.Id,
                            Code = x.StoreGrouping.Code,
                            Name = x.StoreGrouping.Name
                        },
                    }).ToListWithNoLockAsync();
                foreach (DirectSalesOrder DirectSalesOrder in DirectSalesOrders)
                {
                    DirectSalesOrder.BuyerStore.StoreStoreGroupingMappings = StoreStoreGroupingMappings.Where(x => x.StoreId == DirectSalesOrder.BuyerStoreId).ToList();
                }
            }


            return DirectSalesOrders;
        }

        public async Task<int> Count(DirectSalesOrderFilter filter)
        {
            IQueryable<DirectSalesOrderDAO> DirectSalesOrderDAOs = DataContext.DirectSalesOrder.AsNoTracking();
            DirectSalesOrderDAOs = DynamicFilter(DirectSalesOrderDAOs, filter);
            DirectSalesOrderDAOs = OrFilter(DirectSalesOrderDAOs, filter);
            return await DirectSalesOrderDAOs.Distinct().CountWithNoLockAsync();
        }

        public async Task<List<DirectSalesOrder>> List(DirectSalesOrderFilter filter)
        {
            if (filter == null) return new List<DirectSalesOrder>();
            IQueryable<DirectSalesOrderDAO> DirectSalesOrderDAOs = DataContext.DirectSalesOrder.AsNoTracking();
            DirectSalesOrderDAOs = DynamicFilter(DirectSalesOrderDAOs, filter);
            DirectSalesOrderDAOs = OrFilter(DirectSalesOrderDAOs, filter);
            DirectSalesOrderDAOs = DynamicOrder(DirectSalesOrderDAOs, filter);
            List<DirectSalesOrder> DirectSalesOrders = await DynamicSelect(DirectSalesOrderDAOs, filter);
            return DirectSalesOrders;
        }

        public async Task<int> CountAll(DirectSalesOrderFilter filter)
        {
            IQueryable<DirectSalesOrderDAO> DirectSalesOrderDAOs = DataContext.DirectSalesOrder.AsNoTracking();
            DirectSalesOrderDAOs = DynamicFilter(DirectSalesOrderDAOs, filter);
            var query1 = from q in DirectSalesOrderDAOs
                         where q.RequestStateId == RequestStateEnum.NEW.Id &&
                         q.SaleEmployeeId == filter.ApproverId.Equal
                         select q;
            var query2 = from q in DirectSalesOrderDAOs
                         join r in DataContext.RequestWorkflowDefinitionMapping on q.RowId equals r.RequestId
                         join step in DataContext.WorkflowStep on r.WorkflowDefinitionId equals step.WorkflowDefinitionId
                         join rstep in DataContext.RequestWorkflowStepMapping on step.Id equals rstep.WorkflowStepId
                         where rstep.AppUserId == filter.ApproverId.Equal
                         select q;
            DirectSalesOrderDAOs = query1.Union(query2);
            int count = await DirectSalesOrderDAOs.Distinct().CountWithNoLockAsync();
            return count;
        }

        public async Task<List<DirectSalesOrder>> ListAll(DirectSalesOrderFilter filter)
        {
            if (filter == null) return new List<DirectSalesOrder>();
            IQueryable<DirectSalesOrderDAO> DirectSalesOrderDAOs = DataContext.DirectSalesOrder.AsNoTracking();
            DirectSalesOrderDAOs = DynamicFilter(DirectSalesOrderDAOs, filter);
            var query1 = from q in DirectSalesOrderDAOs
                         where q.RequestStateId == RequestStateEnum.NEW.Id &&
                         q.SaleEmployeeId == filter.ApproverId.Equal
                         select q;
            var query2 = from q in DirectSalesOrderDAOs
                         join r in DataContext.RequestWorkflowDefinitionMapping on q.RowId equals r.RequestId
                         join step in DataContext.WorkflowStep on r.WorkflowDefinitionId equals step.WorkflowDefinitionId
                         join rstep in DataContext.RequestWorkflowStepMapping on step.Id equals rstep.WorkflowStepId
                         where rstep.AppUserId == filter.ApproverId.Equal
                         select q;
            DirectSalesOrderDAOs = query1.Union(query2);
            DirectSalesOrderDAOs = DynamicOrder(DirectSalesOrderDAOs, filter);
            List<DirectSalesOrder> DirectSalesOrders = await DynamicSelect(DirectSalesOrderDAOs, filter);
            return DirectSalesOrders;
        }

        public async Task<int> CountNew(DirectSalesOrderFilter filter)
        {
            IQueryable<DirectSalesOrderDAO> DirectSalesOrderDAOs = DataContext.DirectSalesOrder.AsNoTracking();
            DirectSalesOrderDAOs = DynamicFilter(DirectSalesOrderDAOs, filter);
            DirectSalesOrderDAOs = from q in DirectSalesOrderDAOs
                                   where (q.GeneralApprovalStateId == GeneralApprovalStateEnum.REJECTED.Id
                                   || q.GeneralApprovalStateId == GeneralApprovalStateEnum.STORE_REJECTED.Id
                                   || q.GeneralApprovalStateId == GeneralApprovalStateEnum.NEW.Id) &&
                                   q.CreatorId == filter.ApproverId.Equal
                                   select q;

            return await DirectSalesOrderDAOs.Distinct().CountWithNoLockAsync();
        }

        public async Task<List<DirectSalesOrder>> ListNew(DirectSalesOrderFilter filter)
        {
            if (filter == null) return new List<DirectSalesOrder>();
            IQueryable<DirectSalesOrderDAO> DirectSalesOrderDAOs = DataContext.DirectSalesOrder.AsNoTracking();
            DirectSalesOrderDAOs = DynamicFilter(DirectSalesOrderDAOs, filter);
            DirectSalesOrderDAOs = from q in DirectSalesOrderDAOs
                                   where (q.GeneralApprovalStateId == GeneralApprovalStateEnum.REJECTED.Id
                                   || q.GeneralApprovalStateId == GeneralApprovalStateEnum.STORE_REJECTED.Id
                                   || q.GeneralApprovalStateId == GeneralApprovalStateEnum.NEW.Id) &&
                                   (q.CreatorId == filter.ApproverId.Equal || q.SaleEmployeeId == filter.ApproverId.Equal)
                                   select q;

            DirectSalesOrderDAOs = DynamicOrder(DirectSalesOrderDAOs, filter);
            List<DirectSalesOrder> DirectSalesOrders = await DynamicSelect(DirectSalesOrderDAOs, filter);
            return DirectSalesOrders;
        }

        public async Task<int> CountPending(DirectSalesOrderFilter filter)
        {
            IQueryable<DirectSalesOrderDAO> DirectSalesOrderDAOs = DataContext.DirectSalesOrder.AsNoTracking();
            DirectSalesOrderDAOs = DynamicFilter(DirectSalesOrderDAOs, filter);
            if (filter.ApproverId.Equal.HasValue)
            {
                DirectSalesOrderDAOs = from q in DirectSalesOrderDAOs
                                       join r in DataContext.RequestWorkflowDefinitionMapping.Where(x => x.RequestStateId == RequestStateEnum.PENDING.Id) on q.RowId equals r.RequestId
                                       join step in DataContext.WorkflowStep on r.WorkflowDefinitionId equals step.WorkflowDefinitionId
                                       join rstep in DataContext.RequestWorkflowStepMapping.Where(x => x.WorkflowStateId == WorkflowStateEnum.PENDING.Id) on step.Id equals rstep.WorkflowStepId
                                       join ra in DataContext.AppUserRoleMapping on step.RoleId equals ra.RoleId
                                       where ra.AppUserId == filter.ApproverId.Equal && q.RowId == rstep.RequestId
                                       select q;
            }
            return await DirectSalesOrderDAOs.Distinct().CountWithNoLockAsync();
        }

        public async Task<List<DirectSalesOrder>> ListPending(DirectSalesOrderFilter filter)
        {
            if (filter == null) return new List<DirectSalesOrder>();
            IQueryable<DirectSalesOrderDAO> DirectSalesOrderDAOs = DataContext.DirectSalesOrder.AsNoTracking();
            DirectSalesOrderDAOs = DynamicFilter(DirectSalesOrderDAOs, filter);
            if (filter.ApproverId.Equal.HasValue)
            {
                DirectSalesOrderDAOs = from q in DirectSalesOrderDAOs
                                       join r in DataContext.RequestWorkflowDefinitionMapping.Where(x => x.RequestStateId == RequestStateEnum.PENDING.Id) on q.RowId equals r.RequestId
                                       join step in DataContext.WorkflowStep on r.WorkflowDefinitionId equals step.WorkflowDefinitionId
                                       join rstep in DataContext.RequestWorkflowStepMapping.Where(x => x.WorkflowStateId == WorkflowStateEnum.PENDING.Id) on step.Id equals rstep.WorkflowStepId
                                       join ra in DataContext.AppUserRoleMapping on step.RoleId equals ra.RoleId
                                       where ra.AppUserId == filter.ApproverId.Equal && q.RowId == rstep.RequestId
                                       select q;
            }
            DirectSalesOrderDAOs = DynamicOrder(DirectSalesOrderDAOs, filter);
            List<DirectSalesOrder> DirectSalesOrders = await DynamicSelect(DirectSalesOrderDAOs, filter);
            return DirectSalesOrders;
        }

        public async Task<int> CountCompleted(DirectSalesOrderFilter filter)
        {
            IQueryable<DirectSalesOrderDAO> DirectSalesOrderDAOs = DataContext.DirectSalesOrder.AsNoTracking();
            DirectSalesOrderDAOs = DynamicFilter(DirectSalesOrderDAOs, filter);
            if (filter.ApproverId.Equal.HasValue)
            {
                var query1 = from q in DirectSalesOrderDAOs
                             join r in DataContext.RequestWorkflowDefinitionMapping on q.RowId equals r.RequestId
                             join step in DataContext.WorkflowStep on r.WorkflowDefinitionId equals step.WorkflowDefinitionId
                             join rstep in DataContext.RequestWorkflowStepMapping on step.Id equals rstep.WorkflowStepId
                             where
                             (q.GeneralApprovalStateId != GeneralApprovalStateEnum.NEW.Id) &&
                             (rstep.WorkflowStateId == WorkflowStateEnum.APPROVED.Id || rstep.WorkflowStateId == WorkflowStateEnum.REJECTED.Id) &&
                             rstep.AppUserId == filter.ApproverId.Equal && rstep.RequestId == q.RowId
                             select q;
                var query2 = from q in DirectSalesOrderDAOs
                             join r in DataContext.RequestWorkflowDefinitionMapping on q.RowId equals r.RequestId into result
                             from r in result.DefaultIfEmpty()
                             where r == null && q.GeneralApprovalStateId != GeneralApprovalStateEnum.NEW.Id && q.GeneralApprovalStateId != GeneralApprovalStateEnum.STORE_DRAFT.Id
                             && q.CreatorId == filter.ApproverId.Equal
                             select q;
                DirectSalesOrderDAOs = query1.Union(query2);
            }
            return await DirectSalesOrderDAOs.Distinct().CountWithNoLockAsync();
        }

        public async Task<List<DirectSalesOrder>> ListCompleted(DirectSalesOrderFilter filter)
        {
            if (filter == null) return new List<DirectSalesOrder>();
            IQueryable<DirectSalesOrderDAO> DirectSalesOrderDAOs = DataContext.DirectSalesOrder.AsNoTracking();
            DirectSalesOrderDAOs = DynamicFilter(DirectSalesOrderDAOs, filter);
            if (filter.ApproverId.Equal.HasValue)
            {
                var query1 = from q in DirectSalesOrderDAOs
                             join r in DataContext.RequestWorkflowDefinitionMapping on q.RowId equals r.RequestId
                             join step in DataContext.WorkflowStep on r.WorkflowDefinitionId equals step.WorkflowDefinitionId
                             join rstep in DataContext.RequestWorkflowStepMapping on step.Id equals rstep.WorkflowStepId
                             where
                             (q.GeneralApprovalStateId != GeneralApprovalStateEnum.NEW.Id) &&
                             (rstep.WorkflowStateId == WorkflowStateEnum.APPROVED.Id || rstep.WorkflowStateId == WorkflowStateEnum.REJECTED.Id) &&
                             rstep.AppUserId == filter.ApproverId.Equal && rstep.RequestId == q.RowId
                             select q;
                var query2 = from q in DirectSalesOrderDAOs
                             join r in DataContext.RequestWorkflowDefinitionMapping on q.RowId equals r.RequestId into result
                             from r in result.DefaultIfEmpty()
                             where r == null && q.GeneralApprovalStateId != GeneralApprovalStateEnum.NEW.Id && q.GeneralApprovalStateId != GeneralApprovalStateEnum.STORE_DRAFT.Id
                             && q.CreatorId == filter.ApproverId.Equal
                             select q;
                DirectSalesOrderDAOs = query1.Union(query2);
            }
            DirectSalesOrderDAOs = DynamicOrder(DirectSalesOrderDAOs, filter);
            List<DirectSalesOrder> DirectSalesOrders = await DynamicSelect(DirectSalesOrderDAOs, filter);
            return DirectSalesOrders;
        }

        public async Task<int> CountInScoped(DirectSalesOrderFilter filter)
        {
            IQueryable<DirectSalesOrderDAO> DirectSalesOrderDAOs = DataContext.DirectSalesOrder.AsNoTracking();
            DirectSalesOrderDAOs = DynamicFilter(DirectSalesOrderDAOs, filter);
            if (filter.ApproverId.Equal.HasValue)
            {
                DirectSalesOrderDAOs = DirectSalesOrderDAOs.Where(x => x.SaleEmployeeId == filter.ApproverId.Equal || x.CreatorId == filter.ApproverId.Equal);

            }
            return await DirectSalesOrderDAOs.Distinct().CountWithNoLockAsync();
        }
        public async Task<List<DirectSalesOrder>> ListInScoped(DirectSalesOrderFilter filter)
        {
            if (filter == null) return new List<DirectSalesOrder>();
            IQueryable<DirectSalesOrderDAO> DirectSalesOrderDAOs = DataContext.DirectSalesOrder.AsNoTracking();
            DirectSalesOrderDAOs = DynamicFilter(DirectSalesOrderDAOs, filter);
            if (filter.ApproverId.Equal.HasValue)
            {
                DirectSalesOrderDAOs = DirectSalesOrderDAOs.Where(x => x.SaleEmployeeId == filter.ApproverId.Equal || x.CreatorId == filter.ApproverId.Equal);
            }
            DirectSalesOrderDAOs = DynamicOrder(DirectSalesOrderDAOs, filter);
            List<DirectSalesOrder> DirectSalesOrders = await DynamicSelect(DirectSalesOrderDAOs, filter);
            return DirectSalesOrders;
        }


        public async Task<DirectSalesOrder> Get(long Id)
        {
            DirectSalesOrder DirectSalesOrder = await DataContext.DirectSalesOrder.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new DirectSalesOrder()
            {
                Id = x.Id,
                Code = x.Code,
                OrganizationId = x.OrganizationId,
                BuyerStoreId = x.BuyerStoreId,
                PhoneNumber = x.PhoneNumber,
                StoreAddress = x.StoreAddress,
                DeliveryAddress = x.DeliveryAddress,
                SaleEmployeeId = x.SaleEmployeeId,
                OrderDate = x.OrderDate,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeliveryDate = x.DeliveryDate,
                EditedPriceStatusId = x.EditedPriceStatusId,
                Note = x.Note,
                RequestStateId = x.RequestStateId,
                ErpApprovalStateId = x.ErpApprovalStateId,
                StoreApprovalStateId = x.StoreApprovalStateId,
                SubTotal = x.SubTotal,
                GeneralDiscountPercentage = x.GeneralDiscountPercentage,
                GeneralDiscountAmount = x.GeneralDiscountAmount,
                PromotionCode = x.PromotionCode,
                PromotionValue = x.PromotionValue,
                TotalTaxAmount = x.TotalTaxAmount,
                TotalAfterTax = x.TotalAfterTax,
                Total = x.Total,
                RowId = x.RowId,
                StoreCheckingId = x.StoreCheckingId,
                CreatorId = x.CreatorId,
                GeneralApprovalStateId = x.GeneralApprovalStateId,
                StoreBalanceCheckStateId = x.StoreBalanceCheckStateId,
                InventoryCheckStateId = x.InventoryCheckStateId,
                DirectSalesOrderSourceTypeId = x.DirectSalesOrderSourceTypeId,
                GeneralApprovalState = x.GeneralApprovalState == null ? null : new GeneralApprovalState
                {
                    Id = x.GeneralApprovalState.Id,
                    Code = x.GeneralApprovalState.Code,
                    Name = x.GeneralApprovalState.Name,
                },
                DirectSalesOrderSourceType = x.DirectSalesOrderSourceType == null ? null : new DirectSalesOrderSourceType
                {
                    Id = x.DirectSalesOrderSourceType.Id,
                    Code = x.DirectSalesOrderSourceType.Code,
                    Name = x.DirectSalesOrderSourceType.Name,
                },
                BuyerStore = x.BuyerStore == null ? null : new Store
                {
                    Id = x.BuyerStore.Id,
                    Code = x.BuyerStore.Code,
                    CodeDraft = x.BuyerStore.CodeDraft,
                    Name = x.BuyerStore.Name,
                    ParentStoreId = x.BuyerStore.ParentStoreId,
                    OrganizationId = x.BuyerStore.OrganizationId,
                    StoreTypeId = x.BuyerStore.StoreTypeId,
                    Telephone = x.BuyerStore.Telephone,
                    ProvinceId = x.BuyerStore.ProvinceId,
                    DistrictId = x.BuyerStore.DistrictId,
                    WardId = x.BuyerStore.WardId,
                    Address = x.BuyerStore.Address,
                    DeliveryAddress = x.BuyerStore.DeliveryAddress,
                    Latitude = x.BuyerStore.Latitude,
                    Longitude = x.BuyerStore.Longitude,
                    DeliveryLatitude = x.BuyerStore.DeliveryLatitude,
                    DeliveryLongitude = x.BuyerStore.DeliveryLongitude,
                    OwnerName = x.BuyerStore.OwnerName,
                    OwnerPhone = x.BuyerStore.OwnerPhone,
                    OwnerEmail = x.BuyerStore.OwnerEmail,
                    TaxCode = x.BuyerStore.TaxCode,
                    LegalEntity = x.BuyerStore.LegalEntity,
                    StatusId = x.BuyerStore.StatusId,
                    IsStoreApprovalDirectSalesOrder = x.BuyerStore.IsStoreApprovalDirectSalesOrder,
                },
                EditedPriceStatus = x.EditedPriceStatus == null ? null : new EditedPriceStatus
                {
                    Id = x.EditedPriceStatus.Id,
                    Code = x.EditedPriceStatus.Code,
                    Name = x.EditedPriceStatus.Name,
                },
                Organization = x.Organization == null ? null : new Organization
                {
                    Id = x.Organization.Id,
                    Code = x.Organization.Code,
                    Name = x.Organization.Name,
                    Address = x.Organization.Address,
                    Phone = x.Organization.Phone,
                    Path = x.Organization.Path,
                    ParentId = x.Organization.ParentId,
                    Email = x.Organization.Email,
                    StatusId = x.Organization.StatusId,
                    Level = x.Organization.Level
                },
                RequestState = x.RequestState == null ? null : new RequestState
                {
                    Id = x.RequestState.Id,
                    Code = x.RequestState.Code,
                    Name = x.RequestState.Name,
                },
                StoreApprovalState = x.StoreApprovalState == null ? null : new StoreApprovalState
                {
                    Id = x.StoreApprovalState.Id,
                    Code = x.StoreApprovalState.Code,
                    Name = x.StoreApprovalState.Name,
                },
                StoreBalanceCheckState = x.StoreBalanceCheckState == null ? null : new CheckState
                {
                    Id = x.StoreBalanceCheckState.Id,
                    Code = x.StoreBalanceCheckState.Code,
                    Name = x.StoreBalanceCheckState.Name,
                },
                InventoryCheckState = x.InventoryCheckState == null ? null : new CheckState
                {
                    Id = x.InventoryCheckState.Id,
                    Code = x.InventoryCheckState.Code,
                    Name = x.InventoryCheckState.Name,
                },
                ErpApprovalState = x.ErpApprovalState == null ? null : new ErpApprovalState
                {
                    Id = x.ErpApprovalState.Id,
                    Code = x.ErpApprovalState.Code,
                    Name = x.ErpApprovalState.Name,
                },
                SaleEmployee = x.SaleEmployee == null ? null : new AppUser
                {
                    Id = x.SaleEmployee.Id,
                    Username = x.SaleEmployee.Username,
                    DisplayName = x.SaleEmployee.DisplayName,
                    Address = x.SaleEmployee.Address,
                    Email = x.SaleEmployee.Email,
                    Phone = x.SaleEmployee.Phone,
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (DirectSalesOrder == null)
                return null;

            RequestWorkflowDefinitionMappingDAO RequestWorkflowDefinitionMappingDAO = await DataContext.RequestWorkflowDefinitionMapping
              .Where(x => DirectSalesOrder.RowId == x.RequestId)
              .Include(x => x.RequestState)
              .AsNoTracking()
              .FirstOrDefaultWithNoLockAsync();
            if (RequestWorkflowDefinitionMappingDAO != null)
            {
                DirectSalesOrder.RequestStateId = RequestWorkflowDefinitionMappingDAO.RequestStateId;
                DirectSalesOrder.RequestState = new RequestState
                {
                    Id = RequestWorkflowDefinitionMappingDAO.RequestState.Id,
                    Code = RequestWorkflowDefinitionMappingDAO.RequestState.Code,
                    Name = RequestWorkflowDefinitionMappingDAO.RequestState.Name,
                };
            }

            DirectSalesOrder.DirectSalesOrderContents = await DataContext.DirectSalesOrderContent.AsNoTracking()
                .Where(x => x.DirectSalesOrderId == DirectSalesOrder.Id)
                .Select(x => new DirectSalesOrderContent
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
                    EditedPriceStatusId = x.EditedPriceStatusId,
                    DiscountPercentage = x.DiscountPercentage,
                    DiscountAmount = x.DiscountAmount,
                    GeneralDiscountPercentage = x.GeneralDiscountPercentage,
                    GeneralDiscountAmount = x.GeneralDiscountAmount,
                    Amount = x.Amount,
                    TaxPercentage = x.TaxPercentage,
                    TaxAmount = x.TaxAmount,
                    Factor = x.Factor,
                    InventoryCheckStateId = x.InventoryCheckStateId,
                    EditedPriceStatus = x.EditedPriceStatus == null ? null : new EditedPriceStatus
                    {
                        Id = x.EditedPriceStatus.Id,
                        Code = x.EditedPriceStatus.Code,
                        Name = x.EditedPriceStatus.Name,
                    },
                    Item = new Item
                    {
                        Id = x.Item.Id,
                        Code = x.Item.Code,
                        ERPCode = x.Item.ERPCode,
                        Name = x.Item.Name,
                        ProductId = x.Item.ProductId,
                        RetailPrice = x.Item.RetailPrice,
                        SalePrice = x.Item.SalePrice,
                        ScanCode = x.Item.ScanCode,
                        StatusId = x.Item.StatusId,
                        Product = new Product
                        {
                            Id = x.Item.Product.Id,
                            Code = x.Item.Product.Code,
                            Name = x.Item.Product.Name,
                            TaxTypeId = x.Item.Product.TaxTypeId,
                            UnitOfMeasureId = x.Item.Product.UnitOfMeasureId,
                            UnitOfMeasureGroupingId = x.Item.Product.UnitOfMeasureGroupingId,
                            TaxType = new TaxType
                            {
                                Id = x.Item.Product.TaxType.Id,
                                Code = x.Item.Product.TaxType.Code,
                                Name = x.Item.Product.TaxType.Name,
                                StatusId = x.Item.Product.TaxType.StatusId,
                                Percentage = x.Item.Product.TaxType.Percentage,
                            },
                            UnitOfMeasure = new UnitOfMeasure
                            {
                                Id = x.Item.Product.UnitOfMeasure.Id,
                                Code = x.Item.Product.UnitOfMeasure.Code,
                                Name = x.Item.Product.UnitOfMeasure.Name,
                                Description = x.Item.Product.UnitOfMeasure.Description,
                                StatusId = x.Item.Product.UnitOfMeasure.StatusId,
                            },
                            UnitOfMeasureGrouping = new UnitOfMeasureGrouping
                            {
                                Id = x.Item.Product.UnitOfMeasureGrouping.Id,
                                Code = x.Item.Product.UnitOfMeasureGrouping.Code,
                                Name = x.Item.Product.UnitOfMeasureGrouping.Name,
                                Description = x.Item.Product.UnitOfMeasureGrouping.Description,
                                StatusId = x.Item.Product.UnitOfMeasureGrouping.StatusId,
                                UnitOfMeasureId = x.Item.Product.UnitOfMeasureGrouping.UnitOfMeasureId
                            }
                        }
                    },
                    PrimaryUnitOfMeasure = new UnitOfMeasure
                    {
                        Id = x.PrimaryUnitOfMeasure.Id,
                        Code = x.PrimaryUnitOfMeasure.Code,
                        Name = x.PrimaryUnitOfMeasure.Name,
                        Description = x.PrimaryUnitOfMeasure.Description,
                        StatusId = x.PrimaryUnitOfMeasure.StatusId,
                    },
                    UnitOfMeasure = new UnitOfMeasure
                    {
                        Id = x.UnitOfMeasure.Id,
                        Code = x.UnitOfMeasure.Code,
                        Name = x.UnitOfMeasure.Name,
                        Description = x.UnitOfMeasure.Description,
                        StatusId = x.UnitOfMeasure.StatusId,
                    },
                }).ToListWithNoLockAsync();
            DirectSalesOrder.DirectSalesOrderPromotions = await DataContext.DirectSalesOrderPromotion.AsNoTracking()
                .Where(x => x.DirectSalesOrderId == DirectSalesOrder.Id)
                .Select(x => new DirectSalesOrderPromotion
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
                    InventoryCheckStateId = x.InventoryCheckStateId,
                    Item = new Item
                    {
                        Id = x.Item.Id,
                        Code = x.Item.Code,
                        ERPCode = x.Item.ERPCode,
                        Name = x.Item.Name,
                        ProductId = x.Item.ProductId,
                        RetailPrice = x.Item.RetailPrice,
                        SalePrice = x.Item.SalePrice,
                        ScanCode = x.Item.ScanCode,
                        StatusId = x.Item.StatusId,
                        Product = new Product
                        {
                            Id = x.Item.Product.Id,
                            Code = x.Item.Product.Code,
                            Name = x.Item.Product.Name,
                            TaxTypeId = x.Item.Product.TaxTypeId,
                            UnitOfMeasureId = x.Item.Product.UnitOfMeasureId,
                            UnitOfMeasureGroupingId = x.Item.Product.UnitOfMeasureGroupingId,
                            TaxType = new TaxType
                            {
                                Id = x.Item.Product.TaxType.Id,
                                Code = x.Item.Product.TaxType.Code,
                                Name = x.Item.Product.TaxType.Name,
                                StatusId = x.Item.Product.TaxType.StatusId,
                                Percentage = x.Item.Product.TaxType.Percentage,
                            },
                            UnitOfMeasure = new UnitOfMeasure
                            {
                                Id = x.Item.Product.UnitOfMeasure.Id,
                                Code = x.Item.Product.UnitOfMeasure.Code,
                                Name = x.Item.Product.UnitOfMeasure.Name,
                                Description = x.Item.Product.UnitOfMeasure.Description,
                                StatusId = x.Item.Product.UnitOfMeasure.StatusId,
                            },
                            UnitOfMeasureGrouping = new UnitOfMeasureGrouping
                            {
                                Id = x.Item.Product.UnitOfMeasureGrouping.Id,
                                Code = x.Item.Product.UnitOfMeasureGrouping.Code,
                                Name = x.Item.Product.UnitOfMeasureGrouping.Name,
                                Description = x.Item.Product.UnitOfMeasureGrouping.Description,
                                StatusId = x.Item.Product.UnitOfMeasureGrouping.StatusId,
                                UnitOfMeasureId = x.Item.Product.UnitOfMeasureGrouping.UnitOfMeasureId
                            }
                        }
                    },
                    PrimaryUnitOfMeasure = new UnitOfMeasure
                    {
                        Id = x.PrimaryUnitOfMeasure.Id,
                        Code = x.PrimaryUnitOfMeasure.Code,
                        Name = x.PrimaryUnitOfMeasure.Name,
                        Description = x.PrimaryUnitOfMeasure.Description,
                        StatusId = x.PrimaryUnitOfMeasure.StatusId,
                    },
                    UnitOfMeasure = new UnitOfMeasure
                    {
                        Id = x.UnitOfMeasure.Id,
                        Code = x.UnitOfMeasure.Code,
                        Name = x.UnitOfMeasure.Name,
                        Description = x.UnitOfMeasure.Description,
                        StatusId = x.UnitOfMeasure.StatusId,
                    },
                }).ToListWithNoLockAsync();

            decimal GeneralDiscountAmount = DirectSalesOrder.GeneralDiscountAmount.HasValue ? DirectSalesOrder.GeneralDiscountAmount.Value : 0;
            decimal DiscountAmount = DirectSalesOrder.DirectSalesOrderContents != null ?
                DirectSalesOrder.DirectSalesOrderContents
                .Select(x => x.DiscountAmount.GetValueOrDefault(0))
                .Sum() : 0;
            DirectSalesOrder.TotalDiscountAmount = GeneralDiscountAmount + DiscountAmount;
            DirectSalesOrder.TotalRequestedQuantity = DirectSalesOrder.DirectSalesOrderContents != null ?
                DirectSalesOrder.DirectSalesOrderContents
                .Select(x => x.RequestedQuantity)
                .Sum() : 0;
            return DirectSalesOrder;
        }

        public async Task<List<DirectSalesOrder>> List(List<long> Ids)
        {
            ITempTableQuery<TempTable<long>> tempTableQuery = await DataContext
              .BulkInsertValuesIntoTempTableAsync<long>(Ids);

            var query = from s in DataContext.DirectSalesOrder
                        join tt in tempTableQuery.Query on s.Id equals tt.Column1
                        select s;
            List<DirectSalesOrder> DirectSalesOrders = await query.AsNoTracking()
                .Select(x => new DirectSalesOrder()
                {
                    Id = x.Id,
                    Code = x.Code,
                    OrganizationId = x.OrganizationId,
                    BuyerStoreId = x.BuyerStoreId,
                    PhoneNumber = x.PhoneNumber,
                    StoreAddress = x.StoreAddress,
                    DeliveryAddress = x.DeliveryAddress,
                    SaleEmployeeId = x.SaleEmployeeId,
                    OrderDate = x.OrderDate,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    DeliveryDate = x.DeliveryDate,
                    EditedPriceStatusId = x.EditedPriceStatusId,
                    Note = x.Note,
                    RequestStateId = x.RequestStateId,
                    ErpApprovalStateId = x.ErpApprovalStateId,
                    StoreApprovalStateId = x.StoreApprovalStateId,
                    SubTotal = x.SubTotal,
                    GeneralDiscountPercentage = x.GeneralDiscountPercentage,
                    GeneralDiscountAmount = x.GeneralDiscountAmount,
                    PromotionCode = x.PromotionCode,
                    PromotionValue = x.PromotionValue,
                    TotalTaxAmount = x.TotalTaxAmount,
                    TotalAfterTax = x.TotalAfterTax,
                    Total = x.Total,
                    RowId = x.RowId,
                    StoreCheckingId = x.StoreCheckingId,
                    CreatorId = x.CreatorId,
                    GeneralApprovalStateId = x.GeneralApprovalStateId,
                    StoreBalanceCheckStateId = x.StoreBalanceCheckStateId,
                    InventoryCheckStateId = x.InventoryCheckStateId,
                    DirectSalesOrderSourceTypeId = x.DirectSalesOrderSourceTypeId,
                    GeneralApprovalState = x.GeneralApprovalState == null ? null : new GeneralApprovalState
                    {
                        Id = x.GeneralApprovalState.Id,
                        Code = x.GeneralApprovalState.Code,
                        Name = x.GeneralApprovalState.Name,
                    },
                    DirectSalesOrderSourceType = x.DirectSalesOrderSourceType == null ? null : new DirectSalesOrderSourceType
                    {
                        Id = x.DirectSalesOrderSourceType.Id,
                        Code = x.DirectSalesOrderSourceType.Code,
                        Name = x.DirectSalesOrderSourceType.Name,
                    },
                    BuyerStore = x.BuyerStore == null ? null : new Store
                    {
                        Id = x.BuyerStore.Id,
                        Code = x.BuyerStore.Code,
                        CodeDraft = x.BuyerStore.CodeDraft,
                        Name = x.BuyerStore.Name,
                        ParentStoreId = x.BuyerStore.ParentStoreId,
                        OrganizationId = x.BuyerStore.OrganizationId,
                        StoreTypeId = x.BuyerStore.StoreTypeId,
                        Telephone = x.BuyerStore.Telephone,
                        ProvinceId = x.BuyerStore.ProvinceId,
                        DistrictId = x.BuyerStore.DistrictId,
                        WardId = x.BuyerStore.WardId,
                        Address = x.BuyerStore.Address,
                        DeliveryAddress = x.BuyerStore.DeliveryAddress,
                        Latitude = x.BuyerStore.Latitude,
                        Longitude = x.BuyerStore.Longitude,
                        DeliveryLatitude = x.BuyerStore.DeliveryLatitude,
                        DeliveryLongitude = x.BuyerStore.DeliveryLongitude,
                        OwnerName = x.BuyerStore.OwnerName,
                        OwnerPhone = x.BuyerStore.OwnerPhone,
                        OwnerEmail = x.BuyerStore.OwnerEmail,
                        TaxCode = x.BuyerStore.TaxCode,
                        LegalEntity = x.BuyerStore.LegalEntity,
                        StatusId = x.BuyerStore.StatusId,
                    },
                    EditedPriceStatus = x.EditedPriceStatus == null ? null : new EditedPriceStatus
                    {
                        Id = x.EditedPriceStatus.Id,
                        Code = x.EditedPriceStatus.Code,
                        Name = x.EditedPriceStatus.Name,
                    },
                    InventoryCheckState = x.InventoryCheckState == null ? null : new CheckState
                    {
                        Id = x.InventoryCheckState.Id,
                        Code = x.InventoryCheckState.Code,
                        Name = x.InventoryCheckState.Name,
                    },
                    StoreBalanceCheckState = x.StoreBalanceCheckState == null ? null : new CheckState
                    {
                        Id = x.StoreBalanceCheckState.Id,
                        Code = x.StoreBalanceCheckState.Code,
                        Name = x.StoreBalanceCheckState.Name,
                    },
                    Organization = x.Organization == null ? null : new Organization
                    {
                        Id = x.Organization.Id,
                        Code = x.Organization.Code,
                        Name = x.Organization.Name,
                        Address = x.Organization.Address,
                        Phone = x.Organization.Phone,
                        Path = x.Organization.Path,
                        ParentId = x.Organization.ParentId,
                        Email = x.Organization.Email,
                        StatusId = x.Organization.StatusId,
                        Level = x.Organization.Level
                    },
                    RequestState = x.RequestState == null ? null : new RequestState
                    {
                        Id = x.RequestState.Id,
                        Code = x.RequestState.Code,
                        Name = x.RequestState.Name,
                    },
                    ErpApprovalState = x.ErpApprovalState == null ? null : new ErpApprovalState
                    {
                        Id = x.ErpApprovalState.Id,
                        Code = x.ErpApprovalState.Code,
                        Name = x.ErpApprovalState.Name,
                    },
                    StoreApprovalState = x.StoreApprovalState == null ? null : new StoreApprovalState
                    {
                        Id = x.StoreApprovalState.Id,
                        Code = x.StoreApprovalState.Code,
                        Name = x.StoreApprovalState.Name,
                    },
                    SaleEmployee = x.SaleEmployee == null ? null : new AppUser
                    {
                        Id = x.SaleEmployee.Id,
                        Username = x.SaleEmployee.Username,
                        DisplayName = x.SaleEmployee.DisplayName,
                        Address = x.SaleEmployee.Address,
                        Email = x.SaleEmployee.Email,
                        Phone = x.SaleEmployee.Phone,
                    },
                })
                .ToListWithNoLockAsync();

            List<Guid> RowIds = DirectSalesOrders.Select(x => x.RowId).ToList();
            ITempTableQuery<TempTable<Guid>> tempGuidQuery = await DataContext
             .BulkInsertValuesIntoTempTableAsync<Guid>(RowIds);

            var RequestWorkflowDefinitionMapping = from s in DataContext.RequestWorkflowDefinitionMapping
                                                   join tt in tempGuidQuery.Query on s.RequestId equals tt.Column1
                                                   select s;

            List<RequestWorkflowDefinitionMappingDAO> RequestWorkflowDefinitionMappingDAOs = await RequestWorkflowDefinitionMapping
              .Include(x => x.RequestState)
              .AsNoTracking()
              .ToListWithNoLockAsync();

            var DirectSalesOrderContent = from s in DataContext.DirectSalesOrderContent
                                          join tt in tempTableQuery.Query on s.DirectSalesOrderId equals tt.Column1
                                          select s;

            List<DirectSalesOrderContent> DirectSalesOrderContents = await DirectSalesOrderContent.AsNoTracking()
                .Select(x => new DirectSalesOrderContent
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
                    EditedPriceStatusId = x.EditedPriceStatusId,
                    DiscountPercentage = x.DiscountPercentage,
                    DiscountAmount = x.DiscountAmount,
                    GeneralDiscountPercentage = x.GeneralDiscountPercentage,
                    GeneralDiscountAmount = x.GeneralDiscountAmount,
                    Amount = x.Amount,
                    TaxPercentage = x.TaxPercentage,
                    TaxAmount = x.TaxAmount,
                    Factor = x.Factor,
                    InventoryCheckStateId = x.InventoryCheckStateId,
                    EditedPriceStatus = x.EditedPriceStatus == null ? null : new EditedPriceStatus
                    {
                        Id = x.EditedPriceStatus.Id,
                        Code = x.EditedPriceStatus.Code,
                        Name = x.EditedPriceStatus.Name,
                    },
                    Item = new Item
                    {
                        Id = x.Item.Id,
                        Code = x.Item.Code,
                        Name = x.Item.Name,
                        ProductId = x.Item.ProductId,
                        RetailPrice = x.Item.RetailPrice,
                        SalePrice = x.Item.SalePrice,
                        ScanCode = x.Item.ScanCode,
                        StatusId = x.Item.StatusId,
                        Product = new Product
                        {
                            Id = x.Item.Product.Id,
                            Code = x.Item.Product.Code,
                            Name = x.Item.Product.Name,
                            TaxTypeId = x.Item.Product.TaxTypeId,
                            UnitOfMeasureId = x.Item.Product.UnitOfMeasureId,
                            UnitOfMeasureGroupingId = x.Item.Product.UnitOfMeasureGroupingId,
                            TaxType = new TaxType
                            {
                                Id = x.Item.Product.TaxType.Id,
                                Code = x.Item.Product.TaxType.Code,
                                Name = x.Item.Product.TaxType.Name,
                                StatusId = x.Item.Product.TaxType.StatusId,
                                Percentage = x.Item.Product.TaxType.Percentage,
                            },
                            UnitOfMeasure = new UnitOfMeasure
                            {
                                Id = x.Item.Product.UnitOfMeasure.Id,
                                Code = x.Item.Product.UnitOfMeasure.Code,
                                Name = x.Item.Product.UnitOfMeasure.Name,
                                Description = x.Item.Product.UnitOfMeasure.Description,
                                StatusId = x.Item.Product.UnitOfMeasure.StatusId,
                            },
                            UnitOfMeasureGrouping = new UnitOfMeasureGrouping
                            {
                                Id = x.Item.Product.UnitOfMeasureGrouping.Id,
                                Code = x.Item.Product.UnitOfMeasureGrouping.Code,
                                Name = x.Item.Product.UnitOfMeasureGrouping.Name,
                                Description = x.Item.Product.UnitOfMeasureGrouping.Description,
                                StatusId = x.Item.Product.UnitOfMeasureGrouping.StatusId,
                                UnitOfMeasureId = x.Item.Product.UnitOfMeasureGrouping.UnitOfMeasureId
                            }
                        }
                    },
                    PrimaryUnitOfMeasure = new UnitOfMeasure
                    {
                        Id = x.PrimaryUnitOfMeasure.Id,
                        Code = x.PrimaryUnitOfMeasure.Code,
                        Name = x.PrimaryUnitOfMeasure.Name,
                        Description = x.PrimaryUnitOfMeasure.Description,
                        StatusId = x.PrimaryUnitOfMeasure.StatusId,
                    },
                    UnitOfMeasure = new UnitOfMeasure
                    {
                        Id = x.UnitOfMeasure.Id,
                        Code = x.UnitOfMeasure.Code,
                        Name = x.UnitOfMeasure.Name,
                        Description = x.UnitOfMeasure.Description,
                        StatusId = x.UnitOfMeasure.StatusId,
                    },
                }).ToListWithNoLockAsync();

            var DirectSalesOrderPromotion = from s in DataContext.DirectSalesOrderPromotion
                                            join tt in tempTableQuery.Query on s.DirectSalesOrderId equals tt.Column1
                                            select s;


            List<DirectSalesOrderPromotion> DirectSalesOrderPromotions = await DirectSalesOrderPromotion.AsNoTracking()
                .Select(x => new DirectSalesOrderPromotion
                {
                    Id = x.Id,
                    DirectSalesOrderId = x.DirectSalesOrderId,
                    ItemId = x.ItemId,
                    UnitOfMeasureId = x.UnitOfMeasureId,
                    Quantity = x.Quantity,
                    PrimaryUnitOfMeasureId = x.PrimaryUnitOfMeasureId,
                    RequestedQuantity = x.RequestedQuantity,
                    Note = x.Note,
                    InventoryCheckStateId = x.InventoryCheckStateId,
                    Factor = x.Factor,
                    Item = new Item
                    {
                        Id = x.Item.Id,
                        Code = x.Item.Code,
                        Name = x.Item.Name,
                        ProductId = x.Item.ProductId,
                        RetailPrice = x.Item.RetailPrice,
                        SalePrice = x.Item.SalePrice,
                        ScanCode = x.Item.ScanCode,
                        StatusId = x.Item.StatusId,
                        Product = new Product
                        {
                            Id = x.Item.Product.Id,
                            Code = x.Item.Product.Code,
                            Name = x.Item.Product.Name,
                            TaxTypeId = x.Item.Product.TaxTypeId,
                            UnitOfMeasureId = x.Item.Product.UnitOfMeasureId,
                            UnitOfMeasureGroupingId = x.Item.Product.UnitOfMeasureGroupingId,
                            TaxType = new TaxType
                            {
                                Id = x.Item.Product.TaxType.Id,
                                Code = x.Item.Product.TaxType.Code,
                                Name = x.Item.Product.TaxType.Name,
                                StatusId = x.Item.Product.TaxType.StatusId,
                                Percentage = x.Item.Product.TaxType.Percentage,
                            },
                            UnitOfMeasure = new UnitOfMeasure
                            {
                                Id = x.Item.Product.UnitOfMeasure.Id,
                                Code = x.Item.Product.UnitOfMeasure.Code,
                                Name = x.Item.Product.UnitOfMeasure.Name,
                                Description = x.Item.Product.UnitOfMeasure.Description,
                                StatusId = x.Item.Product.UnitOfMeasure.StatusId,
                            },
                            UnitOfMeasureGrouping = new UnitOfMeasureGrouping
                            {
                                Id = x.Item.Product.UnitOfMeasureGrouping.Id,
                                Code = x.Item.Product.UnitOfMeasureGrouping.Code,
                                Name = x.Item.Product.UnitOfMeasureGrouping.Name,
                                Description = x.Item.Product.UnitOfMeasureGrouping.Description,
                                StatusId = x.Item.Product.UnitOfMeasureGrouping.StatusId,
                                UnitOfMeasureId = x.Item.Product.UnitOfMeasureGrouping.UnitOfMeasureId
                            }
                        }
                    },
                    PrimaryUnitOfMeasure = new UnitOfMeasure
                    {
                        Id = x.PrimaryUnitOfMeasure.Id,
                        Code = x.PrimaryUnitOfMeasure.Code,
                        Name = x.PrimaryUnitOfMeasure.Name,
                        Description = x.PrimaryUnitOfMeasure.Description,
                        StatusId = x.PrimaryUnitOfMeasure.StatusId,
                    },
                    UnitOfMeasure = new UnitOfMeasure
                    {
                        Id = x.UnitOfMeasure.Id,
                        Code = x.UnitOfMeasure.Code,
                        Name = x.UnitOfMeasure.Name,
                        Description = x.UnitOfMeasure.Description,
                        StatusId = x.UnitOfMeasure.StatusId,
                    },
                }).ToListWithNoLockAsync();

            foreach (var DirectSalesOrder in DirectSalesOrders)
            {
                RequestWorkflowDefinitionMappingDAO RequestWorkflowDefinitionMappingDAO = RequestWorkflowDefinitionMappingDAOs
                    .Where(x => x.RequestId == DirectSalesOrder.RowId)
                    .FirstOrDefault();
                if (RequestWorkflowDefinitionMappingDAO != null)
                {
                    DirectSalesOrder.RequestStateId = RequestWorkflowDefinitionMappingDAO.RequestStateId;
                    DirectSalesOrder.RequestState = new RequestState
                    {
                        Id = RequestWorkflowDefinitionMappingDAO.RequestState.Id,
                        Code = RequestWorkflowDefinitionMappingDAO.RequestState.Code,
                        Name = RequestWorkflowDefinitionMappingDAO.RequestState.Name,
                    };
                }

                DirectSalesOrder.DirectSalesOrderContents = DirectSalesOrderContents
                    .Where(x => x.DirectSalesOrderId == DirectSalesOrder.Id)
                    .ToList();

                DirectSalesOrder.DirectSalesOrderPromotions = DirectSalesOrderPromotions
                    .Where(x => x.DirectSalesOrderId == DirectSalesOrder.Id)
                    .ToList();

                decimal GeneralDiscountAmount = DirectSalesOrder.GeneralDiscountAmount.HasValue ? DirectSalesOrder.GeneralDiscountAmount.Value : 0;
                decimal DiscountAmount = DirectSalesOrder.DirectSalesOrderContents != null ?
                    DirectSalesOrder.DirectSalesOrderContents
                    .Select(x => x.DiscountAmount.GetValueOrDefault(0))
                    .Sum() : 0;
                DirectSalesOrder.TotalDiscountAmount = GeneralDiscountAmount + DiscountAmount;
                DirectSalesOrder.TotalRequestedQuantity = DirectSalesOrder.DirectSalesOrderContents != null ?
                    DirectSalesOrder.DirectSalesOrderContents
                    .Select(x => x.RequestedQuantity)
                    .Sum() : 0;
            };
            return DirectSalesOrders;
        }

        public async Task<bool> Create(DirectSalesOrder DirectSalesOrder)
        {
            DirectSalesOrderDAO DirectSalesOrderDAO = new DirectSalesOrderDAO();
            DirectSalesOrderDAO.Id = DirectSalesOrder.Id;
            DirectSalesOrderDAO.Code = DirectSalesOrder.Code;
            DirectSalesOrderDAO.OrganizationId = DirectSalesOrder.OrganizationId;
            DirectSalesOrderDAO.BuyerStoreId = DirectSalesOrder.BuyerStoreId;
            DirectSalesOrderDAO.BuyerStoreTypeId = DirectSalesOrder.BuyerStoreTypeId;
            DirectSalesOrderDAO.PhoneNumber = DirectSalesOrder.PhoneNumber;
            DirectSalesOrderDAO.StoreAddress = DirectSalesOrder.StoreAddress;
            DirectSalesOrderDAO.DeliveryAddress = DirectSalesOrder.DeliveryAddress;
            DirectSalesOrderDAO.SaleEmployeeId = DirectSalesOrder.SaleEmployeeId;
            DirectSalesOrderDAO.OrderDate = DirectSalesOrder.OrderDate;
            DirectSalesOrderDAO.DeliveryDate = DirectSalesOrder.DeliveryDate;
            DirectSalesOrderDAO.EditedPriceStatusId = DirectSalesOrder.EditedPriceStatusId;
            DirectSalesOrderDAO.Note = DirectSalesOrder.Note;
            DirectSalesOrderDAO.RequestStateId = DirectSalesOrder.RequestStateId;
            DirectSalesOrderDAO.SubTotal = DirectSalesOrder.SubTotal;
            DirectSalesOrderDAO.GeneralDiscountPercentage = DirectSalesOrder.GeneralDiscountPercentage;
            DirectSalesOrderDAO.GeneralDiscountAmount = DirectSalesOrder.GeneralDiscountAmount;
            DirectSalesOrderDAO.PromotionCode = DirectSalesOrder.PromotionCode;
            DirectSalesOrderDAO.PromotionValue = DirectSalesOrder.PromotionValue;
            DirectSalesOrderDAO.TotalTaxAmount = DirectSalesOrder.TotalTaxAmount;
            DirectSalesOrderDAO.TotalAfterTax = DirectSalesOrder.TotalAfterTax;
            DirectSalesOrderDAO.Total = DirectSalesOrder.Total;
            DirectSalesOrderDAO.RowId = Guid.NewGuid();
            DirectSalesOrderDAO.StoreCheckingId = DirectSalesOrder.StoreCheckingId;
            DirectSalesOrderDAO.CreatorId = DirectSalesOrder.CreatorId;
            DirectSalesOrderDAO.CreatedAt = StaticParams.DateTimeNow;
            DirectSalesOrderDAO.UpdatedAt = StaticParams.DateTimeNow;
            DirectSalesOrderDAO.GeneralApprovalStateId = DirectSalesOrder.GeneralApprovalStateId;
            DirectSalesOrderDAO.StoreBalanceCheckStateId = DirectSalesOrder.StoreBalanceCheckStateId;
            DirectSalesOrderDAO.InventoryCheckStateId = DirectSalesOrder.InventoryCheckStateId;
            DirectSalesOrderDAO.DirectSalesOrderSourceTypeId = DirectSalesOrder.DirectSalesOrderSourceTypeId;
            DataContext.DirectSalesOrder.Add(DirectSalesOrderDAO);
            await DataContext.SaveChangesAsync();
            DirectSalesOrder.Id = DirectSalesOrderDAO.Id;
            DirectSalesOrder.RowId = DirectSalesOrderDAO.RowId;
            await SaveReference(DirectSalesOrder);
            return true;
        }

        public async Task<bool> Update(DirectSalesOrder DirectSalesOrder)
        {
            DirectSalesOrderDAO DirectSalesOrderDAO = DataContext.DirectSalesOrder.Where(x => x.Id == DirectSalesOrder.Id).FirstOrDefault();
            if (DirectSalesOrderDAO == null)
                return false;
            DirectSalesOrderDAO.Id = DirectSalesOrder.Id;
            DirectSalesOrderDAO.Code = DirectSalesOrder.Code;
            DirectSalesOrderDAO.BuyerStoreId = DirectSalesOrder.BuyerStoreId;
            DirectSalesOrderDAO.BuyerStoreTypeId = DirectSalesOrder.BuyerStoreTypeId;
            DirectSalesOrderDAO.PhoneNumber = DirectSalesOrder.PhoneNumber;
            DirectSalesOrderDAO.StoreAddress = DirectSalesOrder.StoreAddress;
            DirectSalesOrderDAO.DeliveryAddress = DirectSalesOrder.DeliveryAddress;
            DirectSalesOrderDAO.SaleEmployeeId = DirectSalesOrder.SaleEmployeeId;
            DirectSalesOrderDAO.OrderDate = DirectSalesOrder.OrderDate;
            DirectSalesOrderDAO.DeliveryDate = DirectSalesOrder.DeliveryDate;
            DirectSalesOrderDAO.EditedPriceStatusId = DirectSalesOrder.EditedPriceStatusId;
            DirectSalesOrderDAO.Note = DirectSalesOrder.Note;
            DirectSalesOrderDAO.RequestStateId = DirectSalesOrder.RequestStateId;
            DirectSalesOrderDAO.ErpApprovalStateId = DirectSalesOrder.ErpApprovalStateId;
            DirectSalesOrderDAO.SubTotal = DirectSalesOrder.SubTotal;
            DirectSalesOrderDAO.GeneralDiscountPercentage = DirectSalesOrder.GeneralDiscountPercentage;
            DirectSalesOrderDAO.GeneralDiscountAmount = DirectSalesOrder.GeneralDiscountAmount;
            DirectSalesOrderDAO.PromotionCode = DirectSalesOrder.PromotionCode;
            DirectSalesOrderDAO.PromotionValue = DirectSalesOrder.PromotionValue;
            DirectSalesOrderDAO.TotalTaxAmount = DirectSalesOrder.TotalTaxAmount;
            DirectSalesOrderDAO.TotalAfterTax = DirectSalesOrder.TotalAfterTax;
            DirectSalesOrderDAO.Total = DirectSalesOrder.Total;
            DirectSalesOrderDAO.StoreCheckingId = DirectSalesOrder.StoreCheckingId;
            DirectSalesOrderDAO.StoreBalanceCheckStateId = DirectSalesOrder.StoreBalanceCheckStateId;
            DirectSalesOrderDAO.InventoryCheckStateId = DirectSalesOrder.InventoryCheckStateId;
            DirectSalesOrderDAO.UpdatedAt = StaticParams.DateTimeNow;
            await DataContext.SaveChangesAsync();
            await SaveReference(DirectSalesOrder);
            return true;
        }
        public async Task<bool> BulkUpdate(List<DirectSalesOrder> DirectSalesOrders)
        {
            IdFilter IdFilter = new IdFilter { In = DirectSalesOrders.Select(x => x.Id).ToList() };
            List<DirectSalesOrderDAO> DirectSalesOrderDAOs = new List<DirectSalesOrderDAO>();
            List<DirectSalesOrderDAO> DbDirectSalesOrderDAOs = await DataContext.DirectSalesOrder
                .Where(x => x.Id, IdFilter)
                .ToListWithNoLockAsync();
            foreach (DirectSalesOrder DirectSalesOrder in DirectSalesOrders)
            {
                DirectSalesOrderDAO DirectSalesOrderDAO = DbDirectSalesOrderDAOs
                        .Where(x => x.Id == DirectSalesOrder.Id)
                        .FirstOrDefault();
                if (DirectSalesOrderDAO == null) continue;
                DirectSalesOrderDAO.Code = DirectSalesOrder.Code;
                DirectSalesOrderDAO.BuyerStoreId = DirectSalesOrder.BuyerStoreId;
                DirectSalesOrderDAO.PhoneNumber = DirectSalesOrder.PhoneNumber;
                DirectSalesOrderDAO.StoreAddress = DirectSalesOrder.StoreAddress;
                DirectSalesOrderDAO.DeliveryAddress = DirectSalesOrder.DeliveryAddress;
                DirectSalesOrderDAO.SaleEmployeeId = DirectSalesOrder.SaleEmployeeId;
                DirectSalesOrderDAO.OrderDate = DirectSalesOrder.OrderDate;
                DirectSalesOrderDAO.DeliveryDate = DirectSalesOrder.DeliveryDate;
                DirectSalesOrderDAO.EditedPriceStatusId = DirectSalesOrder.EditedPriceStatusId;
                DirectSalesOrderDAO.Note = DirectSalesOrder.Note;
                DirectSalesOrderDAO.RequestStateId = DirectSalesOrder.RequestStateId;
                DirectSalesOrderDAO.ErpApprovalStateId = DirectSalesOrder.ErpApprovalStateId;
                DirectSalesOrderDAO.SubTotal = DirectSalesOrder.SubTotal;
                DirectSalesOrderDAO.GeneralDiscountPercentage = DirectSalesOrder.GeneralDiscountPercentage;
                DirectSalesOrderDAO.GeneralDiscountAmount = DirectSalesOrder.GeneralDiscountAmount;
                DirectSalesOrderDAO.PromotionCode = DirectSalesOrder.PromotionCode;
                DirectSalesOrderDAO.PromotionValue = DirectSalesOrder.PromotionValue;
                DirectSalesOrderDAO.TotalTaxAmount = DirectSalesOrder.TotalTaxAmount;
                DirectSalesOrderDAO.TotalAfterTax = DirectSalesOrder.TotalAfterTax;
                DirectSalesOrderDAO.Total = DirectSalesOrder.Total;
                DirectSalesOrderDAO.StoreCheckingId = DirectSalesOrder.StoreCheckingId;
                DirectSalesOrderDAO.StoreBalanceCheckStateId = DirectSalesOrder.StoreBalanceCheckStateId;
                DirectSalesOrderDAO.InventoryCheckStateId = DirectSalesOrder.InventoryCheckStateId;
                DirectSalesOrderDAO.UpdatedAt = StaticParams.DateTimeNow;
                DirectSalesOrderDAOs.Add(DirectSalesOrderDAO);
            }
            await DataContext.BulkMergeAsync(DirectSalesOrderDAOs);
            foreach (DirectSalesOrder DirectSalesOrder in DirectSalesOrders)
            {
                await SaveReference(DirectSalesOrder);
            }
            return true;
        }

        public async Task<bool> Delete(DirectSalesOrder DirectSalesOrder)
        {
            await DataContext.DirectSalesOrderTransaction.Where(x => x.DirectSalesOrderId == DirectSalesOrder.Id).DeleteFromQueryAsync();
            await DataContext.DirectSalesOrderContent.Where(x => x.DirectSalesOrderId == DirectSalesOrder.Id).DeleteFromQueryAsync();
            await DataContext.DirectSalesOrderPromotion.Where(x => x.DirectSalesOrderId == DirectSalesOrder.Id).DeleteFromQueryAsync();
            await DataContext.DirectSalesOrder.Where(x => x.Id == DirectSalesOrder.Id).DeleteFromQueryAsync();
            return true;
        }

        public async Task<bool> BulkMerge(List<DirectSalesOrder> DirectSalesOrders)
        {
            IdFilter IdFilter = new IdFilter { In = DirectSalesOrders.Select(x => x.Id).ToList() };
            List<DirectSalesOrderDAO> DirectSalesOrderDAOs = new List<DirectSalesOrderDAO>();
            List<DirectSalesOrderDAO> DbDirectSalesOrderDAOs = await DataContext.DirectSalesOrder
                .Where(x => x.Id, IdFilter)
                .ToListWithNoLockAsync();
            foreach (DirectSalesOrder DirectSalesOrder in DirectSalesOrders)
            {
                DirectSalesOrderDAO DirectSalesOrderDAO = DbDirectSalesOrderDAOs
                        .Where(x => x.Id == DirectSalesOrder.Id)
                        .FirstOrDefault();
                if (DirectSalesOrderDAO == null)
                {
                    DirectSalesOrderDAO = new DirectSalesOrderDAO();
                    DirectSalesOrderDAO.CreatedAt = StaticParams.DateTimeNow;
                    DirectSalesOrderDAO.RowId = Guid.NewGuid();
                    DirectSalesOrder.RowId = DirectSalesOrderDAO.RowId;
                }
                DirectSalesOrderDAO.Code = DirectSalesOrder.Code;
                DirectSalesOrderDAO.ErpCode = DirectSalesOrder.ErpCode;
                DirectSalesOrderDAO.OrganizationId = DirectSalesOrder.OrganizationId;
                DirectSalesOrderDAO.BuyerStoreId = DirectSalesOrder.BuyerStoreId;
                DirectSalesOrderDAO.BuyerStoreTypeId = DirectSalesOrder.BuyerStoreTypeId;
                DirectSalesOrderDAO.PhoneNumber = DirectSalesOrder.PhoneNumber;
                DirectSalesOrderDAO.StoreAddress = DirectSalesOrder.StoreAddress;
                DirectSalesOrderDAO.DeliveryAddress = DirectSalesOrder.DeliveryAddress;
                DirectSalesOrderDAO.SaleEmployeeId = DirectSalesOrder.SaleEmployeeId;
                DirectSalesOrderDAO.OrderDate = DirectSalesOrder.OrderDate;
                DirectSalesOrderDAO.DeliveryDate = DirectSalesOrder.DeliveryDate;
                DirectSalesOrderDAO.ErpApprovalStateId = DirectSalesOrder.ErpApprovalStateId;
                DirectSalesOrderDAO.StoreApprovalStateId = DirectSalesOrder.StoreApprovalStateId;
                DirectSalesOrderDAO.RequestStateId = DirectSalesOrder.RequestStateId;
                DirectSalesOrderDAO.DirectSalesOrderSourceTypeId = DirectSalesOrder.DirectSalesOrderSourceTypeId;
                DirectSalesOrderDAO.EditedPriceStatusId = DirectSalesOrder.EditedPriceStatusId;
                DirectSalesOrderDAO.Note = DirectSalesOrder.Note;
                DirectSalesOrderDAO.SubTotal = DirectSalesOrder.SubTotal;
                DirectSalesOrderDAO.GeneralDiscountPercentage = DirectSalesOrder.GeneralDiscountPercentage;
                DirectSalesOrderDAO.GeneralDiscountAmount = DirectSalesOrder.GeneralDiscountAmount;
                DirectSalesOrderDAO.TotalTaxAmount = DirectSalesOrder.TotalTaxAmount;
                DirectSalesOrderDAO.TotalAfterTax = DirectSalesOrder.TotalAfterTax;
                DirectSalesOrderDAO.PromotionCode = DirectSalesOrder.PromotionCode;
                DirectSalesOrderDAO.PromotionValue = DirectSalesOrder.PromotionValue;
                DirectSalesOrderDAO.Total = DirectSalesOrder.Total;
                DirectSalesOrderDAO.StoreCheckingId = DirectSalesOrder.StoreCheckingId;
                DirectSalesOrderDAO.StoreUserCreatorId = DirectSalesOrder.StoreUserCreatorId;
                DirectSalesOrderDAO.CreatorId = DirectSalesOrder.CreatorId;
                DirectSalesOrderDAO.GeneralApprovalStateId = DirectSalesOrder.GeneralApprovalStateId;
                DirectSalesOrderDAO.StoreBalanceCheckStateId = DirectSalesOrder.StoreBalanceCheckStateId;
                DirectSalesOrderDAO.InventoryCheckStateId = DirectSalesOrder.InventoryCheckStateId;
                DirectSalesOrderDAO.UpdatedAt = StaticParams.DateTimeNow;
                DirectSalesOrderDAOs.Add(DirectSalesOrderDAO);
            }
            await DataContext.BulkMergeAsync(DirectSalesOrderDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<DirectSalesOrder> DirectSalesOrders)
        {
            List<long> Ids = DirectSalesOrders.Select(x => x.Id).ToList();
            await DataContext.DirectSalesOrder
                .Where(x => Ids.Contains(x.Id))
                .DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(DirectSalesOrder DirectSalesOrder)
        {
            await DataContext.DirectSalesOrderContent
                .Where(x => x.DirectSalesOrderId == DirectSalesOrder.Id)
                .DeleteFromQueryAsync();
            List<DirectSalesOrderContentDAO> DirectSalesOrderContentDAOs = new List<DirectSalesOrderContentDAO>();
            if (DirectSalesOrder.DirectSalesOrderContents != null)
            {
                foreach (DirectSalesOrderContent DirectSalesOrderContent in DirectSalesOrder.DirectSalesOrderContents)
                {
                    DirectSalesOrderContentDAO DirectSalesOrderContentDAO = new DirectSalesOrderContentDAO();
                    DirectSalesOrderContentDAO.Id = DirectSalesOrderContent.Id;
                    DirectSalesOrderContentDAO.DirectSalesOrderId = DirectSalesOrder.Id;
                    DirectSalesOrderContentDAO.ItemId = DirectSalesOrderContent.ItemId;
                    DirectSalesOrderContentDAO.UnitOfMeasureId = DirectSalesOrderContent.UnitOfMeasureId;
                    DirectSalesOrderContentDAO.Quantity = DirectSalesOrderContent.Quantity;
                    DirectSalesOrderContentDAO.PrimaryUnitOfMeasureId = DirectSalesOrderContent.PrimaryUnitOfMeasureId;
                    DirectSalesOrderContentDAO.RequestedQuantity = DirectSalesOrderContent.RequestedQuantity;
                    DirectSalesOrderContentDAO.PrimaryPrice = DirectSalesOrderContent.PrimaryPrice;
                    DirectSalesOrderContentDAO.SalePrice = DirectSalesOrderContent.SalePrice;
                    DirectSalesOrderContentDAO.EditedPriceStatusId = DirectSalesOrderContent.EditedPriceStatusId;
                    DirectSalesOrderContentDAO.DiscountPercentage = DirectSalesOrderContent.DiscountPercentage;
                    DirectSalesOrderContentDAO.DiscountAmount = DirectSalesOrderContent.DiscountAmount;
                    DirectSalesOrderContentDAO.GeneralDiscountPercentage = DirectSalesOrderContent.GeneralDiscountPercentage;
                    DirectSalesOrderContentDAO.GeneralDiscountAmount = DirectSalesOrderContent.GeneralDiscountAmount;
                    DirectSalesOrderContentDAO.Amount = DirectSalesOrderContent.Amount;
                    DirectSalesOrderContentDAO.TaxPercentage = DirectSalesOrderContent.TaxPercentage;
                    DirectSalesOrderContentDAO.TaxAmount = DirectSalesOrderContent.TaxAmount;
                    DirectSalesOrderContentDAO.Factor = DirectSalesOrderContent.Factor;
                    DirectSalesOrderContentDAO.InventoryCheckStateId = DirectSalesOrderContent.InventoryCheckStateId;
                    DirectSalesOrderContentDAOs.Add(DirectSalesOrderContentDAO);
                }
                await DataContext.DirectSalesOrderContent.BulkMergeAsync(DirectSalesOrderContentDAOs);
            }
            await DataContext.DirectSalesOrderPromotion
                .Where(x => x.DirectSalesOrderId == DirectSalesOrder.Id)
                .DeleteFromQueryAsync();
            List<DirectSalesOrderPromotionDAO> DirectSalesOrderPromotionDAOs = new List<DirectSalesOrderPromotionDAO>();
            if (DirectSalesOrder.DirectSalesOrderPromotions != null)
            {
                foreach (DirectSalesOrderPromotion DirectSalesOrderPromotion in DirectSalesOrder.DirectSalesOrderPromotions)
                {
                    DirectSalesOrderPromotionDAO DirectSalesOrderPromotionDAO = new DirectSalesOrderPromotionDAO();
                    DirectSalesOrderPromotionDAO.Id = DirectSalesOrderPromotion.Id;
                    DirectSalesOrderPromotionDAO.DirectSalesOrderId = DirectSalesOrder.Id;
                    DirectSalesOrderPromotionDAO.ItemId = DirectSalesOrderPromotion.ItemId;
                    DirectSalesOrderPromotionDAO.UnitOfMeasureId = DirectSalesOrderPromotion.UnitOfMeasureId;
                    DirectSalesOrderPromotionDAO.Quantity = DirectSalesOrderPromotion.Quantity;
                    DirectSalesOrderPromotionDAO.PrimaryUnitOfMeasureId = DirectSalesOrderPromotion.PrimaryUnitOfMeasureId;
                    DirectSalesOrderPromotionDAO.RequestedQuantity = DirectSalesOrderPromotion.RequestedQuantity;
                    DirectSalesOrderPromotionDAO.Note = DirectSalesOrderPromotion.Note;
                    DirectSalesOrderPromotionDAO.Factor = DirectSalesOrderPromotion.Factor;
                    DirectSalesOrderPromotionDAO.InventoryCheckStateId = DirectSalesOrderPromotion.InventoryCheckStateId;
                    DirectSalesOrderPromotionDAOs.Add(DirectSalesOrderPromotionDAO);
                }
                await DataContext.DirectSalesOrderPromotion.BulkMergeAsync(DirectSalesOrderPromotionDAOs);
            }

            await DataContext.DirectSalesOrderTransaction.Where(x => x.DirectSalesOrderId == DirectSalesOrder.Id).DeleteFromQueryAsync();
            List<DirectSalesOrderTransactionDAO> DirectSalesOrderTransactionDAOs = new List<DirectSalesOrderTransactionDAO>();
            if (DirectSalesOrder.DirectSalesOrderContents != null)
            {
                foreach (var DirectSalesOrderContent in DirectSalesOrder.DirectSalesOrderContents)
                {
                    DirectSalesOrderTransactionDAO DirectSalesOrderTransactionDAO = new DirectSalesOrderTransactionDAO
                    {
                        DirectSalesOrderId = DirectSalesOrder.Id,
                        ItemId = DirectSalesOrderContent.ItemId,
                        OrganizationId = DirectSalesOrder.OrganizationId,
                        BuyerStoreId = DirectSalesOrder.BuyerStoreId,
                        SalesEmployeeId = DirectSalesOrder.SaleEmployeeId,
                        OrderDate = DirectSalesOrder.OrderDate,
                        TypeId = TransactionTypeEnum.SALES_CONTENT.Id,
                        UnitOfMeasureId = DirectSalesOrderContent.PrimaryUnitOfMeasureId,
                        Quantity = DirectSalesOrderContent.RequestedQuantity,
                        Revenue = DirectSalesOrderContent.Amount - (DirectSalesOrderContent.GeneralDiscountAmount ?? 0) + (DirectSalesOrderContent.TaxAmount ?? 0),
                        Discount = (DirectSalesOrderContent.DiscountAmount ?? 0) + (DirectSalesOrderContent.GeneralDiscountAmount ?? 0)
                    };
                    DirectSalesOrderTransactionDAOs.Add(DirectSalesOrderTransactionDAO);
                }
            }

            if (DirectSalesOrder.DirectSalesOrderPromotions != null)
            {
                foreach (var DirectSalesOrderPromotion in DirectSalesOrder.DirectSalesOrderPromotions)
                {
                    DirectSalesOrderTransactionDAO DirectSalesOrderTransactionDAO = new DirectSalesOrderTransactionDAO
                    {
                        DirectSalesOrderId = DirectSalesOrder.Id,
                        ItemId = DirectSalesOrderPromotion.ItemId,
                        OrganizationId = DirectSalesOrder.OrganizationId,
                        BuyerStoreId = DirectSalesOrder.BuyerStoreId,
                        SalesEmployeeId = DirectSalesOrder.SaleEmployeeId,
                        OrderDate = DirectSalesOrder.OrderDate,
                        TypeId = TransactionTypeEnum.PROMOTION.Id,
                        UnitOfMeasureId = DirectSalesOrderPromotion.PrimaryUnitOfMeasureId,
                        Quantity = DirectSalesOrderPromotion.RequestedQuantity,
                    };
                    DirectSalesOrderTransactionDAOs.Add(DirectSalesOrderTransactionDAO);
                }
            }
            await DataContext.DirectSalesOrderTransaction.BulkMergeAsync(DirectSalesOrderTransactionDAOs);
        }

        public async Task<bool> UpdateState(DirectSalesOrder DirectSalesOrder, SystemConfiguration SystemConfiguration)
        {
            var GeneralApprovalStateId = GeneralApprovalStateEnum.NEW.Id;
            if (SystemConfiguration.START_WORKFLOW_BY_USER_TYPE == GlobalUserTypeEnum.STOREUSER.Id)
            {
                // TH đại ko tham gia phê duyệt
                if (DirectSalesOrder.StoreApprovalStateId == null && DirectSalesOrder.RequestStateId == RequestStateEnum.PENDING.Id)
                    GeneralApprovalStateId = GeneralApprovalStateEnum.PENDING.Id;
                if (DirectSalesOrder.StoreApprovalStateId == null && DirectSalesOrder.RequestStateId == RequestStateEnum.APPROVED.Id)
                    GeneralApprovalStateId = GeneralApprovalStateEnum.APPROVED.Id;
                if (DirectSalesOrder.StoreApprovalStateId == null && DirectSalesOrder.RequestStateId == RequestStateEnum.REJECTED.Id)
                    GeneralApprovalStateId = GeneralApprovalStateEnum.REJECTED.Id;
                // TH đại lý tham gia phê duyệt
                if (DirectSalesOrder.StoreApprovalStateId == StoreApprovalStateEnum.DRAFT.Id)
                    GeneralApprovalStateId = GeneralApprovalStateEnum.STORE_DRAFT.Id;
                if (DirectSalesOrder.StoreApprovalStateId == StoreApprovalStateEnum.PENDING.Id)
                    GeneralApprovalStateId = GeneralApprovalStateEnum.STORE_PENDING.Id;
                if (DirectSalesOrder.StoreApprovalStateId == StoreApprovalStateEnum.REJECTED.Id)
                    GeneralApprovalStateId = GeneralApprovalStateEnum.STORE_REJECTED.Id;
                if (DirectSalesOrder.StoreApprovalStateId == StoreApprovalStateEnum.APPROVED.Id && DirectSalesOrder.RequestStateId == RequestStateEnum.PENDING.Id)
                    GeneralApprovalStateId = GeneralApprovalStateEnum.PENDING.Id;
                if (DirectSalesOrder.StoreApprovalStateId == StoreApprovalStateEnum.APPROVED.Id && DirectSalesOrder.RequestStateId == RequestStateEnum.APPROVED.Id)
                    GeneralApprovalStateId = GeneralApprovalStateEnum.APPROVED.Id;
                if (DirectSalesOrder.StoreApprovalStateId == StoreApprovalStateEnum.APPROVED.Id && DirectSalesOrder.RequestStateId == RequestStateEnum.REJECTED.Id)
                    GeneralApprovalStateId = GeneralApprovalStateEnum.REJECTED.Id;
            }
            else
            {
                if (DirectSalesOrder.RequestStateId == RequestStateEnum.PENDING.Id)
                    GeneralApprovalStateId = GeneralApprovalStateEnum.PENDING.Id;
                if (DirectSalesOrder.RequestStateId == RequestStateEnum.REJECTED.Id)
                    GeneralApprovalStateId = GeneralApprovalStateEnum.REJECTED.Id;
                if (DirectSalesOrder.RequestStateId == RequestStateEnum.APPROVED.Id && DirectSalesOrder.StoreApprovalStateId == null)
                    GeneralApprovalStateId = GeneralApprovalStateEnum.APPROVED.Id;
                if (DirectSalesOrder.RequestStateId == RequestStateEnum.APPROVED.Id && DirectSalesOrder.StoreApprovalStateId == StoreApprovalStateEnum.PENDING.Id)
                    GeneralApprovalStateId = GeneralApprovalStateEnum.STORE_PENDING.Id;
                if (DirectSalesOrder.RequestStateId == RequestStateEnum.APPROVED.Id && DirectSalesOrder.StoreApprovalStateId == StoreApprovalStateEnum.REJECTED.Id)
                    GeneralApprovalStateId = GeneralApprovalStateEnum.STORE_REJECTED.Id;
                if (DirectSalesOrder.RequestStateId == RequestStateEnum.APPROVED.Id && DirectSalesOrder.StoreApprovalStateId == StoreApprovalStateEnum.APPROVED.Id)
                    GeneralApprovalStateId = GeneralApprovalStateEnum.STORE_APPROVED.Id;
                if (DirectSalesOrder.StoreApprovalStateId == StoreApprovalStateEnum.DRAFT.Id)
                    GeneralApprovalStateId = GeneralApprovalStateEnum.STORE_DRAFT.Id;
            }


            await DataContext.DirectSalesOrder.Where(x => x.Id == DirectSalesOrder.Id)
                .UpdateFromQueryAsync(x => new DirectSalesOrderDAO
                {
                    RequestStateId = DirectSalesOrder.RequestStateId,
                    StoreApprovalStateId = DirectSalesOrder.StoreApprovalStateId,
                    GeneralApprovalStateId = GeneralApprovalStateId,
                    UpdatedAt = StaticParams.DateTimeNow
                });
            return true;
        }

        public async Task<bool> BulkUpdateCheckState(List<DirectSalesOrder> DirectSalesOrders)
        {
            IdFilter IdFilter = new IdFilter { In = DirectSalesOrders.Select(x => x.Id).ToList() };
            List<DirectSalesOrderDAO> DirectSalesOrderDAOs = new List<DirectSalesOrderDAO>();
            List<DirectSalesOrderContentDAO> DirectSalesOrderContentDAOs = new List<DirectSalesOrderContentDAO>();
            List<DirectSalesOrderPromotionDAO> DirectSalesOrderPromotionDAOs = new List<DirectSalesOrderPromotionDAO>();
            List<DirectSalesOrderDAO> DbDirectSalesOrderDAOs = await DataContext.DirectSalesOrder
                .Where(x => x.Id, IdFilter)
                .ToListWithNoLockAsync();
            List<DirectSalesOrderContentDAO> DbDirectSalesOrderContentDAOs = await DataContext.DirectSalesOrderContent
                .Where(x => x.DirectSalesOrderId, IdFilter)
                .ToListWithNoLockAsync();
            List<DirectSalesOrderPromotionDAO> DbDirectSalesOrderPromotionDAOs = await DataContext.DirectSalesOrderPromotion
                .Where(x => x.DirectSalesOrderId, IdFilter)
                .ToListWithNoLockAsync();
            foreach (DirectSalesOrder DirectSalesOrder in DirectSalesOrders)
            {
                DirectSalesOrderDAO DirectSalesOrderDAO = DbDirectSalesOrderDAOs
                        .Where(x => x.Id == DirectSalesOrder.Id)
                        .FirstOrDefault();
                DirectSalesOrderDAO.StoreBalanceCheckStateId = DirectSalesOrder.StoreBalanceCheckStateId;
                DirectSalesOrderDAO.InventoryCheckStateId = DirectSalesOrder.InventoryCheckStateId;
                DirectSalesOrderDAO.UpdatedAt = StaticParams.DateTimeNow;
                DirectSalesOrderDAOs.Add(DirectSalesOrderDAO);
                if (DirectSalesOrder.DirectSalesOrderContents != null)
                {
                    foreach (DirectSalesOrderContent DirectSalesOrderContent in DirectSalesOrder.DirectSalesOrderContents)
                    {
                        DirectSalesOrderContentDAO DirectSalesOrderContentDAO = DbDirectSalesOrderContentDAOs
                                .Where(x => x.Id == DirectSalesOrderContent.Id)
                                .FirstOrDefault();
                        DirectSalesOrderContentDAO.InventoryCheckStateId = DirectSalesOrderContent.InventoryCheckStateId;
                        DirectSalesOrderContentDAOs.Add(DirectSalesOrderContentDAO);
                    }
                }
                if (DirectSalesOrder.DirectSalesOrderPromotions != null)
                {
                    foreach (DirectSalesOrderPromotion DirectSalesOrderPromotion in DirectSalesOrder.DirectSalesOrderPromotions)
                    {
                        DirectSalesOrderPromotionDAO DirectSalesOrderPromotionDAO = DbDirectSalesOrderPromotionDAOs
                                .Where(x => x.Id == DirectSalesOrderPromotion.Id)
                                .FirstOrDefault();
                        DirectSalesOrderPromotionDAO.InventoryCheckStateId = DirectSalesOrderPromotion.InventoryCheckStateId;
                        DirectSalesOrderPromotionDAOs.Add(DirectSalesOrderPromotionDAO);
                    }
                }

            }
            await DataContext.BulkUpdateAsync(DirectSalesOrderDAOs);
            await DataContext.BulkUpdateAsync(DirectSalesOrderContentDAOs);
            await DataContext.BulkUpdateAsync(DirectSalesOrderPromotionDAOs);
            return true;
        }
    }
}
