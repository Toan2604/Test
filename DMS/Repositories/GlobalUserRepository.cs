using DMS.Entities;
using DMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Repositories
{
    public interface IGlobalUserRepository
    {
        Task<int> Count(GlobalUserFilter GlobalUserFilter);
        Task<List<GlobalUser>> List(GlobalUserFilter GlobalUserFilter);
        Task<List<GlobalUser>> List(List<long> Ids);
        Task<GlobalUser> Get(long Id);
        Task<GlobalUser> Get(Guid RowId);
        Task<bool> BulkMerge(List<GlobalUser> GlobalUsers);
    }
    public class GlobalUserRepository : IGlobalUserRepository
    {
        private DataContext DataContext;
        public GlobalUserRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<GlobalUserDAO> DynamicFilter(IQueryable<GlobalUserDAO> query, GlobalUserFilter filter)
        {
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Username, filter.Username);
            query = query.Where(q => q.DisplayName, filter.DisplayName);
            query = query.Where(q => q.RowId, filter.RowId);
            query = query.Where(q => q.GlobalUserTypeId, filter.GlobalUserTypeId);
            return query;
        }

        private IQueryable<GlobalUserDAO> DynamicOrder(IQueryable<GlobalUserDAO> query, GlobalUserFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case GlobalUserOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case GlobalUserOrder.Username:
                            query = query.OrderBy(q => q.Username);
                            break;
                        case GlobalUserOrder.DisplayName:
                            query = query.OrderBy(q => q.DisplayName);
                            break;
                        case GlobalUserOrder.RowId:
                            query = query.OrderBy(q => q.RowId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case GlobalUserOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case GlobalUserOrder.Username:
                            query = query.OrderByDescending(q => q.Username);
                            break;
                        case GlobalUserOrder.DisplayName:
                            query = query.OrderByDescending(q => q.DisplayName);
                            break;
                        case GlobalUserOrder.RowId:
                            query = query.OrderByDescending(q => q.RowId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<GlobalUser>> DynamicSelect(IQueryable<GlobalUserDAO> query, GlobalUserFilter filter)
        {
            List<GlobalUser> CallCategories = await query.Select(q => new GlobalUser()
            {
                Id = filter.Selects.Contains(GlobalUserSelect.Id) ? q.Id : default(long),
                Username = filter.Selects.Contains(GlobalUserSelect.Username) ? q.Username : default(string),
                DisplayName = filter.Selects.Contains(GlobalUserSelect.DisplayName) ? q.DisplayName : default(string),
                RowId = q.RowId,
                GlobalUserTypeId = q.GlobalUserTypeId,
                GlobalUserType = filter.Selects.Contains(GlobalUserSelect.GlobalUserType) && q.GlobalUserType != null ? new GlobalUserType
                {
                    Id = q.GlobalUserType.Id,
                    Name = q.GlobalUserType.Name,
                    Code = q.GlobalUserType.Code,
                } : null,
            }).ToListWithNoLockAsync();
            return CallCategories;
        }

        public async Task<int> Count(GlobalUserFilter filter)
        {
            IQueryable<GlobalUserDAO> CallCategories = DataContext.GlobalUser.AsNoTracking();
            CallCategories = DynamicFilter(CallCategories, filter);
            return await CallCategories.CountWithNoLockAsync();
        }

        public async Task<List<GlobalUser>> List(GlobalUserFilter filter)
        {
            if (filter == null) return new List<GlobalUser>();
            IQueryable<GlobalUserDAO> GlobalUserDAOs = DataContext.GlobalUser.AsNoTracking();
            GlobalUserDAOs = DynamicFilter(GlobalUserDAOs, filter);
            GlobalUserDAOs = DynamicOrder(GlobalUserDAOs, filter);
            List<GlobalUser> GlobalUsers = await DynamicSelect(GlobalUserDAOs, filter);
            return GlobalUsers;
        }

        public async Task<List<GlobalUser>> List(List<long> Ids)
        {
            List<GlobalUser> GlobalUsers = await DataContext.GlobalUser.AsNoTracking()
            .Where(x => Ids.Contains(x.Id)).Select(x => new GlobalUser()
            {
                Id = x.Id,
                Username = x.Username,
                DisplayName = x.DisplayName,
                Avatar = x.Avatar,
                RowId = x.RowId,
                GlobalUserTypeId = x.GlobalUserTypeId,
            }).ToListWithNoLockAsync();

            return GlobalUsers;
        }

        public async Task<GlobalUser> Get(long Id)
        {
            GlobalUser GlobalUser = await DataContext.GlobalUser.AsNoTracking()
            .Where(x => x.Id == Id)
            .Select(x => new GlobalUser()
            {
                Id = x.Id,
                Username = x.Username,
                DisplayName = x.DisplayName,
                Avatar = x.Avatar,
                RowId = x.RowId,
                GlobalUserTypeId = x.GlobalUserTypeId,
            }).FirstOrDefaultWithNoLockAsync();

            if (GlobalUser == null)
                return null;

            return GlobalUser;
        }

        public async Task<GlobalUser> Get(Guid RowId)
        {
            GlobalUser GlobalUser = await DataContext.GlobalUser.Where(x => x.RowId == RowId)
                .Select(x => new GlobalUser()
                {
                    Id = x.Id,
                    RowId = x.RowId,
                    Username = x.Username,
                    DisplayName = x.DisplayName,
                    Avatar = x.Avatar,
                    GlobalUserTypeId = x.GlobalUserTypeId,
                }).FirstOrDefaultWithNoLockAsync();

            if (GlobalUser == null)
                return null;

            return GlobalUser;
        }

        public async Task<bool> BulkMerge(List<GlobalUser> GlobalUsers)
        {
            List<GlobalUserDAO> GlobalUserDAOs = GlobalUsers.Select(x => new GlobalUserDAO
            {
                Id = x.Id,
                Username = x.Username,
                DisplayName = x.DisplayName,
                Avatar = x.Avatar,
                RowId = x.RowId,
                GlobalUserTypeId = x.GlobalUserTypeId,
            }).ToList();
            await DataContext.BulkMergeAsync(GlobalUserDAOs);
            return true;
        }
    }
}
