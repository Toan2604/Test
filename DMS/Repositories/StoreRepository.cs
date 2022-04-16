using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Thinktecture;
using Thinktecture.EntityFrameworkCore.TempTables;
using TrueSight.Common;

namespace DMS.Repositories
{
    public interface IStoreRepository
    {
        Task<int> Count(StoreFilter StoreFilter);
        Task<int> CountAll(StoreFilter StoreFilter);
        Task<List<Store>> List(StoreFilter StoreFilter);
        Task<List<long>> ListToIds(StoreFilter filter);
        Task<List<Store>> List(List<long> Ids);
        Task<Store> Get(long Id);
        Task<bool> Create(Store Store);
        Task<bool> Update(Store Store);
        Task<bool> Delete(Store Store);
        Task<bool> BulkMerge(List<Store> Stores);
        Task<bool> BulkDelete(List<Store> Stores);
        Task<bool> Used(List<long> Ids);
    }
    public class StoreRepository : IStoreRepository
    {
        private DataContext DataContext;
        public StoreRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private async Task<IQueryable<StoreDAO>> DynamicFilter(IQueryable<StoreDAO> query, StoreFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.DeletedAt == null);

            if (filter.AppUserId != null)
            {
                if (filter.AppUserId.In != null && filter.AppUserId.In.Count > 0)
                {
                    var mapping = await DataContext.AppUserStoreMapping.AsNoTracking()
                        .Where(q => q.AppUserId, filter.AppUserId)
                        .ToListWithNoLockAsync();
                    // Nhân viên đi tuyến
                    List<long> LineAppUserIds = mapping.Select(x => x.AppUserId).Distinct().ToList();
                    // Nhân viên không đi tuyến
                    List<long> SubAppUserIds = filter.AppUserId.In.Except(LineAppUserIds).ToList();
                    List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization
                        .Where(x => x.DeletedAt == null)
                        .Where(x => x.StatusId, new IdFilter { Equal = StatusEnum.ACTIVE.Id })
                        .Select(x => new OrganizationDAO
                        {
                            Id = x.Id,
                            Path = x.Path
                        }).ToListWithNoLockAsync();
                    IdFilter SubAppUserIdFilter = new IdFilter { In = SubAppUserIds };
                    var OrganizationPaths = await DataContext.AppUser
                        .Where(x => x.Id, new IdFilter { In = filter.AppUserId.In })
                        .Select(x => new { AppUserId = x.Id, OrganizationPath = x.Organization.Path })
                        .ToListWithNoLockAsync();
                    IQueryable<StoreDAO> StoreOutLine;
                    IQueryable<StoreDAO> StoreInLine;
                    List<long> StoreIds = mapping.Select(x => x.StoreId).Distinct().ToList();
                    // Xét các nhân viên không có cấu hình đi tuyến
                    {
                        List<string> SubOrganizationPaths = OrganizationPaths
                           .Where(x => SubAppUserIds.Any(y => y == x.AppUserId))
                           .Select(x => x.OrganizationPath)
                           .ToList();
                        List<long> SubOrganizationIds = OrganizationDAOs
                            .Where(x => SubOrganizationPaths.Any(o => x.Path.StartsWith(o)))
                            .Select(x => x.Id)
                            .ToList();
                        StoreOutLine = query.Where(q => q.OrganizationId, new IdFilter { In = SubOrganizationIds });
                    }
                    // xét các nhân viên có cấu hình đi tuyến
                    {
                        List<string> SubOrganizationPaths = OrganizationPaths
                            .Where(x => LineAppUserIds.Any(y => y == x.AppUserId))
                            .Select(x => x.OrganizationPath)
                            .ToList();
                        List<long> SubOrganizationIds = OrganizationDAOs
                            .Where(x => SubOrganizationPaths.Any(o => x.Path.StartsWith(o)))
                            .Select(x => x.Id)
                            .ToList();
                        var subquery1 = query.Where(q => q.Id, new IdFilter { In = StoreIds });
                        var subquery2 = query
                            .Where(q => q.StoreStatusId == StoreStatusEnum.DRAFT.Id)
                            .Where(q => q.OrganizationId, new IdFilter { In = SubOrganizationIds });
                        StoreInLine = subquery1.Union(subquery2);
                    }
                    query = StoreOutLine.Union(StoreInLine);
                }
                else if (filter.AppUserId.Equal != null)
                {
                    var tempQuery = from q in query
                                    join m in DataContext.AppUserStoreMapping on q.Id equals m.StoreId
                                    where m.AppUserId == filter.AppUserId.Equal.Value
                                    select q;
                    string OrganizationPath = await DataContext.AppUser.AsNoTracking()
                        .Where(x => x.Id, new IdFilter { Equal = filter.AppUserId.Equal })
                        .Select(x => x.Organization.Path).FirstOrDefaultWithNoLockAsync();

                    //nếu ko có cửa hàng nào thì lấy all cửa hàng của Org nhân viên
                    int tempQueryCount = tempQuery.Count();
                    if (tempQuery != null && tempQueryCount == 0)
                    {
                        query = query.Where(q => q.Organization.Path.StartsWith(OrganizationPath));
                    }
                    //nếu có thì cộng thêm với all draft store của org
                    else if (tempQuery != null && tempQueryCount > 0)
                    {
                        var StoreInScopedIds = await tempQuery.Select(x => x.Id).Distinct().ToListWithNoLockAsync();
                        IdFilter StoreInScopedIdFilter = new IdFilter { In = StoreInScopedIds };
                        var Storequery = query
                             .Where(q => q.StoreStatusId == StoreStatusEnum.DRAFT.Id)
                             .Where(q => q.Organization.Path.StartsWith(OrganizationPath));
                        var StoreInScopequery = query.Where(q => q.Id, StoreInScopedIdFilter);
                        query = StoreInScopequery;
                        if (Storequery != null && Storequery.Count() > 0)
                            query = query.Union(Storequery);
                    }
                }
            }

