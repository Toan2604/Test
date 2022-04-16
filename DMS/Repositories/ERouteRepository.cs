using DMS.Common;
using DMS.Entities;
using DMS.Helpers;
using DMS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Repositories
{
    public interface IERouteRepository
    {
        Task<int> Count(ERouteFilter ERouteFilter);
        Task<int> CountAll(ERouteFilter ERouteFilter);
        Task<List<ERoute>> List(ERouteFilter ERouteFilter);
        Task<int> CountNew(ERouteFilter ERouteFilter);
        Task<List<ERoute>> ListNew(ERouteFilter ERouteFilter);
        Task<int> CountPending(ERouteFilter ERouteFilter);
        Task<List<ERoute>> ListPending(ERouteFilter ERouteFilter);
        Task<int> CountCompleted(ERouteFilter ERouteFilter);
        Task<List<ERoute>> ListCompleted(ERouteFilter ERouteFilter);
        Task<ERoute> Get(long Id);
        Task<bool> Create(ERoute ERoute);
        Task<bool> Update(ERoute ERoute);
        Task<bool> UpdateState(ERoute ERoute);
        Task<bool> Delete(ERoute ERoute);
        Task<bool> BulkMerge(List<ERoute> ERoutes);
        Task<bool> BulkDelete(List<ERoute> ERoutes);
        Task<bool> Used(List<long> Ids, bool Value = true);
    }
    public class ERouteRepository : CacheRepository, IERouteRepository
    {
        private DataContext DataContext;
        public ERouteRepository(DataContext DataContext, IRedisStore RedisStore, IConfiguration Configuration) : base(DataContext,RedisStore,Configuration)
        {
            this.DataContext = DataContext;
        }

        private async Task<IQueryable<ERouteDAO>> DynamicFilter(IQueryable<ERouteDAO> query, ERouteFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.CreatedAt, filter.CreatedAt);
            query = query.Where(q => q.UpdatedAt, filter.UpdatedAt);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.SaleEmployeeId, filter.AppUserId);
            query = query.Where(q => q.StartDate, filter.StartDate);
            query = query.Where(q => q.EndDate == null).Union(query.Where(q => q.EndDate, filter.EndDate));
            query = query.Where(q => q.ERouteTypeId, filter.ERouteTypeId);
            query = query.Where(q => q.StatusId, filter.StatusId);
            query = query.Where(q => q.CreatorId, filter.CreatorId);
            query = query.Where(q => q.RequestStateId, filter.RequestStateId);
            if (filter.OrganizationId != null)
            {
                if (filter.OrganizationId.Equal != null)
                {
                    OrganizationDAO OrganizationDAO = await DataContext.Organization.AsNoTracking()
                        .Where(o => o.Id == filter.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
                    query = query.Where(q => q.Organization.Path.StartsWith(OrganizationDAO.Path));
                }
                if (filter.OrganizationId.NotEqual != null)
                {
                    OrganizationDAO OrganizationDAO = await DataContext.Organization.AsNoTracking()
                        .Where(o => o.Id == filter.OrganizationId.NotEqual.Value).FirstOrDefaultWithNoLockAsync();
                    query = query.Where(q => !q.Organization.Path.StartsWith(OrganizationDAO.Path));
                }
                if (filter.OrganizationId.In != null)
                {
                    List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.AsNoTracking()
                        .Where(o => o.DeletedAt == null && o.StatusId == 1).ToListWithNoLockAsync();
                    List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => filter.OrganizationId.In.Contains(o.Id)).ToList();
                    List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> Ids = Branches.Select(o => o.Id).ToList();
                    IdFilter IdFilter = new IdFilter { In = Ids };
                    query = query.Where(x => x.OrganizationId, IdFilter);
                }
                if (filter.OrganizationId.NotIn != null)
                {
                    List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.AsNoTracking()
                        .Where(o => o.DeletedAt == null && o.StatusId == 1).ToListWithNoLockAsync();
                    List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => filter.OrganizationId.NotIn.Contains(o.Id)).ToList();
                    List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> Ids = Branches.Select(o => o.Id).ToList();
                    IdFilter IdFilter = new IdFilter { NotIn = Ids };
                    query = query.Where(x => x.OrganizationId, IdFilter);
                }
            }
            if (filter.StoreId != null)
            {
                if (filter.StoreId.Equal.HasValue)
                {
                    query = from q in query
                            join ec in DataContext.ERouteContent on q.Id equals ec.ERouteId
                            where ec.StoreId == filter.StoreId.Equal
                            select q;
                    query = query.Distinct();
                }
                if (filter.StoreId.NotEqual.HasValue)
                {
                    query = from q in query
                            join ec in DataContext.ERouteContent on q.Id equals ec.ERouteId
                            where ec.StoreId != filter.StoreId.Equal
                            select q;
                    query = query.Distinct();
                }
            }
            return query;
        }

        private IQueryable<ERouteDAO> OrFilter(IQueryable<ERouteDAO> query, ERouteFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<ERouteDAO> initQuery = query.Where(q => false);
            foreach (ERouteFilter ERouteFilter in filter.OrFilter)
            {
                IQueryable<ERouteDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, ERouteFilter.Id);
                queryable = queryable.Where(q => q.Code, ERouteFilter.Code);
                queryable = queryable.Where(q => q.Name, ERouteFilter.Name);
                queryable = queryable.Where(q => q.OrganizationId, ERouteFilter.OrganizationId);
                queryable = queryable.Where(q => q.SaleEmployeeId, ERouteFilter.AppUserId);
                queryable = queryable.Where(q => q.StartDate, ERouteFilter.StartDate);
                queryable = queryable.Where(q => q.EndDate, ERouteFilter.EndDate);
                queryable = queryable.Where(q => q.ERouteTypeId, ERouteFilter.ERouteTypeId);
                queryable = queryable.Where(q => q.StatusId, ERouteFilter.StatusId);
                queryable = queryable.Where(q => q.CreatorId, ERouteFilter.CreatorId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<ERouteDAO> DynamicOrder(IQueryable<ERouteDAO> query, ERouteFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case ERouteOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case ERouteOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case ERouteOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case ERouteOrder.SaleEmployee:
                            query = query.OrderBy(q => q.SaleEmployeeId);
                            break;
                        case ERouteOrder.Organization:
                            query = query.OrderBy(q => q.OrganizationId);
                            break;
                        case ERouteOrder.StartDate:
                            query = query.OrderBy(q => q.StartDate);
                            break;
                        case ERouteOrder.EndDate:
                            query = query.OrderBy(q => q.EndDate);
                            break;
                        case ERouteOrder.ERouteType:
                            query = query.OrderBy(q => q.ERouteTypeId);
                            break;
                        case ERouteOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                        case ERouteOrder.Creator:
                            query = query.OrderBy(q => q.CreatorId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case ERouteOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case ERouteOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case ERouteOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case ERouteOrder.SaleEmployee:
                            query = query.OrderByDescending(q => q.SaleEmployeeId);
                            break;
                        case ERouteOrder.Organization:
                            query = query.OrderByDescending(q => q.OrganizationId);
                            break;
                        case ERouteOrder.StartDate:
                            query = query.OrderByDescending(q => q.StartDate);
                            break;
                        case ERouteOrder.EndDate:
                            query = query.OrderByDescending(q => q.EndDate);
                            break;
                        case ERouteOrder.ERouteType:
                            query = query.OrderByDescending(q => q.ERouteTypeId);
                            break;
                        case ERouteOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                        case ERouteOrder.Creator:
                            query = query.OrderByDescending(q => q.CreatorId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<ERoute>> DynamicSelect(IQueryable<ERouteDAO> query, ERouteFilter filter)
        {
            List<ERoute> ERoutes = await query.Select(q => new ERoute()
            {
                Id = filter.Selects.Contains(ERouteSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(ERouteSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(ERouteSelect.Name) ? q.Name : default(string),
                SaleEmployeeId = filter.Selects.Contains(ERouteSelect.SaleEmployee) ? q.SaleEmployeeId : default(long),
                OrganizationId = filter.Selects.Contains(ERouteSelect.Organization) ? q.OrganizationId : default(long),
                StartDate = filter.Selects.Contains(ERouteSelect.StartDate) ? q.StartDate : default(DateTime),
                RealStartDate = filter.Selects.Contains(ERouteSelect.RealStartDate) ? q.RealStartDate : default(DateTime),
                EndDate = filter.Selects.Contains(ERouteSelect.EndDate) ? q.EndDate : null,
                ERouteTypeId = filter.Selects.Contains(ERouteSelect.ERouteType) ? q.ERouteTypeId : default(long),
                StatusId = filter.Selects.Contains(ERouteSelect.Status) ? q.StatusId : default(long),
                CreatorId = filter.Selects.Contains(ERouteSelect.Creator) ? q.CreatorId : default(long),
                RequestStateId = filter.Selects.Contains(ERouteSelect.RequestState) ? q.RequestStateId : default(long),
                Creator = filter.Selects.Contains(ERouteSelect.Creator) && q.Creator != null ? new AppUser
                {
                    Id = q.Creator.Id,
                    Username = q.Creator.Username,
                    DisplayName = q.Creator.DisplayName,
                    Address = q.Creator.Address,
                    Email = q.Creator.Email,
                    Phone = q.Creator.Phone,
                } : null,
                Organization = filter.Selects.Contains(ERouteSelect.Organization) && q.Organization != null ? new Organization
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
                ERouteType = filter.Selects.Contains(ERouteSelect.ERouteType) && q.ERouteType != null ? new ERouteType
                {
                    Id = q.ERouteType.Id,
                    Code = q.ERouteType.Code,
                    Name = q.ERouteType.Name,
                } : null,
                RequestState = filter.Selects.Contains(ERouteSelect.RequestState) && q.RequestState != null ? new RequestState
                {
                    Id = q.RequestState.Id,
                    Code = q.RequestState.Code,
                    Name = q.RequestState.Name,
                } : null,
                SaleEmployee = filter.Selects.Contains(ERouteSelect.SaleEmployee) && q.SaleEmployee != null ? new AppUser
                {
                    Id = q.SaleEmployee.Id,
                    Username = q.SaleEmployee.Username,
                    DisplayName = q.SaleEmployee.DisplayName,
                    Address = q.SaleEmployee.Address,
                    Email = q.SaleEmployee.Email,
                    Phone = q.SaleEmployee.Phone,
                } : null,
                Status = filter.Selects.Contains(ERouteSelect.Status) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                } : null,
                CreatedAt = q.CreatedAt,
                UpdatedAt = q.UpdatedAt,
                Used = q.Used,
            }).ToListAsync();

            if (filter.Selects.Contains(ERouteSelect.ERouteContent))
            {
                List<long> ERouteIds = ERoutes.Select(x => x.Id).ToList();
                IdFilter IdFilter = new IdFilter { In = ERouteIds };
                List<ERouteContent> ERouteContents = await DataContext.ERouteContent
                    .Where(x => x.ERouteId, IdFilter)
                    .Select(x => new ERouteContent
                    {
                        Id = x.Id,
                        StoreId = x.StoreId,
                    }).ToListWithNoLockAsync();
            }
            return ERoutes;
        }

        public async Task<int> Count(ERouteFilter filter)
        {
            IQueryable<ERouteDAO> ERoutes = DataContext.ERoute.AsNoTracking();
            ERoutes = await DynamicFilter(ERoutes, filter);
            ERoutes = OrFilter(ERoutes, filter);
            return await ERoutes.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(ERouteFilter filter)
        {
            IQueryable<ERouteDAO> ERoutes = DataContext.ERoute.AsNoTracking();
            ERoutes = await DynamicFilter(ERoutes, filter);
            return await ERoutes.CountWithNoLockAsync();
        }

        public async Task<List<ERoute>> List(ERouteFilter filter)
        {
            if (filter == null) return new List<ERoute>();
            IQueryable<ERouteDAO> ERouteDAOs = DataContext.ERoute.AsNoTracking();
            ERouteDAOs = await DynamicFilter(ERouteDAOs, filter);
            ERouteDAOs = OrFilter(ERouteDAOs, filter);
            ERouteDAOs = DynamicOrder(ERouteDAOs, filter);
            List<ERoute> ERoutes = await DynamicSelect(ERouteDAOs, filter);
            return ERoutes;
        }

        public async Task<int> CountNew(ERouteFilter filter)
        {
            IQueryable<ERouteDAO> ERouteDAOs = DataContext.ERoute.AsNoTracking();
            ERouteDAOs = await DynamicFilter(ERouteDAOs, filter);
            ERouteDAOs = from q in ERouteDAOs
                         where (q.RequestStateId == RequestStateEnum.NEW.Id || q.RequestStateId == RequestStateEnum.REJECTED.Id) &&
                         q.CreatorId == filter.ApproverId.Equal
                         select q;

            return await ERouteDAOs.Distinct().CountWithNoLockAsync();
        }

        public async Task<List<ERoute>> ListNew(ERouteFilter filter)
        {
            if (filter == null) return new List<ERoute>();
            IQueryable<ERouteDAO> ERouteDAOs = DataContext.ERoute.AsNoTracking();
            ERouteDAOs = await DynamicFilter(ERouteDAOs, filter);
            ERouteDAOs = from q in ERouteDAOs
                         where (q.RequestStateId == RequestStateEnum.NEW.Id || q.RequestStateId == RequestStateEnum.REJECTED.Id) &&
                         q.CreatorId == filter.ApproverId.Equal
                         select q;

            ERouteDAOs = DynamicOrder(ERouteDAOs, filter);
            List<ERoute> ERoutes = await DynamicSelect(ERouteDAOs, filter);
            return ERoutes;
        }

        public async Task<int> CountPending(ERouteFilter filter)
        {
            IQueryable<ERouteDAO> ERouteDAOs = DataContext.ERoute.AsNoTracking();
            ERouteDAOs = await DynamicFilter(ERouteDAOs, filter);
            if (filter.ApproverId.Equal.HasValue)
            {
                ERouteDAOs = from q in ERouteDAOs
                             join r in DataContext.RequestWorkflowDefinitionMapping.Where(x => x.RequestStateId == RequestStateEnum.PENDING.Id) on q.RowId equals r.RequestId
                             join step in DataContext.WorkflowStep on r.WorkflowDefinitionId equals step.WorkflowDefinitionId
                             join rstep in DataContext.RequestWorkflowStepMapping.Where(x => x.WorkflowStateId == WorkflowStateEnum.PENDING.Id) on step.Id equals rstep.WorkflowStepId
                             join ra in DataContext.AppUserRoleMapping on step.RoleId equals ra.RoleId
                             where ra.AppUserId == filter.ApproverId.Equal && q.RowId == rstep.RequestId
                             select q;
            }
            return await ERouteDAOs.Distinct().CountWithNoLockAsync();
        }

        public async Task<List<ERoute>> ListPending(ERouteFilter filter)
        {
            if (filter == null) return new List<ERoute>();
            IQueryable<ERouteDAO> ERouteDAOs = DataContext.ERoute.AsNoTracking();
            ERouteDAOs = await DynamicFilter(ERouteDAOs, filter);
            if (filter.ApproverId.Equal.HasValue)
            {
                ERouteDAOs = from q in ERouteDAOs
                             join r in DataContext.RequestWorkflowDefinitionMapping.Where(x => x.RequestStateId == RequestStateEnum.PENDING.Id) on q.RowId equals r.RequestId
                             join step in DataContext.WorkflowStep on r.WorkflowDefinitionId equals step.WorkflowDefinitionId
                             join rstep in DataContext.RequestWorkflowStepMapping.Where(x => x.WorkflowStateId == WorkflowStateEnum.PENDING.Id) on step.Id equals rstep.WorkflowStepId
                             join ra in DataContext.AppUserRoleMapping on step.RoleId equals ra.RoleId
                             where ra.AppUserId == filter.ApproverId.Equal && q.RowId == rstep.RequestId
                             select q;
            }
            ERouteDAOs = DynamicOrder(ERouteDAOs, filter);
            List<ERoute> ERoutes = await DynamicSelect(ERouteDAOs, filter);
            return ERoutes;
        }

        public async Task<int> CountCompleted(ERouteFilter filter)
        {
            IQueryable<ERouteDAO> ERouteDAOs = DataContext.ERoute.AsNoTracking();
            ERouteDAOs = await DynamicFilter(ERouteDAOs, filter);
            if (filter.ApproverId.Equal.HasValue)
            {
                var query1 = from q in ERouteDAOs
                             join r in DataContext.RequestWorkflowDefinitionMapping on q.RowId equals r.RequestId
                             join step in DataContext.WorkflowStep on r.WorkflowDefinitionId equals step.WorkflowDefinitionId
                             join rstep in DataContext.RequestWorkflowStepMapping on step.Id equals rstep.WorkflowStepId
                             where
                             q.RequestStateId != RequestStateEnum.NEW.Id &&
                             rstep.WorkflowStateId == WorkflowStateEnum.APPROVED.Id &&
                             rstep.AppUserId == filter.ApproverId.Equal
                             select q;
                var query2 = from q in ERouteDAOs
                             join r in DataContext.RequestWorkflowDefinitionMapping on q.RowId equals r.RequestId into result
                             from r in result.DefaultIfEmpty()
                             where r == null && q.RequestStateId != RequestStateEnum.NEW.Id && q.CreatorId == filter.ApproverId.Equal
                             select q;
                ERouteDAOs = query1.Union(query2);
            }
            return await ERouteDAOs.Distinct().CountWithNoLockAsync();
        }

        public async Task<List<ERoute>> ListCompleted(ERouteFilter filter)
        {
            if (filter == null) return new List<ERoute>();
            IQueryable<ERouteDAO> ERouteDAOs = DataContext.ERoute.AsNoTracking();
            ERouteDAOs = await DynamicFilter(ERouteDAOs, filter);
            if (filter.ApproverId.Equal.HasValue)
            {
                var query1 = from q in ERouteDAOs
                             join r in DataContext.RequestWorkflowDefinitionMapping on q.RowId equals r.RequestId
                             join step in DataContext.WorkflowStep on r.WorkflowDefinitionId equals step.WorkflowDefinitionId
                             join rstep in DataContext.RequestWorkflowStepMapping on step.Id equals rstep.WorkflowStepId
                             where
                             q.RequestStateId != RequestStateEnum.NEW.Id &&
                             rstep.WorkflowStateId == WorkflowStateEnum.APPROVED.Id &&
                             rstep.AppUserId == filter.ApproverId.Equal
                             select q;
                var query2 = from q in ERouteDAOs
                             join r in DataContext.RequestWorkflowDefinitionMapping on q.RowId equals r.RequestId into result
                             from r in result.DefaultIfEmpty()
                             where r == null && q.RequestStateId != RequestStateEnum.NEW.Id && q.CreatorId == filter.ApproverId.Equal
                             select q;
                ERouteDAOs = query1.Union(query2);
            }
            ERouteDAOs = DynamicOrder(ERouteDAOs, filter);
            List<ERoute> ERoutes = await DynamicSelect(ERouteDAOs, filter);
            return ERoutes;
        }

        public async Task<ERoute> Get(long Id)
        {
            ERoute ERoute = await DataContext.ERoute.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new ERoute()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Used = x.Used,
                SaleEmployeeId = x.SaleEmployeeId,
                OrganizationId = x.OrganizationId,
                StartDate = x.StartDate,
                RealStartDate = x.RealStartDate,
                EndDate = x.EndDate,
                ERouteTypeId = x.ERouteTypeId,
                StatusId = x.StatusId,
                CreatorId = x.CreatorId,
                RequestStateId = x.RequestStateId,
                RowId = x.RowId,
                Creator = x.Creator == null ? null : new AppUser
                {
                    Id = x.Creator.Id,
                    Username = x.Creator.Username,
                    DisplayName = x.Creator.DisplayName,
                    Address = x.Creator.Address,
                    Email = x.Creator.Email,
                    Phone = x.Creator.Phone,
                },
                RequestState = x.RequestState == null ? null : new RequestState
                {
                    Id = x.RequestState.Id,
                    Code = x.RequestState.Code,
                    Name = x.RequestState.Name,
                },
                ERouteType = x.ERouteType == null ? null : new ERouteType
                {
                    Id = x.ERouteType.Id,
                    Code = x.ERouteType.Code,
                    Name = x.ERouteType.Name,
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
                SaleEmployee = x.SaleEmployee == null ? null : new AppUser
                {
                    Id = x.SaleEmployee.Id,
                    Username = x.SaleEmployee.Username,
                    DisplayName = x.SaleEmployee.DisplayName,
                    Address = x.SaleEmployee.Address,
                    Email = x.SaleEmployee.Email,
                    Phone = x.SaleEmployee.Phone,
                },
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (ERoute == null)
                return null;
            RequestWorkflowDefinitionMappingDAO RequestWorkflowDefinitionMappingDAO = await DataContext.RequestWorkflowDefinitionMapping
               .Where(x => ERoute.RowId == x.RequestId)
               .Include(x => x.RequestState)
               .AsNoTracking()
               .FirstOrDefaultWithNoLockAsync();
            if (RequestWorkflowDefinitionMappingDAO != null)
            {
                ERoute.RequestStateId = RequestWorkflowDefinitionMappingDAO.RequestStateId;
                ERoute.RequestState = new RequestState
                {
                    Id = RequestWorkflowDefinitionMappingDAO.RequestState.Id,
                    Code = RequestWorkflowDefinitionMappingDAO.RequestState.Code,
                    Name = RequestWorkflowDefinitionMappingDAO.RequestState.Name,
                };
            }

            // Ch? này m?c where join v?i b?ng Store ?? check organization khác null là ok

            ERoute.ERouteContents = await DataContext.ERouteContent.Where(e => e.Store.DeletedAt == null && e.ERouteId == Id)
                .Select(x => new ERouteContent
                {
                    Id = x.Id,
                    ERouteId = x.ERouteId,
                    StoreId = x.StoreId,
                    OrderNumber = x.OrderNumber,
                    Monday = x.Monday,
                    Tuesday = x.Tuesday,
                    Wednesday = x.Wednesday,
                    Thursday = x.Thursday,
                    Friday = x.Friday,
                    Saturday = x.Saturday,
                    Sunday = x.Sunday,
                    Week1 = x.Week1,
                    Week2 = x.Week2,
                    Week3 = x.Week3,
                    Week4 = x.Week4,
                    Store = new Store
                    {
                        Id = x.Store.Id,
                        Code = x.Store.Code,
                        CodeDraft = x.Store.CodeDraft,
                        Name = x.Store.Name,
                        ParentStoreId = x.Store.ParentStoreId,
                        OrganizationId = x.Store.OrganizationId,
                        StoreTypeId = x.Store.StoreTypeId,
                        Telephone = x.Store.Telephone,
                        ProvinceId = x.Store.ProvinceId,
                        DistrictId = x.Store.DistrictId,
                        WardId = x.Store.WardId,
                        Address = x.Store.Address,
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
                        StatusId = x.Store.StatusId,
                        StoreStatusId = x.Store.StoreStatusId,
                        StoreStatus = x.Store.StoreStatus == null ? null : new StoreStatus
                        {
                            Id = x.Store.StoreStatus.Id,
                            Name = x.Store.StoreStatus.Name,
                            Code = x.Store.StoreStatus.Code,
                        },
                        District = x.Store.District == null ? null : new District
                        {
                            Id = x.Store.District.Id,
                            Name = x.Store.District.Name,
                            Code = x.Store.District.Code
                        },
                        Province = x.Store.Province == null ? null : new Province
                        {
                            Id = x.Store.Province.Id,
                            Name = x.Store.Province.Name,
                            Code = x.Store.Province.Code
                        },
                        Ward = x.Store.Ward == null ? null : new Ward
                        {
                            Id = x.Store.Ward.Id,
                            Name = x.Store.Ward.Name,
                            Code = x.Store.Ward.Code
                        },
                    },
                }).ToListWithNoLockAsync();


            //ERoute.ERouteContents = await DataContext.ERouteContent.Where(e => e.ERouteId == Id)
            //    .Select(x => new ERouteContent
            //    {
            //        Id = x.Id,
            //        ERouteId = x.ERouteId,
            //        StoreId = x.StoreId,
            //        OrderNumber = x.OrderNumber,
            //        Monday = x.Monday,
            //        Tuesday = x.Tuesday,
            //        Wednesday = x.Wednesday,
            //        Thursday = x.Thursday,
            //        Friday = x.Friday,
            //        Saturday = x.Saturday,
            //        Sunday = x.Sunday,
            //        Week1 = x.Week1,
            //        Week2 = x.Week2,
            //        Week3 = x.Week3,
            //        Week4 = x.Week4,
            //        Store = new Store
            //        {
            //            Id = x.Store.Id,
            //            Code = x.Store.Code,
            //            CodeDraft = x.Store.CodeDraft,
            //            Name = x.Store.Name,
            //            ParentStoreId = x.Store.ParentStoreId,
            //            OrganizationId = x.Store.OrganizationId,
            //            StoreTypeId = x.Store.StoreTypeId,
            //            StoreGroupingId = x.Store.StoreGroupingId,
            //            Telephone = x.Store.Telephone,
            //            ProvinceId = x.Store.ProvinceId,
            //            DistrictId = x.Store.DistrictId,
            //            WardId = x.Store.WardId,
            //            Address = x.Store.Address,
            //            DeliveryAddress = x.Store.DeliveryAddress,
            //            Latitude = x.Store.Latitude,
            //            Longitude = x.Store.Longitude,
            //            DeliveryLatitude = x.Store.DeliveryLatitude,
            //            DeliveryLongitude = x.Store.DeliveryLongitude,
            //            OwnerName = x.Store.OwnerName,
            //            OwnerPhone = x.Store.OwnerPhone,
            //            OwnerEmail = x.Store.OwnerEmail,
            //            TaxCode = x.Store.TaxCode,
            //            LegalEntity = x.Store.LegalEntity,
            //            StatusId = x.Store.StatusId,
            //        },
            //    }).ToListWithNoLockAsync();
            ERoute.TotalStoreCounter = ERoute.ERouteContents.Select(x => x.StoreId).Count();
            return ERoute;
        }
        public async Task<bool> Create(ERoute ERoute)
        {
            ERouteDAO ERouteDAO = new ERouteDAO();
            ERouteDAO.Id = ERoute.Id;
            ERouteDAO.Code = ERoute.Code;
            ERouteDAO.Name = ERoute.Name;
            ERouteDAO.SaleEmployeeId = ERoute.SaleEmployeeId;
            ERouteDAO.OrganizationId = ERoute.OrganizationId;
            ERouteDAO.StartDate = ERoute.StartDate;
            ERouteDAO.RealStartDate = ERoute.RealStartDate;
            ERouteDAO.EndDate = ERoute.EndDate;
            ERouteDAO.ERouteTypeId = ERoute.ERouteTypeId;
            ERouteDAO.StatusId = ERoute.StatusId;
            ERouteDAO.CreatorId = ERoute.CreatorId;
            ERouteDAO.RowId = Guid.NewGuid();
            ERouteDAO.RequestStateId = ERoute.RequestStateId;
            ERouteDAO.CreatedAt = StaticParams.DateTimeNow;
            ERouteDAO.UpdatedAt = StaticParams.DateTimeNow;
            DataContext.ERoute.Add(ERouteDAO);
            await DataContext.SaveChangesAsync();
            ERoute.Id = ERouteDAO.Id;
            await SaveReference(ERoute);
            await RemoveCache();
            return true;
        }

        public async Task<bool> Update(ERoute ERoute)
        {
            ERouteDAO ERouteDAO = DataContext.ERoute.Where(x => x.Id == ERoute.Id).FirstOrDefault();
            if (ERouteDAO == null)
                return false;
            ERouteDAO.Id = ERoute.Id;
            ERouteDAO.Code = ERoute.Code;
            ERouteDAO.Name = ERoute.Name;
            ERouteDAO.SaleEmployeeId = ERoute.SaleEmployeeId;
            ERouteDAO.OrganizationId = ERoute.OrganizationId;
            ERouteDAO.StartDate = ERoute.StartDate;
            ERouteDAO.EndDate = ERoute.EndDate;
            ERouteDAO.RealStartDate = ERoute.RealStartDate;
            ERouteDAO.ERouteTypeId = ERoute.ERouteTypeId;
            ERouteDAO.StatusId = ERoute.StatusId;
            ERouteDAO.CreatorId = ERoute.CreatorId;
            ERouteDAO.RequestStateId = ERoute.RequestStateId;
            ERouteDAO.UpdatedAt = StaticParams.DateTimeNow;
            await DataContext.SaveChangesAsync();
            await SaveReference(ERoute);
            await RemoveCache();
            return true;
        }

        public async Task<bool> Delete(ERoute ERoute)
        {
            var ERouteContentIds = await DataContext.ERouteContent.Where(x => x.ERouteId == ERoute.Id).Select(x => x.Id).ToListWithNoLockAsync();
            var ERouteChangeRequestIds = await DataContext.ERouteChangeRequest.Where(x => x.ERouteId == ERoute.Id).Select(x => x.Id).ToListWithNoLockAsync();
            await DataContext.ERouteChangeRequestContent.Where(x => ERouteChangeRequestIds.Contains(x.ERouteChangeRequestId)).DeleteFromQueryAsync();
            await DataContext.ERouteChangeRequest.Where(x => x.ERouteId == ERoute.Id).DeleteFromQueryAsync();
            await DataContext.ERouteContentDay.Where(x => ERouteContentIds.Contains(x.ERouteContentId)).DeleteFromQueryAsync();
            await DataContext.ERouteContent.Where(x => x.ERouteId == ERoute.Id).DeleteFromQueryAsync();
            await DataContext.ERoute.Where(x => x.Id == ERoute.Id).UpdateFromQueryAsync(x => new ERouteDAO { DeletedAt = StaticParams.DateTimeNow });
            await RemoveCache();
            return true;
        }

        public async Task<bool> BulkMerge(List<ERoute> ERoutes)
        {
            List<ERouteDAO> ERouteDAOs = new List<ERouteDAO>();
            foreach (ERoute ERoute in ERoutes)
            {
                ERouteDAO ERouteDAO = new ERouteDAO();
                ERouteDAO.Id = ERoute.Id;
                ERouteDAO.Code = ERoute.Code;
                ERouteDAO.Name = ERoute.Name;
                ERouteDAO.SaleEmployeeId = ERoute.SaleEmployeeId;
                ERouteDAO.OrganizationId = ERoute.OrganizationId;
                ERouteDAO.StartDate = ERoute.StartDate;
                ERouteDAO.EndDate = ERoute.EndDate;
                ERouteDAO.ERouteTypeId = ERoute.ERouteTypeId;
                ERouteDAO.StatusId = ERoute.StatusId;
                ERouteDAO.CreatorId = ERoute.CreatorId;
                ERouteDAO.RequestStateId = ERoute.RequestStateId;
                ERouteDAO.CreatedAt = StaticParams.DateTimeNow;
                ERouteDAO.UpdatedAt = StaticParams.DateTimeNow;
                ERouteDAOs.Add(ERouteDAO);
            }
            await DataContext.BulkMergeAsync(ERouteDAOs);
            await RemoveCache();
            return true;
        }

        public async Task<bool> BulkDelete(List<ERoute> ERoutes)
        {
            List<long> Ids = ERoutes.Select(x => x.Id).ToList();
            await DataContext.ERouteContentDay
                .Where(x => Ids.Contains(x.ERouteContent.ERouteId))
                .DeleteFromQueryAsync();
            await DataContext.ERouteContent
                .Where(x => Ids.Contains(x.ERouteId))
                .DeleteFromQueryAsync();
            await DataContext.ERoute
                .Where(x => Ids.Contains(x.Id))
                .UpdateFromQueryAsync(x => new ERouteDAO { DeletedAt = StaticParams.DateTimeNow });
            await RemoveCache();
            return true;
        }

        private async Task SaveReference(ERoute ERoute)
        {
            var ERouteContentIds = await DataContext.ERouteContent
                .Where(x => x.ERouteId == ERoute.Id).Select(x => x.Id).ToListWithNoLockAsync();
            await DataContext.ERouteContentDay
                .Where(x => ERouteContentIds.Contains(x.ERouteContentId)).DeleteFromQueryAsync();
            await DataContext.ERouteContent
                .Where(x => x.ERouteId == ERoute.Id).DeleteFromQueryAsync();
            if (ERoute.ERouteContents != null)
            {
                ERoute.ERouteContents.ForEach(x => x.RowId = Guid.NewGuid());
                List<ERouteContentDAO> ERouteContentDAOs = new List<ERouteContentDAO>();
                foreach (ERouteContent ERouteContent in ERoute.ERouteContents)
                {
                    ERouteContentDAO ERouteContentDAO = new ERouteContentDAO();
                    ERouteContentDAO.Id = ERouteContent.Id;
                    ERouteContentDAO.ERouteId = ERoute.Id;
                    ERouteContentDAO.StoreId = ERouteContent.StoreId;
                    ERouteContentDAO.OrderNumber = ERouteContent.OrderNumber;
                    ERouteContentDAO.Monday = ERouteContent.Monday;
                    ERouteContentDAO.Tuesday = ERouteContent.Tuesday;
                    ERouteContentDAO.Wednesday = ERouteContent.Wednesday;
                    ERouteContentDAO.Thursday = ERouteContent.Thursday;
                    ERouteContentDAO.Friday = ERouteContent.Friday;
                    ERouteContentDAO.Saturday = ERouteContent.Saturday;
                    ERouteContentDAO.Sunday = ERouteContent.Sunday;
                    ERouteContentDAO.Week1 = ERouteContent.Week1;
                    ERouteContentDAO.Week2 = ERouteContent.Week2;
                    ERouteContentDAO.Week3 = ERouteContent.Week3;
                    ERouteContentDAO.Week4 = ERouteContent.Week4;
                    ERouteContentDAO.RowId = ERouteContent.RowId;
                    ERouteContentDAOs.Add(ERouteContentDAO);
                }
                await DataContext.ERouteContent.BulkMergeAsync(ERouteContentDAOs);

                List<ERouteContentDayDAO> ERouteContentDayDAOs = new List<ERouteContentDayDAO>();
                foreach (ERouteContent ERouteContent in ERoute.ERouteContents)
                {
                    ERouteContent.Id = ERouteContentDAOs.Where(x => x.RowId == ERouteContent.RowId).Select(x => x.Id).FirstOrDefault();
                    List<ERouteContentDay> ERouteContentDays = await BuildERouteContentDays(ERouteContent);
                    List<ERouteContentDayDAO> eRouteContentDayDAOs = ERouteContentDays.Select(x => new ERouteContentDayDAO
                    {
                        ERouteContentId = ERouteContent.Id,
                        Planned = x.Planned,
                        OrderDay = x.OrderDay
                    }).ToList();
                    ERouteContentDayDAOs.AddRange(eRouteContentDayDAOs);
                }

                await DataContext.ERouteContentDay.BulkMergeAsync(ERouteContentDayDAOs);
            }
        }

        private async Task<List<ERouteContentDay>> BuildERouteContentDays(ERouteContent ERouteContent)
        {
            List<ERouteContentDay> ERouteContentDays = new List<ERouteContentDay>();
            for (int i = 0; i < 28; i++)
            {
                ERouteContentDays.Add(new ERouteContentDay
                {
                    ERouteContentId = ERouteContent.Id,
                    OrderDay = i,
                    Planned = false
                });
            }

            if (ERouteContent.Week1)
            {
                if (ERouteContent.Monday)
                {
                    ERouteContentDays[0].Planned = true;
                }
                if (ERouteContent.Tuesday)
                {
                    ERouteContentDays[1].Planned = true;
                }
                if (ERouteContent.Wednesday)
                {
                    ERouteContentDays[2].Planned = true;
                }
                if (ERouteContent.Thursday)
                {
                    ERouteContentDays[3].Planned = true;
                }
                if (ERouteContent.Friday)
                {
                    ERouteContentDays[4].Planned = true;
                }
                if (ERouteContent.Saturday)
                {
                    ERouteContentDays[5].Planned = true;
                }
                if (ERouteContent.Sunday)
                {
                    ERouteContentDays[6].Planned = true;
                }
            }
            if (ERouteContent.Week2)
            {
                if (ERouteContent.Monday)
                {
                    ERouteContentDays[7].Planned = true;
                }
                if (ERouteContent.Tuesday)
                {
                    ERouteContentDays[8].Planned = true;
                }
                if (ERouteContent.Wednesday)
                {
                    ERouteContentDays[9].Planned = true;
                }
                if (ERouteContent.Thursday)
                {
                    ERouteContentDays[10].Planned = true;
                }
                if (ERouteContent.Friday)
                {
                    ERouteContentDays[11].Planned = true;
                }
                if (ERouteContent.Saturday)
                {
                    ERouteContentDays[12].Planned = true;
                }
                if (ERouteContent.Sunday)
                {
                    ERouteContentDays[13].Planned = true;
                }
            }
            if (ERouteContent.Week3)
            {
                if (ERouteContent.Monday)
                {
                    ERouteContentDays[14].Planned = true;
                }
                if (ERouteContent.Tuesday)
                {
                    ERouteContentDays[15].Planned = true;
                }
                if (ERouteContent.Wednesday)
                {
                    ERouteContentDays[16].Planned = true;
                }
                if (ERouteContent.Thursday)
                {
                    ERouteContentDays[17].Planned = true;
                }
                if (ERouteContent.Friday)
                {
                    ERouteContentDays[18].Planned = true;
                }
                if (ERouteContent.Saturday)
                {
                    ERouteContentDays[19].Planned = true;
                }
                if (ERouteContent.Sunday)
                {
                    ERouteContentDays[20].Planned = true;
                }
            }
            if (ERouteContent.Week4)
            {
                if (ERouteContent.Monday)
                {
                    ERouteContentDays[21].Planned = true;
                }
                if (ERouteContent.Tuesday)
                {
                    ERouteContentDays[22].Planned = true;
                }
                if (ERouteContent.Wednesday)
                {
                    ERouteContentDays[23].Planned = true;
                }
                if (ERouteContent.Thursday)
                {
                    ERouteContentDays[24].Planned = true;
                }
                if (ERouteContent.Friday)
                {
                    ERouteContentDays[25].Planned = true;
                }
                if (ERouteContent.Saturday)
                {
                    ERouteContentDays[26].Planned = true;
                }
                if (ERouteContent.Sunday)
                {
                    ERouteContentDays[27].Planned = true;
                }
            }

            return ERouteContentDays;
        }

        public async Task<bool> UpdateState(ERoute ERoute)
        {
            await DataContext.ERoute.Where(x => x.Id == ERoute.Id)
                .UpdateFromQueryAsync(x => new ERouteDAO
                {
                    RequestStateId = ERoute.RequestStateId,
                    UpdatedAt = StaticParams.DateTimeNow
                });
            return true;
        }

       

        //private async Task<bool> StoreActive(long id)
        //{
        //    bool flag = true;
        //    IQueryable<ERouteContentDAO> ERouteContentDAOs = DataContext.ERouteContent.AsNoTracking();
        //    ERouteContentDAOs = from e in DataContext.ERouteContent
        //                        join s in DataContext.Store on e.StoreId equals s.Id
        //                        where s.ParentStoreId != null && e.ERouteId == id
        //                        select e;
        //    List<ERouteContent> eRouteContents = new List<ERouteContent>();
        //    eRouteContents = await ERouteContentDAOs.Select(e => e).ToListWithNoLockAsync();

        //    return flag;
        //}

        public async Task<bool> Used(List<long> Ids, bool Value = true)
        {
            await DataContext.ERoute.Where(x => Ids.Contains(x.Id))
                .UpdateFromQueryAsync(x => new ERouteDAO { Used = Value });
            return true;
        }
        private async Task RemoveCache()
        {
            await RemoveFromCache(nameof(ERouteContentRepository));
        }
    }
}
