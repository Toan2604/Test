using DMS.Entities;
using DMS.Helpers;
using DMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Repositories
{
    public interface IERouteChangeRequestRepository
    {
        Task<int> Count(ERouteChangeRequestFilter ERouteChangeRequestFilter);
        Task<int> CountAll(ERouteChangeRequestFilter ERouteChangeRequestFilter);
        Task<List<ERouteChangeRequest>> List(ERouteChangeRequestFilter ERouteChangeRequestFilter);
        Task<ERouteChangeRequest> Get(long Id);
        Task<bool> Create(ERouteChangeRequest ERouteChangeRequest);
        Task<bool> Update(ERouteChangeRequest ERouteChangeRequest);
        Task<bool> Delete(ERouteChangeRequest ERouteChangeRequest);
        Task<bool> BulkMerge(List<ERouteChangeRequest> ERouteChangeRequests);
        Task<bool> BulkDelete(List<ERouteChangeRequest> ERouteChangeRequests);
    }
    public class ERouteChangeRequestRepository : IERouteChangeRequestRepository
    {
        private DataContext DataContext;
        public ERouteChangeRequestRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<ERouteChangeRequestDAO> DynamicFilter(IQueryable<ERouteChangeRequestDAO> query, ERouteChangeRequestFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.CreatedAt, filter.CreatedAt);
            query = query.Where(q => q.UpdatedAt, filter.UpdatedAt);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.ERouteId, filter.ERouteId);
            query = query.Where(q => q.CreatorId, filter.CreatorId);
            query = query.Where(q => q.RequestStateId, filter.RequestStateId);
            query = query.Where(q => q.ERoute.Code, filter.Code);
            query = query.Where(q => q.ERoute.Name, filter.Name);
            query = query.Where(q => q.ERoute.ERouteTypeId, filter.ERouteTypeId);
            query = query.Where(q => q.ERoute.SaleEmployeeId, filter.SaleEmployeeId);
            query = query.Where(q => q.ERoute.StartDate, filter.StartDate);
            query = query.Where(q => q.ERoute.EndDate, filter.EndDate);
            query = query.Where(q => q.ERoute.StatusId, filter.StatusId);
            if (filter.StoreId != null)
            {
                if (filter.StoreId.Equal.HasValue)
                {
                    query = from q in query
                            join ecrc in DataContext.ERouteChangeRequestContent on q.Id equals ecrc.ERouteChangeRequestId
                            where ecrc.StoreId == filter.StoreId.Equal
                            select q;
                    query = query.Distinct();
                }
            }
            return query;
        }

        private IQueryable<ERouteChangeRequestDAO> OrFilter(IQueryable<ERouteChangeRequestDAO> query, ERouteChangeRequestFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<ERouteChangeRequestDAO> initQuery = query.Where(q => false);
            foreach (ERouteChangeRequestFilter ERouteChangeRequestFilter in filter.OrFilter)
            {
                IQueryable<ERouteChangeRequestDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, ERouteChangeRequestFilter.Id);
                queryable = queryable.Where(q => q.ERouteId, ERouteChangeRequestFilter.ERouteId);
                queryable = queryable.Where(q => q.CreatorId, ERouteChangeRequestFilter.CreatorId);
                queryable = queryable.Where(q => q.RequestStateId, ERouteChangeRequestFilter.RequestStateId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<ERouteChangeRequestDAO> DynamicOrder(IQueryable<ERouteChangeRequestDAO> query, ERouteChangeRequestFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case ERouteChangeRequestOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case ERouteChangeRequestOrder.ERoute:
                            query = query.OrderBy(q => q.ERouteId);
                            break;
                        case ERouteChangeRequestOrder.Creator:
                            query = query.OrderBy(q => q.CreatorId);
                            break;
                        case ERouteChangeRequestOrder.RequestState:
                            query = query.OrderBy(q => q.RequestStateId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case ERouteChangeRequestOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case ERouteChangeRequestOrder.ERoute:
                            query = query.OrderByDescending(q => q.ERouteId);
                            break;
                        case ERouteChangeRequestOrder.Creator:
                            query = query.OrderByDescending(q => q.CreatorId);
                            break;
                        case ERouteChangeRequestOrder.RequestState:
                            query = query.OrderByDescending(q => q.RequestStateId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<ERouteChangeRequest>> DynamicSelect(IQueryable<ERouteChangeRequestDAO> query, ERouteChangeRequestFilter filter)
        {
            List<ERouteChangeRequest> ERouteChangeRequests = await query.Select(q => new ERouteChangeRequest()
            {
                Id = filter.Selects.Contains(ERouteChangeRequestSelect.Id) ? q.Id : default(long),
                ERouteId = filter.Selects.Contains(ERouteChangeRequestSelect.ERoute) ? q.ERouteId : default(long),
                CreatorId = filter.Selects.Contains(ERouteChangeRequestSelect.Creator) ? q.CreatorId : default(long),
                RequestStateId = filter.Selects.Contains(ERouteChangeRequestSelect.RequestState) ? q.RequestStateId : default(long),
                Creator = filter.Selects.Contains(ERouteChangeRequestSelect.Creator) && q.Creator != null ? new AppUser
                {
                    Id = q.Creator.Id,
                    Username = q.Creator.Username,
                    Address = q.Creator.Address,
                    Email = q.Creator.Email,
                } : null,
                ERoute = filter.Selects.Contains(ERouteChangeRequestSelect.ERoute) && q.ERoute != null ? new ERoute
                {
                    Id = q.ERoute.Id,
                    Code = q.ERoute.Code,
                    Name = q.ERoute.Name,
                    SaleEmployeeId = q.ERoute.SaleEmployeeId,
                    StartDate = q.ERoute.StartDate,
                    EndDate = q.ERoute.EndDate,
                    ERouteTypeId = q.ERoute.ERouteTypeId,
                    StatusId = q.ERoute.StatusId,
                    CreatorId = q.ERoute.CreatorId,
                    Creator = q.ERoute.Creator == null ? null : new AppUser
                    {
                        Id = q.ERoute.Creator.Id,
                        Username = q.ERoute.Creator.Username,
                        DisplayName = q.ERoute.Creator.DisplayName,
                        Birthday = q.ERoute.Creator.Birthday,
                    },
                    ERouteType = q.ERoute.ERouteType == null ? null : new ERouteType
                    {
                        Id = q.ERoute.ERouteType.Id,
                        Code = q.ERoute.ERouteType.Code,
                        Name = q.ERoute.ERouteType.Name,
                    },

                    SaleEmployee = q.ERoute.SaleEmployee == null ? null : new AppUser
                    {
                        Id = q.ERoute.SaleEmployee.Id,
                        Username = q.ERoute.SaleEmployee.Username,
                        DisplayName = q.ERoute.SaleEmployee.DisplayName,
                        Birthday = q.ERoute.SaleEmployee.Birthday,
                    },
                    Status = q.ERoute.Status == null ? null : new Status
                    {
                        Id = q.ERoute.Status.Id,
                        Code = q.ERoute.Status.Code,
                        Name = q.ERoute.Status.Name,
                    },
                } : null,
                RequestState = filter.Selects.Contains(ERouteChangeRequestSelect.RequestState) && q.RequestState != null ? new RequestState
                {
                    Id = q.RequestState.Id,
                    Code = q.RequestState.Code,
                    Name = q.RequestState.Name,
                } : null,
                CreatedAt = q.CreatedAt,
                UpdatedAt = q.UpdatedAt,
            }).ToListWithNoLockAsync();
            return ERouteChangeRequests;
        }

        public async Task<int> Count(ERouteChangeRequestFilter filter)
        {
            IQueryable<ERouteChangeRequestDAO> ERouteChangeRequests = DataContext.ERouteChangeRequest.AsNoTracking();
            ERouteChangeRequests = DynamicFilter(ERouteChangeRequests, filter);
            ERouteChangeRequests = OrFilter(ERouteChangeRequests, filter);
            return await ERouteChangeRequests.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(ERouteChangeRequestFilter filter)
        {
            IQueryable<ERouteChangeRequestDAO> ERouteChangeRequests = DataContext.ERouteChangeRequest.AsNoTracking();
            ERouteChangeRequests = DynamicFilter(ERouteChangeRequests, filter);
            return await ERouteChangeRequests.CountWithNoLockAsync();
        }

        public async Task<List<ERouteChangeRequest>> List(ERouteChangeRequestFilter filter)
        {
            if (filter == null) return new List<ERouteChangeRequest>();
            IQueryable<ERouteChangeRequestDAO> ERouteChangeRequestDAOs = DataContext.ERouteChangeRequest.AsNoTracking();
            ERouteChangeRequestDAOs = DynamicFilter(ERouteChangeRequestDAOs, filter);
            ERouteChangeRequestDAOs = OrFilter(ERouteChangeRequestDAOs, filter);
            ERouteChangeRequestDAOs = DynamicOrder(ERouteChangeRequestDAOs, filter);
            List<ERouteChangeRequest> ERouteChangeRequests = await DynamicSelect(ERouteChangeRequestDAOs, filter);
            return ERouteChangeRequests;
        }

        public async Task<ERouteChangeRequest> Get(long Id)
        {
            ERouteChangeRequest ERouteChangeRequest = await DataContext.ERouteChangeRequest.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new ERouteChangeRequest()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Id = x.Id,
                ERouteId = x.ERouteId,
                CreatorId = x.CreatorId,
                RequestStateId = x.RequestStateId,
                Creator = x.Creator == null ? null : new AppUser
                {
                    Id = x.Creator.Id,
                    Username = x.Creator.Username,
                    DisplayName = x.Creator.DisplayName,
                    Address = x.Creator.Address,
                    Email = x.Creator.Email,
                },
                ERoute = x.ERoute == null ? null : new ERoute
                {
                    Id = x.ERoute.Id,
                    Code = x.ERoute.Code,
                    Name = x.ERoute.Name,
                    SaleEmployeeId = x.ERoute.SaleEmployeeId,
                    StartDate = x.ERoute.StartDate,
                    EndDate = x.ERoute.EndDate,
                    ERouteTypeId = x.ERoute.ERouteTypeId,
                    StatusId = x.ERoute.StatusId,
                    CreatorId = x.ERoute.CreatorId,
                    Creator = x.ERoute.Creator == null ? null : new AppUser
                    {
                        Id = x.ERoute.Creator.Id,
                        Username = x.ERoute.Creator.Username,
                        DisplayName = x.ERoute.Creator.DisplayName,
                        Birthday = x.ERoute.Creator.Birthday,
                    },
                    ERouteType = x.ERoute.ERouteType == null ? null : new ERouteType
                    {
                        Id = x.ERoute.ERouteType.Id,
                        Code = x.ERoute.ERouteType.Code,
                        Name = x.ERoute.ERouteType.Name,
                    },
                    SaleEmployee = x.ERoute.SaleEmployee == null ? null : new AppUser
                    {
                        Id = x.ERoute.SaleEmployee.Id,
                        Username = x.ERoute.SaleEmployee.Username,
                        DisplayName = x.ERoute.SaleEmployee.DisplayName,
                        Birthday = x.ERoute.SaleEmployee.Birthday,
                    },
                    Status = x.ERoute.Status == null ? null : new Status
                    {
                        Id = x.ERoute.Status.Id,
                        Code = x.ERoute.Status.Code,
                        Name = x.ERoute.Status.Name,
                    },

                },
                RequestState = x.RequestState == null ? null : new RequestState
                {
                    Id = x.RequestState.Id,
                    Code = x.RequestState.Code,
                    Name = x.RequestState.Name,
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (ERouteChangeRequest == null)
                return null;
            ERouteChangeRequest.ERouteChangeRequestContents = await DataContext.ERouteChangeRequestContent.AsNoTracking()
                .Where(x => x.ERouteChangeRequestId == ERouteChangeRequest.Id)
                .Where(x => x.DeletedAt == null)
                .Select(x => new ERouteChangeRequestContent
                {
                    Id = x.Id,
                    ERouteChangeRequestId = x.ERouteChangeRequestId,
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
                    },
                }).ToListWithNoLockAsync();

            return ERouteChangeRequest;
        }
        public async Task<bool> Create(ERouteChangeRequest ERouteChangeRequest)
        {
            ERouteChangeRequestDAO ERouteChangeRequestDAO = new ERouteChangeRequestDAO();
            ERouteChangeRequestDAO.Id = ERouteChangeRequest.Id;
            ERouteChangeRequestDAO.ERouteId = ERouteChangeRequest.ERouteId;
            ERouteChangeRequestDAO.CreatorId = ERouteChangeRequest.CreatorId;
            ERouteChangeRequestDAO.RequestStateId = ERouteChangeRequest.RequestStateId;
            ERouteChangeRequestDAO.CreatedAt = StaticParams.DateTimeNow;
            ERouteChangeRequestDAO.UpdatedAt = StaticParams.DateTimeNow;
            DataContext.ERouteChangeRequest.Add(ERouteChangeRequestDAO);
            await DataContext.SaveChangesAsync();
            ERouteChangeRequest.Id = ERouteChangeRequestDAO.Id;
            await SaveReference(ERouteChangeRequest);
            return true;
        }

        public async Task<bool> Update(ERouteChangeRequest ERouteChangeRequest)
        {
            ERouteChangeRequestDAO ERouteChangeRequestDAO = DataContext.ERouteChangeRequest.Where(x => x.Id == ERouteChangeRequest.Id).FirstOrDefault();
            if (ERouteChangeRequestDAO == null)
                return false;
            ERouteChangeRequestDAO.Id = ERouteChangeRequest.Id;
            ERouteChangeRequestDAO.ERouteId = ERouteChangeRequest.ERouteId;
            ERouteChangeRequestDAO.CreatorId = ERouteChangeRequest.CreatorId;
            ERouteChangeRequestDAO.RequestStateId = ERouteChangeRequest.RequestStateId;
            ERouteChangeRequestDAO.UpdatedAt = StaticParams.DateTimeNow;
            await DataContext.SaveChangesAsync();
            await SaveReference(ERouteChangeRequest);
            return true;
        }

        public async Task<bool> Delete(ERouteChangeRequest ERouteChangeRequest)
        {
            await DataContext.ERouteChangeRequestContent.Where(x => x.ERouteChangeRequestId == ERouteChangeRequest.Id).DeleteFromQueryAsync();
            await DataContext.ERouteChangeRequest.Where(x => x.Id == ERouteChangeRequest.Id).UpdateFromQueryAsync(x => new ERouteChangeRequestDAO { DeletedAt = StaticParams.DateTimeNow });
            return true;
        }

        public async Task<bool> BulkMerge(List<ERouteChangeRequest> ERouteChangeRequests)
        {
            List<ERouteChangeRequestDAO> ERouteChangeRequestDAOs = new List<ERouteChangeRequestDAO>();
            foreach (ERouteChangeRequest ERouteChangeRequest in ERouteChangeRequests)
            {
                ERouteChangeRequestDAO ERouteChangeRequestDAO = new ERouteChangeRequestDAO();
                ERouteChangeRequestDAO.Id = ERouteChangeRequest.Id;
                ERouteChangeRequestDAO.ERouteId = ERouteChangeRequest.ERouteId;
                ERouteChangeRequestDAO.CreatorId = ERouteChangeRequest.CreatorId;
                ERouteChangeRequestDAO.RequestStateId = ERouteChangeRequest.RequestStateId;
                ERouteChangeRequestDAO.CreatedAt = StaticParams.DateTimeNow;
                ERouteChangeRequestDAO.UpdatedAt = StaticParams.DateTimeNow;
                ERouteChangeRequestDAOs.Add(ERouteChangeRequestDAO);
            }
            await DataContext.BulkMergeAsync(ERouteChangeRequestDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<ERouteChangeRequest> ERouteChangeRequests)
        {
            List<long> Ids = ERouteChangeRequests.Select(x => x.Id).ToList();
            IdFilter IdFilter = new IdFilter { In = Ids };
            await DataContext.ERouteChangeRequestContent
                .Where(x => Ids.Contains(x.ERouteChangeRequestId))
                .DeleteFromQueryAsync();
            await DataContext.ERouteChangeRequest
                .Where(x => Ids.Contains(x.Id))
                .UpdateFromQueryAsync(x => new ERouteChangeRequestDAO { DeletedAt = StaticParams.DateTimeNow });
            return true;
        }

        private async Task SaveReference(ERouteChangeRequest ERouteChangeRequest)
        {
            List<ERouteChangeRequestContentDAO> ERouteChangeRequestContentDAOs = await DataContext.ERouteChangeRequestContent
                .Where(x => x.ERouteChangeRequestId == ERouteChangeRequest.Id).ToListWithNoLockAsync();
            ERouteChangeRequestContentDAOs.ForEach(x => x.DeletedAt = StaticParams.DateTimeNow);
            if (ERouteChangeRequest.ERouteChangeRequestContents != null)
            {
                foreach (ERouteChangeRequestContent ERouteChangeRequestContent in ERouteChangeRequest.ERouteChangeRequestContents)
                {
                    ERouteChangeRequestContentDAO ERouteChangeRequestContentDAO = ERouteChangeRequestContentDAOs
                        .Where(x => x.Id == ERouteChangeRequestContent.Id && x.Id != 0).FirstOrDefault();
                    if (ERouteChangeRequestContentDAO == null)
                    {
                        ERouteChangeRequestContentDAO = new ERouteChangeRequestContentDAO();
                        ERouteChangeRequestContentDAO.Id = ERouteChangeRequestContent.Id;
                        ERouteChangeRequestContentDAO.ERouteChangeRequestId = ERouteChangeRequest.Id;
                        ERouteChangeRequestContentDAO.StoreId = ERouteChangeRequestContent.StoreId;
                        ERouteChangeRequestContentDAO.OrderNumber = ERouteChangeRequestContent.OrderNumber;
                        ERouteChangeRequestContentDAO.Monday = ERouteChangeRequestContent.Monday;
                        ERouteChangeRequestContentDAO.Tuesday = ERouteChangeRequestContent.Tuesday;
                        ERouteChangeRequestContentDAO.Wednesday = ERouteChangeRequestContent.Wednesday;
                        ERouteChangeRequestContentDAO.Thursday = ERouteChangeRequestContent.Thursday;
                        ERouteChangeRequestContentDAO.Friday = ERouteChangeRequestContent.Friday;
                        ERouteChangeRequestContentDAO.Saturday = ERouteChangeRequestContent.Saturday;
                        ERouteChangeRequestContentDAO.Sunday = ERouteChangeRequestContent.Sunday;
                        ERouteChangeRequestContentDAO.Week1 = ERouteChangeRequestContent.Week1;
                        ERouteChangeRequestContentDAO.Week2 = ERouteChangeRequestContent.Week2;
                        ERouteChangeRequestContentDAO.Week3 = ERouteChangeRequestContent.Week3;
                        ERouteChangeRequestContentDAO.Week4 = ERouteChangeRequestContent.Week4;
                        ERouteChangeRequestContentDAOs.Add(ERouteChangeRequestContentDAO);
                        ERouteChangeRequestContentDAO.CreatedAt = StaticParams.DateTimeNow;
                        ERouteChangeRequestContentDAO.UpdatedAt = StaticParams.DateTimeNow;
                        ERouteChangeRequestContentDAO.DeletedAt = null;
                    }
                    else
                    {
                        ERouteChangeRequestContentDAO.Id = ERouteChangeRequestContent.Id;
                        ERouteChangeRequestContentDAO.ERouteChangeRequestId = ERouteChangeRequest.Id;
                        ERouteChangeRequestContentDAO.StoreId = ERouteChangeRequestContent.StoreId;
                        ERouteChangeRequestContentDAO.OrderNumber = ERouteChangeRequestContent.OrderNumber;
                        ERouteChangeRequestContentDAO.Monday = ERouteChangeRequestContent.Monday;
                        ERouteChangeRequestContentDAO.Tuesday = ERouteChangeRequestContent.Tuesday;
                        ERouteChangeRequestContentDAO.Wednesday = ERouteChangeRequestContent.Wednesday;
                        ERouteChangeRequestContentDAO.Thursday = ERouteChangeRequestContent.Thursday;
                        ERouteChangeRequestContentDAO.Friday = ERouteChangeRequestContent.Friday;
                        ERouteChangeRequestContentDAO.Saturday = ERouteChangeRequestContent.Saturday;
                        ERouteChangeRequestContentDAO.Sunday = ERouteChangeRequestContent.Sunday;
                        ERouteChangeRequestContentDAO.Week1 = ERouteChangeRequestContent.Week1;
                        ERouteChangeRequestContentDAO.Week2 = ERouteChangeRequestContent.Week2;
                        ERouteChangeRequestContentDAO.Week3 = ERouteChangeRequestContent.Week3;
                        ERouteChangeRequestContentDAO.Week4 = ERouteChangeRequestContent.Week4;
                        ERouteChangeRequestContentDAO.UpdatedAt = StaticParams.DateTimeNow;
                        ERouteChangeRequestContentDAO.DeletedAt = null;
                    }
                }
                await DataContext.ERouteChangeRequestContent.BulkMergeAsync(ERouteChangeRequestContentDAOs);
            }
        }

    }
}