            if (filter.StoreCheckingStatusId != null)
            {
                if (filter.StoreCheckingStatusId.Equal.HasValue)
                {
                    var Now = StaticParams.DateTimeNow.AddHours(filter.TimeZone);
                    bool FilterByMonth = filter.StoreCheckingStatusId.Equal == StoreCheckingStatusEnum.CHECKED_IN_MONTH.Id ||
                        filter.StoreCheckingStatusId.Equal == StoreCheckingStatusEnum.NOTCHECKED_IN_MONTH.Id;

                    var Start = Now.Date.AddHours(0 - filter.TimeZone);
                    var End = Now.Date.AddHours(0 - filter.TimeZone).AddDays(1).AddSeconds(-1);
                    if (FilterByMonth)
                    {
                        Start = new DateTime(Now.Year, Now.Month, 1).AddHours(0 - filter.TimeZone);
                        End = new DateTime(Now.Year, Now.Month, 1).AddMonths(1).AddSeconds(-1).AddHours(0 - filter.TimeZone);
                    }

                    var storeCheckingQuery = DataContext.StoreChecking
                           .Where(sc => sc.CheckOutAt.HasValue && Start <= sc.CheckOutAt.Value && sc.CheckOutAt.Value <= End);
                    if (filter.AppUserId != null && filter.AppUserId.Equal.HasValue)
                    {
                        storeCheckingQuery = storeCheckingQuery.Where(x => x.SaleEmployeeId == filter.AppUserId.Equal.Value);
                    }

                    var storeIds = await storeCheckingQuery.Select(x => x.StoreId).Distinct().ToListWithNoLockAsync();
                    if (filter.StoreCheckingStatusId.Equal.Value == StoreCheckingStatusEnum.CHECKED.Id ||
                        filter.StoreCheckingStatusId.Equal.Value == StoreCheckingStatusEnum.CHECKED_IN_MONTH.Id)
                    {
                        IdFilter IdFilter = new IdFilter { In = storeIds };
                        query = query.Where(q => q.Id, IdFilter);
                    }
                    if (filter.StoreCheckingStatusId.Equal.Value == StoreCheckingStatusEnum.NOTCHECKED.Id ||
                        filter.StoreCheckingStatusId.Equal.Value == StoreCheckingStatusEnum.NOTCHECKED_IN_MONTH.Id)
                    {
                        IdFilter IdFilter = new IdFilter { NotIn = storeIds };
                        query = query.Where(q => q.Id, IdFilter);
                    }
                }
            }
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
                    List<long> Ids = Branches.Select(o => o.Id).Distinct().ToList();
                    IdFilter IdFilter = new IdFilter { In = Ids };
                    query = query.Where(q => q.OrganizationId, IdFilter);
                }
                if (filter.OrganizationId.NotIn != null)
                {
                    List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.AsNoTracking()
                        .Where(o => o.DeletedAt == null && o.StatusId == 1).ToListWithNoLockAsync();
                    List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => filter.OrganizationId.NotIn.Contains(o.Id)).ToList();
                    List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> Ids = Branches.Select(o => o.Id).Distinct().ToList();
                    IdFilter IdFilter = new IdFilter { NotIn = Ids };
                    query = query.Where(q => q.OrganizationId, IdFilter);
                }
            }

            if (filter.StoreGroupingId != null)
            {
                if (filter.StoreGroupingId.Equal != null)
                {
                    StoreGroupingDAO StoreGroupingDAO = await DataContext.StoreGrouping
                        .Where(o => o.Id == filter.StoreGroupingId.Equal.Value).FirstOrDefaultWithNoLockAsync();
                    query = from q in query
                            join ssg in DataContext.StoreStoreGroupingMapping on q.Id equals ssg.StoreId
                            join sg in DataContext.StoreGrouping on ssg.StoreGroupingId equals sg.Id
                            where sg.Path.StartsWith(StoreGroupingDAO.Path)
                            select q;
                }
                if (filter.StoreGroupingId.NotEqual != null)
                {
                    StoreGroupingDAO StoreGroupingDAO = await DataContext.StoreGrouping
                        .Where(o => o.Id == filter.StoreGroupingId.NotEqual.Value).FirstOrDefaultWithNoLockAsync();
                    query = from q in query
                            join ssg in DataContext.StoreStoreGroupingMapping on q.Id equals ssg.StoreId
                            join sg in DataContext.StoreGrouping on ssg.StoreGroupingId equals sg.Id
                            where !sg.Path.StartsWith(StoreGroupingDAO.Path)
                            select q;
                }
                if (filter.StoreGroupingId.In != null)
                {
                    List<StoreGroupingDAO> StoreGroupingDAOs = await DataContext.StoreGrouping
                        .Where(o => o.DeletedAt == null).ToListWithNoLockAsync();
                    List<StoreGroupingDAO> Parents = StoreGroupingDAOs.Where(o => filter.StoreGroupingId.In.Contains(o.Id)).ToList();
                    List<StoreGroupingDAO> Branches = StoreGroupingDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> StoreGroupingIds = Branches.Select(x => x.Id).ToList();
                    query = from q in query
                            join ssg in DataContext.StoreStoreGroupingMapping on q.Id equals ssg.StoreId
                            join sg in DataContext.StoreGrouping on ssg.StoreGroupingId equals sg.Id
                            where StoreGroupingIds.Contains(sg.Id)
                            select q;
                }
                if (filter.StoreGroupingId.NotIn != null)
                {
                    List<StoreGroupingDAO> StoreGroupingDAOs = await DataContext.StoreGrouping
                        .Where(o => o.DeletedAt == null).ToListWithNoLockAsync();
                    List<StoreGroupingDAO> Parents = StoreGroupingDAOs.Where(o => filter.StoreGroupingId.NotIn.Contains(o.Id)).ToList();
                    List<StoreGroupingDAO> Branches = StoreGroupingDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> StoreGroupingIds = Branches.Select(x => x.Id).ToList();
                    query = from q in query
                            join ssg in DataContext.StoreStoreGroupingMapping on q.Id equals ssg.StoreId
                            join sg in DataContext.StoreGrouping on ssg.StoreGroupingId equals sg.Id
                            where !StoreGroupingIds.Contains(sg.Id)
                            select q;
                }
            }

            if (filter.StoreStatusId != null && filter.StoreStatusId.Equal.HasValue && filter.StoreStatusId.Equal != StoreStatusEnum.ALL.Id)
            {
                if (filter.StoreStatusId.Equal == StoreStatusEnum.DRAFT.Id)
                {
                    query = query.Where(q => q.StoreStatusId, filter.StoreStatusId);
                    if (filter.StoreDraftTypeId != null && filter.StoreDraftTypeId.HasValue && filter.StoreDraftTypeId.Equal == StoreDraftTypeEnum.MINE.Id && filter.AppUserId.HasValue)
                        query = query.Where(q => q.AppUserId.Value == filter.AppUserId.Equal);
                }
                if (filter.StoreStatusId.Equal == StoreStatusEnum.OFFICIAL.Id)
                {
                    query = query.Where(q => q.StoreStatusId, filter.StoreStatusId);
                }
            }
            if (filter.StoreStatusId != null && filter.StoreStatusId.NotEqual.HasValue)
            {
                if (filter.StoreStatusId.NotEqual == StoreStatusEnum.DRAFT.Id)
                {
                    query = query.Where(q => q.StoreStatusId, filter.StoreStatusId);
                }
            }
            if (filter.StoreUserStatusId != null && filter.StoreUserStatusId.HasValue)
            {
                var StoreUserQuery = DataContext.StoreUser.AsNoTracking();
                var OpenedStoreIds = await StoreUserQuery.Where(x => x.StatusId == StatusEnum.ACTIVE.Id).Select(x => x.StoreId).ToListWithNoLockAsync();
                var LockedStoreIds = await StoreUserQuery.Where(x => x.StatusId == StatusEnum.INACTIVE.Id).Select(x => x.StoreId).ToListWithNoLockAsync();
                if (filter.StoreUserStatusId.Equal == StoreUserStatusEnum.NOT_YET_CREATED.Id)
                {
                    query = query.Where(q => q.Id, new IdFilter { NotIn = OpenedStoreIds });
                    query = query.Where(q => q.Id, new IdFilter { NotIn = LockedStoreIds });
                } // filter các cửa hàng chưa mở tài khoản
                if (filter.StoreUserStatusId.Equal == StoreUserStatusEnum.ALREADY_CREATED.Id)
                {
                    query = query.Where(x => x.Id, new IdFilter { In = OpenedStoreIds });
                } // filter các cửa hàng chưa mở tài khoản
                if (filter.StoreUserStatusId.Equal == StoreUserStatusEnum.ALREADY_LOCKED.Id)
                {
                    query = query.Where(x => x.Id, new IdFilter { In = LockedStoreIds });
                } // filter các cửa hàng bị khóa tài khoản
            }

            if (filter.Name?.HasValue == true)
                query = query.Where(q => q.Name, filter.Name).Union(query.Where(q => q.UnsignName, filter.Name));
            if (filter.Address?.HasValue == true)
                query = query.Where(q => q.Address, filter.Address).Union(query.Where(q => q.UnsignAddress, filter.Address));
            if (filter.Id?.In != null)
            {
                List<long> TempIds = query.Select(x => x.Id).ToList();
                filter.Id.In = filter.Id.In.Intersect(TempIds).ToList();
            }
            query = query.Where(q => q.EstimatedRevenueId, filter.EstimatedRevenueId);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.CreatedAt, filter.CreatedAt);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.CodeDraft, filter.CodeDraft);
            query = query.Where(q => q.ParentStoreId.Value, filter.ParentStoreId);
            query = query.Where(q => q.Telephone, filter.Telephone);
            query = query.Where(q => q.StoreTypeId, filter.StoreTypeId);
            query = query.Where(q => q.ProvinceId, filter.ProvinceId);
            query = query.Where(q => q.DistrictId, filter.DistrictId);
            query = query.Where(q => q.WardId, filter.WardId);
            query = query.Where(q => q.DeliveryAddress, filter.DeliveryAddress);
            query = query.Where(q => q.Latitude, filter.Latitude);
            query = query.Where(q => q.Longitude, filter.Longitude);
            query = query.Where(q => q.DeliveryLatitude, filter.DeliveryLatitude);
            query = query.Where(q => q.DeliveryLongitude, filter.DeliveryLongitude);
            query = query.Where(q => q.OwnerName, filter.OwnerName);
            query = query.Where(q => q.OwnerPhone, filter.OwnerPhone);
            query = query.Where(q => q.OwnerEmail, filter.OwnerEmail);
            query = query.Where(q => q.StatusId, filter.StatusId);
            query = query.Where(q => q.CreatorId, filter.CreatorId);

            query = query.Distinct();
            if (filter.Search != null)
                query = query.Where(q =>
                q.CodeDraft.ToLower().Contains(filter.Search.ToLower()) ||
                q.Code.ToLower().Contains(filter.Search.ToLower()) ||
                q.Address.ToLower().Contains(filter.Search.ToLower()) ||
                q.UnsignAddress.ToLower().Contains(filter.Search.ToLower()) ||
                q.UnsignName.ToLower().Contains(filter.Search.ToLower()) ||
                q.Name.ToLower().Contains(filter.Search.ToLower()));
            return query;
        }

        private async Task<IQueryable<StoreDAO>> OrFilter(IQueryable<StoreDAO> query, StoreFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<StoreDAO> initQuery = query.Where(q => false);
            foreach (StoreFilter StoreFilter in filter.OrFilter)
            {
                IQueryable<StoreDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, StoreFilter.Id);
                queryable = queryable.Where(q => q.Code, StoreFilter.Code);
                queryable = queryable.Where(q => q.CodeDraft, StoreFilter.CodeDraft);
                queryable = queryable.Where(q => q.Name, StoreFilter.Name);
                queryable = queryable.Where(q => q.ParentStoreId, StoreFilter.ParentStoreId);
                if (StoreFilter.OrganizationId != null)
                {
                    if (StoreFilter.OrganizationId.Equal != null)
                    {
                        OrganizationDAO OrganizationDAO = await DataContext.Organization
                            .Where(o => o.Id == StoreFilter.OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
                        queryable = queryable.Where(q => q.Organization.Path.StartsWith(OrganizationDAO.Path));
                    }
                    if (StoreFilter.OrganizationId.NotEqual != null)
                    {
                        OrganizationDAO OrganizationDAO = await DataContext.Organization
                            .Where(o => o.Id == StoreFilter.OrganizationId.NotEqual.Value).FirstOrDefaultWithNoLockAsync();
                        queryable = queryable.Where(q => !q.Organization.Path.StartsWith(OrganizationDAO.Path));
                    }
                    if (StoreFilter.OrganizationId.In != null)
                    {
                        List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization
                            .Where(o => o.DeletedAt == null && o.StatusId == 1).ToListWithNoLockAsync();
                        List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => StoreFilter.OrganizationId.In.Contains(o.Id)).ToList();
                        List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> Ids = Branches.Select(o => o.Id).ToList();
                        IdFilter IdFilter = new IdFilter { In = Ids };
                        queryable = queryable.Where(q => q.OrganizationId, IdFilter);
                    }
                    if (StoreFilter.OrganizationId.NotIn != null)
                    {
                        List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization
                            .Where(o => o.DeletedAt == null && o.StatusId == 1).ToListWithNoLockAsync();
                        List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => StoreFilter.OrganizationId.NotIn.Contains(o.Id)).ToList();
                        List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> Ids = Branches.Select(o => o.Id).ToList();
                        IdFilter IdFilter = new IdFilter { NotIn = Ids };
                        queryable = queryable.Where(q => q.OrganizationId, IdFilter);
                    }
                }
                queryable = queryable.Where(q => q.StoreTypeId, StoreFilter.StoreTypeId);
                if (StoreFilter.StoreGroupingId != null)
                {
                    if (StoreFilter.StoreGroupingId.Equal != null)
                    {
                        StoreGroupingDAO StoreGroupingDAO = await DataContext.StoreGrouping
                            .Where(o => o.Id == StoreFilter.StoreGroupingId.Equal.Value).FirstOrDefaultWithNoLockAsync();
                        queryable = from q in queryable
                                    join ssg in DataContext.StoreStoreGroupingMapping on q.Id equals ssg.StoreId
                                    join sg in DataContext.ProductGrouping on ssg.StoreGroupingId equals sg.Id
                                    where sg.Path.StartsWith(StoreGroupingDAO.Path)
                                    select q;
                    }
                    if (StoreFilter.StoreGroupingId.NotEqual != null)
                    {
                        StoreGroupingDAO StoreGroupingDAO = await DataContext.StoreGrouping
                            .Where(o => o.Id == StoreFilter.StoreGroupingId.Equal.Value).FirstOrDefaultWithNoLockAsync();
                        queryable = from q in queryable
                                    join ssg in DataContext.StoreStoreGroupingMapping on q.Id equals ssg.StoreId
                                    join sg in DataContext.ProductGrouping on ssg.StoreGroupingId equals sg.Id
                                    where !sg.Path.StartsWith(StoreGroupingDAO.Path)
                                    select q;
                    }
                    if (StoreFilter.StoreGroupingId.In != null)
                    {
                        List<StoreGroupingDAO> StoreGroupingDAOs = await DataContext.StoreGrouping
                            .Where(o => o.DeletedAt == null).ToListWithNoLockAsync();
                        List<StoreGroupingDAO> Parents = StoreGroupingDAOs.Where(o => StoreFilter.StoreGroupingId.In.Contains(o.Id)).ToList();
                        List<StoreGroupingDAO> Branches = StoreGroupingDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> StoreGroupingIds = Branches.Select(o => o.Id).ToList();
                        queryable = from q in queryable
                                    join ssg in DataContext.StoreStoreGroupingMapping on q.Id equals ssg.StoreId
                                    join sg in DataContext.ProductGrouping on ssg.StoreGroupingId equals sg.Id
                                    where StoreGroupingIds.Contains(sg.Id)
                                    select q;
                    }
                    if (StoreFilter.StoreGroupingId.NotIn != null)
                    {
                        List<StoreGroupingDAO> StoreGroupingDAOs = await DataContext.StoreGrouping
                            .Where(o => o.DeletedAt == null).ToListWithNoLockAsync();
                        List<StoreGroupingDAO> Parents = StoreGroupingDAOs.Where(o => StoreFilter.StoreGroupingId.In.Contains(o.Id)).ToList();
                        List<StoreGroupingDAO> Branches = StoreGroupingDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> StoreGroupingIds = Branches.Select(o => o.Id).ToList();
                        queryable = from q in queryable
                                    join ssg in DataContext.StoreStoreGroupingMapping on q.Id equals ssg.StoreId
                                    join sg in DataContext.ProductGrouping on ssg.StoreGroupingId equals sg.Id
                                    where !StoreGroupingIds.Contains(sg.Id)
                                    select q;
                    }
                    queryable = queryable.Distinct();
                }
                queryable = queryable.Where(q => q.EstimatedRevenueId, StoreFilter.EstimatedRevenueId);
                queryable = queryable.Where(q => q.Telephone, StoreFilter.Telephone);
                queryable = queryable.Where(q => q.ProvinceId, StoreFilter.ProvinceId);
                queryable = queryable.Where(q => q.DistrictId, StoreFilter.DistrictId);
                queryable = queryable.Where(q => q.WardId, StoreFilter.WardId);
                queryable = queryable.Where(q => q.Address, StoreFilter.Address);
                queryable = queryable.Where(q => q.DeliveryAddress, StoreFilter.DeliveryAddress);
                queryable = queryable.Where(q => q.Latitude, StoreFilter.Latitude);
                queryable = queryable.Where(q => q.Longitude, StoreFilter.Longitude);
                queryable = queryable.Where(q => q.DeliveryLatitude, StoreFilter.DeliveryLatitude);
                queryable = queryable.Where(q => q.DeliveryLongitude, StoreFilter.DeliveryLongitude);
                queryable = queryable.Where(q => q.OwnerName, StoreFilter.OwnerName);
                queryable = queryable.Where(q => q.OwnerPhone, StoreFilter.OwnerPhone);
                queryable = queryable.Where(q => q.OwnerEmail, StoreFilter.OwnerEmail);
                queryable = queryable.Where(q => q.StatusId, StoreFilter.StatusId);
                if (StoreFilter.AppUserId != null)
                {
                    if (StoreFilter.AppUserId.In != null && StoreFilter.AppUserId.In.Count > 0)
                    {
                        var mapping = await DataContext.AppUserStoreMapping.AsNoTracking()
                            .Where(q => q.AppUserId, StoreFilter.AppUserId)
                            .ToListWithNoLockAsync();
                        // Nhân viên đi tuyến
                        List<long> LineAppUserIds = mapping.Select(x => x.AppUserId).Distinct().ToList();
                        // Nhân viên không đi tuyến
                        List<long> SubAppUserIds = StoreFilter.AppUserId.In.Except(LineAppUserIds).ToList();
                        List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization
                            .Where(x => x.DeletedAt == null && x.StatusId == StatusEnum.ACTIVE.Id)
                            .Select(x => new OrganizationDAO
                            {
                                Id = x.Id,
                                Path = x.Path
                            }).ToListWithNoLockAsync();
                        IdFilter SubAppUserIdFilter = new IdFilter { In = SubAppUserIds };
                        var OrganizationPaths = await DataContext.AppUser
                            .Where(x => x.Id, new IdFilter { In = StoreFilter.AppUserId.In })
                            .Select(x => new { AppUserId = x.Id, OrganizationPath = x.Organization.Path })
                            .ToListWithNoLockAsync();
                        IQueryable<StoreDAO> StoreOutLine;
                        IQueryable<StoreDAO> StoreInLine;
                        List<long> StoreIds = mapping.Select(x => x.StoreId).Distinct().ToList();
                        // Xét các nhân viên không có cấu hình đi tuyến
                        {
                            List<string> SubOrganizationPaths = OrganizationPaths
                               .Where(x => SubAppUserIds.Any(y => y == x.AppUserId))
                               .Select(x => x.OrganizationPath)
                               .ToList();
                            List<long> SubOrganizationIds = OrganizationDAOs
                                .Where(x => SubOrganizationPaths.Any(o => x.Path.StartsWith(o)))
                                .Select(x => x.Id)
                                .ToList();
                            StoreOutLine = queryable.Where(q => q.OrganizationId, new IdFilter { In = SubOrganizationIds });
                        }
                        // xét các nhân viên có cấu hình đi tuyến
                        {
                            List<string> SubOrganizationPaths = OrganizationPaths
                                .Where(x => LineAppUserIds.Any(y => y == x.AppUserId))
                                .Select(x => x.OrganizationPath)
                                .ToList();
                            List<long> SubOrganizationIds = OrganizationDAOs
                                .Where(x => SubOrganizationPaths.Any(o => x.Path.StartsWith(o)))
                                .Select(x => x.Id)
                                .ToList();
                            var subquery1 = queryable.Where(q => q.Id, new IdFilter { In = StoreIds });
                            var subquery2 = queryable
                                .Where(q => q.StoreStatusId == StoreStatusEnum.DRAFT.Id)
                                .Where(q => q.OrganizationId, new IdFilter { In = SubOrganizationIds });
                            StoreInLine = subquery1.Union(subquery2);
                        }
                        queryable = StoreOutLine.Union(StoreInLine);
                    }
                    else if (StoreFilter.AppUserId.Equal != null)
                    {
                        var tempQuery = from q in queryable
                                        join m in DataContext.AppUserStoreMapping on q.Id equals m.StoreId
                                        where m.AppUserId == StoreFilter.AppUserId.Equal.Value
                                        select q;
                        string OrganizationPath = await DataContext.AppUser.Where(x => x.Id == StoreFilter.AppUserId.Equal)
                            .Select(x => x.Organization.Path).FirstOrDefaultWithNoLockAsync();

                        //nếu ko có cửa hàng nào thì lấy all cửa hàng của Org nhân viên
                        int tempQueryCount = await tempQuery.CountWithNoLockAsync();
                        if (tempQuery != null && tempQueryCount == 0)
                        {
                            queryable = queryable.Where(q => q.Organization.Path.StartsWith(OrganizationPath));
                        }
                        //nếu có thì cộng thêm với all draft store của org
                        else if (tempQuery != null && tempQueryCount > 0)
                        {
                            var StoreInScopedIds = await tempQuery.Select(x => x.Id).Distinct().ToListWithNoLockAsync();
                            IdFilter StoreInScopedIdFilter = new IdFilter { In = StoreInScopedIds };
                            var DraftStorequery = queryable
                                 .Where(q => q.StoreStatusId == StoreStatusEnum.DRAFT.Id)
                                 .Where(q => q.Organization.Path.StartsWith(OrganizationPath));
                            var StoreInScopequery = queryable.Where(q => q.Id, StoreInScopedIdFilter);
                            queryable = StoreInScopequery;
                            if (DraftStorequery != null && DraftStorequery.Count() > 0)
                                queryable = queryable.Union(DraftStorequery);
                        }
                    }
                }
                queryable = queryable.Where(q => q.StoreStatusId, StoreFilter.StoreStatusId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<StoreDAO> DynamicOrder(IQueryable<StoreDAO> query, StoreFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case StoreOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case StoreOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case StoreOrder.CodeDraft:
                            query = query.OrderBy(q => q.CodeDraft);
                            break;
                        case StoreOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case StoreOrder.ParentStore:
                            query = query.OrderBy(q => q.ParentStoreId);
                            break;
                        case StoreOrder.Organization:
                            query = query.OrderBy(q => q.OrganizationId);
                            break;
                        case StoreOrder.StoreType:
                            query = query.OrderBy(q => q.StoreTypeId);
                            break;
                        case StoreOrder.Telephone:
                            query = query.OrderBy(q => q.Telephone);
                            break;
                        case StoreOrder.Province:
                            query = query.OrderBy(q => q.ProvinceId);
                            break;
                        case StoreOrder.District:
                            query = query.OrderBy(q => q.DistrictId);
                            break;
                        case StoreOrder.Ward:
                            query = query.OrderBy(q => q.WardId);
                            break;
                        case StoreOrder.Address:
                            query = query.OrderBy(q => q.Address);
                            break;
                        case StoreOrder.DeliveryAddress:
                            query = query.OrderBy(q => q.DeliveryAddress);
                            break;
                        case StoreOrder.Latitude:
                            query = query.OrderBy(q => q.Latitude);
                            break;
                        case StoreOrder.Longitude:
                            query = query.OrderBy(q => q.Longitude);
                            break;
                        case StoreOrder.DeliveryLatitude:
                            query = query.OrderBy(q => q.DeliveryLatitude);
                            break;
                        case StoreOrder.DeliveryLongitude:
                            query = query.OrderBy(q => q.DeliveryLongitude);
                            break;
                        case StoreOrder.OwnerName:
                            query = query.OrderBy(q => q.OwnerName);
                            break;
                        case StoreOrder.OwnerPhone:
                            query = query.OrderBy(q => q.OwnerPhone);
                            break;
                        case StoreOrder.OwnerEmail:
                            query = query.OrderBy(q => q.OwnerEmail);
                            break;
                        case StoreOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                        case StoreOrder.AppUser:
                            query = query.OrderBy(q => q.AppUserId);
                            break;
                        case StoreOrder.StoreStatus:
                            query = query.OrderBy(q => q.StoreStatusId);
                            break;
                        case StoreOrder.EstimatedRevenue:
                            query = query.OrderBy(q => q.EstimatedRevenueId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case StoreOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case StoreOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case StoreOrder.CodeDraft:
                            query = query.OrderByDescending(q => q.CodeDraft);
                            break;
                        case StoreOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case StoreOrder.ParentStore:
                            query = query.OrderByDescending(q => q.ParentStoreId);
                            break;
                        case StoreOrder.Organization:
                            query = query.OrderByDescending(q => q.OrganizationId);
                            break;
                        case StoreOrder.StoreType:
                            query = query.OrderByDescending(q => q.StoreTypeId);
                            break;
                        case StoreOrder.Telephone:
                            query = query.OrderByDescending(q => q.Telephone);
                            break;
                        case StoreOrder.Province:
                            query = query.OrderByDescending(q => q.ProvinceId);
                            break;
                        case StoreOrder.District:
                            query = query.OrderByDescending(q => q.DistrictId);
                            break;
                        case StoreOrder.Ward:
                            query = query.OrderByDescending(q => q.WardId);
                            break;
                        case StoreOrder.Address:
                            query = query.OrderByDescending(q => q.Address);
                            break;
                        case StoreOrder.DeliveryAddress:
                            query = query.OrderByDescending(q => q.DeliveryAddress);
                            break;
                        case StoreOrder.Latitude:
                            query = query.OrderByDescending(q => q.Latitude);
                            break;
                        case StoreOrder.Longitude:
                            query = query.OrderByDescending(q => q.Longitude);
                            break;
                        case StoreOrder.DeliveryLatitude:
                            query = query.OrderByDescending(q => q.DeliveryLatitude);
                            break;
                        case StoreOrder.DeliveryLongitude:
                            query = query.OrderByDescending(q => q.DeliveryLongitude);
                            break;
                        case StoreOrder.OwnerName:
                            query = query.OrderByDescending(q => q.OwnerName);
                            break;
                        case StoreOrder.OwnerPhone:
                            query = query.OrderByDescending(q => q.OwnerPhone);
                            break;
                        case StoreOrder.OwnerEmail:
                            query = query.OrderByDescending(q => q.OwnerEmail);
                            break;
                        case StoreOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                        case StoreOrder.AppUser:
                            query = query.OrderByDescending(q => q.AppUserId);
                            break;
                        case StoreOrder.StoreStatus:
                            query = query.OrderByDescending(q => q.StoreStatusId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<Store>> DynamicSelect(IQueryable<StoreDAO> query, StoreFilter filter)
        {
            List<long> Ids = await query.Select(q => q.Id).ToListWithNoLockAsync();
            IdFilter IdFilter = new IdFilter { In = Ids };
            query = DataContext.Store.AsNoTracking()
                .Where(x => x.Id, IdFilter);
            filter.Skip = 0;
            query = DynamicOrder(query, filter);
            List<Store> Stores = await query.Select(q => new Store()
            {
                Id = filter.Selects.Contains(StoreSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(StoreSelect.Code) ? q.Code : default(string),
                CodeDraft = filter.Selects.Contains(StoreSelect.CodeDraft) ? q.CodeDraft : default(string),
                Name = filter.Selects.Contains(StoreSelect.Name) ? q.Name : default(string),
                UnsignName = filter.Selects.Contains(StoreSelect.UnsignName) ? q.UnsignName : default(string),
                ParentStoreId = filter.Selects.Contains(StoreSelect.ParentStore) ? q.ParentStoreId : default(long?),
                OrganizationId = filter.Selects.Contains(StoreSelect.Organization) ? q.OrganizationId : default(long),
                StoreTypeId = filter.Selects.Contains(StoreSelect.StoreType) ? q.StoreTypeId : default(long),
                Telephone = filter.Selects.Contains(StoreSelect.Telephone) ? q.Telephone : default(string),
                ProvinceId = filter.Selects.Contains(StoreSelect.Province) ? q.ProvinceId : null,
                DistrictId = filter.Selects.Contains(StoreSelect.District) ? q.DistrictId : null,
                WardId = filter.Selects.Contains(StoreSelect.Ward) ? q.WardId : null,
                Address = filter.Selects.Contains(StoreSelect.Address) ? q.Address : default(string),
                UnsignAddress = filter.Selects.Contains(StoreSelect.UnsignAddress) ? q.UnsignAddress : default(string),
                DeliveryAddress = filter.Selects.Contains(StoreSelect.DeliveryAddress) ? q.DeliveryAddress : default(string),
                Latitude = filter.Selects.Contains(StoreSelect.Latitude) ? q.Latitude : default(decimal),
                Longitude = filter.Selects.Contains(StoreSelect.Longitude) ? q.Longitude : default(decimal),
                DeliveryLatitude = filter.Selects.Contains(StoreSelect.DeliveryLatitude) ? q.DeliveryLatitude : null,
                DeliveryLongitude = filter.Selects.Contains(StoreSelect.DeliveryLongitude) ? q.DeliveryLongitude : null,
                OwnerName = filter.Selects.Contains(StoreSelect.OwnerName) ? q.OwnerName : default(string),
                OwnerPhone = filter.Selects.Contains(StoreSelect.OwnerPhone) ? q.OwnerPhone : default(string),
                OwnerEmail = filter.Selects.Contains(StoreSelect.OwnerEmail) ? q.OwnerEmail : default(string),
                TaxCode = filter.Selects.Contains(StoreSelect.TaxCode) ? q.TaxCode : default(string),
                LegalEntity = filter.Selects.Contains(StoreSelect.LegalEntity) ? q.LegalEntity : default(string),
                StatusId = filter.Selects.Contains(StoreSelect.Status) ? q.StatusId : default(long),
                StoreScoutingId = filter.Selects.Contains(StoreSelect.StoreScouting) ? q.StoreScoutingId : null,
                AppUserId = filter.Selects.Contains(StoreSelect.AppUser) ? q.AppUserId : null,
                CreatorId = filter.Selects.Contains(StoreSelect.Creator) ? q.CreatorId : default(long),
                StoreStatusId = filter.Selects.Contains(StoreSelect.StoreStatus) ? q.StoreStatusId : default(long),
                Used = filter.Selects.Contains(StoreSelect.Used) ? q.Used : default(bool),
                CreatedAt = filter.Selects.Contains(StoreSelect.CreatedAt) ? q.CreatedAt : default(DateTime),
                UpdatedAt = filter.Selects.Contains(StoreSelect.UpdatedAt) ? q.UpdatedAt : default(DateTime),
                RowId = filter.Selects.Contains(StoreSelect.RowId) ? q.RowId : default(Guid),
                Description = filter.Selects.Contains(StoreSelect.Description) ? q.Description : default(string),
                DebtLimited = filter.Selects.Contains(StoreSelect.DebtLimited) ? q.DebtLimited : null,
                EstimatedRevenueId = filter.Selects.Contains(StoreSelect.EstimatedRevenue) ? q.EstimatedRevenueId : null,
                EstimatedRevenue = filter.Selects.Contains(StoreSelect.EstimatedRevenue) && q.EstimatedRevenue != null ? new EstimatedRevenue
                {
                    Id = q.EstimatedRevenue.Id,
                    Code = q.EstimatedRevenue.Code,
                    Name = q.EstimatedRevenue.Name,
                } : null,
                District = filter.Selects.Contains(StoreSelect.District) && q.District != null ? new District
                {
                    Id = q.District.Id,
                    Code = q.District.Code,
                    Name = q.District.Name,
                } : null,
                Organization = filter.Selects.Contains(StoreSelect.Organization) && q.Organization != null ? new Organization
                {
                    Id = q.Organization.Id,
                    Code = q.Organization.Code,
                    Name = q.Organization.Name,
                    ParentId = q.Organization.ParentId,
                    Path = q.Organization.Path,
                } : null,
                ParentStore = filter.Selects.Contains(StoreSelect.ParentStore) && q.ParentStore != null ? new Store
                {
                    Id = q.ParentStore.Id,
                    Code = q.ParentStore.Code,
                    Name = q.ParentStore.Name,
                } : null,
                Province = filter.Selects.Contains(StoreSelect.Province) && q.Province != null ? new Province
                {
                    Id = q.Province.Id,
                    Code = q.Province.Code,
                    Name = q.Province.Name,
                } : null,
                AppUser = filter.Selects.Contains(StoreSelect.AppUser) && q.AppUser != null ? new AppUser
                {
                    Id = q.AppUser.Id,
                    Username = q.AppUser.Username,
                    DisplayName = q.AppUser.DisplayName,
                } : null,
                Creator = filter.Selects.Contains(StoreSelect.Creator) && q.Creator != null ? new AppUser
                {
                    Id = q.Creator.Id,
                    Username = q.Creator.Username,
                    DisplayName = q.Creator.DisplayName,
                } : null,
                Status = filter.Selects.Contains(StoreSelect.Status) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                } : null,
                StoreType = filter.Selects.Contains(StoreSelect.StoreType) && q.StoreType != null ? new StoreType
                {
                    Id = q.StoreType.Id,
                    Code = q.StoreType.Code,
                    Name = q.StoreType.Name,
                    Color = q.StoreType.Color == null ? null : new Color
                    {
                        Id = q.StoreType.Color.Id,
                        Code = q.StoreType.Color.Code,
                        Name = q.StoreType.Color.Name,
                    },
                } : null,
                Ward = filter.Selects.Contains(StoreSelect.Ward) && q.Ward != null ? new Ward
                {
                    Id = q.Ward.Id,
                    Code = q.Ward.Code,
                    Name = q.Ward.Name,
                } : null,
                StoreStatus = filter.Selects.Contains(StoreSelect.StoreStatus) && q.StoreStatus != null ? new StoreStatus
                {
                    Id = q.StoreStatus.Id,
                    Code = q.StoreStatus.Code,
                    Name = q.StoreStatus.Name,
                } : null,

            }).ToListWithNoLockAsync();
            var s = query.ToSql();
            if (filter.Selects.Contains(StoreSelect.StoreUser))
            {
                IdFilter StoreIdFilter = new IdFilter() { In = Stores.Select(s => s.Id).ToList() };
                var StoreUsers = await DataContext.StoreUser.AsNoTracking()
                    .Where(q => q.StoreId, StoreIdFilter)
                    .Select(x => new StoreUser
                    {
                        Id = x.Id,
                        StoreId = x.StoreId,
                        Username = x.Username,
                        DisplayName = x.DisplayName,
                        StatusId = x.StatusId,
                        Status = x.Status == null ? null : new Status
                        {
                            Id = x.Status.Id,
                            Code = x.Status.Code,
                            Name = x.Status.Name,
                        }
                    }).ToListWithNoLockAsync();
                foreach (var Store in Stores)
                {
                    Store.StoreUser = StoreUsers.Where(x => x.StoreId == Store.Id).Skip(0).Take(1).FirstOrDefault();
                    if (Store.StoreUser != null)
                    {
                        Store.StoreUserId = Store.StoreUser.Id;
                    }

                }
            }

            if (filter.Selects.Contains(StoreSelect.StoreGrouping))
            {
                IdFilter StoreIdFilter = new IdFilter() { In = Stores.Select(s => s.Id).ToList() };
                var StoreStoreGroupingMappings = await DataContext.StoreStoreGroupingMapping.AsNoTracking()
                .Where(x => x.StoreId, StoreIdFilter)
                .Select(x => new StoreStoreGroupingMapping
                {
                    StoreId = x.StoreId,
                    StoreGroupingId = x.StoreGroupingId,
                    StoreGrouping = new StoreGrouping
                    {
                        Id = x.StoreGrouping.Id,
                        Code = x.StoreGrouping.Code,
                        Name = x.StoreGrouping.Name,
                    },
                }).ToListWithNoLockAsync();
                foreach (var Store in Stores)
                {
                    Store.StoreStoreGroupingMappings = StoreStoreGroupingMappings.Where(x => x.StoreId == Store.Id).ToList();
                }
            }

            if (filter.Selects.Contains(StoreSelect.BrandInStores))
            {
                IdFilter StoreIdFilter = new IdFilter() { In = Stores.Select(s => s.Id).ToList() };
                var BrandInStores = await DataContext.BrandInStore
                    .Where(x => x.DeletedAt == null)
                    .Where(x => x.StoreId, StoreIdFilter)
                    .Select(x => new BrandInStore
                    {
                        Id = x.Id,
                        BrandId = x.BrandId,
                        CreatorId = x.CreatorId,
                        StoreId = x.StoreId,
                        Top = x.Top,
                        Brand = x.Brand == null ? null : new Brand
                        {
                            Id = x.Brand.Id,
                            Code = x.Brand.Code,
                            Name = x.Brand.Name,
                        },
                    }).ToListWithNoLockAsync();
                var BrandInStoreIds = BrandInStores.Select(x => x.Id).ToList();
                IdFilter BrandInStoreIdFilter = new IdFilter { In = BrandInStoreIds };
                var BrandInStoreProductGroupingMappings = await DataContext.BrandInStoreProductGroupingMapping
                    .Where(x => x.ProductGrouping.DeletedAt == null)
                    .Where(x => x.BrandInStoreId, BrandInStoreIdFilter)
                    .Select(x => new BrandInStoreProductGroupingMapping
                    {
                        BrandInStoreId = x.BrandInStoreId,
                        ProductGroupingId = x.ProductGroupingId,
                        ProductGrouping = x.ProductGrouping == null ? null : new ProductGrouping
                        {
                            Id = x.ProductGrouping.Id,
                            Code = x.ProductGrouping.Code,
                            Name = x.ProductGrouping.Name,
                        }
                    }).ToListWithNoLockAsync();
                var BrandInStoreShowingCategoryMappings = await DataContext.BrandInStoreShowingCategoryMapping
                    .Where(x => x.ShowingCategory.DeletedAt == null)
                    .Where(x => x.BrandInStoreId, BrandInStoreIdFilter)
                    .Select(x => new BrandInStoreShowingCategoryMapping
                    {
                        BrandInStoreId = x.BrandInStoreId,
                        ShowingCategoryId = x.ShowingCategoryId,
                        ShowingCategory = x.ShowingCategory == null ? null : new ShowingCategory
                        {
                            Id = x.ShowingCategory.Id,
                            Code = x.ShowingCategory.Code,
                            Name = x.ShowingCategory.Name,
                        }
                    }).ToListWithNoLockAsync();

                foreach (var BrandInStore in BrandInStores)
                {
                    BrandInStore.BrandInStoreProductGroupingMappings = BrandInStoreProductGroupingMappings.Where(x => x.BrandInStoreId == BrandInStore.Id).ToList();
                    BrandInStore.BrandInStoreShowingCategoryMappings = BrandInStoreShowingCategoryMappings.Where(x => x.BrandInStoreId == BrandInStore.Id).ToList();
                }
                foreach (var Store in Stores)
                {
                    Store.BrandInStores = BrandInStores.Where(x => x.StoreId == Store.Id).ToList();
                }
            }
            return Stores;
        }

        public async Task<int> Count(StoreFilter filter)
        {
            if (filter == null) return 0;
            IQueryable<StoreDAO> StoreDAOs = DataContext.Store;
            StoreDAOs = await DynamicFilter(StoreDAOs, filter);
            StoreDAOs = await OrFilter(StoreDAOs, filter);
            var count = await StoreDAOs.CountWithNoLockAsync();
            return count;
        }

        public async Task<int> CountAll(StoreFilter filter)
        {
            IQueryable<StoreDAO> Stores = DataContext.Store;
            Stores = await DynamicFilter(Stores, filter);
            int count = await Stores.CountWithNoLockAsync();
            return count;
        }

        public async Task<List<Store>> List(StoreFilter filter)
        {
            if (filter == null) return new List<Store>();
            List<Store> Stores = new List<Store>();
            IQueryable<StoreDAO> StoreDAOs = DataContext.Store.AsNoTracking();
            StoreDAOs = await DynamicFilter(StoreDAOs, filter);
            StoreDAOs = await OrFilter(StoreDAOs, filter);
            StoreDAOs = DynamicOrder(StoreDAOs, filter);
            Stores = await DynamicSelect(StoreDAOs, filter);
            return Stores;
        }
        public async Task<List<long>> ListToIds(StoreFilter filter)
        {
            if (filter == null) return new List<long>();
            List<long> StoreIds = new List<long>();
            IQueryable<StoreDAO> StoreDAOs = DataContext.Store.AsNoTracking();
            StoreDAOs = await DynamicFilter(StoreDAOs, filter);
            StoreDAOs = await OrFilter(StoreDAOs, filter);
            StoreIds = await StoreDAOs.Select(x => x.Id).Distinct().ToListWithNoLockAsync();
            return StoreIds;
        }
        public async Task<List<Store>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };
            var query = DataContext.Store.AsNoTracking()
                .Where(x => x.Id, IdFilter);
            List<Store> Stores = await query
                .Select(x => new Store
                {
                    Id = x.Id,
                    Code = x.Code,
                    CodeDraft = x.CodeDraft,
                    Name = x.Name,
                    UnsignName = x.UnsignName,
                    ParentStoreId = x.ParentStoreId,
                    OrganizationId = x.OrganizationId,
                    StoreTypeId = x.StoreTypeId,
                    Telephone = x.Telephone,
                    ProvinceId = x.ProvinceId,
                    DistrictId = x.DistrictId,
                    WardId = x.WardId,
                    Address = x.Address,
                    UnsignAddress = x.UnsignAddress,
                    DeliveryAddress = x.DeliveryAddress,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    DeliveryLatitude = x.DeliveryLatitude,
                    DeliveryLongitude = x.DeliveryLongitude,
                    OwnerName = x.OwnerName,
                    OwnerPhone = x.OwnerPhone,
                    OwnerEmail = x.OwnerEmail,
                    TaxCode = x.TaxCode,
                    LegalEntity = x.LegalEntity,
                    StatusId = x.StatusId,
                    RowId = x.RowId,
                    Used = x.Used,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    DeletedAt = x.DeletedAt,
                    AppUserId = x.AppUserId,
                    CreatorId = x.CreatorId,
                    StoreStatusId = x.StoreStatusId,
                    Description = x.Description,
                    DebtLimited = x.DebtLimited,
                    District = x.District == null ? null : new District
                    {
                        Id = x.District.Id,
                        Code = x.District.Code,
                        Name = x.District.Name,
                        Priority = x.District.Priority,
                        ProvinceId = x.District.ProvinceId,
                        StatusId = x.District.StatusId,
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
                        Address = x.Organization.Address,
                        Email = x.Organization.Email,
                        RowId = x.StoreType.RowId
                    },
                    ParentStore = x.ParentStore == null ? null : new Store
                    {
                        Id = x.ParentStore.Id,
                        Code = x.ParentStore.Code,
                        Name = x.ParentStore.Name,
                        ParentStoreId = x.ParentStore.ParentStoreId,
                        OrganizationId = x.ParentStore.OrganizationId,
                        StoreTypeId = x.ParentStore.StoreTypeId,
                        Telephone = x.ParentStore.Telephone,
                        ProvinceId = x.ParentStore.ProvinceId,
                        DistrictId = x.ParentStore.DistrictId,
                        WardId = x.ParentStore.WardId,
                        Address = x.ParentStore.Address,
                        DeliveryAddress = x.ParentStore.DeliveryAddress,
                        Latitude = x.ParentStore.Latitude,
                        Longitude = x.ParentStore.Longitude,
                        OwnerName = x.ParentStore.OwnerName,
                        OwnerPhone = x.ParentStore.OwnerPhone,
                        OwnerEmail = x.ParentStore.OwnerEmail,
                        TaxCode = x.ParentStore.TaxCode,
                        LegalEntity = x.ParentStore.LegalEntity,
                        StatusId = x.ParentStore.StatusId,
                        RowId = x.StoreType.RowId
                    },
                    Province = x.Province == null ? null : new Province
                    {
                        Id = x.Province.Id,
                        Code = x.Province.Code,
                        Name = x.Province.Name,
                        Priority = x.Province.Priority,
                        StatusId = x.Province.StatusId,
                    },
                    AppUser = x.AppUser == null ? null : new AppUser
                    {
                        Id = x.AppUser.Id,
                        Username = x.AppUser.Username,
                        DisplayName = x.AppUser.DisplayName,
                        Address = x.AppUser.Address,
                        Email = x.AppUser.Email,
                        Phone = x.AppUser.Phone,
                        PositionId = x.AppUser.PositionId,
                        Department = x.AppUser.Department,
                        OrganizationId = x.AppUser.OrganizationId,
                        StatusId = x.AppUser.StatusId,
                        Avatar = x.AppUser.Avatar,
                        ProvinceId = x.AppUser.ProvinceId,
                        SexId = x.AppUser.SexId,
                        Birthday = x.AppUser.Birthday,
                        RowId = x.StoreType.RowId
                    },
                    Creator = x.Creator == null ? null : new AppUser
                    {
                        Id = x.Creator.Id,
                        Username = x.Creator.Username,
                        DisplayName = x.Creator.DisplayName,
                        Address = x.Creator.Address,
                        Email = x.Creator.Email,
                        Phone = x.Creator.Phone,
                        PositionId = x.Creator.PositionId,
                        Department = x.Creator.Department,
                        OrganizationId = x.Creator.OrganizationId,
                        StatusId = x.Creator.StatusId,
                        Avatar = x.Creator.Avatar,
                        ProvinceId = x.Creator.ProvinceId,
                        SexId = x.Creator.SexId,
                        Birthday = x.Creator.Birthday,
                        RowId = x.StoreType.RowId
                    },
                    Status = x.Status == null ? null : new Status
                    {
                        Id = x.Status.Id,
                        Code = x.Status.Code,
                        Name = x.Status.Name,
                    },
                    StoreType = x.StoreType == null ? null : new StoreType
                    {
                        Id = x.StoreType.Id,
                        Code = x.StoreType.Code,
                        Name = x.StoreType.Name,
                        ColorId = x.StoreType.ColorId,
                        StatusId = x.StoreType.StatusId,
                        RowId = x.StoreType.RowId,
                        Color = x.StoreType.Color == null ? null : new Color
                        {
                            Id = x.StoreType.Color.Id,
                            Code = x.StoreType.Color.Code,
                            Name = x.StoreType.Color.Name,
                        }
                    },
                    Ward = x.Ward == null ? null : new Ward
                    {
                        Id = x.Ward.Id,
                        Code = x.Ward.Code,
                        Name = x.Ward.Name,
                        Priority = x.Ward.Priority,
                        DistrictId = x.Ward.DistrictId,
                        StatusId = x.Ward.StatusId,
                    },
                    StoreStatus = x.StoreStatus == null ? null : new StoreStatus
                    {
                        Id = x.StoreStatus.Id,
                        Code = x.StoreStatus.Code,
                        Name = x.StoreStatus.Name,
                    },
                }).ToListWithNoLockAsync();

            var StoreImageMappings = await DataContext.StoreImageMapping.AsNoTracking()
                .WhereBulkContains(Ids, x => x.StoreId)
                .Select(x => new StoreImageMapping
                {
                    ImageId = x.ImageId,
                    StoreId = x.StoreId,
                    Image = x.Image == null ? null : new Image
                    {
                        Id = x.Image.Id,
                        Url = x.Image.Url,
                        ThumbnailUrl = x.Image.ThumbnailUrl,
                        Name = x.Image.Name,
                        RowId = x.Image.RowId,
                        CreatedAt = x.Image.CreatedAt,
                        UpdatedAt = x.Image.UpdatedAt,
                        DeletedAt = x.Image.DeletedAt,
                    }
                }).ToListWithNoLockAsync();

            var BrandInStores = await DataContext.BrandInStore
                .Where(x => x.DeletedAt == null)
                .WhereBulkContains(Ids, x => x.StoreId)
                .Select(x => new BrandInStore
                {
                    Id = x.Id,
                    BrandId = x.BrandId,
                    CreatorId = x.CreatorId,
                    StoreId = x.StoreId,
                    Top = x.Top,
                    Brand = x.Brand == null ? null : new Brand
                    {
                        Id = x.Brand.Id,
                        Code = x.Brand.Code,
                        Name = x.Brand.Name,
                    },
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    DeletedAt = x.DeletedAt,
                }).ToListWithNoLockAsync();

            var BrandInStoreIds = BrandInStores.Select(x => x.Id).ToList();
            var BrandInStoreProductGroupingMappings = await DataContext.BrandInStoreProductGroupingMapping
                .Where(x => x.ProductGrouping.DeletedAt == null)
                .WhereBulkContains(BrandInStoreIds, x => x.BrandInStoreId)
                .Select(x => new BrandInStoreProductGroupingMapping
                {
                    BrandInStoreId = x.BrandInStoreId,
                    ProductGroupingId = x.ProductGroupingId,
                    ProductGrouping = x.ProductGrouping == null ? null : new ProductGrouping
                    {
                        Id = x.ProductGrouping.Id,
                        Code = x.ProductGrouping.Code,
                        Name = x.ProductGrouping.Name,
                    }
                }).ToListWithNoLockAsync();
            var BrandInStoreShowingCategoryMappings = await DataContext.BrandInStoreShowingCategoryMapping
                .Where(x => x.ShowingCategory.DeletedAt == null)
                .WhereBulkContains(BrandInStoreIds, x => x.BrandInStoreId)
                .Select(x => new BrandInStoreShowingCategoryMapping
                {
                    BrandInStoreId = x.BrandInStoreId,
                    ShowingCategoryId = x.ShowingCategoryId,
                    ShowingCategory = x.ShowingCategory == null ? null : new ShowingCategory
                    {
                        Id = x.ShowingCategory.Id,
                        Code = x.ShowingCategory.Code,
                        Name = x.ShowingCategory.Name,
                    }
                }).ToListWithNoLockAsync();
            foreach (var BrandInStore in BrandInStores)
            {
                BrandInStore.BrandInStoreProductGroupingMappings = BrandInStoreProductGroupingMappings.Where(x => x.BrandInStoreId == BrandInStore.Id).ToList();
                BrandInStore.BrandInStoreShowingCategoryMappings = BrandInStoreShowingCategoryMappings.Where(x => x.BrandInStoreId == BrandInStore.Id).ToList();
            }

            List<StoreStoreGroupingMapping> StoreStoreGroupingMappings = await DataContext.StoreStoreGroupingMapping.AsNoTracking()
                .WhereBulkContains(Ids, x => x.StoreId)
                .Select(x => new StoreStoreGroupingMapping
                {
                    StoreId = x.StoreId,
                    StoreGroupingId = x.StoreGroupingId,
                    StoreGrouping = new StoreGrouping
                    {
                        Id = x.StoreGrouping.Id,
                        Code = x.StoreGrouping.Code,
                        Name = x.StoreGrouping.Name,
                        ParentId = x.StoreGrouping.ParentId,
                        Path = x.StoreGrouping.Path,
                        Level = x.StoreGrouping.Level,
                        RowId = x.StoreGrouping.RowId,
                        CreatedAt = x.StoreGrouping.CreatedAt,
                        UpdatedAt = x.StoreGrouping.UpdatedAt,
                        DeletedAt = x.StoreGrouping.DeletedAt,
                    },
                }).ToListWithNoLockAsync();

            List<AppUserStoreMapping> AppUserStoreMappings = await DataContext.AppUserStoreMapping.AsNoTracking()
                .WhereBulkContains(Ids, x => x.StoreId)
                .Select(x => new AppUserStoreMapping
                {
                    StoreId = x.StoreId,
                    AppUserId = x.AppUserId,
                    AppUser = new AppUser
                    {
                        Id = x.AppUser.Id,
                        DisplayName = x.AppUser.DisplayName,
                        Username = x.AppUser.Username,
                        OrganizationId = x.AppUser.OrganizationId,
                    },
                }).ToListWithNoLockAsync();
            foreach (Store Store in Stores)
            {
                Store.StoreImageMappings = StoreImageMappings.Where(x => x.StoreId == Store.Id).ToList();
                Store.BrandInStores = BrandInStores.Where(x => x.StoreId == Store.Id).ToList();
                Store.StoreStoreGroupingMappings = StoreStoreGroupingMappings.Where(x => x.StoreId == Store.Id).ToList();
                Store.AppUserStoreMappings = AppUserStoreMappings.Where(x => x.StoreId == Store.Id).ToList();
            }

            return Stores;
        }
        public async Task<Store> Get(long Id)
        {
            Store Store = await DataContext.Store.AsNoTracking()
                .Where(x => x.Id == Id).Select(x => new Store()
                {
                    Id = x.Id,
                    Code = x.Code,
                    CodeDraft = x.CodeDraft,
                    Name = x.Name,
                    UnsignName = x.UnsignName,
                    ParentStoreId = x.ParentStoreId,
                    OrganizationId = x.OrganizationId,
                    StoreTypeId = x.StoreTypeId,
                    Telephone = x.Telephone,
                    ProvinceId = x.ProvinceId,
                    DistrictId = x.DistrictId,
                    WardId = x.WardId,
                    Address = x.Address,
                    UnsignAddress = x.UnsignAddress,
                    DeliveryAddress = x.DeliveryAddress,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    DeliveryLatitude = x.DeliveryLatitude,
                    DeliveryLongitude = x.DeliveryLongitude,
                    OwnerName = x.OwnerName,
                    OwnerPhone = x.OwnerPhone,
                    OwnerEmail = x.OwnerEmail,
                    TaxCode = x.TaxCode,
                    LegalEntity = x.LegalEntity,
                    StatusId = x.StatusId,
                    RowId = x.RowId,
                    Used = x.Used,
                    StoreScoutingId = x.StoreScoutingId,
                    AppUserId = x.AppUserId,
                    CreatorId = x.CreatorId,
                    StoreStatusId = x.StoreStatusId,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    DeletedAt = x.DeletedAt,
                    Description = x.Description,
                    DebtLimited = x.DebtLimited,
                    EstimatedRevenueId = x.EstimatedRevenueId,
                    IsStoreApprovalDirectSalesOrder = x.IsStoreApprovalDirectSalesOrder,
                    EstimatedRevenue = x.EstimatedRevenue == null ? null : new EstimatedRevenue
                    {
                        Id = x.EstimatedRevenue.Id,
                        Code = x.EstimatedRevenue.Code,
                        Name = x.EstimatedRevenue.Name,
                    },
                    StoreScouting = x.StoreScouting == null ? null : new StoreScouting
                    {
                        Id = x.StoreScouting.Id,
                        Code = x.StoreScouting.Code,
                        Name = x.StoreScouting.Name,
                    },
                    District = x.District == null ? null : new District
                    {
                        Id = x.District.Id,
                        Name = x.District.Name,
                    },
                    Organization = x.Organization == null ? null : new Organization
                    {
                        Id = x.Organization.Id,
                        Code = x.Organization.Code,
                        Name = x.Organization.Name,
                        ParentId = x.Organization.ParentId,
                        Path = x.Organization.Path,
                    },
                    ParentStore = x.ParentStore == null ? null : new Store
                    {
                        Id = x.ParentStore.Id,
                        Code = x.ParentStore.Code,
                        Name = x.ParentStore.Name,
                    },
                    Province = x.Province == null ? null : new Province
                    {
                        Id = x.Province.Id,
                        Name = x.Province.Name,
                        Priority = x.Province.Priority,
                        StatusId = x.Province.StatusId,
                    },
                    AppUser = x.AppUser == null ? null : new AppUser
                    {
                        Id = x.AppUser.Id,
                        Username = x.AppUser.Username,
                        DisplayName = x.AppUser.DisplayName,
                    },
                    Creator = x.Creator == null ? null : new AppUser
                    {
                        Id = x.Creator.Id,
                        Username = x.Creator.Username,
                        DisplayName = x.Creator.DisplayName,
                    },
                    Status = x.Status == null ? null : new Status
                    {
                        Id = x.Status.Id,
                        Code = x.Status.Code,
                        Name = x.Status.Name,
                    },
                    StoreType = x.StoreType == null ? null : new StoreType
                    {
                        Id = x.StoreType.Id,
                        Code = x.StoreType.Code,
                        Name = x.StoreType.Name,
                        ColorId = x.StoreType.ColorId,
                        StatusId = x.StoreType.StatusId,
                        Color = x.StoreType.Color == null ? null : new Color
                        {
                            Id = x.StoreType.Color.Id,
                            Code = x.StoreType.Color.Code,
                            Name = x.StoreType.Color.Name,
                        }
                    },
                    Ward = x.Ward == null ? null : new Ward
                    {
                        Id = x.Ward.Id,
                        Name = x.Ward.Name,
                        Priority = x.Ward.Priority,
                        DistrictId = x.Ward.DistrictId,
                        StatusId = x.Ward.StatusId,
                    },
                    StoreStatus = x.StoreStatus == null ? null : new StoreStatus
                    {
                        Id = x.StoreStatus.Id,
                        Code = x.StoreStatus.Code,
                        Name = x.StoreStatus.Name,
                    },
                }).FirstOrDefaultWithNoLockAsync();

            if (Store == null)
                return null;

            Store.StoreImageMappings = await DataContext.StoreImageMapping
                .Where(x => x.StoreId == Id)
                .Select(x => new StoreImageMapping
                {
                    StoreId = x.StoreId,
                    ImageId = x.ImageId,
                    Image = new Image
                    {
                        Id = x.Image.Id,
                        Name = x.Image.Name,
                        Url = x.Image.Url,
                        ThumbnailUrl = x.Image.ThumbnailUrl,
                    }
                }).ToListWithNoLockAsync();

            Store.AlbumImageMappings = await DataContext.AlbumImageMapping
                .Where(x => x.StoreId == Id)
                .Select(x => new AlbumImageMapping
                {
                    StoreId = x.StoreId,
                    ImageId = x.ImageId,
                    ShootingAt = x.ShootingAt,
                    SaleEmployeeId = x.SaleEmployeeId,
                    Distance = x.Distance,
                    AlbumId = x.AlbumId,
                    Image = new Image
                    {
                        Id = x.Image.Id,
                        Name = x.Image.Name,
                        Url = x.Image.Url,
                        ThumbnailUrl = x.Image.ThumbnailUrl,
                    }
                }).ToListWithNoLockAsync();

            Store.BrandInStores = await DataContext.BrandInStore
                .Where(x => x.StoreId == Id && x.DeletedAt == null)
                .Select(x => new BrandInStore
                {
                    Id = x.Id,
                    StoreId = x.StoreId,
                    BrandId = x.BrandId,
                    Top = x.Top,
                    CreatorId = x.CreatorId,
                    Brand = x.Brand == null ? null : new Brand
                    {
                        Id = x.Brand.Id,
                        Code = x.Brand.Code,
                        Name = x.Brand.Name,
                        RowId = x.RowId
                    },
                    Creator = x.Creator == null ? null : new AppUser
                    {
                        Id = x.Creator.Id,
                        Username = x.Creator.Username,
                        DisplayName = x.Creator.DisplayName,
                    },
                    DeletedAt = x.DeletedAt,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                }).OrderBy(x => x.Top).ToListWithNoLockAsync();

            var BrandInStoreIds = Store.BrandInStores.Select(x => x.Id).ToList();

            if (BrandInStoreIds.Count > 0)
            {
                var BrandInStoreProductGroupingMappings = await DataContext.BrandInStoreProductGroupingMapping
                    .Where(x => x.ProductGrouping.DeletedAt == null)
                    .Where(x => x.BrandInStoreId, new IdFilter { In = BrandInStoreIds })
                    .Select(x => new BrandInStoreProductGroupingMapping
                    {
                        BrandInStoreId = x.BrandInStoreId,
                        ProductGroupingId = x.ProductGroupingId,
                        ProductGrouping = x.ProductGrouping == null ? null : new ProductGrouping
                        {
                            Id = x.ProductGrouping.Id,
                            Code = x.ProductGrouping.Code,
                            Name = x.ProductGrouping.Name,
                            RowId = x.ProductGrouping.RowId
                        },
                    }).ToListWithNoLockAsync();
                var BrandInStoreShowingCategoryMappings = await DataContext.BrandInStoreShowingCategoryMapping
                    .Where(x => x.ShowingCategory.DeletedAt == null)
                    .Where(x => x.BrandInStoreId, new IdFilter { In = BrandInStoreIds })
                    .Select(x => new BrandInStoreShowingCategoryMapping
                    {
                        BrandInStoreId = x.BrandInStoreId,
                        ShowingCategoryId = x.ShowingCategoryId,
                        ShowingCategory = x.ShowingCategory == null ? null : new ShowingCategory
                        {
                            Id = x.ShowingCategory.Id,
                            Code = x.ShowingCategory.Code,
                            Name = x.ShowingCategory.Name,
                            RowId = x.ShowingCategory.RowId
                        },
                    }).ToListWithNoLockAsync();
                foreach (var BrandInStore in Store.BrandInStores)
                {
                    BrandInStore.BrandInStoreProductGroupingMappings = BrandInStoreProductGroupingMappings
                        .Where(x => x.BrandInStoreId == BrandInStore.Id)
                        .ToList();
                    BrandInStore.BrandInStoreShowingCategoryMappings = BrandInStoreShowingCategoryMappings
                        .Where(x => x.BrandInStoreId == BrandInStore.Id)
                        .ToList();
                }

            }
            Store.StoreStoreGroupingMappings = await DataContext.StoreStoreGroupingMapping.AsNoTracking()
                .Where(x => x.StoreId == Id)
                .Select(x => new StoreStoreGroupingMapping
                {
                    StoreId = x.StoreId,
                    StoreGroupingId = x.StoreGroupingId,
                    StoreGrouping = new StoreGrouping
                    {
                        Id = x.StoreGrouping.Id,
                        Code = x.StoreGrouping.Code,
                        Name = x.StoreGrouping.Name,
                    },
                }).ToListWithNoLockAsync();
            Store.AppUserStoreMappings = await DataContext.AppUserStoreMapping.AsNoTracking()
                .Where(x => x.StoreId == Id)
                .Select(x => new AppUserStoreMapping
                {
                    StoreId = x.StoreId,
                    AppUserId = x.AppUserId,
                    AppUser = new AppUser
                    {
                        Id = x.AppUser.Id,
                        Username = x.AppUser.Username,
                        DisplayName = x.AppUser.DisplayName,
                    },
                }).ToListWithNoLockAsync();

            return Store;
        }
        public async Task<bool> Create(Store Store)
        {
            StoreDAO StoreDAO = new StoreDAO();
            StoreDAO.Id = Store.Id;
            StoreDAO.Code = Guid.NewGuid().ToString();
            StoreDAO.CodeDraft = Store.CodeDraft;
            StoreDAO.Name = Store.Name;
            StoreDAO.UnsignName = Store.UnsignName;
            StoreDAO.ParentStoreId = Store.ParentStoreId;
            StoreDAO.OrganizationId = Store.OrganizationId;
            StoreDAO.StoreTypeId = Store.StoreTypeId;
            StoreDAO.Telephone = Store.Telephone;
            StoreDAO.ProvinceId = Store.ProvinceId;
            StoreDAO.DistrictId = Store.DistrictId;
            StoreDAO.WardId = Store.WardId;
            StoreDAO.Address = Store.Address;
            StoreDAO.UnsignAddress = Store.UnsignAddress;
            StoreDAO.DeliveryAddress = Store.DeliveryAddress;
            StoreDAO.Latitude = Store.Latitude;
            StoreDAO.Longitude = Store.Longitude;
            StoreDAO.DeliveryLatitude = Store.DeliveryLatitude;
            StoreDAO.DeliveryLongitude = Store.DeliveryLongitude;
            StoreDAO.OwnerName = Store.OwnerName;
            StoreDAO.OwnerPhone = Store.OwnerPhone;
            StoreDAO.OwnerEmail = Store.OwnerEmail;
            StoreDAO.TaxCode = Store.TaxCode;
            StoreDAO.LegalEntity = Store.LegalEntity;
            StoreDAO.StatusId = Store.StatusId;
            StoreDAO.AppUserId = Store.AppUserId;
            StoreDAO.CreatorId = Store.CreatorId;
            StoreDAO.StoreStatusId = Store.StoreStatusId;
            StoreDAO.StoreScoutingId = Store.StoreScoutingId;
            StoreDAO.Description = Store.Description;
            StoreDAO.DebtLimited = Store.DebtLimited;
            StoreDAO.EstimatedRevenueId = Store.EstimatedRevenueId;
            StoreDAO.RowId = Guid.NewGuid();
            StoreDAO.CreatedAt = StaticParams.DateTimeNow;
            StoreDAO.UpdatedAt = StaticParams.DateTimeNow;
            StoreDAO.Used = false;
            StoreDAO.IsStoreApprovalDirectSalesOrder = Store.IsStoreApprovalDirectSalesOrder;
            DataContext.Store.Add(StoreDAO);
            await DataContext.SaveChangesAsync();
            Store.Id = StoreDAO.Id;
            Store.RowId = StoreDAO.RowId;
            StoreDAO.Code = $"{Store.Organization.Code}.{Store.StoreType.Code}.{(10000000 + StoreDAO.Id).ToString().Substring(1)}";
            await DataContext.SaveChangesAsync();
            await SaveReference(Store);
            return true;
        }

        public async Task<bool> Update(Store Store)
        {
            StoreDAO StoreDAO = DataContext.Store.Where(x => x.Id == Store.Id).FirstOrDefault();
            if (StoreDAO == null)
                return false;

            StoreDAO.Code = $"{Store.Organization.Code}.{Store.StoreType.Code}.{(10000000 + StoreDAO.Id).ToString().Substring(1)}";
            StoreDAO.CodeDraft = Store.CodeDraft;
            StoreDAO.Name = Store.Name;
            StoreDAO.UnsignName = Store.UnsignName;
            StoreDAO.ParentStoreId = Store.ParentStoreId;
            StoreDAO.OrganizationId = Store.OrganizationId;
            StoreDAO.StoreTypeId = Store.StoreTypeId;
            StoreDAO.Telephone = Store.Telephone;
            StoreDAO.ProvinceId = Store.ProvinceId;
            StoreDAO.DistrictId = Store.DistrictId;
            StoreDAO.WardId = Store.WardId;
            StoreDAO.Address = Store.Address;
            StoreDAO.UnsignAddress = Store.UnsignAddress;
            StoreDAO.DeliveryAddress = Store.DeliveryAddress;
            StoreDAO.Latitude = Store.Latitude;
            StoreDAO.Longitude = Store.Longitude;
            StoreDAO.DeliveryLatitude = Store.DeliveryLatitude;
            StoreDAO.DeliveryLongitude = Store.DeliveryLongitude;
            StoreDAO.OwnerName = Store.OwnerName;
            StoreDAO.OwnerPhone = Store.OwnerPhone;
            StoreDAO.OwnerEmail = Store.OwnerEmail;
            StoreDAO.TaxCode = Store.TaxCode;
            StoreDAO.LegalEntity = Store.LegalEntity;
            StoreDAO.StatusId = Store.StatusId;
            StoreDAO.AppUserId = Store.AppUserId;
            StoreDAO.StoreStatusId = Store.StoreStatusId;
            StoreDAO.StoreScoutingId = Store.StoreScoutingId;
            StoreDAO.Description = Store.Description;
            StoreDAO.DebtLimited = Store.DebtLimited;
            StoreDAO.EstimatedRevenueId = Store.EstimatedRevenueId;
            StoreDAO.UpdatedAt = StaticParams.DateTimeNow;
            StoreDAO.IsStoreApprovalDirectSalesOrder = Store.IsStoreApprovalDirectSalesOrder;
            await DataContext.SaveChangesAsync();
            await SaveReference(Store);
            return true;
        }

        public async Task<bool> Delete(Store Store)
        {
            await DataContext.StoreStoreGroupingMapping.Where(x => x.StoreId == Store.Id).DeleteFromQueryAsync();
            await DataContext.AppUserStoreMapping.Where(x => x.StoreId == Store.Id).DeleteFromQueryAsync();
            await DataContext.Store.Where(x => x.ParentStoreId == Store.Id).UpdateFromQueryAsync(x => new StoreDAO { ParentStoreId = null });
            await DataContext.Store.Where(x => x.Id == Store.Id).UpdateFromQueryAsync(x => new StoreDAO { DeletedAt = StaticParams.DateTimeNow });
            return true;
        }

        public async Task<bool> BulkMerge(List<Store> Stores)
        {
            List<StoreDAO> StoreDAOs = new List<StoreDAO>();
            foreach (Store Store in Stores)
            {
                StoreDAO StoreDAO = new StoreDAO();
                StoreDAO.Id = Store.Id;
                StoreDAO.Code = Guid.NewGuid().ToString();
                StoreDAO.CodeDraft = Store.CodeDraft;
                StoreDAO.Name = Store.Name;
                StoreDAO.UnsignName = Store.UnsignName;
                StoreDAO.ParentStoreId = Store.ParentStoreId;
                StoreDAO.OrganizationId = Store.OrganizationId;
                StoreDAO.StoreTypeId = Store.StoreTypeId;
                StoreDAO.Telephone = Store.Telephone;
                StoreDAO.ProvinceId = Store.ProvinceId;
                StoreDAO.DistrictId = Store.DistrictId;
                StoreDAO.WardId = Store.WardId;
                StoreDAO.Address = Store.Address;
                StoreDAO.UnsignAddress = Store.UnsignAddress;
                StoreDAO.DeliveryAddress = Store.DeliveryAddress;
                StoreDAO.Latitude = Store.Latitude;
                StoreDAO.Longitude = Store.Longitude;
                StoreDAO.DeliveryLatitude = Store.DeliveryLatitude;
                StoreDAO.DeliveryLongitude = Store.DeliveryLongitude;
                StoreDAO.OwnerName = Store.OwnerName;
                StoreDAO.OwnerPhone = Store.OwnerPhone;
                StoreDAO.OwnerEmail = Store.OwnerEmail;
                StoreDAO.TaxCode = Store.TaxCode;
                StoreDAO.LegalEntity = Store.LegalEntity;
                StoreDAO.StatusId = Store.StatusId;
                StoreDAO.AppUserId = Store.AppUserId;
                StoreDAO.CreatorId = Store.CreatorId;
                StoreDAO.StoreStatusId = Store.StoreStatusId;
                StoreDAO.StoreScoutingId = Store.StoreScoutingId;
                StoreDAO.DebtLimited = Store.DebtLimited;
                StoreDAO.RowId = Guid.NewGuid();
                StoreDAO.Used = Store.Used;
                StoreDAO.CreatedAt = Store.CreatedAt == StaticParams.DateTimeMin ? StaticParams.DateTimeNow : Store.CreatedAt;
                StoreDAO.UpdatedAt = StaticParams.DateTimeNow;
                StoreDAOs.Add(StoreDAO);
                Store.RowId = StoreDAO.RowId;
            }
            await DataContext.BulkMergeAsync(StoreDAOs);
            try
            {
                foreach (StoreDAO StoreDAO in StoreDAOs)
                {
                    Store Store = Stores.Where(x => x.RowId == StoreDAO.RowId).FirstOrDefault();
                    Store.Id = StoreDAO.Id;
                    StoreDAO.Code = $"{Store.Organization.Code}.{Store.StoreType.Code}.{(10000000 + StoreDAO.Id).ToString().Substring(1)}";
                }
                await DataContext.BulkMergeAsync(StoreDAOs);

                foreach (var Store in Stores)
                {
                    Store.Id = StoreDAOs.Where(p => p.RowId == Store.RowId).Select(p => p.Id).FirstOrDefault();
                    if (Store.StoreStoreGroupingMappings != null)
                        Store.StoreStoreGroupingMappings.ForEach(x => x.StoreId = Store.Id);
                }

                List<StoreStoreGroupingMapping> StoreStoreGroupingMappings = Stores.Where(x => x.StoreStoreGroupingMappings != null).SelectMany(p => p.StoreStoreGroupingMappings).ToList();
                List<StoreStoreGroupingMappingDAO> StoreStoreGroupingMappingDAOs = new List<StoreStoreGroupingMappingDAO>();
                foreach (StoreStoreGroupingMapping StoreStoreGroupingMapping in StoreStoreGroupingMappings)
                {
                    StoreStoreGroupingMappingDAO StoreStoreGroupingMappingDAO = new StoreStoreGroupingMappingDAO
                    {
                        StoreId = StoreStoreGroupingMapping.StoreId,
                        StoreGroupingId = StoreStoreGroupingMapping.StoreGroupingId,
                    };
                    StoreStoreGroupingMappingDAOs.Add(StoreStoreGroupingMappingDAO);
                }
                List<long> StoreIds = Stores.Select(x => x.Id).ToList();
                IdFilter IdFilter = new IdFilter { In = StoreIds };
                await DataContext.StoreStoreGroupingMapping.Where(x => IdFilter.In.Contains(x.StoreId)).DeleteFromQueryAsync();
                await DataContext.StoreStoreGroupingMapping.BulkMergeAsync(StoreStoreGroupingMappingDAOs);
            }
            catch (Exception e)
            {

                throw e;
            }

            return true;
        }

        public async Task<bool> BulkDelete(List<Store> Stores)
        {
            List<long> Ids = Stores.Select(x => x.Id).ToList();
            await DataContext.StoreStoreGroupingMapping
                .Where(x => Ids.Contains(x.StoreId))
                .DeleteFromQueryAsync();
            await DataContext.Store
                .Where(x => x.ParentStoreId.HasValue)
                .Where(x => Ids.Contains(x.ParentStoreId.Value))
                .UpdateFromQueryAsync(x => new StoreDAO { ParentStoreId = null });
            await DataContext.Store
                .Where(x => Ids.Contains(x.Id))
                .UpdateFromQueryAsync(x => new StoreDAO { DeletedAt = StaticParams.DateTimeNow });
            return true;
        }
        public async Task<bool> Used(List<long> Ids)
        {
            await DataContext.Store
                .Where(x => Ids.Contains(x.Id))
                .UpdateFromQueryAsync(x => new StoreDAO { Used = true });
            return true;
        }

        private async Task SaveReference(Store Store)
        {
            List<AlbumImageMappingDAO> AlbumImageMappingDAOs = await DataContext.AlbumImageMapping.Where(x => x.StoreId == Store.Id).ToListWithNoLockAsync();
            AlbumImageMappingDAOs.ForEach(x => x.DeletedAt = StaticParams.DateTimeNow);
            if (Store.AlbumImageMappings != null)
            {
                foreach (var AlbumImageMapping in Store.AlbumImageMappings)
                {
                    AlbumImageMappingDAO AlbumImageMappingDAO = AlbumImageMappingDAOs.Where(x => x.AlbumId == AlbumImageMapping.AlbumId && x.ImageId == AlbumImageMapping.ImageId).FirstOrDefault();
                    if (AlbumImageMappingDAO == null)
                    {
                        AlbumImageMappingDAO = new AlbumImageMappingDAO();
                        AlbumImageMappingDAO.AlbumId = AlbumImageMapping.AlbumId;
                        AlbumImageMappingDAO.ImageId = AlbumImageMapping.ImageId;
                        AlbumImageMappingDAO.StoreId = Store.Id;
                        AlbumImageMappingDAO.SaleEmployeeId = AlbumImageMapping.SaleEmployeeId;
                        AlbumImageMappingDAO.ShootingAt = StaticParams.DateTimeNow;
                        AlbumImageMappingDAO.DeletedAt = null;
                        DataContext.AlbumImageMapping.Add(AlbumImageMappingDAO);
                    }
                    else
                    {
                        AlbumImageMappingDAO.AlbumId = AlbumImageMapping.AlbumId;
                        AlbumImageMappingDAO.ShootingAt = AlbumImageMapping.ShootingAt;
                        AlbumImageMappingDAO.DeletedAt = null;
                    }
                }
            }

            await DataContext.StoreImageMapping.Where(x => x.StoreId == Store.Id).DeleteFromQueryAsync();
            if (Store.StoreImageMappings != null)
            {
                foreach (StoreImageMapping StoreImageMapping in Store.StoreImageMappings)
                {
                    StoreImageMappingDAO StoreImageMappingDAO = new StoreImageMappingDAO();
                    StoreImageMappingDAO.StoreId = Store.Id;
                    StoreImageMappingDAO.ImageId = StoreImageMapping.ImageId;
                    DataContext.StoreImageMapping.Add(StoreImageMappingDAO);
                }
            }

            await DataContext.BrandInStoreProductGroupingMapping.Where(x => x.BrandInStore.StoreId == Store.Id).DeleteFromQueryAsync();
            await DataContext.BrandInStoreShowingCategoryMapping.Where(x => x.BrandInStore.StoreId == Store.Id).DeleteFromQueryAsync();
            List<BrandInStoreDAO> BrandInStoreDAOs = await DataContext.BrandInStore.Where(x => x.StoreId == Store.Id).ToListWithNoLockAsync();
            BrandInStoreDAOs.ForEach(x => x.DeletedAt = StaticParams.DateTimeNow);
            if (Store.BrandInStores != null)
            {
                foreach (var BrandInStore in Store.BrandInStores)
                {
                    BrandInStoreDAO BrandInStoreDAO = BrandInStoreDAOs.Where(x => x.BrandId == BrandInStore.BrandId).FirstOrDefault();
                    if (BrandInStoreDAO == null)
                    {
                        BrandInStoreDAO = new BrandInStoreDAO
                        {
                            BrandId = BrandInStore.BrandId,
                            StoreId = Store.Id,
                            Top = BrandInStore.Top,
                            CreatorId = BrandInStore.CreatorId,
                            CreatedAt = StaticParams.DateTimeNow,
                            UpdatedAt = StaticParams.DateTimeNow,
                            RowId = Guid.NewGuid()
                        };
                        BrandInStore.RowId = BrandInStoreDAO.RowId;
                        DataContext.BrandInStore.Add(BrandInStoreDAO);
                    }
                    else
                    {
                        BrandInStoreDAO.Top = BrandInStore.Top;
                        BrandInStoreDAO.UpdatedAt = StaticParams.DateTimeNow;
                        BrandInStoreDAO.DeletedAt = null;
                        BrandInStore.RowId = BrandInStoreDAO.RowId;
                    }
                }
                await DataContext.SaveChangesAsync();

                List<BrandInStoreHistoryDAO> BrandInStoreHistoryDAOs = new List<BrandInStoreHistoryDAO>();
                BrandInStoreDAOs = await DataContext.BrandInStore.Where(x => x.StoreId == Store.Id).ToListWithNoLockAsync();
                foreach (var BrandInStore in Store.BrandInStores)
                {
                    BrandInStoreDAO BrandInStoreDAO = BrandInStoreDAOs.Where(x => x.RowId == BrandInStore.RowId).FirstOrDefault();
                    BrandInStoreHistoryDAO BrandInStoreHistoryDAO = DataContext.BrandInStoreHistory.Where(x => x.BrandInStoreId == BrandInStoreDAO.Id).FirstOrDefault();
                    if (BrandInStoreHistoryDAO == null)
                    {
                        DataContext.BrandInStoreHistory.Add(new BrandInStoreHistoryDAO
                        {
                            BrandInStoreId = BrandInStoreDAO.Id,
                            CreatedAt = StaticParams.DateTimeNow,
                            CreatorId = Store.CreatorId,
                            Top = BrandInStoreDAO.Top,
                            BrandId = BrandInStoreDAO.BrandId,
                            StoreId = BrandInStoreDAO.StoreId
                        });
                    }
                    else if (BrandInStoreHistoryDAO.Top != BrandInStoreDAO.Top)
                    {
                        DataContext.BrandInStoreHistory.Add(new BrandInStoreHistoryDAO
                        {
                            BrandInStoreId = BrandInStoreDAO.Id,
                            CreatedAt = StaticParams.DateTimeNow,
                            CreatorId = Store.CreatorId,
                            BrandId = BrandInStoreDAO.BrandId,
                            StoreId = BrandInStoreDAO.StoreId,
                            Top = BrandInStoreDAO.Top,
                        });
                    }
                }

                List<BrandInStoreProductGroupingMappingDAO> BrandInStoreProductGroupingMappingDAOs = new List<BrandInStoreProductGroupingMappingDAO>();
                foreach (var BrandInStore in Store.BrandInStores)
                {
                    BrandInStore.Id = BrandInStoreDAOs.Where(x => x.RowId == BrandInStore.RowId).Select(x => x.Id).FirstOrDefault();
                    if (BrandInStore.BrandInStoreProductGroupingMappings != null)
                    {
                        foreach (var BrandInStoreProductGroupingMapping in BrandInStore.BrandInStoreProductGroupingMappings)
                        {
                            BrandInStoreProductGroupingMappingDAO BrandInStoreProductGroupingMappingDAO = new BrandInStoreProductGroupingMappingDAO
                            {
                                BrandInStoreId = BrandInStore.Id,
                                ProductGroupingId = BrandInStoreProductGroupingMapping.ProductGroupingId
                            };
                            DataContext.BrandInStoreProductGroupingMapping.Add(BrandInStoreProductGroupingMappingDAO);
                        }
                    }
                }

                List<BrandInStoreShowingCategoryMappingDAO> BrandInStoreShowingCategoryMappingDAOs = new List<BrandInStoreShowingCategoryMappingDAO>();
                foreach (var BrandInStore in Store.BrandInStores)
                {
                    BrandInStore.Id = BrandInStoreDAOs.Where(x => x.RowId == BrandInStore.RowId).Select(x => x.Id).FirstOrDefault();
                    if (BrandInStore.BrandInStoreShowingCategoryMappings != null)
                    {
                        foreach (var BrandInStoreShowingCategoryMapping in BrandInStore.BrandInStoreShowingCategoryMappings)
                        {
                            BrandInStoreShowingCategoryMappingDAO BrandInStoreShowingCategoryMappingDAO = new BrandInStoreShowingCategoryMappingDAO
                            {
                                BrandInStoreId = BrandInStore.Id,
                                ShowingCategoryId = BrandInStoreShowingCategoryMapping.ShowingCategoryId
                            };
                            DataContext.BrandInStoreShowingCategoryMapping.Add(BrandInStoreShowingCategoryMappingDAO);
                        }
                    }
                }
            }

            await DataContext.AppUserStoreMapping.Where(x => x.StoreId == Store.Id).DeleteFromQueryAsync();
            List<AppUserStoreMappingDAO> AppUserStoreMappingDAOs = new List<AppUserStoreMappingDAO>();
            if (Store.AppUserStoreMappings != null)
            {
                foreach (AppUserStoreMapping AppUserStoreMapping in Store.AppUserStoreMappings)
                {
                    AppUserStoreMappingDAO AppUserStoreMappingDAO = new AppUserStoreMappingDAO();
                    AppUserStoreMappingDAO.AppUserId = AppUserStoreMapping.AppUserId;
                    AppUserStoreMappingDAO.StoreId = Store.Id;
                    DataContext.AppUserStoreMapping.Add(AppUserStoreMappingDAO);
                }
            }

            await DataContext.StoreStoreGroupingMapping
                .Where(x => x.StoreId == Store.Id)
                .DeleteFromQueryAsync();
            List<StoreStoreGroupingMappingDAO> StoreStoreGroupingMappingDAOs = new List<StoreStoreGroupingMappingDAO>();
            if (Store.StoreStoreGroupingMappings != null)
            {
                foreach (StoreStoreGroupingMapping StoreStoreGroupingMapping in Store.StoreStoreGroupingMappings)
                {
                    StoreStoreGroupingMappingDAO StoreStoreGroupingMappingDAO = new StoreStoreGroupingMappingDAO()
                    {
                        StoreId = Store.Id,
                        StoreGroupingId = StoreStoreGroupingMapping.StoreGroupingId
                    };
                    DataContext.StoreStoreGroupingMapping.Add(StoreStoreGroupingMappingDAO);
                }
                await DataContext.SaveChangesAsync();
            }
        }
    }
}
