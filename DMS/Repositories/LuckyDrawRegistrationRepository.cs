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
    public interface ILuckyDrawRegistrationRepository
    {
        Task<int> CountAll(LuckyDrawRegistrationFilter LuckyDrawRegistrationFilter);
        Task<int> Count(LuckyDrawRegistrationFilter LuckyDrawRegistrationFilter);
        Task<List<LuckyDrawRegistration>> List(LuckyDrawRegistrationFilter LuckyDrawRegistrationFilter);
        Task<List<LuckyDrawRegistration>> List(List<long> Ids);
        Task<LuckyDrawRegistration> Get(long Id);
        Task<bool> Create(LuckyDrawRegistration LuckyDrawRegistration);
        Task<bool> Update(LuckyDrawRegistration LuckyDrawRegistration);
        Task<bool> Delete(LuckyDrawRegistration LuckyDrawRegistration);
        Task<bool> BulkMerge(List<LuckyDrawRegistration> LuckyDrawRegistrations);
        Task<bool> BulkDelete(List<LuckyDrawRegistration> LuckyDrawRegistrations);
    }
    public class LuckyDrawRegistrationRepository : ILuckyDrawRegistrationRepository
    {
        private DataContext DataContext;
        public LuckyDrawRegistrationRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private async Task<IQueryable<LuckyDrawRegistrationDAO>> DynamicFilter(IQueryable<LuckyDrawRegistrationDAO> query, LuckyDrawRegistrationFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Revenue, filter.Revenue);
            query = query.Where(q => q.TurnCounter, filter.TurnCounter);
            query = query.Where(q => q.IsDrawnByStore, filter.IsDrawnByStore);
            query = query.Where(q => q.Time, filter.Time);
            query = query.Where(q => q.AppUserId, filter.AppUserId);
            query = query.Where(q => q.LuckyDrawId, filter.LuckyDrawId);
            query = query.Where(q => q.StoreId, filter.StoreId);
            if (filter.Search != null)
                query = query.Where(q =>
                q.Store.Name.ToLower().Contains(filter.Search.ToLower()) ||
                q.LuckyDraw.Name.ToLower().Contains(filter.Search.ToLower()));
            return query;
        }

        private async Task<IQueryable<LuckyDrawRegistrationDAO>> OrFilter(IQueryable<LuckyDrawRegistrationDAO> query, LuckyDrawRegistrationFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<LuckyDrawRegistrationDAO> initQuery = query.Where(q => false);
            foreach (LuckyDrawRegistrationFilter LuckyDrawRegistrationFilter in filter.OrFilter)
            {
                IQueryable<LuckyDrawRegistrationDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, LuckyDrawRegistrationFilter.Id);
                queryable = queryable.Where(q => q.Revenue, LuckyDrawRegistrationFilter.Revenue);
                queryable = queryable.Where(q => q.TurnCounter, LuckyDrawRegistrationFilter.TurnCounter);
                queryable = queryable.Where(q => q.IsDrawnByStore, LuckyDrawRegistrationFilter.IsDrawnByStore);
                queryable = queryable.Where(q => q.Time, LuckyDrawRegistrationFilter.Time);
                queryable = queryable.Where(q => q.AppUserId, LuckyDrawRegistrationFilter.AppUserId);
                queryable = queryable.Where(q => q.LuckyDrawId, LuckyDrawRegistrationFilter.LuckyDrawId);
                queryable = queryable.Where(q => q.StoreId, LuckyDrawRegistrationFilter.StoreId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<LuckyDrawRegistrationDAO> DynamicOrder(IQueryable<LuckyDrawRegistrationDAO> query, LuckyDrawRegistrationFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case LuckyDrawRegistrationOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case LuckyDrawRegistrationOrder.LuckyDraw:
                            query = query.OrderBy(q => q.LuckyDrawId);
                            break;
                        case LuckyDrawRegistrationOrder.AppUser:
                            query = query.OrderBy(q => q.AppUserId);
                            break;
                        case LuckyDrawRegistrationOrder.Store:
                            query = query.OrderBy(q => q.StoreId);
                            break;
                        case LuckyDrawRegistrationOrder.Revenue:
                            query = query.OrderBy(q => q.Revenue);
                            break;
                        case LuckyDrawRegistrationOrder.TurnCounter:
                            query = query.OrderBy(q => q.TurnCounter);
                            break;
                        case LuckyDrawRegistrationOrder.IsDrawnByStore:
                            query = query.OrderBy(q => q.IsDrawnByStore);
                            break;
                        case LuckyDrawRegistrationOrder.Time:
                            query = query.OrderBy(q => q.Time);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case LuckyDrawRegistrationOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case LuckyDrawRegistrationOrder.LuckyDraw:
                            query = query.OrderByDescending(q => q.LuckyDrawId);
                            break;
                        case LuckyDrawRegistrationOrder.AppUser:
                            query = query.OrderByDescending(q => q.AppUserId);
                            break;
                        case LuckyDrawRegistrationOrder.Store:
                            query = query.OrderByDescending(q => q.StoreId);
                            break;
                        case LuckyDrawRegistrationOrder.Revenue:
                            query = query.OrderByDescending(q => q.Revenue);
                            break;
                        case LuckyDrawRegistrationOrder.TurnCounter:
                            query = query.OrderByDescending(q => q.TurnCounter);
                            break;
                        case LuckyDrawRegistrationOrder.IsDrawnByStore:
                            query = query.OrderByDescending(q => q.IsDrawnByStore);
                            break;
                        case LuckyDrawRegistrationOrder.Time:
                            query = query.OrderByDescending(q => q.Time);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<LuckyDrawRegistration>> DynamicSelect(IQueryable<LuckyDrawRegistrationDAO> query, LuckyDrawRegistrationFilter filter)
        {
            List<LuckyDrawRegistration> LuckyDrawRegistrations = await query.Select(q => new LuckyDrawRegistration()
            {
                Id = filter.Selects.Contains(LuckyDrawRegistrationSelect.Id) ? q.Id : default(long),
                LuckyDrawId = filter.Selects.Contains(LuckyDrawRegistrationSelect.LuckyDraw) ? q.LuckyDrawId : default(long),
                AppUserId = filter.Selects.Contains(LuckyDrawRegistrationSelect.AppUser) ? q.AppUserId : default(long),
                StoreId = filter.Selects.Contains(LuckyDrawRegistrationSelect.Store) ? q.StoreId : default(long),
                Revenue = filter.Selects.Contains(LuckyDrawRegistrationSelect.Revenue) ? q.Revenue : default(decimal),
                TurnCounter = filter.Selects.Contains(LuckyDrawRegistrationSelect.TurnCounter) ? q.TurnCounter : default(long),
                IsDrawnByStore = filter.Selects.Contains(LuckyDrawRegistrationSelect.IsDrawnByStore) ? q.IsDrawnByStore : default(bool),
                Time = filter.Selects.Contains(LuckyDrawRegistrationSelect.Time) ? q.Time : default(DateTime),
                AppUser = filter.Selects.Contains(LuckyDrawRegistrationSelect.AppUser) && q.AppUser != null ? new AppUser
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
                } : null,
                LuckyDraw = filter.Selects.Contains(LuckyDrawRegistrationSelect.LuckyDraw) && q.LuckyDraw != null ? new LuckyDraw
                {
                    Id = q.LuckyDraw.Id,
                    Code = q.LuckyDraw.Code,
                    Name = q.LuckyDraw.Name,
                    LuckyDrawTypeId = q.LuckyDraw.LuckyDrawTypeId,
                    OrganizationId = q.LuckyDraw.OrganizationId,
                    RevenuePerTurn = q.LuckyDraw.RevenuePerTurn,
                    StartAt = q.LuckyDraw.StartAt,
                    EndAt = q.LuckyDraw.EndAt,
                    AvatarImageId = q.LuckyDraw.AvatarImageId,
                    ImageId = q.LuckyDraw.ImageId,
                    Description = q.LuckyDraw.Description,
                    StatusId = q.LuckyDraw.StatusId,
                    Used = q.LuckyDraw.Used,
                } : null,
                Store = filter.Selects.Contains(LuckyDrawRegistrationSelect.Store) && q.Store != null ? new Store
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
            }).ToListWithNoLockAsync();
            return LuckyDrawRegistrations;
        }

        public async Task<int> CountAll(LuckyDrawRegistrationFilter filter)
        {
            IQueryable<LuckyDrawRegistrationDAO> LuckyDrawRegistrationDAOs = DataContext.LuckyDrawRegistration.AsNoTracking();
            LuckyDrawRegistrationDAOs = await DynamicFilter(LuckyDrawRegistrationDAOs, filter);
            return await LuckyDrawRegistrationDAOs.CountWithNoLockAsync();
        }

        public async Task<int> Count(LuckyDrawRegistrationFilter filter)
        {
            IQueryable<LuckyDrawRegistrationDAO> LuckyDrawRegistrationDAOs = DataContext.LuckyDrawRegistration.AsNoTracking();
            LuckyDrawRegistrationDAOs = await DynamicFilter(LuckyDrawRegistrationDAOs, filter);
            LuckyDrawRegistrationDAOs = await OrFilter(LuckyDrawRegistrationDAOs, filter);
            return await LuckyDrawRegistrationDAOs.CountWithNoLockAsync();
        }

        public async Task<List<LuckyDrawRegistration>> List(LuckyDrawRegistrationFilter filter)
        {
            if (filter == null) return new List<LuckyDrawRegistration>();
            IQueryable<LuckyDrawRegistrationDAO> LuckyDrawRegistrationDAOs = DataContext.LuckyDrawRegistration.AsNoTracking();
            LuckyDrawRegistrationDAOs = await DynamicFilter(LuckyDrawRegistrationDAOs, filter);
            LuckyDrawRegistrationDAOs = await OrFilter(LuckyDrawRegistrationDAOs, filter);
            LuckyDrawRegistrationDAOs = DynamicOrder(LuckyDrawRegistrationDAOs, filter);
            List<LuckyDrawRegistration> LuckyDrawRegistrations = await DynamicSelect(LuckyDrawRegistrationDAOs, filter);
            return LuckyDrawRegistrations;
        }

        public async Task<List<LuckyDrawRegistration>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };

            IQueryable<LuckyDrawRegistrationDAO> query = DataContext.LuckyDrawRegistration.AsNoTracking();
            query = query.Where(q => q.Id, IdFilter);
            List<LuckyDrawRegistration> LuckyDrawRegistrations = await query.AsNoTracking()
            .Select(x => new LuckyDrawRegistration()
            {
                Id = x.Id,
                LuckyDrawId = x.LuckyDrawId,
                AppUserId = x.AppUserId,
                StoreId = x.StoreId,
                Revenue = x.Revenue,
                TurnCounter = x.TurnCounter,
                IsDrawnByStore = x.IsDrawnByStore,
                Time = x.Time,
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
                },
                LuckyDraw = x.LuckyDraw == null ? null : new LuckyDraw
                {
                    Id = x.LuckyDraw.Id,
                    Code = x.LuckyDraw.Code,
                    Name = x.LuckyDraw.Name,
                    LuckyDrawTypeId = x.LuckyDraw.LuckyDrawTypeId,
                    OrganizationId = x.LuckyDraw.OrganizationId,
                    RevenuePerTurn = x.LuckyDraw.RevenuePerTurn,
                    StartAt = x.LuckyDraw.StartAt,
                    EndAt = x.LuckyDraw.EndAt,
                    AvatarImageId = x.LuckyDraw.AvatarImageId,
                    ImageId = x.LuckyDraw.ImageId,
                    Description = x.LuckyDraw.Description,
                    StatusId = x.LuckyDraw.StatusId,
                    Used = x.LuckyDraw.Used,
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
                    Organization = x.Store.Organization == null ? null : new Organization
                    {
                        Id = x.Store.Organization.Id,
                        Code = x.Store.Organization.Code,
                        Name = x.Store.Organization.Name,
                    },
                    Province = x.Store.Province == null ? null : new Province
                    {
                        Id = x.Store.Province.Id,
                        Code = x.Store.Province.Code,
                        Name = x.Store.Province.Name,
                    }
                },
            }).ToListWithNoLockAsync();


            return LuckyDrawRegistrations;
        }

        public async Task<LuckyDrawRegistration> Get(long Id)
        {
            LuckyDrawRegistration LuckyDrawRegistration = await DataContext.LuckyDrawRegistration.AsNoTracking()
            .Where(x => x.Id == Id)
            .Select(x => new LuckyDrawRegistration()
            {
                Id = x.Id,
                LuckyDrawId = x.LuckyDrawId,
                AppUserId = x.AppUserId,
                StoreId = x.StoreId,
                Revenue = x.Revenue,
                TurnCounter = x.TurnCounter,
                IsDrawnByStore = x.IsDrawnByStore,
                Time = x.Time,
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
                },
                LuckyDraw = x.LuckyDraw == null ? null : new LuckyDraw
                {
                    Id = x.LuckyDraw.Id,
                    Code = x.LuckyDraw.Code,
                    Name = x.LuckyDraw.Name,
                    LuckyDrawTypeId = x.LuckyDraw.LuckyDrawTypeId,
                    OrganizationId = x.LuckyDraw.OrganizationId,
                    RevenuePerTurn = x.LuckyDraw.RevenuePerTurn,
                    StartAt = x.LuckyDraw.StartAt,
                    EndAt = x.LuckyDraw.EndAt,
                    AvatarImageId = x.LuckyDraw.AvatarImageId,
                    ImageId = x.LuckyDraw.ImageId,
                    Description = x.LuckyDraw.Description,
                    StatusId = x.LuckyDraw.StatusId,
                    Used = x.LuckyDraw.Used,
                    Image = x.LuckyDraw.Image != null ? new Image
                    {
                        Id = x.LuckyDraw.Image.Id,
                        Url = x.LuckyDraw.Image.Url,
                        ThumbnailUrl = x.LuckyDraw.Image.ThumbnailUrl
                    } : null,
                    AvatarImage = x.LuckyDraw.AvatarImage != null ? new Image
                    {
                        Id = x.LuckyDraw.AvatarImage.Id,
                        Url = x.LuckyDraw.AvatarImage.Url,
                        ThumbnailUrl = x.LuckyDraw.AvatarImage.ThumbnailUrl
                    } : null,
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

            if (LuckyDrawRegistration == null)
                return null;
            LuckyDrawRegistration.LuckyDrawWinners = await DataContext.LuckyDrawWinner
                .Where(x => x.LuckyDrawRegistrationId == LuckyDrawRegistration.Id)
                .Select(x => new LuckyDrawWinner
                {
                    Id = x.Id,
                    LuckyDrawId = x.LuckyDrawId,
                    LuckyDrawRegistrationId = x.LuckyDrawRegistrationId,
                    LuckyDrawStructureId = x.LuckyDrawStructureId,
                    Time = x.Time,
                    RowId = x.RowId,
                    LuckyDrawNumberId = x.LuckyDrawNumberId
                })
                .ToListWithNoLockAsync();
            LuckyDrawRegistration.LuckyDraw.LuckyDrawStructures = await DataContext.LuckyDrawStructure
                .Where(x => x.LuckyDrawId == LuckyDrawRegistration.LuckyDrawId)
                .Select(x => new LuckyDrawStructure
                {
                    Id = x.Id,
                    LuckyDrawId = x.LuckyDrawId,
                    Name = x.Name,
                    Value = x.Value,
                    Quantity = x.Quantity                    
                })
                .ToListWithNoLockAsync();

            return LuckyDrawRegistration;
        }

        public async Task<bool> Create(LuckyDrawRegistration LuckyDrawRegistration)
        {
            LuckyDrawRegistrationDAO LuckyDrawRegistrationDAO = new LuckyDrawRegistrationDAO();
            LuckyDrawRegistrationDAO.Id = LuckyDrawRegistration.Id;
            LuckyDrawRegistrationDAO.LuckyDrawId = LuckyDrawRegistration.LuckyDrawId;
            LuckyDrawRegistrationDAO.AppUserId = LuckyDrawRegistration.AppUserId;
            LuckyDrawRegistrationDAO.StoreId = LuckyDrawRegistration.StoreId;
            LuckyDrawRegistrationDAO.Revenue = LuckyDrawRegistration.Revenue;
            LuckyDrawRegistrationDAO.TurnCounter = LuckyDrawRegistration.TurnCounter;
            LuckyDrawRegistrationDAO.IsDrawnByStore = LuckyDrawRegistration.IsDrawnByStore;
            LuckyDrawRegistrationDAO.Time = LuckyDrawRegistration.Time;
            DataContext.LuckyDrawRegistration.Add(LuckyDrawRegistrationDAO);
            await DataContext.SaveChangesAsync();
            LuckyDrawRegistration.Id = LuckyDrawRegistrationDAO.Id;
            return true;
        }

        public async Task<bool> Update(LuckyDrawRegistration LuckyDrawRegistration)
        {
            LuckyDrawRegistrationDAO LuckyDrawRegistrationDAO = DataContext.LuckyDrawRegistration
                .Where(x => x.Id == LuckyDrawRegistration.Id)
                .FirstOrDefault();
            if (LuckyDrawRegistrationDAO == null)
                return false;
            LuckyDrawRegistrationDAO.Id = LuckyDrawRegistration.Id;
            LuckyDrawRegistrationDAO.LuckyDrawId = LuckyDrawRegistration.LuckyDrawId;
            await DataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(LuckyDrawRegistration LuckyDrawRegistration)
        {
            await DataContext.LuckyDrawRegistration
                .Where(x => x.Id == LuckyDrawRegistration.Id)
                .DeleteFromQueryAsync();
            return true;
        }

        public async Task<bool> BulkMerge(List<LuckyDrawRegistration> LuckyDrawRegistrations)
        {
            IdFilter IdFilter = new IdFilter { In = LuckyDrawRegistrations.Select(x => x.Id).ToList() };
            List<LuckyDrawRegistrationDAO> LuckyDrawRegistrationDAOs = new List<LuckyDrawRegistrationDAO>();
            List<LuckyDrawRegistrationDAO> DbLuckyDrawRegistrationDAOs = await DataContext.LuckyDrawRegistration
                .Where(x => x.Id, IdFilter)
                .ToListWithNoLockAsync();
            foreach (LuckyDrawRegistration LuckyDrawRegistration in LuckyDrawRegistrations)
            {
                LuckyDrawRegistrationDAO LuckyDrawRegistrationDAO = DbLuckyDrawRegistrationDAOs
                        .Where(x => x.Id == LuckyDrawRegistration.Id)
                        .FirstOrDefault();
                if (LuckyDrawRegistrationDAO == null)
                {
                    LuckyDrawRegistrationDAO = new LuckyDrawRegistrationDAO();
                }
                LuckyDrawRegistrationDAO.LuckyDrawId = LuckyDrawRegistration.LuckyDrawId;
                LuckyDrawRegistrationDAO.AppUserId = LuckyDrawRegistration.AppUserId;
                LuckyDrawRegistrationDAO.StoreId = LuckyDrawRegistration.StoreId;
                LuckyDrawRegistrationDAO.Revenue = LuckyDrawRegistration.Revenue;
                LuckyDrawRegistrationDAO.TurnCounter = LuckyDrawRegistration.TurnCounter;
                LuckyDrawRegistrationDAO.IsDrawnByStore = LuckyDrawRegistration.IsDrawnByStore;
                LuckyDrawRegistrationDAO.Time = LuckyDrawRegistration.Time;
                LuckyDrawRegistrationDAOs.Add(LuckyDrawRegistrationDAO);
            }
            await DataContext.BulkMergeAsync(LuckyDrawRegistrationDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<LuckyDrawRegistration> LuckyDrawRegistrations)
        {
            List<long> Ids = LuckyDrawRegistrations.Select(x => x.Id).ToList();
            IdFilter IdFilter = new IdFilter { In = Ids };
            await DataContext.LuckyDrawRegistration
                .Where(x => x.Id, IdFilter)
                .DeleteFromQueryAsync();
            return true;
        }

    }
}
