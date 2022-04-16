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
    public interface IERouteChangeRequestContentRepository
    {
        Task<int> Count(ERouteChangeRequestContentFilter ERouteChangeRequestContentFilter);
        Task<int> CountAll(ERouteChangeRequestContentFilter ERouteChangeRequestContentFilter);
        Task<List<ERouteChangeRequestContent>> List(ERouteChangeRequestContentFilter ERouteChangeRequestContentFilter);
        Task<ERouteChangeRequestContent> Get(long Id);
        Task<bool> Create(ERouteChangeRequestContent ERouteChangeRequestContent);
        Task<bool> Update(ERouteChangeRequestContent ERouteChangeRequestContent);
        Task<bool> Delete(ERouteChangeRequestContent ERouteChangeRequestContent);
        Task<bool> BulkMerge(List<ERouteChangeRequestContent> ERouteChangeRequestContents);
        Task<bool> BulkDelete(List<ERouteChangeRequestContent> ERouteChangeRequestContents);
    }
    public class ERouteChangeRequestContentRepository : IERouteChangeRequestContentRepository
    {
        private DataContext DataContext;
        public ERouteChangeRequestContentRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<ERouteChangeRequestContentDAO> DynamicFilter(IQueryable<ERouteChangeRequestContentDAO> query, ERouteChangeRequestContentFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.CreatedAt, filter.CreatedAt);
            query = query.Where(q => q.UpdatedAt, filter.UpdatedAt);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.ERouteChangeRequestId, filter.ERouteChangeRequestId);
            query = query.Where(q => q.StoreId, filter.StoreId);
            query = query.Where(q => q.OrderNumber, filter.OrderNumber);
            return query;
        }

        private IQueryable<ERouteChangeRequestContentDAO> OrFilter(IQueryable<ERouteChangeRequestContentDAO> query, ERouteChangeRequestContentFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<ERouteChangeRequestContentDAO> initQuery = query.Where(q => false);
            foreach (ERouteChangeRequestContentFilter ERouteChangeRequestContentFilter in filter.OrFilter)
            {
                IQueryable<ERouteChangeRequestContentDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, ERouteChangeRequestContentFilter.Id);
                queryable = queryable.Where(q => q.ERouteChangeRequestId, ERouteChangeRequestContentFilter.ERouteChangeRequestId);
                queryable = queryable.Where(q => q.StoreId, ERouteChangeRequestContentFilter.StoreId);
                queryable = queryable.Where(q => q.OrderNumber, ERouteChangeRequestContentFilter.OrderNumber);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<ERouteChangeRequestContentDAO> DynamicOrder(IQueryable<ERouteChangeRequestContentDAO> query, ERouteChangeRequestContentFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case ERouteChangeRequestContentOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case ERouteChangeRequestContentOrder.ERouteChangeRequest:
                            query = query.OrderBy(q => q.ERouteChangeRequestId);
                            break;
                        case ERouteChangeRequestContentOrder.Store:
                            query = query.OrderBy(q => q.StoreId);
                            break;
                        case ERouteChangeRequestContentOrder.OrderNumber:
                            query = query.OrderBy(q => q.OrderNumber);
                            break;
                        case ERouteChangeRequestContentOrder.Monday:
                            query = query.OrderBy(q => q.Monday);
                            break;
                        case ERouteChangeRequestContentOrder.Tuesday:
                            query = query.OrderBy(q => q.Tuesday);
                            break;
                        case ERouteChangeRequestContentOrder.Wednesday:
                            query = query.OrderBy(q => q.Wednesday);
                            break;
                        case ERouteChangeRequestContentOrder.Thursday:
                            query = query.OrderBy(q => q.Thursday);
                            break;
                        case ERouteChangeRequestContentOrder.Friday:
                            query = query.OrderBy(q => q.Friday);
                            break;
                        case ERouteChangeRequestContentOrder.Saturday:
                            query = query.OrderBy(q => q.Saturday);
                            break;
                        case ERouteChangeRequestContentOrder.Sunday:
                            query = query.OrderBy(q => q.Sunday);
                            break;
                        case ERouteChangeRequestContentOrder.Week1:
                            query = query.OrderBy(q => q.Week1);
                            break;
                        case ERouteChangeRequestContentOrder.Week2:
                            query = query.OrderBy(q => q.Week2);
                            break;
                        case ERouteChangeRequestContentOrder.Week3:
                            query = query.OrderBy(q => q.Week3);
                            break;
                        case ERouteChangeRequestContentOrder.Week4:
                            query = query.OrderBy(q => q.Week4);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case ERouteChangeRequestContentOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case ERouteChangeRequestContentOrder.ERouteChangeRequest:
                            query = query.OrderByDescending(q => q.ERouteChangeRequestId);
                            break;
                        case ERouteChangeRequestContentOrder.Store:
                            query = query.OrderByDescending(q => q.StoreId);
                            break;
                        case ERouteChangeRequestContentOrder.OrderNumber:
                            query = query.OrderByDescending(q => q.OrderNumber);
                            break;
                        case ERouteChangeRequestContentOrder.Monday:
                            query = query.OrderByDescending(q => q.Monday);
                            break;
                        case ERouteChangeRequestContentOrder.Tuesday:
                            query = query.OrderByDescending(q => q.Tuesday);
                            break;
                        case ERouteChangeRequestContentOrder.Wednesday:
                            query = query.OrderByDescending(q => q.Wednesday);
                            break;
                        case ERouteChangeRequestContentOrder.Thursday:
                            query = query.OrderByDescending(q => q.Thursday);
                            break;
                        case ERouteChangeRequestContentOrder.Friday:
                            query = query.OrderByDescending(q => q.Friday);
                            break;
                        case ERouteChangeRequestContentOrder.Saturday:
                            query = query.OrderByDescending(q => q.Saturday);
                            break;
                        case ERouteChangeRequestContentOrder.Sunday:
                            query = query.OrderByDescending(q => q.Sunday);
                            break;
                        case ERouteChangeRequestContentOrder.Week1:
                            query = query.OrderByDescending(q => q.Week1);
                            break;
                        case ERouteChangeRequestContentOrder.Week2:
                            query = query.OrderByDescending(q => q.Week2);
                            break;
                        case ERouteChangeRequestContentOrder.Week3:
                            query = query.OrderByDescending(q => q.Week3);
                            break;
                        case ERouteChangeRequestContentOrder.Week4:
                            query = query.OrderByDescending(q => q.Week4);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<ERouteChangeRequestContent>> DynamicSelect(IQueryable<ERouteChangeRequestContentDAO> query, ERouteChangeRequestContentFilter filter)
        {
            List<ERouteChangeRequestContent> ERouteChangeRequestContents = await query.Select(q => new ERouteChangeRequestContent()
            {
                Id = filter.Selects.Contains(ERouteChangeRequestContentSelect.Id) ? q.Id : default(long),
                ERouteChangeRequestId = filter.Selects.Contains(ERouteChangeRequestContentSelect.ERouteChangeRequest) ? q.ERouteChangeRequestId : default(long),
                StoreId = filter.Selects.Contains(ERouteChangeRequestContentSelect.Store) ? q.StoreId : default(long),
                OrderNumber = filter.Selects.Contains(ERouteChangeRequestContentSelect.OrderNumber) ? q.OrderNumber : default(long?),
                Monday = filter.Selects.Contains(ERouteChangeRequestContentSelect.Monday) ? q.Monday : default(bool),
                Tuesday = filter.Selects.Contains(ERouteChangeRequestContentSelect.Tuesday) ? q.Tuesday : default(bool),
                Wednesday = filter.Selects.Contains(ERouteChangeRequestContentSelect.Wednesday) ? q.Wednesday : default(bool),
                Thursday = filter.Selects.Contains(ERouteChangeRequestContentSelect.Thursday) ? q.Thursday : default(bool),
                Friday = filter.Selects.Contains(ERouteChangeRequestContentSelect.Friday) ? q.Friday : default(bool),
                Saturday = filter.Selects.Contains(ERouteChangeRequestContentSelect.Saturday) ? q.Saturday : default(bool),
                Sunday = filter.Selects.Contains(ERouteChangeRequestContentSelect.Sunday) ? q.Sunday : default(bool),
                Week1 = filter.Selects.Contains(ERouteChangeRequestContentSelect.Week1) ? q.Week1 : default(bool),
                Week2 = filter.Selects.Contains(ERouteChangeRequestContentSelect.Week2) ? q.Week2 : default(bool),
                Week3 = filter.Selects.Contains(ERouteChangeRequestContentSelect.Week3) ? q.Week3 : default(bool),
                Week4 = filter.Selects.Contains(ERouteChangeRequestContentSelect.Week4) ? q.Week4 : default(bool),
                ERouteChangeRequest = filter.Selects.Contains(ERouteChangeRequestContentSelect.ERouteChangeRequest) && q.ERouteChangeRequest != null ? new ERouteChangeRequest
                {
                    Id = q.ERouteChangeRequest.Id,
                    ERouteId = q.ERouteChangeRequest.ERouteId,
                    CreatorId = q.ERouteChangeRequest.CreatorId,
                    RequestStateId = q.ERouteChangeRequest.RequestStateId,
                } : null,
                Store = filter.Selects.Contains(ERouteChangeRequestContentSelect.Store) && q.Store != null ? new Store
                {
                    Id = q.Store.Id,
                    Code = q.Store.Code,
                    CodeDraft = q.Store.CodeDraft,
                    Name = q.Store.Name,
                    ParentStoreId = q.Store.ParentStoreId,
                    OrganizationId = q.Store.OrganizationId,
                    StoreTypeId = q.Store.StoreTypeId,
                    Telephone = q.Store.Telephone,
                    ProvinceId = q.Store.ProvinceId,
                    DistrictId = q.Store.DistrictId,
                    WardId = q.Store.WardId,
                    Address = q.Store.Address,
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
                    StatusId = q.Store.StatusId,
                } : null,
                CreatedAt = q.CreatedAt,
                UpdatedAt = q.UpdatedAt,
            }).ToListWithNoLockAsync();
            return ERouteChangeRequestContents;
        }

        public async Task<int> Count(ERouteChangeRequestContentFilter filter)
        {
            IQueryable<ERouteChangeRequestContentDAO> ERouteChangeRequestContents = DataContext.ERouteChangeRequestContent.AsNoTracking();
            ERouteChangeRequestContents = DynamicFilter(ERouteChangeRequestContents, filter);
            ERouteChangeRequestContents = OrFilter(ERouteChangeRequestContents, filter);
            return await ERouteChangeRequestContents.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(ERouteChangeRequestContentFilter filter)
        {
            IQueryable<ERouteChangeRequestContentDAO> ERouteChangeRequestContents = DataContext.ERouteChangeRequestContent.AsNoTracking();
            ERouteChangeRequestContents = DynamicFilter(ERouteChangeRequestContents, filter);
            return await ERouteChangeRequestContents.CountWithNoLockAsync();
        }

        public async Task<List<ERouteChangeRequestContent>> List(ERouteChangeRequestContentFilter filter)
        {
            if (filter == null) return new List<ERouteChangeRequestContent>();
            IQueryable<ERouteChangeRequestContentDAO> ERouteChangeRequestContentDAOs = DataContext.ERouteChangeRequestContent.AsNoTracking();
            ERouteChangeRequestContentDAOs = DynamicFilter(ERouteChangeRequestContentDAOs, filter);
            ERouteChangeRequestContentDAOs = OrFilter(ERouteChangeRequestContentDAOs, filter);
            ERouteChangeRequestContentDAOs = DynamicOrder(ERouteChangeRequestContentDAOs, filter);
            List<ERouteChangeRequestContent> ERouteChangeRequestContents = await DynamicSelect(ERouteChangeRequestContentDAOs, filter);
            return ERouteChangeRequestContents;
        }

        public async Task<ERouteChangeRequestContent> Get(long Id)
        {
            ERouteChangeRequestContent ERouteChangeRequestContent = await DataContext.ERouteChangeRequestContent.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new ERouteChangeRequestContent()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
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
                ERouteChangeRequest = x.ERouteChangeRequest == null ? null : new ERouteChangeRequest
                {
                    Id = x.ERouteChangeRequest.Id,
                    ERouteId = x.ERouteChangeRequest.ERouteId,
                    CreatorId = x.ERouteChangeRequest.CreatorId,
                    RequestStateId = x.ERouteChangeRequest.RequestStateId,
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

            if (ERouteChangeRequestContent == null)
                return null;

            return ERouteChangeRequestContent;
        }
        public async Task<bool> Create(ERouteChangeRequestContent ERouteChangeRequestContent)
        {
            ERouteChangeRequestContentDAO ERouteChangeRequestContentDAO = new ERouteChangeRequestContentDAO();
            ERouteChangeRequestContentDAO.Id = ERouteChangeRequestContent.Id;
            ERouteChangeRequestContentDAO.ERouteChangeRequestId = ERouteChangeRequestContent.ERouteChangeRequestId;
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
            ERouteChangeRequestContentDAO.CreatedAt = StaticParams.DateTimeNow;
            ERouteChangeRequestContentDAO.UpdatedAt = StaticParams.DateTimeNow;
            DataContext.ERouteChangeRequestContent.Add(ERouteChangeRequestContentDAO);
            await DataContext.SaveChangesAsync();
            ERouteChangeRequestContent.Id = ERouteChangeRequestContentDAO.Id;
            await SaveReference(ERouteChangeRequestContent);
            return true;
        }

        public async Task<bool> Update(ERouteChangeRequestContent ERouteChangeRequestContent)
        {
            ERouteChangeRequestContentDAO ERouteChangeRequestContentDAO = DataContext.ERouteChangeRequestContent.Where(x => x.Id == ERouteChangeRequestContent.Id).FirstOrDefault();
            if (ERouteChangeRequestContentDAO == null)
                return false;
            ERouteChangeRequestContentDAO.Id = ERouteChangeRequestContent.Id;
            ERouteChangeRequestContentDAO.ERouteChangeRequestId = ERouteChangeRequestContent.ERouteChangeRequestId;
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
            await DataContext.SaveChangesAsync();
            await SaveReference(ERouteChangeRequestContent);
            return true;
        }

        public async Task<bool> Delete(ERouteChangeRequestContent ERouteChangeRequestContent)
        {
            await DataContext.ERouteChangeRequestContent.Where(x => x.Id == ERouteChangeRequestContent.Id).UpdateFromQueryAsync(x => new ERouteChangeRequestContentDAO { DeletedAt = StaticParams.DateTimeNow });
            return true;
        }

        public async Task<bool> BulkMerge(List<ERouteChangeRequestContent> ERouteChangeRequestContents)
        {
            List<ERouteChangeRequestContentDAO> ERouteChangeRequestContentDAOs = new List<ERouteChangeRequestContentDAO>();
            foreach (ERouteChangeRequestContent ERouteChangeRequestContent in ERouteChangeRequestContents)
            {
                ERouteChangeRequestContentDAO ERouteChangeRequestContentDAO = new ERouteChangeRequestContentDAO();
                ERouteChangeRequestContentDAO.Id = ERouteChangeRequestContent.Id;
                ERouteChangeRequestContentDAO.ERouteChangeRequestId = ERouteChangeRequestContent.ERouteChangeRequestId;
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
                ERouteChangeRequestContentDAO.CreatedAt = StaticParams.DateTimeNow;
                ERouteChangeRequestContentDAO.UpdatedAt = StaticParams.DateTimeNow;
                ERouteChangeRequestContentDAOs.Add(ERouteChangeRequestContentDAO);
            }
            await DataContext.BulkMergeAsync(ERouteChangeRequestContentDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<ERouteChangeRequestContent> ERouteChangeRequestContents)
        {
            List<long> Ids = ERouteChangeRequestContents.Select(x => x.Id).ToList();
            await DataContext.ERouteChangeRequestContent
                .Where(x => Ids.Contains(x.Id))
                .UpdateFromQueryAsync(x => new ERouteChangeRequestContentDAO { DeletedAt = StaticParams.DateTimeNow });
            return true;
        }

        private async Task SaveReference(ERouteChangeRequestContent ERouteChangeRequestContent)
        {
        }

    }
}
