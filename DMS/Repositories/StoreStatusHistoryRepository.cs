using DMS.Entities;
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
    public interface IStoreStatusHistoryRepository
    {
        Task<int> Count(StoreStatusHistoryFilter StoreStatusHistoryFilter);
        Task<List<StoreStatusHistory>> List(StoreStatusHistoryFilter StoreStatusHistoryFilter);
        Task<List<StoreStatusHistory>> List(List<long> Ids);
        Task<bool> Create(StoreStatusHistory StoreStatusHistory);
        Task<bool> BulkMerge(List<StoreStatusHistory> StoreStatusHistories);
    }
    public class StoreStatusHistoryRepository : IStoreStatusHistoryRepository
    {
        private DataContext DataContext;
        public StoreStatusHistoryRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<StoreStatusHistoryDAO> DynamicFilter(IQueryable<StoreStatusHistoryDAO> query, StoreStatusHistoryFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.CreatedAt, filter.CreatedAt);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.StoreId, filter.StoreId);
            query = query.Where(q => q.AppUserId, filter.AppUserId);
            query = query.Where(q => q.StoreStatusId, filter.StoreStatusId);
            query = query.Where(q => q.PreviousStoreStatusId.Value, filter.PreviousStoreStatusId);
            return query;
        }


        private IQueryable<StoreStatusHistoryDAO> DynamicOrder(IQueryable<StoreStatusHistoryDAO> query, StoreStatusHistoryFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case StoreStatusHistoryOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case StoreStatusHistoryOrder.Store:
                            query = query.OrderBy(q => q.StoreId);
                            break;
                        case StoreStatusHistoryOrder.AppUser:
                            query = query.OrderBy(q => q.AppUserId);
                            break;
                        case StoreStatusHistoryOrder.StoreStatus:
                            query = query.OrderBy(q => q.StoreStatusId);
                            break;
                        case StoreStatusHistoryOrder.PreviousStoreStatus:
                            query = query.OrderBy(q => q.PreviousStoreStatusId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case StoreStatusHistoryOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case StoreStatusHistoryOrder.Store:
                            query = query.OrderByDescending(q => q.StoreId);
                            break;
                        case StoreStatusHistoryOrder.AppUser:
                            query = query.OrderByDescending(q => q.AppUserId);
                            break;
                        case StoreStatusHistoryOrder.StoreStatus:
                            query = query.OrderByDescending(q => q.StoreStatusId);
                            break;
                        case StoreStatusHistoryOrder.PreviousStoreStatus:
                            query = query.OrderByDescending(q => q.PreviousStoreStatusId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<StoreStatusHistory>> DynamicSelect(IQueryable<StoreStatusHistoryDAO> query, StoreStatusHistoryFilter filter)
        {
            List<StoreStatusHistory> StoreHistories = await query.Select(q => new StoreStatusHistory()
            {
                Id = filter.Selects.Contains(StoreStatusHistorySelect.Id) ? q.Id : default(long),
                StoreId = filter.Selects.Contains(StoreStatusHistorySelect.Store) ? q.StoreId : default(long),
                AppUserId = filter.Selects.Contains(StoreStatusHistorySelect.AppUser) ? q.AppUserId : default(long),
                StoreStatusId = filter.Selects.Contains(StoreStatusHistorySelect.StoreStatus) ? q.StoreStatusId : default(long),
                AppUser = filter.Selects.Contains(StoreStatusHistorySelect.AppUser) && q.AppUser != null ? new AppUser
                {
                    Id = q.AppUser.Id,
                    Username = q.AppUser.Username,
                    DisplayName = q.AppUser.DisplayName,
                } : null,
                PreviousStoreStatus = filter.Selects.Contains(StoreStatusHistorySelect.PreviousStoreStatus) && q.StoreStatus != null ? new StoreStatus
                {
                    Id = q.StoreStatus.Id,
                    Code = q.StoreStatus.Code,
                    Name = q.StoreStatus.Name,
                } : null,
                StoreStatus = filter.Selects.Contains(StoreStatusHistorySelect.StoreStatus) && q.StoreStatus != null ? new StoreStatus
                {
                    Id = q.StoreStatus.Id,
                    Code = q.StoreStatus.Code,
                    Name = q.StoreStatus.Name,
                } : null,
            }).ToListWithNoLockAsync();
            return StoreHistories;
        }

        public async Task<int> Count(StoreStatusHistoryFilter filter)
        {
            IQueryable<StoreStatusHistoryDAO> StoreHistories = DataContext.StoreStatusHistory.AsNoTracking();
            StoreHistories = DynamicFilter(StoreHistories, filter);
            return await StoreHistories.CountWithNoLockAsync();
        }

        public async Task<List<StoreStatusHistory>> List(StoreStatusHistoryFilter filter)
        {
            if (filter == null) return new List<StoreStatusHistory>();
            IQueryable<StoreStatusHistoryDAO> StoreStatusHistoryDAOs = DataContext.StoreStatusHistory.AsNoTracking();
            StoreStatusHistoryDAOs = DynamicFilter(StoreStatusHistoryDAOs, filter);
            StoreStatusHistoryDAOs = DynamicOrder(StoreStatusHistoryDAOs, filter);
            List<StoreStatusHistory> StoreHistories = await DynamicSelect(StoreStatusHistoryDAOs, filter);
            return StoreHistories;
        }

        public async Task<List<StoreStatusHistory>> List(List<long> Ids)
        {
            ITempTableQuery<TempTable<long>> tempTableQuery = await DataContext
                  .BulkInsertValuesIntoTempTableAsync<long>(Ids);

            var query = from s in DataContext.StoreStatusHistory
                        join tt in tempTableQuery.Query on s.Id equals tt.Column1
                        select s;

            List<StoreStatusHistory> StoreStatusHistories = await query.AsNoTracking()
                .Select(x => new StoreStatusHistory
                {
                    Id = x.Id,
                    StoreId = x.StoreId,
                    AppUserId = x.AppUserId,
                    StoreStatusId = x.StoreStatusId,
                    PreviousStoreStatusId = x.PreviousStoreStatusId,
                    CreatedAt = x.CreatedAt,
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
                        OwnerName = x.Store.OwnerName,
                        OwnerPhone = x.Store.OwnerPhone,
                        OwnerEmail = x.Store.OwnerEmail,
                        TaxCode = x.Store.TaxCode,
                        LegalEntity = x.Store.LegalEntity,
                        StatusId = x.Store.StatusId,
                    },
                    AppUser = x.AppUser == null ? null : new AppUser
                    {
                        Id = x.AppUser.Id,
                        Username = x.AppUser.Username,
                        DisplayName = x.AppUser.DisplayName,
                        Email = x.AppUser.Email,
                        Phone = x.AppUser.Phone,
                        Address = x.AppUser.Address,
                        Department = x.AppUser.Department,
                        PositionId = x.AppUser.PositionId,
                        RowId = x.AppUser.RowId,
                        SexId = x.AppUser.SexId,
                        StatusId = x.AppUser.StatusId,
                        OrganizationId = x.AppUser.OrganizationId,
                        Organization = x.AppUser.Organization == null ? null : new Organization
                        {
                            Id = x.AppUser.Organization.Id,
                            Code = x.AppUser.Organization.Code,
                            Name = x.AppUser.Organization.Name,
                        },
                    },
                    StoreStatus = x.StoreStatus == null ? null : new StoreStatus
                    {
                        Id = x.StoreStatus.Id,
                        Code = x.StoreStatus.Code,
                        Name = x.StoreStatus.Name,
                    },
                    PreviousStoreStatus = x.PreviousStoreStatus == null ? null : new StoreStatus
                    {
                        Id = x.PreviousStoreStatus.Id,
                        Code = x.PreviousStoreStatus.Code,
                        Name = x.PreviousStoreStatus.Name,
                    },
                }).ToListWithNoLockAsync();
            return StoreStatusHistories;
        }

        public async Task<bool> Create(StoreStatusHistory StoreStatusHistory)
        {
            StoreStatusHistoryDAO Old = await DataContext.StoreStatusHistory
                .Where(x => x.StoreId == StoreStatusHistory.StoreId)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultWithNoLockAsync();
            StoreStatusHistoryDAO StoreStatusHistoryDAO = new StoreStatusHistoryDAO();
            StoreStatusHistoryDAO.Id = StoreStatusHistory.Id;
            StoreStatusHistoryDAO.StoreId = StoreStatusHistory.StoreId;
            StoreStatusHistoryDAO.AppUserId = StoreStatusHistory.AppUserId;
            StoreStatusHistoryDAO.PreviousStoreStatusId = StoreStatusHistory.PreviousStoreStatusId;
            StoreStatusHistoryDAO.StoreStatusId = StoreStatusHistory.StoreStatusId;
            StoreStatusHistoryDAO.PreviousCreatedAt = Old?.CreatedAt;
            StoreStatusHistoryDAO.CreatedAt = StaticParams.DateTimeNow;
            DataContext.StoreStatusHistory.Add(StoreStatusHistoryDAO);
            await DataContext.SaveChangesAsync();
            StoreStatusHistory.Id = StoreStatusHistoryDAO.Id;
            return true;
        }
        public async Task<bool> BulkMerge(List<StoreStatusHistory> StoreStatusHistories)
        {
            List<StoreStatusHistoryDAO> StoreStatusHistoryDAOs = new List<StoreStatusHistoryDAO>();

            foreach (StoreStatusHistory StoreStatusHistory in StoreStatusHistories)
            {
                StoreStatusHistoryDAO StoreStatusHistoryDAO = new StoreStatusHistoryDAO
                {
                    Id = StoreStatusHistory.Id,
                    StoreId = StoreStatusHistory.StoreId,
                    StoreStatusId = StoreStatusHistory.StoreStatusId,
                    PreviousStoreStatusId = StoreStatusHistory.PreviousStoreStatusId,
                    PreviousCreatedAt = StoreStatusHistory.PreviousCreatedAt,
                    AppUserId = StoreStatusHistory.AppUserId,
                    CreatedAt = StoreStatusHistory.CreatedAt,
                };
                StoreStatusHistoryDAOs.Add(StoreStatusHistoryDAO);
            }
            await DataContext.BulkMergeAsync(StoreStatusHistoryDAOs);
            return true;
        }
    }
}
