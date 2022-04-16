using DMS.Entities;
using DMS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Repositories
{
    public interface IERouteContentRepository
    {
        Task<int> Count(ERouteContentFilter ERouteContentFilter);
        Task<int> CountAll(ERouteContentFilter ERouteContentFilter);
        Task<List<ERouteContent>> List(ERouteContentFilter ERouteContentFilter);
        Task<ERouteContent> Get(long Id);
    }
    public class ERouteContentRepository : CacheRepository, IERouteContentRepository
    {
        private DataContext DataContext;
        public ERouteContentRepository(DataContext DataContext, IRedisStore RedisStore, IConfiguration Configuration)
            : base(DataContext, RedisStore, Configuration)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<ERouteContentDAO> DynamicFilter(IQueryable<ERouteContentDAO> query, ERouteContentFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.ERouteId, filter.ERouteId);
            query = query.Where(q => q.StoreId, filter.StoreId);
            query = query.Where(q => q.OrderNumber, filter.OrderNumber);
            return query;
        }

        private IQueryable<ERouteContentDAO> OrFilter(IQueryable<ERouteContentDAO> query, ERouteContentFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<ERouteContentDAO> initQuery = query.Where(q => false);
            foreach (ERouteContentFilter ERouteContentFilter in filter.OrFilter)
            {
                IQueryable<ERouteContentDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, ERouteContentFilter.Id);
                queryable = queryable.Where(q => q.ERouteId, ERouteContentFilter.ERouteId);
                queryable = queryable.Where(q => q.StoreId, ERouteContentFilter.StoreId);
                queryable = queryable.Where(q => q.OrderNumber, ERouteContentFilter.OrderNumber);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<ERouteContentDAO> DynamicOrder(IQueryable<ERouteContentDAO> query, ERouteContentFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case ERouteContentOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case ERouteContentOrder.ERoute:
                            query = query.OrderBy(q => q.ERouteId);
                            break;
                        case ERouteContentOrder.Store:
                            query = query.OrderBy(q => q.StoreId);
                            break;
                        case ERouteContentOrder.OrderNumber:
                            query = query.OrderBy(q => q.OrderNumber);
                            break;
                        case ERouteContentOrder.Monday:
                            query = query.OrderBy(q => q.Monday);
                            break;
                        case ERouteContentOrder.Tuesday:
                            query = query.OrderBy(q => q.Tuesday);
                            break;
                        case ERouteContentOrder.Wednesday:
                            query = query.OrderBy(q => q.Wednesday);
                            break;
                        case ERouteContentOrder.Thursday:
                            query = query.OrderBy(q => q.Thursday);
                            break;
                        case ERouteContentOrder.Friday:
                            query = query.OrderBy(q => q.Friday);
                            break;
                        case ERouteContentOrder.Saturday:
                            query = query.OrderBy(q => q.Saturday);
                            break;
                        case ERouteContentOrder.Sunday:
                            query = query.OrderBy(q => q.Sunday);
                            break;
                        case ERouteContentOrder.Week1:
                            query = query.OrderBy(q => q.Week1);
                            break;
                        case ERouteContentOrder.Week2:
                            query = query.OrderBy(q => q.Week2);
                            break;
                        case ERouteContentOrder.Week3:
                            query = query.OrderBy(q => q.Week3);
                            break;
                        case ERouteContentOrder.Week4:
                            query = query.OrderBy(q => q.Week4);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case ERouteContentOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case ERouteContentOrder.ERoute:
                            query = query.OrderByDescending(q => q.ERouteId);
                            break;
                        case ERouteContentOrder.Store:
                            query = query.OrderByDescending(q => q.StoreId);
                            break;
                        case ERouteContentOrder.OrderNumber:
                            query = query.OrderByDescending(q => q.OrderNumber);
                            break;
                        case ERouteContentOrder.Monday:
                            query = query.OrderByDescending(q => q.Monday);
                            break;
                        case ERouteContentOrder.Tuesday:
                            query = query.OrderByDescending(q => q.Tuesday);
                            break;
                        case ERouteContentOrder.Wednesday:
                            query = query.OrderByDescending(q => q.Wednesday);
                            break;
                        case ERouteContentOrder.Thursday:
                            query = query.OrderByDescending(q => q.Thursday);
                            break;
                        case ERouteContentOrder.Friday:
                            query = query.OrderByDescending(q => q.Friday);
                            break;
                        case ERouteContentOrder.Saturday:
                            query = query.OrderByDescending(q => q.Saturday);
                            break;
                        case ERouteContentOrder.Sunday:
                            query = query.OrderByDescending(q => q.Sunday);
                            break;
                        case ERouteContentOrder.Week1:
                            query = query.OrderByDescending(q => q.Week1);
                            break;
                        case ERouteContentOrder.Week2:
                            query = query.OrderByDescending(q => q.Week2);
                            break;
                        case ERouteContentOrder.Week3:
                            query = query.OrderByDescending(q => q.Week3);
                            break;
                        case ERouteContentOrder.Week4:
                            query = query.OrderByDescending(q => q.Week4);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<ERouteContent>> DynamicSelect(IQueryable<ERouteContentDAO> query, ERouteContentFilter filter)
        {
            List<ERouteContent> ERouteContents = await query.Select(q => new ERouteContent()
            {
                Id = filter.Selects.Contains(ERouteContentSelect.Id) ? q.Id : default(long),
                ERouteId = filter.Selects.Contains(ERouteContentSelect.ERoute) ? q.ERouteId : default(long),
                StoreId = filter.Selects.Contains(ERouteContentSelect.Store) ? q.StoreId : default(long),
                OrderNumber = filter.Selects.Contains(ERouteContentSelect.OrderNumber) ? q.OrderNumber : null,
                Monday = filter.Selects.Contains(ERouteContentSelect.Monday) ? q.Monday : default(bool),
                Tuesday = filter.Selects.Contains(ERouteContentSelect.Tuesday) ? q.Tuesday : default(bool),
                Wednesday = filter.Selects.Contains(ERouteContentSelect.Wednesday) ? q.Wednesday : default(bool),
                Thursday = filter.Selects.Contains(ERouteContentSelect.Thursday) ? q.Thursday : default(bool),
                Friday = filter.Selects.Contains(ERouteContentSelect.Friday) ? q.Friday : default(bool),
                Saturday = filter.Selects.Contains(ERouteContentSelect.Saturday) ? q.Saturday : default(bool),
                Sunday = filter.Selects.Contains(ERouteContentSelect.Sunday) ? q.Sunday : default(bool),
                Week1 = filter.Selects.Contains(ERouteContentSelect.Week1) ? q.Week1 : default(bool),
                Week2 = filter.Selects.Contains(ERouteContentSelect.Week2) ? q.Week2 : default(bool),
                Week3 = filter.Selects.Contains(ERouteContentSelect.Week3) ? q.Week3 : default(bool),
                Week4 = filter.Selects.Contains(ERouteContentSelect.Week4) ? q.Week4 : default(bool),
            }).ToListWithNoLockAsync();

            var Ids = ERouteContents.Select(x => x.Id).ToList();
            IdFilter IdFilter = new IdFilter { In = Ids };
            var ERouteContentDays = await DataContext.ERouteContentDay
                .Where(x => x.ERouteContentId, IdFilter).Select(x => new ERouteContentDay
                {
                    Id = x.Id,
                    ERouteContentId = x.ERouteContentId,
                    OrderDay = x.OrderDay,
                    Planned = x.Planned,
                }).ToListWithNoLockAsync();

            foreach (var ERouteContent in ERouteContents)
            {
                ERouteContent.ERouteContentDays = ERouteContentDays.Where(x => x.ERouteContentId == ERouteContent.Id && x.Planned).ToList();
            }
            return ERouteContents;
        }

        public async Task<int> Count(ERouteContentFilter filter)
        {
            IQueryable<ERouteContentDAO> ERouteContents = DataContext.ERouteContent.AsNoTracking();
            ERouteContents = DynamicFilter(ERouteContents, filter);
            ERouteContents = OrFilter(ERouteContents, filter);
            return await ERouteContents.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(ERouteContentFilter filter)
        {
            IQueryable<ERouteContentDAO> ERouteContents = DataContext.ERouteContent.AsNoTracking();
            ERouteContents = DynamicFilter(ERouteContents, filter);
            return await ERouteContents.CountWithNoLockAsync();
        }

        public async Task<List<ERouteContent>> List(ERouteContentFilter filter)
        {
            string key = $"{nameof(ERouteContentRepository)}.{nameof(List)}.{filter.ToHash()}";
            if (filter == null) return new List<ERouteContent>();
            List<ERouteContent> ERouteContents;
            if (filter.HasCache)
            {
                ERouteContents = await GetFromCache<List<ERouteContent>>(key);
                if (ERouteContents != null)
                    return ERouteContents;
            }
            IQueryable<ERouteContentDAO> ERouteContentDAOs = DataContext.ERouteContent.AsNoTracking();
            ERouteContentDAOs = DynamicFilter(ERouteContentDAOs, filter);
            ERouteContentDAOs = OrFilter(ERouteContentDAOs, filter);
            ERouteContentDAOs = DynamicOrder(ERouteContentDAOs, filter);
            ERouteContents = await DynamicSelect(ERouteContentDAOs, filter);
            if (filter.HasCache)
            {
                await SetToCache(key, ERouteContents);
            }
            return ERouteContents;
        }

        public async Task<ERouteContent> Get(long Id)
        {
            ERouteContent ERouteContent = await DataContext.ERouteContent.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new ERouteContent()
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
                ERoute = x.ERoute == null ? null : new ERoute
                {
                    Id = x.ERoute.Id,
                    Code = x.ERoute.Code,
                    Name = x.ERoute.Name,
                    SaleEmployeeId = x.ERoute.SaleEmployeeId,
                    StartDate = x.ERoute.StartDate,
                    EndDate = x.ERoute.EndDate,
                    StatusId = x.ERoute.StatusId,
                    CreatorId = x.ERoute.CreatorId,
                },
                Store = x.Store == null ? null : new Store
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
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (ERouteContent == null)
                return null;

            return ERouteContent;
        }
    }
}
