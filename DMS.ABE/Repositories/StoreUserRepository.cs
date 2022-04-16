using DMS.ABE.Common;
using TrueSight.Common;
using DMS.ABE.Entities;
using DMS.ABE.Helpers;
using DMS.ABE.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.ABE.Repositories
{
    public interface IStoreUserRepository
    {
        Task<int> Count(StoreUserFilter StoreUserFilter);
        Task<List<StoreUser>> List(StoreUserFilter StoreUserFilter);
        Task<List<StoreUser>> List(List<long> Ids);
        Task<StoreUser> Get(long Id);
        Task<bool> Create(StoreUser StoreUser);
        Task<bool> Update(StoreUser StoreUser);
        Task<bool> Delete(StoreUser StoreUser);
        Task<bool> BulkMerge(List<StoreUser> StoreUsers);
        Task<bool> BulkDelete(List<StoreUser> StoreUsers);
    }
    public class StoreUserRepository : IStoreUserRepository
    {
        private DataContext DataContext;
        public StoreUserRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<StoreUserDAO> DynamicFilter(IQueryable<StoreUserDAO> query, StoreUserFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.CreatedAt, filter.CreatedAt);
            query = query.Where(q => q.UpdatedAt, filter.UpdatedAt);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.StoreId, filter.StoreId);
            query = query.Where(q => q.Username, filter.Username);
            query = query.Where(q => q.DisplayName, filter.DisplayName);
            query = query.Where(q => q.StatusId, filter.StatusId);
            query = query.Where(q => q.RowId, filter.RowId);
            query = OrFilter(query, filter);
            return query;
        }

        private IQueryable<StoreUserDAO> OrFilter(IQueryable<StoreUserDAO> query, StoreUserFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<StoreUserDAO> initQuery = query.Where(q => false);
            foreach (StoreUserFilter StoreUserFilter in filter.OrFilter)
            {
                IQueryable<StoreUserDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, StoreUserFilter.Id);
                queryable = queryable.Where(q => q.StoreId, StoreUserFilter.StoreId);
                queryable = queryable.Where(q => q.Username, StoreUserFilter.Username);
                queryable = queryable.Where(q => q.DisplayName, StoreUserFilter.DisplayName);
                queryable = queryable.Where(q => q.StatusId, StoreUserFilter.StatusId);
                queryable = queryable.Where(q => q.RowId, StoreUserFilter.RowId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<StoreUserDAO> DynamicOrder(IQueryable<StoreUserDAO> query, StoreUserFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case StoreUserOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case StoreUserOrder.Store:
                            query = query.OrderBy(q => q.StoreId);
                            break;
                        case StoreUserOrder.Username:
                            query = query.OrderBy(q => q.Username);
                            break;
                        case StoreUserOrder.DisplayName:
                            query = query.OrderBy(q => q.DisplayName);
                            break;
                        case StoreUserOrder.Password:
                            query = query.OrderBy(q => q.Password);
                            break;
                        case StoreUserOrder.OtpCode:
                            query = query.OrderBy(q => q.OtpCode);
                            break;
                        case StoreUserOrder.OtpExpired:
                            query = query.OrderBy(q => q.OtpExpired);
                            break;
                        case StoreUserOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                        case StoreUserOrder.Row:
                            query = query.OrderBy(q => q.RowId);
                            break;
                        case StoreUserOrder.Used:
                            query = query.OrderBy(q => q.Used);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case StoreUserOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case StoreUserOrder.Store:
                            query = query.OrderByDescending(q => q.StoreId);
                            break;
                        case StoreUserOrder.Username:
                            query = query.OrderByDescending(q => q.Username);
                            break;
                        case StoreUserOrder.DisplayName:
                            query = query.OrderByDescending(q => q.DisplayName);
                            break;
                        case StoreUserOrder.Password:
                            query = query.OrderByDescending(q => q.Password);
                            break;
                        case StoreUserOrder.OtpCode:
                            query = query.OrderByDescending(q => q.OtpCode);
                            break;
                        case StoreUserOrder.OtpExpired:
                            query = query.OrderByDescending(q => q.OtpExpired);
                            break;
                        case StoreUserOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                        case StoreUserOrder.Row:
                            query = query.OrderByDescending(q => q.RowId);
                            break;
                        case StoreUserOrder.Used:
                            query = query.OrderByDescending(q => q.Used);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<StoreUser>> DynamicSelect(IQueryable<StoreUserDAO> query, StoreUserFilter filter)
        {
            List<StoreUser> StoreUsers = await query.Select(q => new StoreUser()
            {
                Id = filter.Selects.Contains(StoreUserSelect.Id) ? q.Id : default(long),
                StoreId = filter.Selects.Contains(StoreUserSelect.Store) ? q.StoreId : default(long),
                Username = filter.Selects.Contains(StoreUserSelect.Username) ? q.Username : default(string),
                DisplayName = filter.Selects.Contains(StoreUserSelect.DisplayName) ? q.DisplayName : default(string),
                Password = filter.Selects.Contains(StoreUserSelect.Password) ? q.Password : default(string),
                OtpCode = filter.Selects.Contains(StoreUserSelect.OtpCode) ? q.OtpCode : default(string),
                OtpExpired = filter.Selects.Contains(StoreUserSelect.OtpExpired) ? q.OtpExpired : default(DateTime?),
                StatusId = filter.Selects.Contains(StoreUserSelect.Status) ? q.StatusId : default(long),
                RowId = filter.Selects.Contains(StoreUserSelect.Row) ? q.RowId : default(Guid),
                Used = filter.Selects.Contains(StoreUserSelect.Used) ? q.Used : default(bool),
                Status = filter.Selects.Contains(StoreUserSelect.Status) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                } : null,
                Store = filter.Selects.Contains(StoreUserSelect.Store) && q.Store != null ? new Store
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
                    RowId = q.Store.RowId,
                    Used = q.Store.Used,
                    StoreScoutingId = q.Store.StoreScoutingId,
                    StoreStatusId = q.Store.StoreStatusId,
                } : null,
            }).ToListAsync();
            return StoreUsers;
        }

        public async Task<int> Count(StoreUserFilter filter)
        {
            IQueryable<StoreUserDAO> StoreUsers = DataContext.StoreUser.AsNoTracking();
            StoreUsers = DynamicFilter(StoreUsers, filter);
            return await StoreUsers.CountAsync();
        }

        public async Task<List<StoreUser>> List(StoreUserFilter filter)
        {
            if (filter == null) return new List<StoreUser>();
            IQueryable<StoreUserDAO> StoreUserDAOs = DataContext.StoreUser.AsNoTracking();
            StoreUserDAOs = DynamicFilter(StoreUserDAOs, filter);
            StoreUserDAOs = DynamicOrder(StoreUserDAOs, filter);
            List<StoreUser> StoreUsers = await DynamicSelect(StoreUserDAOs, filter);
            return StoreUsers;
        }

        public async Task<List<StoreUser>> List(List<long> Ids)
        {
            List<StoreUser> StoreUsers = await DataContext.StoreUser.AsNoTracking()
            .Where(x => Ids.Contains(x.Id)).Select(x => new StoreUser()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Id = x.Id,
                StoreId = x.StoreId,
                Username = x.Username,
                DisplayName = x.DisplayName,
                Password = x.Password,
                OtpCode = x.OtpCode,
                OtpExpired = x.OtpExpired,
                StatusId = x.StatusId,
                RowId = x.RowId,
                Used = x.Used,
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
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
                    RowId = x.Store.RowId,
                    Used = x.Store.Used,
                    StoreScoutingId = x.Store.StoreScoutingId,
                    StoreStatusId = x.Store.StoreStatusId,
                },
            }).ToListAsync();


            return StoreUsers;
        }

        public async Task<StoreUser> Get(long Id)
        {
            StoreUser StoreUser = await DataContext.StoreUser.AsNoTracking()
            .Where(x => x.Id == Id)
            .Where(x => x.DeletedAt == null)
            .Select(x => new StoreUser()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Id = x.Id,
                StoreId = x.StoreId,
                Username = x.Username,
                DisplayName = x.DisplayName,
                Password = x.Password,
                OtpCode = x.OtpCode,
                OtpExpired = x.OtpExpired,
                StatusId = x.StatusId,
                RowId = x.RowId,
                Used = x.Used,
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
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
                    RowId = x.Store.RowId,
                    Used = x.Store.Used,
                    StoreScoutingId = x.Store.StoreScoutingId,
                    StoreStatusId = x.Store.StoreStatusId,
                },
            }).FirstOrDefaultAsync();

            if (StoreUser == null)
                return null;

            return StoreUser;
        }
        public async Task<bool> Create(StoreUser StoreUser)
        {
            StoreUserDAO StoreUserDAO = new StoreUserDAO();
            StoreUserDAO.Id = StoreUser.Id;
            StoreUserDAO.StoreId = StoreUser.StoreId;
            StoreUserDAO.Username = StoreUser.Username;
            StoreUserDAO.DisplayName = StoreUser.DisplayName;
            StoreUserDAO.Password = StoreUser.Password;
            StoreUserDAO.OtpCode = StoreUser.OtpCode;
            StoreUserDAO.OtpExpired = StoreUser.OtpExpired;
            StoreUserDAO.StatusId = StoreUser.StatusId;
            StoreUserDAO.RowId = Guid.NewGuid();
            StoreUserDAO.Used = StoreUser.Used;
            StoreUserDAO.CreatedAt = StaticParams.DateTimeNow;
            StoreUserDAO.UpdatedAt = StaticParams.DateTimeNow;
            DataContext.StoreUser.Add(StoreUserDAO);
            await DataContext.SaveChangesAsync();
            StoreUser.Id = StoreUserDAO.Id;
            await SaveReference(StoreUser);
            return true;
        }

        public async Task<bool> Update(StoreUser StoreUser)
        {
            StoreUserDAO StoreUserDAO = DataContext.StoreUser.Where(x => x.Id == StoreUser.Id).FirstOrDefault();
            if (StoreUserDAO == null)
                return false;
            StoreUserDAO.Id = StoreUser.Id;
            StoreUserDAO.StoreId = StoreUser.StoreId;
            StoreUserDAO.Username = StoreUser.Username;
            StoreUserDAO.DisplayName = StoreUser.DisplayName;
            StoreUserDAO.Password = StoreUser.Password;
            StoreUserDAO.OtpCode = StoreUser.OtpCode;
            StoreUserDAO.OtpExpired = StoreUser.OtpExpired;
            StoreUserDAO.StatusId = StoreUser.StatusId;
            StoreUserDAO.RowId = StoreUser.RowId;
            StoreUserDAO.Used = StoreUser.Used;
            StoreUserDAO.UpdatedAt = StaticParams.DateTimeNow;
            await DataContext.SaveChangesAsync();
            await SaveReference(StoreUser);
            return true;
        }

        public async Task<bool> Delete(StoreUser StoreUser)
        {
            await DataContext.StoreUser.Where(x => x.Id == StoreUser.Id).UpdateFromQueryAsync(x => new StoreUserDAO { DeletedAt = StaticParams.DateTimeNow, UpdatedAt = StaticParams.DateTimeNow });
            return true;
        }

        public async Task<bool> BulkMerge(List<StoreUser> StoreUsers)
        {
            List<StoreUserDAO> StoreUserDAOs = new List<StoreUserDAO>();
            foreach (StoreUser StoreUser in StoreUsers)
            {
                StoreUserDAO StoreUserDAO = new StoreUserDAO();
                StoreUserDAO.Id = StoreUser.Id;
                StoreUserDAO.StoreId = StoreUser.StoreId;
                StoreUserDAO.Username = StoreUser.Username;
                StoreUserDAO.DisplayName = StoreUser.DisplayName;
                StoreUserDAO.Password = StoreUser.Password;
                StoreUserDAO.OtpCode = StoreUser.OtpCode;
                StoreUserDAO.OtpExpired = StoreUser.OtpExpired;
                StoreUserDAO.StatusId = StoreUser.StatusId;
                StoreUserDAO.RowId = StoreUser.RowId;
                StoreUserDAO.Used = StoreUser.Used;
                StoreUserDAO.CreatedAt = StaticParams.DateTimeNow;
                StoreUserDAO.UpdatedAt = StaticParams.DateTimeNow;
                StoreUserDAOs.Add(StoreUserDAO);
            }
            await DataContext.BulkMergeAsync(StoreUserDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<StoreUser> StoreUsers)
        {
            List<long> Ids = StoreUsers.Select(x => x.Id).ToList();
            await DataContext.StoreUser
                .Where(x => Ids.Contains(x.Id))
                .UpdateFromQueryAsync(x => new StoreUserDAO { DeletedAt = StaticParams.DateTimeNow, UpdatedAt = StaticParams.DateTimeNow });
            return true;
        }

        private async Task SaveReference(StoreUser StoreUser)
        {
        }

    }
}
