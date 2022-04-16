using DMS.ABE.Common;
using TrueSight.Common;
using DMS.ABE.Entities;
using DMS.ABE.Enums;
using DMS.ABE.Helpers;
using DMS.ABE.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thinktecture;
using Thinktecture.EntityFrameworkCore.TempTables;

namespace DMS.ABE.Repositories
{
    public interface IStoreRepository
    {
        Task<int> Count(StoreFilter StoreFilter);
        Task<List<Store>> List(StoreFilter StoreFilter);
        Task<List<Store>> List(List<long> Ids);
        Task<int> CountInScoped(StoreFilter filter, long AppUserId);
        Task<List<Store>> ListInScoped(StoreFilter filter, long AppUserId);
        Task<Store> Get(long Id);
        Task<bool> Create(Store Store);
        Task<bool> Update(Store Store);
        Task<bool> Delete(Store Store);
        Task<bool> BulkMerge(List<Store> Stores);
        Task<bool> BulkDelete(List<Store> Stores);
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

            query = query.Where(q => !q.DeletedAt.HasValue);
            if (filter.Search != null)
                query = query.Where(q =>
                q.CodeDraft.ToLower().Contains(filter.Search.ToLower()) ||
                q.Code.ToLower().Contains(filter.Search.ToLower()) ||
                q.Address.ToLower().Contains(filter.Search.ToLower()) ||
                q.UnsignAddress.ToLower().Contains(filter.Search.ToLower()) ||
                q.UnsignName.ToLower().Contains(filter.Search.ToLower()) ||
                q.Name.ToLower().Contains(filter.Search.ToLower()));
            if (filter.Id != null)
            {
                if (filter.Id.In != null && filter.Id.In.Count > 0)
                {
                    if (filter.Id.NotIn == null || filter.Id.NotIn.Count == 0)
                    {
                        ITempTableQuery<TempTable<long>> tempTableQuery = await DataContext
                        .BulkInsertValuesIntoTempTableAsync<long>(filter.Id.In.Distinct().ToList());
                        query = query.Join(tempTableQuery.Query,
                                           c => c.Id,
                                           t => t.Column1,
                                           (c, t) => c);
                    }
                    else
                    {
                        ITempTableQuery<TempTable<long>> tempTableQuery = await DataContext
                            .BulkInsertValuesIntoTempTableAsync<long>(filter.Id.In.Distinct().ToList());
                        query = query.Where(x => !filter.Id.NotIn.Contains(x.Id));
                        query = query.Join(tempTableQuery.Query,
                                           c => c.Id,
                                           t => t.Column1,
                                           (c, t) => c);
                    }

                }
                else
                {
                    query = query.Where(q => q.Id, filter.Id);
                }
            }
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.CodeDraft, filter.CodeDraft);
            query = query.Where(q => q.Name, filter.Name).Union(query.Where(q => q.UnsignName, filter.Name));
            query = query.Where(q => q.ParentStoreId, filter.ParentStoreId);
            if (filter.StoreCheckingStatusId != null)
            {
                if (filter.StoreCheckingStatusId.Equal.HasValue)
                {
                    var Start = StaticParams.DateTimeNow.AddHours(filter.TimeZone).Date.AddHours(0 - filter.TimeZone);
                    var End = StaticParams.DateTimeNow.AddHours(filter.TimeZone).Date.AddHours(0 - filter.TimeZone).AddDays(1).AddSeconds(-1);
                    var storeCheckingQuery = DataContext.StoreChecking
                           .Where(sc => sc.CheckOutAt.HasValue && Start <= sc.CheckOutAt.Value && sc.CheckOutAt.Value <= End);
                    if (filter.SalesEmployeeId != null && filter.SalesEmployeeId.Equal.HasValue)
                    {
                        storeCheckingQuery = storeCheckingQuery.Where(x => x.SaleEmployeeId == filter.SalesEmployeeId.Equal.Value);
                    }
                    var storeIds = storeCheckingQuery.Select(x => x.StoreId).ToList();
                    if (filter.StoreCheckingStatusId.Equal.Value == StoreCheckingStatusEnum.CHECKED.Id)
                        query = query.Where(q => storeIds.Contains(q.Id));
                    if (filter.StoreCheckingStatusId.Equal.Value == StoreCheckingStatusEnum.NOTCHECKED.Id)
                        query = query.Where(q => !storeIds.Contains(q.Id));
                }
            }
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
                    List<long> Ids = Branches.Select(o => o.Id).ToList();
                    query = query.Where(q => Ids.Contains(q.OrganizationId));
                }
                if (filter.OrganizationId.NotIn != null)
                {
                    List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                        .Where(o => o.DeletedAt == null && o.StatusId == 1).ToList();
                    List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => filter.OrganizationId.NotIn.Contains(o.Id)).ToList();
                    List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> Ids = Branches.Select(o => o.Id).ToList();
                    query = query.Where(q => !Ids.Contains(q.OrganizationId));
                }
            }
            if (filter.StoreGroupingId != null)
            {
                if (filter.StoreGroupingId.Equal != null)
                {
                    StoreGroupingDAO StoreGroupingDAO = DataContext.StoreGrouping
                        .Where(o => o.Id == filter.StoreGroupingId.Equal.Value).FirstOrDefault();
                    query = from q in query
                            join ssg in DataContext.StoreStoreGroupingMapping on q.Id equals ssg.StoreId
                            join sg in DataContext.StoreGrouping on ssg.StoreGroupingId equals sg.Id
                            where sg.Path.StartsWith(StoreGroupingDAO.Path)
                            select q;
                }
                if (filter.StoreGroupingId.NotEqual != null)
                {
                    StoreGroupingDAO StoreGroupingDAO = DataContext.StoreGrouping
                        .Where(o => o.Id == filter.StoreGroupingId.NotEqual.Value).FirstOrDefault();
                    query = from q in query
                            join ssg in DataContext.StoreStoreGroupingMapping on q.Id equals ssg.StoreId
                            join sg in DataContext.StoreGrouping on ssg.StoreGroupingId equals sg.Id
                            where !sg.Path.StartsWith(StoreGroupingDAO.Path)
                            select q;
                }
                if (filter.StoreGroupingId.In != null)
                {
                    List<StoreGroupingDAO> StoreGroupingDAOs = DataContext.StoreGrouping
                        .Where(o => o.DeletedAt == null).ToList();
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
                    List<StoreGroupingDAO> StoreGroupingDAOs = DataContext.StoreGrouping
                        .Where(o => o.DeletedAt == null).ToList();
                    List<StoreGroupingDAO> Parents = StoreGroupingDAOs.Where(o => filter.StoreGroupingId.NotIn.Contains(o.Id)).ToList();
                    List<StoreGroupingDAO> Branches = StoreGroupingDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> StoreGroupingIds = Branches.Select(x => x.Id).ToList();
                    query = from q in query
                            join ssg in DataContext.StoreStoreGroupingMapping on q.Id equals ssg.StoreId
                            join sg in DataContext.StoreGrouping on ssg.StoreGroupingId equals sg.Id
                            where !StoreGroupingIds.Contains(sg.Id)
                            select q;
                }
                query = query.Distinct();
            }
            query = query.Where(q => q.StoreTypeId, filter.StoreTypeId);
            query = query.Where(q => q.Telephone, filter.Telephone);
            query = query.Where(q => q.ProvinceId, filter.ProvinceId);
            query = query.Where(q => q.DistrictId, filter.DistrictId);
            query = query.Where(q => q.WardId, filter.WardId);
            query = query.Where(q => q.Address, filter.Address).Union(query.Where(q => q.UnsignAddress, filter.Address));
            query = query.Where(q => q.DeliveryAddress, filter.DeliveryAddress);
            query = query.Where(q => q.Latitude, filter.Latitude);
            query = query.Where(q => q.Longitude, filter.Longitude);
            query = query.Where(q => q.DeliveryLatitude, filter.DeliveryLatitude);
            query = query.Where(q => q.DeliveryLongitude, filter.DeliveryLongitude);
            query = query.Where(q => q.OwnerName, filter.OwnerName);
            query = query.Where(q => q.OwnerPhone, filter.OwnerPhone);
            query = query.Where(q => q.OwnerEmail, filter.OwnerEmail);
            query = query.Where(q => q.StatusId, filter.StatusId);
            query = query.Where(q => q.AppUserId, filter.AppUserId);
            if (filter.StoreStatusId != null && filter.StoreStatusId.Equal.HasValue && filter.StoreStatusId.Equal != StoreStatusEnum.ALL.Id)
            {
                if (filter.StoreStatusId.Equal == StoreStatusEnum.DRAFT.Id)
                {
                    query = query.Where(q => q.StoreStatusId, filter.StoreStatusId);
                    if (filter.StoreDraftTypeId != null && filter.StoreDraftTypeId.HasValue && filter.StoreDraftTypeId.Equal == StoreDraftTypeEnum.MINE.Id && filter.SalesEmployeeId.HasValue)
                        query = query.Where(q => q.AppUserId.Value == filter.SalesEmployeeId.Equal);
                }
                if (filter.StoreStatusId.Equal == StoreStatusEnum.OFFICIAL.Id)
                {
                    query = query.Where(q => q.StoreStatusId, filter.StoreStatusId);
                }
            }

            query = OrFilter(query, filter);
            query = query.Distinct();
            return query;
        }

        private IQueryable<StoreDAO> OrFilter(IQueryable<StoreDAO> query, StoreFilter filter)
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
                        OrganizationDAO OrganizationDAO = DataContext.Organization
                            .Where(o => o.Id == StoreFilter.OrganizationId.Equal.Value).FirstOrDefault();
                        queryable = queryable.Where(q => q.Organization.Path.StartsWith(OrganizationDAO.Path));
                    }
                    if (StoreFilter.OrganizationId.NotEqual != null)
                    {
                        OrganizationDAO OrganizationDAO = DataContext.Organization
                            .Where(o => o.Id == StoreFilter.OrganizationId.NotEqual.Value).FirstOrDefault();
                        queryable = queryable.Where(q => !q.Organization.Path.StartsWith(OrganizationDAO.Path));
                    }
                    if (StoreFilter.OrganizationId.In != null)
                    {
                        List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                            .Where(o => o.DeletedAt == null && o.StatusId == 1).ToList();
                        List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => StoreFilter.OrganizationId.In.Contains(o.Id)).ToList();
                        List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> Ids = Branches.Select(o => o.Id).ToList();
                        queryable = queryable.Where(q => Ids.Contains(q.OrganizationId));
                    }
                    if (StoreFilter.OrganizationId.NotIn != null)
                    {
                        List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                            .Where(o => o.DeletedAt == null && o.StatusId == 1).ToList();
                        List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => StoreFilter.OrganizationId.NotIn.Contains(o.Id)).ToList();
                        List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> Ids = Branches.Select(o => o.Id).ToList();
                        queryable = queryable.Where(q => !Ids.Contains(q.OrganizationId));
                    }
                }
                queryable = queryable.Where(q => q.StoreTypeId, StoreFilter.StoreTypeId);
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
                queryable = queryable.Where(q => q.AppUserId, StoreFilter.AppUserId);
                if (StoreFilter.StoreStatusId != null && StoreFilter.StoreStatusId.Equal.HasValue && StoreFilter.StoreStatusId.Equal != StoreStatusEnum.ALL.Id)
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
                ProvinceId = filter.Selects.Contains(StoreSelect.Province) ? q.ProvinceId : default(long),
                DistrictId = filter.Selects.Contains(StoreSelect.District) ? q.DistrictId : default(long),
                WardId = filter.Selects.Contains(StoreSelect.Ward) ? q.WardId : default(long),
                Address = filter.Selects.Contains(StoreSelect.Address) ? q.Address : default(string),
                UnsignAddress = filter.Selects.Contains(StoreSelect.UnsignAddress) ? q.UnsignAddress : default(string),
                DeliveryAddress = filter.Selects.Contains(StoreSelect.DeliveryAddress) ? q.DeliveryAddress : default(string),
                Latitude = filter.Selects.Contains(StoreSelect.Latitude) ? q.Latitude : default(decimal),
                Longitude = filter.Selects.Contains(StoreSelect.Longitude) ? q.Longitude : default(decimal),
                DeliveryLatitude = filter.Selects.Contains(StoreSelect.DeliveryLatitude) ? q.DeliveryLatitude : default(decimal?),
                DeliveryLongitude = filter.Selects.Contains(StoreSelect.DeliveryLongitude) ? q.DeliveryLongitude : default(decimal?),
                OwnerName = filter.Selects.Contains(StoreSelect.OwnerName) ? q.OwnerName : default(string),
                OwnerPhone = filter.Selects.Contains(StoreSelect.OwnerPhone) ? q.OwnerPhone : default(string),
                OwnerEmail = filter.Selects.Contains(StoreSelect.OwnerEmail) ? q.OwnerEmail : default(string),
                TaxCode = filter.Selects.Contains(StoreSelect.TaxCode) ? q.TaxCode : default(string),
                LegalEntity = filter.Selects.Contains(StoreSelect.LegalEntity) ? q.LegalEntity : default(string),
                StatusId = filter.Selects.Contains(StoreSelect.Status) ? q.StatusId : default(long),
                StoreScoutingId = filter.Selects.Contains(StoreSelect.StoreScouting) ? q.StoreScoutingId : default(long?),
                AppUserId = filter.Selects.Contains(StoreSelect.AppUser) ? q.AppUserId : default(long?),
                CreatorId = filter.Selects.Contains(StoreSelect.Creator) ? q.CreatorId : default(long),
                StoreStatusId = filter.Selects.Contains(StoreSelect.StoreStatus) ? q.StoreStatusId : default(long),
                Description = filter.Selects.Contains(StoreSelect.Description) ? q.Description : default(string),
                DebtLimited = filter.Selects.Contains(StoreSelect.DebtLimited) ? q.DebtLimited : default(decimal?),
                District = filter.Selects.Contains(StoreSelect.District) && q.District != null ? new District
                {
                    Id = q.District.Id,
                    Code = q.District.Code,
                    Name = q.District.Name,
                    Priority = q.District.Priority,
                    ProvinceId = q.District.ProvinceId,
                    StatusId = q.District.StatusId,
                } : null,
                Organization = filter.Selects.Contains(StoreSelect.Organization) && q.Organization != null ? new Organization
                {
                    Id = q.Organization.Id,
                    Code = q.Organization.Code,
                    Name = q.Organization.Name,
                    ParentId = q.Organization.ParentId,
                    Path = q.Organization.Path,
                    Level = q.Organization.Level,
                    StatusId = q.Organization.StatusId,
                    Phone = q.Organization.Phone,
                    Address = q.Organization.Address,
                    Email = q.Organization.Email,
                } : null,
                ParentStore = filter.Selects.Contains(StoreSelect.ParentStore) && q.ParentStore != null ? new Store
                {
                    Id = q.ParentStore.Id,
                    Code = q.ParentStore.Code,
                    Name = q.ParentStore.Name,
                    ParentStoreId = q.ParentStore.ParentStoreId,
                    OrganizationId = q.ParentStore.OrganizationId,
                    StoreTypeId = q.ParentStore.StoreTypeId,
                    Telephone = q.ParentStore.Telephone,
                    ProvinceId = q.ParentStore.ProvinceId,
                    DistrictId = q.ParentStore.DistrictId,
                    WardId = q.ParentStore.WardId,
                    Address = q.ParentStore.Address,
                    DeliveryAddress = q.ParentStore.DeliveryAddress,
                    Latitude = q.ParentStore.Latitude,
                    Longitude = q.ParentStore.Longitude,
                    DeliveryLatitude = q.ParentStore.DeliveryLatitude,
                    DeliveryLongitude = q.ParentStore.DeliveryLongitude,
                    OwnerName = q.ParentStore.OwnerName,
                    OwnerPhone = q.ParentStore.OwnerPhone,
                    OwnerEmail = q.ParentStore.OwnerEmail,
                    TaxCode = q.ParentStore.TaxCode,
                    LegalEntity = q.ParentStore.LegalEntity,
                    StatusId = q.ParentStore.StatusId,
                } : null,
                Province = filter.Selects.Contains(StoreSelect.Province) && q.Province != null ? new Province
                {
                    Id = q.Province.Id,
                    Code = q.Province.Code,
                    Name = q.Province.Name,
                    Priority = q.Province.Priority,
                    StatusId = q.Province.StatusId,
                } : null,
                AppUser = filter.Selects.Contains(StoreSelect.AppUser) && q.AppUser != null ? new AppUser
                {
                    Id = q.AppUser.Id,
                    Username = q.AppUser.Username,
                    DisplayName = q.AppUser.DisplayName,
                    Address = q.AppUser.Address,
                    Email = q.AppUser.Email,
                    Phone = q.AppUser.Phone,
                    PositionId = q.AppUser.PositionId,
                    Department = q.AppUser.Department,
                    OrganizationId = q.AppUser.OrganizationId,
                    StatusId = q.AppUser.StatusId,
                    Avatar = q.AppUser.Avatar,
                    ProvinceId = q.AppUser.ProvinceId,
                    SexId = q.AppUser.SexId,
                    Birthday = q.AppUser.Birthday,
                } : null,
                Creator = filter.Selects.Contains(StoreSelect.Creator) && q.Creator != null ? new AppUser
                {
                    Id = q.Creator.Id,
                    Username = q.Creator.Username,
                    DisplayName = q.Creator.DisplayName,
                    Address = q.Creator.Address,
                    Email = q.Creator.Email,
                    Phone = q.Creator.Phone,
                    PositionId = q.Creator.PositionId,
                    Department = q.Creator.Department,
                    OrganizationId = q.Creator.OrganizationId,
                    StatusId = q.Creator.StatusId,
                    Avatar = q.Creator.Avatar,
                    ProvinceId = q.Creator.ProvinceId,
                    SexId = q.Creator.SexId,
                    Birthday = q.Creator.Birthday,
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
                    StatusId = q.StoreType.StatusId,
                    ColorId = q.StoreType.ColorId,
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
                    Priority = q.Ward.Priority,
                    DistrictId = q.Ward.DistrictId,
                    StatusId = q.Ward.StatusId,
                } : null,
                StoreStoreGroupingMappings = filter.Selects.Contains(StoreSelect.StoreStoreGroupingMapping) && q.StoreStoreGroupingMappings != null ?
                q.StoreStoreGroupingMappings.Select(p => new StoreStoreGroupingMapping
                {
                    StoreId = p.StoreId,
                    StoreGroupingId = p.StoreGroupingId,
                    StoreGrouping = new StoreGrouping
                    {
                        Id = p.StoreGrouping.Id,
                        Code = p.StoreGrouping.Code,
                        Name = p.StoreGrouping.Name,
                        ParentId = p.StoreGrouping.ParentId,
                        Path = p.StoreGrouping.Path,
                        Level = p.StoreGrouping.Level,
                        StatusId = p.StoreGrouping.StatusId
                    },
                }).ToList() : null,
                StoreStatus = filter.Selects.Contains(StoreSelect.StoreStatus) && q.StoreStatus != null ? new StoreStatus
                {
                    Id = q.StoreStatus.Id,
                    Code = q.StoreStatus.Code,
                    Name = q.StoreStatus.Name,
                } : null,
                Used = q.Used,
                CreatedAt = q.CreatedAt,
                RowId = q.RowId,
            }).ToListAsync();

            var StoreIds = Stores.Select(x => x.Id).ToList();
            ITempTableQuery<TempTable<long>> tempTableQuery = await DataContext
                    .BulkInsertValuesIntoTempTableAsync<long>(StoreIds);

            if (filter.Selects.Contains(StoreSelect.StoreImageMappings))
            {
                var StoreImageMappings = await DataContext.StoreImageMapping
                    .Join(tempTableQuery.Query,
                            c => c.StoreId,
                            t => t.Column1,
                            (c, t) => c).Select(x => new StoreImageMapping
                            {
                                StoreId = x.StoreId,
                                ImageId = x.ImageId,
                                Image = x.Image == null ? null : new Image
                                {
                                    Id = x.Image.Id,
                                    Name = x.Image.Name,
                                    Url = x.Image.Url,
                                    ThumbnailUrl = x.Image.ThumbnailUrl,
                                }
                            }).ToListAsync();
                foreach (var Store in Stores)
                {
                    Store.StoreImageMappings = StoreImageMappings.Where(x => x.StoreId == Store.Id).Skip(0).Take(1).ToList();
                }
            }

            if (filter.Selects.Contains(StoreSelect.StoreAppUserMappings))
            {
                var StoreAppUserMappings = await DataContext.AppUserStoreMapping
                .Join(tempTableQuery.Query,
                        c => c.StoreId,
                        t => t.Column1,
                        (c, t) => c).Select(x => new AppUserStoreMapping
                        {
                            StoreId = x.StoreId,
                            AppUserId = x.AppUserId,
                            AppUser = x.AppUser == null ? null : new AppUser
                            {
                                Id = x.AppUser.Id,
                                Username = x.AppUser.Username,
                                DisplayName = x.AppUser.DisplayName,
                            }
                        }).ToListAsync();
                foreach (var Store in Stores)
                {
                    Store.StoreAppUserMappings = StoreAppUserMappings.Where(x => x.StoreId == Store.Id).Skip(0).Take(int.MaxValue).ToList();
                }
            }

            return Stores;
        }

        public async Task<int> Count(StoreFilter filter)
        {
            IQueryable<StoreDAO> Stores = DataContext.Store;
            Stores = await DynamicFilter(Stores, filter);
            int count = await Stores.CountAsync();
            return count;
        }

        public async Task<List<Store>> List(StoreFilter filter)
        {
            if (filter == null) return new List<Store>();
            IQueryable<StoreDAO> StoreDAOs = DataContext.Store.AsNoTracking();
            StoreDAOs = await DynamicFilter(StoreDAOs, filter);
            StoreDAOs = DynamicOrder(StoreDAOs, filter);
            List<Store> Stores = await DynamicSelect(StoreDAOs, filter);
            return Stores;
        }

        public async Task<List<Store>> List(List<long> Ids)
        {
            ITempTableQuery<TempTable<long>> tempTableQuery = await DataContext
                        .BulkInsertValuesIntoTempTableAsync<long>(Ids);

            var query = from s in DataContext.Store
                        join tt in tempTableQuery.Query on s.Id equals tt.Column1
                        select s;

            List<Store> Stores = await query.Select(x => new Store
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
                DebtLimited = x.DebtLimited,
                District = x.District == null ? null : new District
                {
                    Id = x.District.Id,
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
            }).ToListAsync();

            var queryStoreImageMappings = from i in DataContext.StoreImageMapping
                                          join tt in tempTableQuery.Query on i.StoreId equals tt.Column1
                                          select i;
            var StoreImageMappings = await queryStoreImageMappings.Select(x => new StoreImageMapping
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
            }).ToListAsync();

            var queryBrandInStores = from bs in DataContext.BrandInStore
                                     join tt in tempTableQuery.Query on bs.StoreId equals tt.Column1
                                     select bs;
            var BrandInStores = await queryBrandInStores
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
                }).ToListAsync();
            var BrandInStoreIds = BrandInStores.Select(x => x.Id).ToList();
            var BrandInStoreProductGroupingMappings = await DataContext.BrandInStoreProductGroupingMapping
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
                }).ToListAsync();
            foreach (var BrandInStore in BrandInStores)
            {
                BrandInStore.BrandInStoreProductGroupingMappings = BrandInStoreProductGroupingMappings.Where(x => x.BrandInStoreId == BrandInStore.Id).ToList();
            }
            foreach (var Store in Stores)
            {
                Store.StoreImageMappings = StoreImageMappings.Where(x => x.StoreId == Store.Id).ToList();
                Store.BrandInStores = BrandInStores.Where(x => x.StoreId == Store.Id).ToList();
            }
            List<StoreStoreGroupingMapping> StoreStoreGroupingMappings = await DataContext.StoreStoreGroupingMapping.AsNoTracking()
            .Where(x => Ids.Contains(x.StoreId))
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
            }).ToListAsync();
            foreach (Store Store in Stores)
            {
                Store.StoreStoreGroupingMappings = StoreStoreGroupingMappings
                    .Where(x => x.StoreId == Store.Id)
                    .ToList();
            }
            return Stores;
        }
        public async Task<int> CountInScoped(StoreFilter filter, long AppUserId)
        {
            if (filter == null) return 0;
            AppUserDAO AppUserDAO = await DataContext.AppUser.Where(x => x.Id == AppUserId)
                .Include(x => x.Organization)
                .FirstOrDefaultAsync();
            List<long> StoreIds = await DataContext.AppUserStoreMapping
                .Where(au => au.AppUserId == AppUserId)
                .Select(au => au.StoreId)
                .ToListAsync();
            if (StoreIds.Count == 0)
            {
                OrganizationDAO OrganizationDAO = await DataContext.Organization.Where(x => x.Id == AppUserDAO.OrganizationId).FirstOrDefaultAsync();
                StoreIds = await DataContext.Store
                .Where(x => x.Organization.Path.StartsWith(OrganizationDAO.Path))
                .Select(x => x.Id)
                .ToListAsync();
            }

            List<long> DraftStoreIds = await DataContext.Store
                .Where(x =>
                    x.StoreStatusId == StoreStatusEnum.DRAFT.Id &&
                    x.Organization.Path.StartsWith(AppUserDAO.Organization.Path) &&
                    x.DeletedAt == null)
              .Select(x => x.Id).ToListAsync();
            //cộng thêm đại lý dự thảo
            StoreIds.AddRange(DraftStoreIds);
            StoreIds = StoreIds.Distinct().ToList();
            if (filter.Id == null) filter.Id = new IdFilter();
            if (filter.Id.In == null || filter.Id.In.Count == 0)
                filter.Id.In = StoreIds;

            //nếu nhân viên đã có phạm vi đi tuyến, lấy giao giữa tập đc phân quyền và phạm vi đi tuyến
            filter.Id.In = filter.Id.In.Intersect(StoreIds).ToList();

            IQueryable<StoreDAO> Stores = DataContext.Store;
            Stores = await DynamicFilter(Stores, filter);
            int count = await Stores.CountAsync();
            return count;
        }

        public async Task<List<Store>> ListInScoped(StoreFilter filter, long AppUserId)
        {
            if (filter == null) return new List<Store>();
            AppUserDAO AppUserDAO = await DataContext.AppUser.Where(x => x.Id == AppUserId)
                .Include(x => x.Organization)
                .FirstOrDefaultAsync();
            List<long> StoreIds = await DataContext.AppUserStoreMapping
                .Where(au => au.AppUserId == AppUserId)
                .Select(au => au.StoreId)
                .ToListAsync();
            if (StoreIds.Count == 0)
            {
                OrganizationDAO OrganizationDAO = await DataContext.Organization.Where(x => x.Id == AppUserDAO.OrganizationId).FirstOrDefaultAsync();
                StoreIds = await DataContext.Store
                .Where(x => x.Organization.Path.StartsWith(OrganizationDAO.Path))
                .Select(x => x.Id)
                .ToListAsync();
            }

            List<long> DraftStoreIds = await DataContext.Store
                .Where(x =>
                    x.StoreStatusId == StoreStatusEnum.DRAFT.Id &&
                    x.Organization.Path.StartsWith(AppUserDAO.Organization.Path) &&
                    x.DeletedAt == null)
              .Select(x => x.Id).ToListAsync();

            //cộng thêm đại lý dự thảo
            StoreIds.AddRange(DraftStoreIds);
            StoreIds = StoreIds.Distinct().ToList();
            if (filter.Id == null) filter.Id = new IdFilter();
            if (filter.Id.In == null || filter.Id.In.Count == 0)
                filter.Id.In = StoreIds;

            //nếu nhân viên đã có phạm vi đi tuyến, lấy giao giữa tập đc phân quyền và phạm vi đi tuyến
            filter.Id.In = filter.Id.In.Intersect(StoreIds).ToList();

            IQueryable<StoreDAO> StoreDAOs = DataContext.Store.AsNoTracking();
            StoreDAOs = await DynamicFilter(StoreDAOs, filter);
            StoreDAOs = DynamicOrder(StoreDAOs, filter);
            List<Store> Stores = await DynamicSelect(StoreDAOs, filter);
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
                }).FirstOrDefaultAsync();

            if (Store == null)
                return null;

            Store.StoreImageMappings = await DataContext.StoreImageMapping
                .Where(x => x.StoreId == Id).Select(x => new StoreImageMapping
                {
                    StoreId = x.StoreId,
                    ImageId = x.ImageId,
                    Image = new Image
                    {
                        Id = x.Image.Id,
                        Name = x.Image.Name,
                        Url = x.Image.Url,
                        ThumbnailUrl = x.Image.ThumbnailUrl,
                        RowId = x.Image.RowId,
                        CreatedAt = x.Image.CreatedAt,
                        UpdatedAt = x.Image.UpdatedAt,
                        DeletedAt = x.Image.DeletedAt,
                    }
                }).ToListAsync();

            Store.AlbumImageMappings = await DataContext.AlbumImageMapping
                .Where(x => x.StoreId == Id).Select(x => new AlbumImageMapping
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
                }).ToListAsync();

            Store.BrandInStores = await DataContext.BrandInStore
                .Where(x => x.StoreId == Id && x.DeletedAt == null).Select(x => new BrandInStore
                {
                    Id = x.Id,
                    StoreId = x.StoreId,
                    BrandId = x.BrandId,
                    Top = x.Top,
                    CreatorId = x.CreatorId,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    Brand = x.Brand == null ? null : new Brand
                    {
                        Id = x.Brand.Id,
                        Code = x.Brand.Code,
                        Name = x.Brand.Name,
                    },
                    Creator = x.Creator == null ? null : new AppUser
                    {
                        Id = x.Creator.Id,
                        Username = x.Creator.Username,
                        DisplayName = x.Creator.DisplayName,
                    }
                }).OrderBy(x => x.Top).ToListAsync();

            var BrandInStoreIds = Store.BrandInStores.Select(x => x.Id).ToList();
            var BrandInStoreProductGroupingMappings = await DataContext.BrandInStoreProductGroupingMapping
                .Where(x => BrandInStoreIds.Contains(x.BrandInStoreId)).Select(x => new BrandInStoreProductGroupingMapping
                {
                    BrandInStoreId = x.BrandInStoreId,
                    ProductGroupingId = x.ProductGroupingId,
                    ProductGrouping = x.ProductGrouping == null ? null : new ProductGrouping
                    {
                        Id = x.ProductGrouping.Id,
                        Code = x.ProductGrouping.Code,
                        Name = x.ProductGrouping.Name,
                    },
                }).ToListAsync();
            foreach (var BrandInStore in Store.BrandInStores)
            {
                BrandInStore.BrandInStoreProductGroupingMappings = BrandInStoreProductGroupingMappings
                    .Where(x => x.BrandInStoreId == BrandInStore.Id)
                    .ToList();
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
                        ParentId = x.StoreGrouping.ParentId,
                        Path = x.StoreGrouping.Path,
                        Level = x.StoreGrouping.Level,
                        RowId = x.StoreGrouping.RowId,
                        CreatedAt = x.StoreGrouping.CreatedAt,
                        UpdatedAt = x.StoreGrouping.UpdatedAt,
                        DeletedAt = x.StoreGrouping.DeletedAt,
                    },
                }).ToListAsync();
            return Store;
        }
        public async Task<bool> Create(Store Store)
        {
            StoreDAO StoreDAO = new StoreDAO();
            StoreDAO.Id = Store.Id;
            StoreDAO.Code = Store.Code;
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
            StoreDAO.RowId = Guid.NewGuid();
            StoreDAO.CreatedAt = StaticParams.DateTimeNow;
            StoreDAO.UpdatedAt = StaticParams.DateTimeNow;
            StoreDAO.Used = false;
            DataContext.Store.Add(StoreDAO);
            await DataContext.SaveChangesAsync();
            Store.Id = StoreDAO.Id;
            Store.RowId = StoreDAO.RowId;
            await SaveReference(Store);
            return true;
        }

        public async Task<bool> Update(Store Store)
        {
            StoreDAO StoreDAO = DataContext.Store.Where(x => x.Id == Store.Id).FirstOrDefault();
            if (StoreDAO == null)
                return false;
            StoreDAO.Id = Store.Id;
            StoreDAO.Code = Store.Code;
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
            StoreDAO.UpdatedAt = StaticParams.DateTimeNow;
            await DataContext.SaveChangesAsync();
            await SaveReference(Store);
            return true;
        }

        public async Task<bool> Delete(Store Store)
        {
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
                StoreDAO.Code = Store.Code;
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
                StoreDAO.RowId = Store.RowId;
                StoreDAO.Used = Store.Used;
                StoreDAO.CreatedAt = StaticParams.DateTimeNow;
                StoreDAO.UpdatedAt = StaticParams.DateTimeNow;
                StoreDAOs.Add(StoreDAO);
            }
            await DataContext.BulkMergeAsync(StoreDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<Store> Stores)
        {
            List<long> Ids = Stores.Select(x => x.Id).ToList();
            await DataContext.Store.Where(x => Ids.Contains(x.ParentStoreId.Value)).UpdateFromQueryAsync(x => new StoreDAO { ParentStoreId = null });
            await DataContext.Store
                .Where(x => Ids.Contains(x.Id))
                .UpdateFromQueryAsync(x => new StoreDAO { DeletedAt = StaticParams.DateTimeNow });
            return true;
        }

        private async Task SaveReference(Store Store)
        {
            List<AlbumImageMappingDAO> AlbumImageMappingDAOs = await DataContext.AlbumImageMapping.Where(x => x.StoreId == Store.Id).ToListAsync();
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
                        AlbumImageMappingDAOs.Add(AlbumImageMappingDAO);
                    }
                    else
                    {
                        AlbumImageMappingDAO.AlbumId = AlbumImageMapping.AlbumId;
                        AlbumImageMappingDAO.ShootingAt = AlbumImageMapping.ShootingAt;
                        AlbumImageMappingDAO.DeletedAt = null;
                    }
                }
            }
            await DataContext.AlbumImageMapping.BulkMergeAsync(AlbumImageMappingDAOs);

            await DataContext.StoreImageMapping.Where(x => x.StoreId == Store.Id).DeleteFromQueryAsync();
            List<StoreImageMappingDAO> StoreImageMappingDAOs = new List<StoreImageMappingDAO>();
            if (Store.StoreImageMappings != null)
            {
                foreach (StoreImageMapping StoreImageMapping in Store.StoreImageMappings)
                {
                    StoreImageMappingDAO StoreImageMappingDAO = new StoreImageMappingDAO();
                    StoreImageMappingDAO.StoreId = Store.Id;
                    StoreImageMappingDAO.ImageId = StoreImageMapping.ImageId;
                    StoreImageMappingDAOs.Add(StoreImageMappingDAO);
                }
                await DataContext.StoreImageMapping.BulkMergeAsync(StoreImageMappingDAOs);
            }

            await DataContext.BrandInStoreProductGroupingMapping.Where(x => x.BrandInStore.StoreId == Store.Id).DeleteFromQueryAsync();
            List<BrandInStoreDAO> BrandInStoreDAOs = await DataContext.BrandInStore.Where(x => x.StoreId == Store.Id).ToListAsync();
            BrandInStoreDAOs.ForEach(x => x.DeletedAt = StaticParams.DateTimeNow);
            if (Store.BrandInStores != null)
            {
                foreach (var BrandInStore in Store.BrandInStores)
                {
                    BrandInStoreDAO BrandInStoreDAO = BrandInStoreDAOs.Where(x => x.StoreId == BrandInStore.StoreId && x.BrandId == BrandInStore.BrandId).FirstOrDefault();
                    if (BrandInStoreDAO == null)
                    {
                        BrandInStoreDAO = new BrandInStoreDAO
                        {
                            BrandId = BrandInStore.BrandId,
                            StoreId = BrandInStore.StoreId,
                            Top = BrandInStore.Top,
                            CreatorId = BrandInStore.CreatorId,
                            CreatedAt = StaticParams.DateTimeNow,
                            UpdatedAt = StaticParams.DateTimeNow,
                            RowId = Guid.NewGuid()
                        };
                        BrandInStoreDAOs.Add(BrandInStoreDAO);
                        BrandInStore.RowId = BrandInStoreDAO.RowId;
                    }
                    else
                    {
                        BrandInStoreDAO.Top = BrandInStore.Top;
                        BrandInStoreDAO.UpdatedAt = StaticParams.DateTimeNow;
                        BrandInStoreDAO.DeletedAt = null;
                        BrandInStore.RowId = BrandInStoreDAO.RowId;
                    }
                }
                await DataContext.BulkMergeAsync(BrandInStoreDAOs);

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
                            BrandInStoreProductGroupingMappingDAOs.Add(BrandInStoreProductGroupingMappingDAO);
                        }
                    }
                }
                await DataContext.BulkMergeAsync(BrandInStoreProductGroupingMappingDAOs);
            }
        }
    }
}
