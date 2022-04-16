using TrueSight.Common;
using DMS.Common;
using DMS.Helpers;
using DMS.Entities;
using DMS.Models;
using DMS.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thinktecture;
using Thinktecture.EntityFrameworkCore.TempTables;

namespace DMS.Repositories
{
    public interface IStoreBalanceRepository
    {
        Task<int> CountAll(StoreBalanceFilter StoreBalanceFilter);
        Task<int> Count(StoreBalanceFilter StoreBalanceFilter);
        Task<List<StoreBalance>> List(StoreBalanceFilter StoreBalanceFilter);
        Task<List<StoreBalance>> List(List<long> Ids);
        Task<StoreBalance> Get(long Id);
        Task<bool> Create(StoreBalance StoreBalance);
        Task<bool> Update(StoreBalance StoreBalance);
        Task<bool> Delete(StoreBalance StoreBalance);
        Task<bool> BulkMerge(List<StoreBalance> StoreBalances);
        Task<bool> BulkDelete(List<StoreBalance> StoreBalances);
        Task<bool> Clean();
    }
    public class StoreBalanceRepository : IStoreBalanceRepository
    {
        private DataContext DataContext;
        public StoreBalanceRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private async Task<IQueryable<StoreBalanceDAO>> DynamicFilter(IQueryable<StoreBalanceDAO> query, StoreBalanceFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.CreatedAt, filter.CreatedAt);
            query = query.Where(q => q.UpdatedAt, filter.UpdatedAt);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.CreditAmount, filter.CreditAmount);
            query = query.Where(q => q.DebitAmount, filter.DebitAmount);
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
                        .Where(o => o.DeletedAt == null && o.StatusId == 1).ToList();
                    List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => filter.OrganizationId.In.Contains(o.Id)).ToList();
                    List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> Ids = Branches.Select(o => o.Id).Distinct().ToList();
                    IdFilter IdFilter = new IdFilter { In = Ids };
                    query = query.Where(q => q.OrganizationId, IdFilter);
                }
                if (filter.OrganizationId.NotIn != null)
                {
                    List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                        .Where(o => o.DeletedAt == null && o.StatusId == 1).ToList();
                    List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => filter.OrganizationId.NotIn.Contains(o.Id)).ToList();
                    List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> Ids = Branches.Select(o => o.Id).Distinct().ToList();
                    IdFilter IdFilter = new IdFilter { NotIn = Ids };
                    query = query.Where(q => q.OrganizationId, IdFilter);
                }
            } 
            query = query.Where(q => q.StoreId, filter.StoreId);
            query = query.Where(q => q.Store.Code, filter.Code);
            query = query.Where(q => q.Store.Name, filter.Name);
            query = query.Where(q => q.Store.CodeDraft, filter.CodeDraft);

            return query;
        }

        private async Task<IQueryable<StoreBalanceDAO>> OrFilter(IQueryable<StoreBalanceDAO> query, StoreBalanceFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<StoreBalanceDAO> initQuery = query.Where(q => false);
            foreach (StoreBalanceFilter StoreBalanceFilter in filter.OrFilter)
            {
                IQueryable<StoreBalanceDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, StoreBalanceFilter.Id);
                queryable = queryable.Where(q => q.CreditAmount, StoreBalanceFilter.CreditAmount);
                queryable = queryable.Where(q => q.DebitAmount, StoreBalanceFilter.DebitAmount);
                if (StoreBalanceFilter.OrganizationId != null && StoreBalanceFilter.OrganizationId.HasValue)
                {
                    if (StoreBalanceFilter.OrganizationId.Equal != null)
                    {
                        OrganizationDAO OrganizationDAO = DataContext.Organization
                            .Where(o => o.Id == StoreBalanceFilter.OrganizationId.Equal.Value).FirstOrDefault();
                        queryable = queryable.Where(q => q.Organization.Path.StartsWith(OrganizationDAO.Path));
                    }
                    if (StoreBalanceFilter.OrganizationId.NotEqual != null)
                    {
                        OrganizationDAO OrganizationDAO = DataContext.Organization
                            .Where(o => o.Id == StoreBalanceFilter.OrganizationId.NotEqual.Value).FirstOrDefault();
                        queryable = queryable.Where(q => !q.Organization.Path.StartsWith(OrganizationDAO.Path));
                    }
                    if (StoreBalanceFilter.OrganizationId.In != null)
                    {
                        List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                            .Where(o => o.DeletedAt == null).ToList();
                        List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => StoreBalanceFilter.OrganizationId.In.Contains(o.Id)).ToList();
                        List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> Ids = Branches.Select(o => o.Id).ToList();
                        IdFilter IdFilter = new IdFilter { In = Ids };
                        queryable = queryable.Where(q => q.OrganizationId, IdFilter);
                    }
                    if (StoreBalanceFilter.OrganizationId.NotIn != null)
                    {
                        List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                            .Where(o => o.DeletedAt == null).ToList();
                        List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => StoreBalanceFilter.OrganizationId.NotIn.Contains(o.Id)).ToList();
                        List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> Ids = Branches.Select(o => o.Id).ToList();
                        IdFilter IdFilter = new IdFilter { NotIn = Ids };
                        queryable = queryable.Where(q => q.OrganizationId, IdFilter);
                    }
                }
                queryable = queryable.Where(q => q.StoreId, StoreBalanceFilter.StoreId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<StoreBalanceDAO> DynamicOrder(IQueryable<StoreBalanceDAO> query, StoreBalanceFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case StoreBalanceOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case StoreBalanceOrder.Organization:
                            query = query.OrderBy(q => q.OrganizationId);
                            break;
                        case StoreBalanceOrder.StoreCode:
                            query = query.OrderBy(q => q.Store.Code);
                            break;
                        case StoreBalanceOrder.StoreName:
                            query = query.OrderBy(q => q.Store.Name);
                            break;
                        case StoreBalanceOrder.StoreCodeDraft:
                            query = query.OrderBy(q => q.Store.CodeDraft);
                            break;
                        case StoreBalanceOrder.CreditAmount:
                            query = query.OrderBy(q => q.CreditAmount);
                            break;
                        case StoreBalanceOrder.DebitAmount:
                            query = query.OrderBy(q => q.DebitAmount);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case StoreBalanceOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case StoreBalanceOrder.Organization:
                            query = query.OrderByDescending(q => q.OrganizationId);
                            break;
                        case StoreBalanceOrder.StoreCode:
                            query = query.OrderByDescending(q => q.Store.Code);
                            break;
                        case StoreBalanceOrder.StoreName:
                            query = query.OrderByDescending(q => q.Store.Name);
                            break;
                        case StoreBalanceOrder.StoreCodeDraft:
                            query = query.OrderByDescending(q => q.Store.CodeDraft);
                            break;
                        case StoreBalanceOrder.CreditAmount:
                            query = query.OrderByDescending(q => q.CreditAmount);
                            break;
                        case StoreBalanceOrder.DebitAmount:
                            query = query.OrderByDescending(q => q.DebitAmount);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<StoreBalance>> DynamicSelect(IQueryable<StoreBalanceDAO> query, StoreBalanceFilter filter)
        {
            List<StoreBalance> StoreBalances = await query.Select(q => new StoreBalance()
            {
                Id = filter.Selects.Contains(StoreBalanceSelect.Id) ? q.Id : default(long),
                OrganizationId = filter.Selects.Contains(StoreBalanceSelect.Organization) ? q.OrganizationId : default(long),
                StoreId = filter.Selects.Contains(StoreBalanceSelect.Store) ? q.StoreId : default(long),
                CreditAmount = filter.Selects.Contains(StoreBalanceSelect.CreditAmount) ? q.CreditAmount : default(decimal),
                DebitAmount = filter.Selects.Contains(StoreBalanceSelect.DebitAmount) ? q.DebitAmount : default(decimal),
                Organization = filter.Selects.Contains(StoreBalanceSelect.Organization) && q.Organization != null ? new Organization
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
                    IsDisplay = q.Organization.IsDisplay,
                } : null,
                Store = filter.Selects.Contains(StoreBalanceSelect.Store) && q.Store != null ? new Store
                {
                    Id = q.Store.Id,
                    Code = q.Store.Code,
                    CodeDraft = q.Store.CodeDraft,
                    Name = q.Store.Name,
                    UnsignName = q.Store.UnsignName,
                    ParentStoreId = q.Store.ParentStoreId,
                    OrganizationId = q.Store.OrganizationId,
                    StoreTypeId = q.Store.StoreTypeId,
                    Telephone = q.Store.Telephone,
                    ProvinceId = q.Store.ProvinceId,
                    DistrictId = q.Store.DistrictId,
                    WardId = q.Store.WardId,
                    Address = q.Store.Address,
                    UnsignAddress = q.Store.UnsignAddress,
                    DeliveryAddress = q.Store.DeliveryAddress,
                    Latitude = q.Store.Latitude,
                    Longitude = q.Store.Longitude,
                    DeliveryLatitude = q.Store.DeliveryLatitude,
                    DeliveryLongitude = q.Store.DeliveryLongitude,
                    OwnerName = q.Store.OwnerName,
                    OwnerPhone = q.Store.OwnerPhone,
                    OwnerEmail = q.Store.OwnerEmail,
                    TaxCode = q.Store.TaxCode,
                    LegalEntity = q.Store.LegalEntity,
                    CreatorId = q.Store.CreatorId,
                    AppUserId = q.Store.AppUserId,
                    StatusId = q.Store.StatusId,
                    Used = q.Store.Used,
                    StoreScoutingId = q.Store.StoreScoutingId,
                    StoreStatusId = q.Store.StoreStatusId,
                    IsStoreApprovalDirectSalesOrder = q.Store.IsStoreApprovalDirectSalesOrder,
                } : null,
                RowId = q.RowId,
                CreatedAt = q.CreatedAt,
                UpdatedAt = q.UpdatedAt,
                DeletedAt = q.DeletedAt,
            }).ToListWithNoLockAsync();
            return StoreBalances;
        }

        public async Task<int> CountAll(StoreBalanceFilter filter)
        {
            IQueryable<StoreBalanceDAO> StoreBalanceDAOs = DataContext.StoreBalance.AsNoTracking();
            StoreBalanceDAOs = await DynamicFilter(StoreBalanceDAOs, filter);
            return await StoreBalanceDAOs.CountWithNoLockAsync();
        }

        public async Task<int> Count(StoreBalanceFilter filter)
        {
            IQueryable<StoreBalanceDAO> StoreBalanceDAOs = DataContext.StoreBalance.AsNoTracking();
            StoreBalanceDAOs = await DynamicFilter(StoreBalanceDAOs, filter);
            StoreBalanceDAOs = await OrFilter(StoreBalanceDAOs, filter);
            return await StoreBalanceDAOs.CountWithNoLockAsync();
        }

        public async Task<List<StoreBalance>> List(StoreBalanceFilter filter)
        {
            if (filter == null) return new List<StoreBalance>();
            IQueryable<StoreBalanceDAO> StoreBalanceDAOs = DataContext.StoreBalance.AsNoTracking();
            StoreBalanceDAOs = await DynamicFilter(StoreBalanceDAOs, filter);
            StoreBalanceDAOs = await OrFilter(StoreBalanceDAOs, filter);
            StoreBalanceDAOs = DynamicOrder(StoreBalanceDAOs, filter);
            List<StoreBalance> StoreBalances = await DynamicSelect(StoreBalanceDAOs, filter);
            return StoreBalances;
        }

        public async Task<List<StoreBalance>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };

            IQueryable<StoreBalanceDAO> query = DataContext.StoreBalance.AsNoTracking();
            query = query.Where(q => q.Id, IdFilter);
            List<StoreBalance> StoreBalances = await query.AsNoTracking()
            .Select(x => new StoreBalance()
            {
                RowId = x.RowId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                Id = x.Id,
                OrganizationId = x.OrganizationId,
                StoreId = x.StoreId,
                CreditAmount = x.CreditAmount,
                DebitAmount = x.DebitAmount,
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
                    IsDisplay = x.Organization.IsDisplay,
                },
                Store = x.Store == null ? null : new Store
                {
                    Id = x.Store.Id,
                    Code = x.Store.Code,
                    CodeDraft = x.Store.CodeDraft,
                    Name = x.Store.Name,
                    UnsignName = x.Store.UnsignName,
                    ParentStoreId = x.Store.ParentStoreId,
                    OrganizationId = x.Store.OrganizationId,
                    StoreTypeId = x.Store.StoreTypeId,
                    Telephone = x.Store.Telephone,
                    ProvinceId = x.Store.ProvinceId,
                    DistrictId = x.Store.DistrictId,
                    WardId = x.Store.WardId,
                    Address = x.Store.Address,
                    UnsignAddress = x.Store.UnsignAddress,
                    DeliveryAddress = x.Store.DeliveryAddress,
                    Latitude = x.Store.Latitude,
                    Longitude = x.Store.Longitude,
                    DeliveryLatitude = x.Store.DeliveryLatitude,
                    DeliveryLongitude = x.Store.DeliveryLongitude,
                    OwnerName = x.Store.OwnerName,
                    OwnerPhone = x.Store.OwnerPhone,
                    OwnerEmail = x.Store.OwnerEmail,
                    TaxCode = x.Store.TaxCode,
                    LegalEntity = x.Store.LegalEntity,
                    CreatorId = x.Store.CreatorId,
                    AppUserId = x.Store.AppUserId,
                    StatusId = x.Store.StatusId,
                    Used = x.Store.Used,
                    StoreScoutingId = x.Store.StoreScoutingId,
                    StoreStatusId = x.Store.StoreStatusId,
                    IsStoreApprovalDirectSalesOrder = x.Store.IsStoreApprovalDirectSalesOrder,
                },
            }).ToListWithNoLockAsync();


            return StoreBalances;
        }

        public async Task<StoreBalance> Get(long Id)
        {
            StoreBalance StoreBalance = await DataContext.StoreBalance.AsNoTracking()
            .Where(x => x.Id == Id)
            .Where(x => x.DeletedAt == null)
            .Select(x => new StoreBalance()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Id = x.Id,
                OrganizationId = x.OrganizationId,
                StoreId = x.StoreId,
                CreditAmount = x.CreditAmount,
                DebitAmount = x.DebitAmount,
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
                    IsDisplay = x.Organization.IsDisplay,
                },
                Store = x.Store == null ? null : new Store
                {
                    Id = x.Store.Id,
                    Code = x.Store.Code,
                    CodeDraft = x.Store.CodeDraft,
                    Name = x.Store.Name,
                    UnsignName = x.Store.UnsignName,
                    ParentStoreId = x.Store.ParentStoreId,
                    OrganizationId = x.Store.OrganizationId,
                    StoreTypeId = x.Store.StoreTypeId,
                    Telephone = x.Store.Telephone,
                    ProvinceId = x.Store.ProvinceId,
                    DistrictId = x.Store.DistrictId,
                    WardId = x.Store.WardId,
                    Address = x.Store.Address,
                    UnsignAddress = x.Store.UnsignAddress,
                    DeliveryAddress = x.Store.DeliveryAddress,
                    Latitude = x.Store.Latitude,
                    Longitude = x.Store.Longitude,
                    DeliveryLatitude = x.Store.DeliveryLatitude,
                    DeliveryLongitude = x.Store.DeliveryLongitude,
                    OwnerName = x.Store.OwnerName,
                    OwnerPhone = x.Store.OwnerPhone,
                    OwnerEmail = x.Store.OwnerEmail,
                    TaxCode = x.Store.TaxCode,
                    LegalEntity = x.Store.LegalEntity,
                    CreatorId = x.Store.CreatorId,
                    AppUserId = x.Store.AppUserId,
                    StatusId = x.Store.StatusId,
                    Used = x.Store.Used,
                    StoreScoutingId = x.Store.StoreScoutingId,
                    StoreStatusId = x.Store.StoreStatusId,
                    IsStoreApprovalDirectSalesOrder = x.Store.IsStoreApprovalDirectSalesOrder,
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (StoreBalance == null)
                return null;

            return StoreBalance;
        }

        public async Task<bool> Create(StoreBalance StoreBalance)
        {
            StoreBalanceDAO StoreBalanceDAO = new StoreBalanceDAO();
            StoreBalanceDAO.Id = StoreBalance.Id;
            StoreBalanceDAO.OrganizationId = StoreBalance.OrganizationId;
            StoreBalanceDAO.StoreId = StoreBalance.StoreId;
            StoreBalanceDAO.CreditAmount = StoreBalance.CreditAmount.Value;
            StoreBalanceDAO.DebitAmount = StoreBalance.DebitAmount.Value;
            StoreBalanceDAO.RowId = Guid.NewGuid();
            StoreBalanceDAO.CreatedAt = StaticParams.DateTimeNow;
            StoreBalanceDAO.UpdatedAt = StaticParams.DateTimeNow;
            DataContext.StoreBalance.Add(StoreBalanceDAO);
            await DataContext.SaveChangesAsync();
            StoreBalance.Id = StoreBalanceDAO.Id;
            await SaveReference(StoreBalance);
            return true;
        }

        public async Task<bool> Update(StoreBalance StoreBalance)
        {
            StoreBalanceDAO StoreBalanceDAO = DataContext.StoreBalance
                .Where(x => x.Id == StoreBalance.Id)
                .FirstOrDefault();
            if (StoreBalanceDAO == null)
                return false;
            StoreBalanceDAO.Id = StoreBalance.Id;
            StoreBalanceDAO.OrganizationId = StoreBalance.OrganizationId;
            StoreBalanceDAO.StoreId = StoreBalance.StoreId;
            StoreBalanceDAO.CreditAmount = StoreBalance.CreditAmount.Value;
            StoreBalanceDAO.DebitAmount = StoreBalance.DebitAmount.Value;
            StoreBalanceDAO.UpdatedAt = StaticParams.DateTimeNow;
            await DataContext.SaveChangesAsync();
            await SaveReference(StoreBalance);
            return true;
        }

        public async Task<bool> Delete(StoreBalance StoreBalance)
        {
            await DataContext.StoreBalance
                .Where(x => x.Id == StoreBalance.Id)
                .UpdateFromQueryAsync(x => new StoreBalanceDAO
                {
                    DeletedAt = StaticParams.DateTimeNow,
                    UpdatedAt = StaticParams.DateTimeNow
                });
            return true;
        }

        public async Task<bool> BulkMerge(List<StoreBalance> StoreBalances)
        {
            IdFilter IdFilter = new IdFilter { In = StoreBalances.Select(x => x.StoreId).ToList() };
            List<StoreBalanceDAO> StoreBalanceDAOs = new List<StoreBalanceDAO>();
            List<StoreBalanceDAO> DbStoreBalanceDAOs = await DataContext.StoreBalance
                .Where(x => x.StoreId, IdFilter)
                .ToListWithNoLockAsync();
            foreach (StoreBalance StoreBalance in StoreBalances)
            {
                StoreBalanceDAO StoreBalanceDAO = DbStoreBalanceDAOs
                        .Where(x => x.StoreId == StoreBalance.StoreId && x.OrganizationId == StoreBalance.OrganizationId)
                        .FirstOrDefault();
                if (StoreBalanceDAO == null)
                {
                    StoreBalanceDAO = new StoreBalanceDAO();
                    StoreBalanceDAO.CreatedAt = StaticParams.DateTimeNow;
                    StoreBalanceDAO.RowId = Guid.NewGuid();
                    StoreBalance.RowId = StoreBalanceDAO.RowId;
                }
                StoreBalanceDAO.OrganizationId = StoreBalance.OrganizationId;
                StoreBalanceDAO.StoreId = StoreBalance.StoreId;
                StoreBalanceDAO.CreditAmount = StoreBalance.CreditAmount.Value;
                StoreBalanceDAO.DebitAmount = StoreBalance.DebitAmount.Value;
                StoreBalanceDAO.UpdatedAt = StaticParams.DateTimeNow;
                StoreBalanceDAOs.Add(StoreBalanceDAO);
            }
            await DataContext.BulkMergeAsync(StoreBalanceDAOs);
            foreach (StoreBalance StoreBalance in StoreBalances)
            {
                StoreBalance.Id = StoreBalanceDAOs.FirstOrDefault(x => x.StoreId == StoreBalance.StoreId && x.OrganizationId == StoreBalance.OrganizationId).Id;
            }
            return true;
        }

        public async Task<bool> BulkDelete(List<StoreBalance> StoreBalances)
        {
            List<long> Ids = StoreBalances.Select(x => x.Id).ToList();
            IdFilter IdFilter = new IdFilter { In = Ids };
            await DataContext.StoreBalance
                .Where(x => Ids.Contains(x.Id))
                .UpdateFromQueryAsync(x => new StoreBalanceDAO
                {
                    DeletedAt = StaticParams.DateTimeNow,
                    UpdatedAt = StaticParams.DateTimeNow
                });
            return true;
        }
        public async Task<bool> Clean()
        {
            await DataContext.StoreBalance.UpdateFromQueryAsync(x => new StoreBalanceDAO
            {
                DeletedAt = StaticParams.DateTimeNow,
                UpdatedAt = StaticParams.DateTimeNow
            });
            return true;
        }

        private async Task SaveReference(StoreBalance StoreBalance)
        {
        }

    }
}
