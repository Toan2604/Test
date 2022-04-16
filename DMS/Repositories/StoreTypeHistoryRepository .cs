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
    public interface IStoreTypeHistoryRepository
    {
        Task<int> Count(StoreTypeHistoryFilter StoreTypeHistoryFilter);
        Task<List<StoreTypeHistory>> List(StoreTypeHistoryFilter StoreTypeHistoryFilter);
        Task<List<StoreTypeHistory>> List(List<long> Ids);
        Task<bool> Create(StoreTypeHistory StoreTypeHistory);
    }
    public class StoreTypeHistoryRepository : IStoreTypeHistoryRepository
    {
        private DataContext DataContext;
        public StoreTypeHistoryRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<StoreTypeHistoryDAO> DynamicFilter(IQueryable<StoreTypeHistoryDAO> query, StoreTypeHistoryFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.CreatedAt, filter.CreatedAt);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.StoreId, filter.StoreId);
            query = query.Where(q => q.AppUserId, filter.AppUserId);
            query = query.Where(q => q.StoreTypeId, filter.StoreTypeId);
            query = query.Where(q => q.PreviousStoreTypeId.Value, filter.PreviousStoreTypeId);
            return query;
        }


        private IQueryable<StoreTypeHistoryDAO> DynamicOrder(IQueryable<StoreTypeHistoryDAO> query, StoreTypeHistoryFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case StoreTypeHistoryOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case StoreTypeHistoryOrder.Store:
                            query = query.OrderBy(q => q.StoreId);
                            break;
                        case StoreTypeHistoryOrder.AppUser:
                            query = query.OrderBy(q => q.AppUserId);
                            break;
                        case StoreTypeHistoryOrder.StoreType:
                            query = query.OrderBy(q => q.StoreTypeId);
                            break;
                        case StoreTypeHistoryOrder.PreviousStoreType:
                            query = query.OrderBy(q => q.PreviousStoreTypeId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case StoreTypeHistoryOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case StoreTypeHistoryOrder.Store:
                            query = query.OrderByDescending(q => q.StoreId);
                            break;
                        case StoreTypeHistoryOrder.AppUser:
                            query = query.OrderByDescending(q => q.AppUserId);
                            break;
                        case StoreTypeHistoryOrder.StoreType:
                            query = query.OrderByDescending(q => q.StoreTypeId);
                            break;
                        case StoreTypeHistoryOrder.PreviousStoreType:
                            query = query.OrderByDescending(q => q.PreviousStoreTypeId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<StoreTypeHistory>> DynamicSelect(IQueryable<StoreTypeHistoryDAO> query, StoreTypeHistoryFilter filter)
        {
            List<StoreTypeHistory> StoreHistories = await query.Select(q => new StoreTypeHistory()
            {
                Id = filter.Selects.Contains(StoreTypeHistorySelect.Id) ? q.Id : default(long),
                StoreId = filter.Selects.Contains(StoreTypeHistorySelect.Store) ? q.StoreId : default(long),
                AppUserId = filter.Selects.Contains(StoreTypeHistorySelect.AppUser) ? q.AppUserId : default(long),
                StoreTypeId = filter.Selects.Contains(StoreTypeHistorySelect.StoreType) ? q.StoreTypeId : default(long),
                AppUser = filter.Selects.Contains(StoreTypeHistorySelect.AppUser) && q.AppUser != null ? new AppUser
                {
                    Id = q.AppUser.Id,
                    Username = q.AppUser.Username,
                    DisplayName = q.AppUser.DisplayName,
                } : null,
                PreviousStoreType = filter.Selects.Contains(StoreTypeHistorySelect.PreviousStoreType) && q.StoreType != null ? new StoreType
                {
                    Id = q.StoreType.Id,
                    Code = q.StoreType.Code,
                    Name = q.StoreType.Name,
                } : null,
                StoreType = filter.Selects.Contains(StoreTypeHistorySelect.StoreType) && q.StoreType != null ? new StoreType
                {
                    Id = q.StoreType.Id,
                    Code = q.StoreType.Code,
                    Name = q.StoreType.Name,
                } : null,
            }).ToListWithNoLockAsync();
            return StoreHistories;
        }

        public async Task<int> Count(StoreTypeHistoryFilter filter)
        {
            IQueryable<StoreTypeHistoryDAO> StoreHistories = DataContext.StoreTypeHistory.AsNoTracking();
            StoreHistories = DynamicFilter(StoreHistories, filter);
            return await StoreHistories.CountWithNoLockAsync();
        }

        public async Task<List<StoreTypeHistory>> List(StoreTypeHistoryFilter filter)
        {
            if (filter == null) return new List<StoreTypeHistory>();
            IQueryable<StoreTypeHistoryDAO> StoreTypeHistoryDAOs = DataContext.StoreTypeHistory.AsNoTracking();
            StoreTypeHistoryDAOs = DynamicFilter(StoreTypeHistoryDAOs, filter);
            StoreTypeHistoryDAOs = DynamicOrder(StoreTypeHistoryDAOs, filter);
            List<StoreTypeHistory> StoreHistories = await DynamicSelect(StoreTypeHistoryDAOs, filter);
            return StoreHistories;
        }

        public async Task<List<StoreTypeHistory>> List(List<long> Ids)
        {
            ITempTableQuery<TempTable<long>> tempTableQuery = await DataContext
                  .BulkInsertValuesIntoTempTableAsync<long>(Ids);

            var query = from s in DataContext.StoreTypeHistory
                        join tt in tempTableQuery.Query on s.Id equals tt.Column1
                        select s;

            List<StoreTypeHistory> StoreTypeHistories = await query.AsNoTracking()
                .Select(x => new StoreTypeHistory
                {
                    Id = x.Id,
                    StoreId = x.StoreId,
                    AppUserId = x.AppUserId,
                    StoreTypeId = x.StoreTypeId,
                    PreviousStoreTypeId = x.PreviousStoreTypeId,
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
                    StoreType = x.StoreType == null ? null : new StoreType
                    {
                        Id = x.StoreType.Id,
                        Code = x.StoreType.Code,
                        Name = x.StoreType.Name,
                    },
                    PreviousStoreType = x.PreviousStoreType == null ? null : new StoreType
                    {
                        Id = x.PreviousStoreType.Id,
                        Code = x.PreviousStoreType.Code,
                        Name = x.PreviousStoreType.Name,
                    },
                }).ToListWithNoLockAsync();
            return StoreTypeHistories;
        }


        public async Task<bool> Create(StoreTypeHistory StoreTypeHistory)
        {
            StoreTypeHistoryDAO Old = await DataContext.StoreTypeHistory
                .Where(x => x.StoreId == StoreTypeHistory.StoreId)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultWithNoLockAsync();
            StoreTypeHistoryDAO StoreTypeHistoryDAO = new StoreTypeHistoryDAO();
            StoreTypeHistoryDAO.Id = StoreTypeHistory.Id;
            StoreTypeHistoryDAO.StoreId = StoreTypeHistory.StoreId;
            StoreTypeHistoryDAO.AppUserId = StoreTypeHistory.AppUserId;
            StoreTypeHistoryDAO.PreviousStoreTypeId = StoreTypeHistory.PreviousStoreTypeId;
            StoreTypeHistoryDAO.StoreTypeId = StoreTypeHistory.StoreTypeId;
            StoreTypeHistoryDAO.PreviousCreatedAt = Old?.CreatedAt;
            StoreTypeHistoryDAO.CreatedAt = StaticParams.DateTimeNow;
            DataContext.StoreTypeHistory.Add(StoreTypeHistoryDAO);
            await DataContext.SaveChangesAsync();
            StoreTypeHistory.Id = StoreTypeHistoryDAO.Id;
            return true;
        }

    }
}
