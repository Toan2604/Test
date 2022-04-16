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
    public interface IRewardHistoryRepository
    {
        Task<int> Count(RewardHistoryFilter RewardHistoryFilter);
        Task<int> CountAll(RewardHistoryFilter RewardHistoryFilter);
        Task<List<RewardHistory>> List(RewardHistoryFilter RewardHistoryFilter);
        Task<RewardHistory> Get(long Id);
        Task<bool> Create(RewardHistory RewardHistory);
        Task<bool> Update(RewardHistory RewardHistory);
        Task<bool> Delete(RewardHistory RewardHistory);
        Task<bool> BulkMerge(List<RewardHistory> RewardHistories);
        Task<bool> BulkDelete(List<RewardHistory> RewardHistories);
    }
    public class RewardHistoryRepository : IRewardHistoryRepository
    {
        private DataContext DataContext;
        public RewardHistoryRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<RewardHistoryDAO> DynamicFilter(IQueryable<RewardHistoryDAO> query, RewardHistoryFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.CreatedAt, filter.CreatedAt);
            query = query.Where(q => q.UpdatedAt, filter.UpdatedAt);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.AppUserId, filter.AppUserId);
            query = query.Where(q => q.StoreId, filter.StoreId);
            query = query.Where(q => q.TurnCounter, filter.TurnCounter);
            query = query.Where(q => q.Revenue, filter.Revenue);
            query = query.Where(q => q.RowId, filter.RowId);
            if (!string.IsNullOrWhiteSpace(filter.Search))
                query = query.Where(q =>
                q.Store.CodeDraft.ToLower().Contains(filter.Search.ToLower()) ||
                q.Store.Code.ToLower().Contains(filter.Search.ToLower()) ||
                q.Store.Address.ToLower().Contains(filter.Search.ToLower()) ||
                q.Store.UnsignAddress.ToLower().Contains(filter.Search.ToLower()) ||
                q.Store.UnsignName.ToLower().Contains(filter.Search.ToLower()) ||
                q.Store.Name.ToLower().Contains(filter.Search.ToLower()));
            return query;
        }

        private IQueryable<RewardHistoryDAO> OrFilter(IQueryable<RewardHistoryDAO> query, RewardHistoryFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<RewardHistoryDAO> initQuery = query.Where(q => false);
            foreach (RewardHistoryFilter RewardHistoryFilter in filter.OrFilter)
            {
                IQueryable<RewardHistoryDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, RewardHistoryFilter.Id);
                queryable = queryable.Where(q => q.AppUserId, RewardHistoryFilter.AppUserId);
                queryable = queryable.Where(q => q.StoreId, RewardHistoryFilter.StoreId);
                queryable = queryable.Where(q => q.TurnCounter, RewardHistoryFilter.TurnCounter);
                queryable = queryable.Where(q => q.RowId, RewardHistoryFilter.RowId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<RewardHistoryDAO> DynamicOrder(IQueryable<RewardHistoryDAO> query, RewardHistoryFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case RewardHistoryOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case RewardHistoryOrder.AppUser:
                            query = query.OrderBy(q => q.AppUserId);
                            break;
                        case RewardHistoryOrder.Store:
                            query = query.OrderBy(q => q.StoreId);
                            break;
                        case RewardHistoryOrder.TurnCounter:
                            query = query.OrderBy(q => q.TurnCounter);
                            break;
                        case RewardHistoryOrder.Revenue:
                            query = query.OrderBy(q => q.Revenue);
                            break;
                        case RewardHistoryOrder.Row:
                            query = query.OrderBy(q => q.RowId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case RewardHistoryOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case RewardHistoryOrder.AppUser:
                            query = query.OrderByDescending(q => q.AppUserId);
                            break;
                        case RewardHistoryOrder.Store:
                            query = query.OrderByDescending(q => q.StoreId);
                            break;
                        case RewardHistoryOrder.TurnCounter:
                            query = query.OrderByDescending(q => q.TurnCounter);
                            break;
                        case RewardHistoryOrder.Revenue:
                            query = query.OrderByDescending(q => q.Revenue);
                            break;
                        case RewardHistoryOrder.Row:
                            query = query.OrderByDescending(q => q.RowId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<RewardHistory>> DynamicSelect(IQueryable<RewardHistoryDAO> query, RewardHistoryFilter filter)
        {
            List<RewardHistory> RewardHistories = await query.Select(q => new RewardHistory()
            {
                Id = filter.Selects.Contains(RewardHistorySelect.Id) ? q.Id : default(long),
                AppUserId = filter.Selects.Contains(RewardHistorySelect.AppUser) ? q.AppUserId : default(long),
                StoreId = filter.Selects.Contains(RewardHistorySelect.Store) ? q.StoreId : default(long),
                TurnCounter = filter.Selects.Contains(RewardHistorySelect.TurnCounter) ? q.TurnCounter : default(long),
                Revenue = filter.Selects.Contains(RewardHistorySelect.Revenue) ? q.Revenue : default(decimal),
                RowId = filter.Selects.Contains(RewardHistorySelect.Row) ? q.RowId : default(Guid),
                AppUser = filter.Selects.Contains(RewardHistorySelect.AppUser) && q.AppUser != null ? new AppUser
                {
                    Id = q.AppUser.Id,
                    Username = q.AppUser.Username,
                    DisplayName = q.AppUser.DisplayName,
                    Address = q.AppUser.Address,
                    Email = q.AppUser.Email,
                    Phone = q.AppUser.Phone,
                    SexId = q.AppUser.SexId,
                    Birthday = q.AppUser.Birthday,
                    Avatar = q.AppUser.Avatar,
                    PositionId = q.AppUser.PositionId,
                    Department = q.AppUser.Department,
                    OrganizationId = q.AppUser.OrganizationId,
                    ProvinceId = q.AppUser.ProvinceId,
                    StatusId = q.AppUser.StatusId,
                    RowId = q.AppUser.RowId,
                } : null,
                Store = filter.Selects.Contains(RewardHistorySelect.Store) && q.Store != null ? new Store
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
                    AppUserId = q.Store.AppUserId,
                    StatusId = q.Store.StatusId,
                    RowId = q.Store.RowId,
                    Used = q.Store.Used,
                    StoreScoutingId = q.Store.StoreScoutingId,
                    StoreStatusId = q.Store.StoreStatusId,
                    Province = q.Store.Province == null ? null : new Province
                    {
                        Id = q.Store.Province.Id,
                        Code = q.Store.Province.Code,
                        Name = q.Store.Province.Name,
                    },
                    District = q.Store.District == null ? null : new District
                    {
                        Id = q.Store.District.Id,
                        Code = q.Store.District.Code,
                        Name = q.Store.District.Name,
                    },
                    Ward = q.Store.Ward == null ? null : new Ward
                    {
                        Id = q.Store.Ward.Id,
                        Code = q.Store.Ward.Code,
                        Name = q.Store.Ward.Name,
                    }
                } : null,
                CreatedAt = q.CreatedAt,
                UpdatedAt = q.UpdatedAt,
            }).ToListWithNoLockAsync();
            return RewardHistories;
        }

        public async Task<int> Count(RewardHistoryFilter filter)
        {
            IQueryable<RewardHistoryDAO> RewardHistories = DataContext.RewardHistory.AsNoTracking();
            RewardHistories = DynamicFilter(RewardHistories, filter);
            RewardHistories = OrFilter(RewardHistories, filter);
            return await RewardHistories.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(RewardHistoryFilter filter)
        {
            IQueryable<RewardHistoryDAO> RewardHistories = DataContext.RewardHistory.AsNoTracking();
            RewardHistories = DynamicFilter(RewardHistories, filter);
            return await RewardHistories.CountWithNoLockAsync();
        }

        public async Task<List<RewardHistory>> List(RewardHistoryFilter filter)
        {
            if (filter == null) return new List<RewardHistory>();
            IQueryable<RewardHistoryDAO> RewardHistoryDAOs = DataContext.RewardHistory.AsNoTracking();
            RewardHistoryDAOs = DynamicFilter(RewardHistoryDAOs, filter);
            RewardHistoryDAOs = OrFilter(RewardHistoryDAOs, filter);
            RewardHistoryDAOs = DynamicOrder(RewardHistoryDAOs, filter);
            List<RewardHistory> RewardHistories = await DynamicSelect(RewardHistoryDAOs, filter);
            return RewardHistories;
        }

        public async Task<RewardHistory> Get(long Id)
        {
            RewardHistory RewardHistory = await DataContext.RewardHistory.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new RewardHistory()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Id = x.Id,
                AppUserId = x.AppUserId,
                StoreId = x.StoreId,
                TurnCounter = x.TurnCounter,
                Revenue = x.Revenue,
                RowId = x.RowId,
                AppUser = x.AppUser == null ? null : new AppUser
                {
                    Id = x.AppUser.Id,
                    Username = x.AppUser.Username,
                    DisplayName = x.AppUser.DisplayName,
                    Address = x.AppUser.Address,
                    Email = x.AppUser.Email,
                    Phone = x.AppUser.Phone,
                    SexId = x.AppUser.SexId,
                    Birthday = x.AppUser.Birthday,
                    Avatar = x.AppUser.Avatar,
                    PositionId = x.AppUser.PositionId,
                    Department = x.AppUser.Department,
                    OrganizationId = x.AppUser.OrganizationId,
                    ProvinceId = x.AppUser.ProvinceId,
                    StatusId = x.AppUser.StatusId,
                    RowId = x.AppUser.RowId,
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
                    AppUserId = x.Store.AppUserId,
                    StatusId = x.Store.StatusId,
                    RowId = x.Store.RowId,
                    Used = x.Store.Used,
                    StoreScoutingId = x.Store.StoreScoutingId,
                    StoreStatusId = x.Store.StoreStatusId,
                    Province = x.Store.Province == null ? null : new Province
                    {
                        Id = x.Store.Province.Id,
                        Code = x.Store.Province.Code,
                        Name = x.Store.Province.Name,
                    },
                    District = x.Store.District == null ? null : new District
                    {
                        Id = x.Store.District.Id,
                        Code = x.Store.District.Code,
                        Name = x.Store.District.Name,
                    },
                    Ward = x.Store.Ward == null ? null : new Ward
                    {
                        Id = x.Store.Ward.Id,
                        Code = x.Store.Ward.Code,
                        Name = x.Store.Ward.Name,
                    }
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (RewardHistory == null)
                return null;
            RewardHistory.RewardHistoryContents = await DataContext.RewardHistoryContent.AsNoTracking()
                .Where(x => x.RewardHistoryId == RewardHistory.Id)
                .Select(x => new RewardHistoryContent
                {
                    Id = x.Id,
                    RewardHistoryId = x.RewardHistoryId,
                    LuckyNumberId = x.LuckyNumberId,
                    LuckyNumber = new LuckyNumber
                    {
                        Id = x.LuckyNumber.Id,
                        Code = x.LuckyNumber.Code,
                        Name = x.LuckyNumber.Name,
                        Value = x.LuckyNumber.Value,
                        RewardStatusId = x.LuckyNumber.RewardStatusId,
                        RowId = x.LuckyNumber.RowId,
                    },
                }).ToListWithNoLockAsync();

            return RewardHistory;
        }
        public async Task<bool> Create(RewardHistory RewardHistory)
        {
            RewardHistoryDAO RewardHistoryDAO = new RewardHistoryDAO();
            RewardHistoryDAO.Id = RewardHistory.Id;
            RewardHistoryDAO.AppUserId = RewardHistory.AppUserId;
            RewardHistoryDAO.StoreId = RewardHistory.StoreId;
            RewardHistoryDAO.TurnCounter = RewardHistory.TurnCounter;
            RewardHistoryDAO.Revenue = RewardHistory.Revenue;
            RewardHistoryDAO.RowId = Guid.NewGuid();
            RewardHistoryDAO.CreatedAt = StaticParams.DateTimeNow;
            RewardHistoryDAO.UpdatedAt = StaticParams.DateTimeNow;
            DataContext.RewardHistory.Add(RewardHistoryDAO);
            await DataContext.SaveChangesAsync();
            RewardHistory.Id = RewardHistoryDAO.Id;
            await SaveReference(RewardHistory);
            return true;
        }

        public async Task<bool> Update(RewardHistory RewardHistory)
        {
            RewardHistoryDAO RewardHistoryDAO = DataContext.RewardHistory.Where(x => x.Id == RewardHistory.Id).FirstOrDefault();
            if (RewardHistoryDAO == null)
                return false;
            RewardHistoryDAO.Id = RewardHistory.Id;
            RewardHistoryDAO.AppUserId = RewardHistory.AppUserId;
            RewardHistoryDAO.StoreId = RewardHistory.StoreId;
            RewardHistoryDAO.Revenue = RewardHistory.Revenue;
            RewardHistoryDAO.RowId = RewardHistory.RowId;
            RewardHistoryDAO.UpdatedAt = StaticParams.DateTimeNow;
            await DataContext.SaveChangesAsync();
            await SaveReference(RewardHistory);
            return true;
        }

        public async Task<bool> Delete(RewardHistory RewardHistory)
        {
            await DataContext.RewardHistory.Where(x => x.Id == RewardHistory.Id).UpdateFromQueryAsync(x => new RewardHistoryDAO { DeletedAt = StaticParams.DateTimeNow, UpdatedAt = StaticParams.DateTimeNow });
            return true;
        }

        public async Task<bool> BulkMerge(List<RewardHistory> RewardHistories)
        {
            List<RewardHistoryDAO> RewardHistoryDAOs = new List<RewardHistoryDAO>();
            foreach (RewardHistory RewardHistory in RewardHistories)
            {
                RewardHistoryDAO RewardHistoryDAO = new RewardHistoryDAO();
                RewardHistoryDAO.Id = RewardHistory.Id;
                RewardHistoryDAO.AppUserId = RewardHistory.AppUserId;
                RewardHistoryDAO.StoreId = RewardHistory.StoreId;
                RewardHistoryDAO.TurnCounter = RewardHistory.TurnCounter;
                RewardHistoryDAO.Revenue = RewardHistory.Revenue;
                RewardHistoryDAO.RowId = RewardHistory.RowId;
                RewardHistoryDAO.CreatedAt = StaticParams.DateTimeNow;
                RewardHistoryDAO.UpdatedAt = StaticParams.DateTimeNow;
                RewardHistoryDAOs.Add(RewardHistoryDAO);
            }
            await DataContext.BulkMergeAsync(RewardHistoryDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<RewardHistory> RewardHistories)
        {
            List<long> Ids = RewardHistories.Select(x => x.Id).ToList();
            await DataContext.RewardHistory
                .Where(x => Ids.Contains(x.Id))
                .UpdateFromQueryAsync(x => new RewardHistoryDAO
                {
                    DeletedAt = StaticParams.DateTimeNow,
                    UpdatedAt = StaticParams.DateTimeNow
                });
            return true;
        }

        private async Task SaveReference(RewardHistory RewardHistory)
        {
            await DataContext.RewardHistoryContent
                .Where(x => x.RewardHistoryId == RewardHistory.Id)
                .DeleteFromQueryAsync();
            List<RewardHistoryContentDAO> RewardHistoryContentDAOs = new List<RewardHistoryContentDAO>();
            if (RewardHistory.RewardHistoryContents != null)
            {
                foreach (RewardHistoryContent RewardHistoryContent in RewardHistory.RewardHistoryContents)
                {
                    RewardHistoryContentDAO RewardHistoryContentDAO = new RewardHistoryContentDAO();
                    RewardHistoryContentDAO.Id = RewardHistoryContent.Id;
                    RewardHistoryContentDAO.RewardHistoryId = RewardHistory.Id;
                    RewardHistoryContentDAO.LuckyNumberId = RewardHistoryContent.LuckyNumberId;
                    RewardHistoryContentDAOs.Add(RewardHistoryContentDAO);
                }
                await DataContext.RewardHistoryContent.BulkMergeAsync(RewardHistoryContentDAOs);
            }
        }

    }
}
