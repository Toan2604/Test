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
using Thinktecture;

namespace DMS.ABE.Repositories
{
    public interface IAppUserRepository
    {
        Task<int> Count(AppUserFilter AppUserFilter);
        Task<List<AppUser>> List(AppUserFilter AppUserFilter);
        Task<AppUser> Get(long Id);
        Task<List<AppUser>> List(List<long> Ids);
        Task<bool> Update(AppUser AppUser);
        Task<bool> BulkMergeERouteScope(List<AppUserStoreMapping> AppUserStoreMappings, List<long> AppUserIds);
        Task<bool> BulkMerge(List<AppUser> AppUsers);
    }
    public class AppUserRepository : IAppUserRepository
    {
        private DataContext DataContext;
        public AppUserRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<AppUserDAO> DynamicFilter(IQueryable<AppUserDAO> query, AppUserFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Username, filter.Username);
            query = query.Where(q => q.DisplayName, filter.DisplayName);
            query = query.Where(q => q.Address, filter.Address);
            query = query.Where(q => q.Email, filter.Email);
            query = query.Where(q => q.Phone, filter.Phone);
            query = query.Where(q => q.StatusId, filter.StatusId);
            query = query.Where(q => q.SexId, filter.SexId);
            query = query.Where(q => q.Birthday, filter.Birthday);
            query = query.Where(q => q.PositionId, filter.PositionId);
            query = query.Where(q => q.Department, filter.Department);
            if (filter.OrganizationId != null)
            {
                if (filter.OrganizationId.Equal != null)
                {
                    OrganizationDAO OrganizationDAO = DataContext.Organization
                        .Where(o => o.Id == filter.OrganizationId.Equal.Value && o.StatusId == 1).FirstOrDefault();
                    query = query.Where(q => q.Organization.Path.StartsWith(OrganizationDAO.Path));
                }
                if (filter.OrganizationId.NotEqual != null)
                {
                    OrganizationDAO OrganizationDAO = DataContext.Organization
                        .Where(o => o.Id == filter.OrganizationId.NotEqual.Value && o.StatusId == 1).FirstOrDefault();
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
            query = query.Where(q => q.ProvinceId, filter.ProvinceId);
            if (filter.RoleId != null)
            {
                if (filter.RoleId.Equal.HasValue)
                {
                    query = from q in query
                            join ar in DataContext.AppUserRoleMapping on q.Id equals ar.AppUserId
                            where ar.RoleId == filter.RoleId.Equal.Value
                            select q;
                }
            }
            query = OrFilter(query, filter);
            return query;
        }

        private IQueryable<AppUserDAO> OrFilter(IQueryable<AppUserDAO> query, AppUserFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<AppUserDAO> initQuery = query.Where(q => false);
            foreach (AppUserFilter AppUserFilter in filter.OrFilter)
            {
                IQueryable<AppUserDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, AppUserFilter.Id);
                queryable = queryable.Where(q => q.Username, AppUserFilter.Username);
                queryable = queryable.Where(q => q.DisplayName, AppUserFilter.DisplayName);
                queryable = queryable.Where(q => q.Address, AppUserFilter.Address);
                queryable = queryable.Where(q => q.Email, AppUserFilter.Email);
                queryable = queryable.Where(q => q.Phone, AppUserFilter.Phone);
                queryable = queryable.Where(q => q.StatusId, AppUserFilter.StatusId);
                queryable = queryable.Where(q => q.SexId, AppUserFilter.SexId);
                queryable = queryable.Where(q => q.Birthday, AppUserFilter.Birthday);
                queryable = queryable.Where(q => q.PositionId, AppUserFilter.PositionId);
                queryable = queryable.Where(q => q.Department, AppUserFilter.Department);
                if (AppUserFilter.OrganizationId != null)
                {
                    if (AppUserFilter.OrganizationId.Equal != null)
                    {
                        OrganizationDAO OrganizationDAO = DataContext.Organization
                            .Where(o => o.Id == AppUserFilter.OrganizationId.Equal.Value).FirstOrDefault();
                        queryable = queryable.Where(q => q.Organization.Path.StartsWith(OrganizationDAO.Path));
                    }
                    if (AppUserFilter.OrganizationId.NotEqual != null)
                    {
                        OrganizationDAO OrganizationDAO = DataContext.Organization
                            .Where(o => o.Id == AppUserFilter.OrganizationId.NotEqual.Value).FirstOrDefault();
                        queryable = queryable.Where(q => !q.Organization.Path.StartsWith(OrganizationDAO.Path));
                    }
                    if (AppUserFilter.OrganizationId.In != null)
                    {
                        List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                            .Where(o => o.DeletedAt == null && o.StatusId == 1).ToList();
                        List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => AppUserFilter.OrganizationId.In.Contains(o.Id)).ToList();
                        List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> Ids = Branches.Select(o => o.Id).ToList();
                        queryable = queryable.Where(q => Ids.Contains(q.OrganizationId));
                    }
                    if (AppUserFilter.OrganizationId.NotIn != null)
                    {
                        List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                            .Where(o => o.DeletedAt == null && o.StatusId == 1).ToList();
                        List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => AppUserFilter.OrganizationId.NotIn.Contains(o.Id)).ToList();
                        List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> Ids = Branches.Select(o => o.Id).ToList();
                        queryable = queryable.Where(q => !Ids.Contains(q.OrganizationId));
                    }
                }
                queryable = queryable.Where(q => q.ProvinceId, filter.ProvinceId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<AppUserDAO> DynamicOrder(IQueryable<AppUserDAO> query, AppUserFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case AppUserOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case AppUserOrder.Username:
                            query = query.OrderBy(q => q.Username);
                            break;
                        case AppUserOrder.DisplayName:
                            query = query.OrderBy(q => q.DisplayName);
                            break;
                        case AppUserOrder.Address:
                            query = query.OrderBy(q => q.Address);
                            break;
                        case AppUserOrder.Email:
                            query = query.OrderBy(q => q.Email);
                            break;
                        case AppUserOrder.Phone:
                            query = query.OrderBy(q => q.Phone);
                            break;
                        case AppUserOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                        case AppUserOrder.Sex:
                            query = query.OrderBy(q => q.Sex);
                            break;
                        case AppUserOrder.Birthday:
                            query = query.OrderBy(q => q.Birthday);
                            break;
                        case AppUserOrder.Position:
                            query = query.OrderBy(q => q.PositionId);
                            break;
                        case AppUserOrder.Department:
                            query = query.OrderBy(q => q.Department);
                            break;
                        case AppUserOrder.Organization:
                            query = query.OrderBy(q => q.OrganizationId);
                            break;
                        case AppUserOrder.Province:
                            query = query.OrderBy(q => q.ProvinceId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case AppUserOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case AppUserOrder.Username:
                            query = query.OrderByDescending(q => q.Username);
                            break;
                        case AppUserOrder.DisplayName:
                            query = query.OrderByDescending(q => q.DisplayName);
                            break;
                        case AppUserOrder.Address:
                            query = query.OrderByDescending(q => q.Address);
                            break;
                        case AppUserOrder.Email:
                            query = query.OrderByDescending(q => q.Email);
                            break;
                        case AppUserOrder.Phone:
                            query = query.OrderByDescending(q => q.Phone);
                            break;
                        case AppUserOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                        case AppUserOrder.Sex:
                            query = query.OrderByDescending(q => q.Sex);
                            break;
                        case AppUserOrder.Birthday:
                            query = query.OrderByDescending(q => q.Birthday);
                            break;
                        case AppUserOrder.Position:
                            query = query.OrderByDescending(q => q.PositionId);
                            break;
                        case AppUserOrder.Department:
                            query = query.OrderByDescending(q => q.Department);
                            break;
                        case AppUserOrder.Organization:
                            query = query.OrderByDescending(q => q.OrganizationId);
                            break;
                        case AppUserOrder.Province:
                            query = query.OrderByDescending(q => q.ProvinceId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<AppUser>> DynamicSelect(IQueryable<AppUserDAO> query, AppUserFilter filter)
        {
            List<AppUser> AppUsers = await query.Select(q => new AppUser()
            {
                Id = filter.Selects.Contains(AppUserSelect.Id) ? q.Id : default(long),
                Username = filter.Selects.Contains(AppUserSelect.Username) ? q.Username : default(string),
                DisplayName = filter.Selects.Contains(AppUserSelect.DisplayName) ? q.DisplayName : default(string),
                Address = filter.Selects.Contains(AppUserSelect.Address) ? q.Address : default(string),
                Avatar = filter.Selects.Contains(AppUserSelect.Avatar) ? q.Avatar : default(string),
                Email = filter.Selects.Contains(AppUserSelect.Email) ? q.Email : default(string),
                Phone = filter.Selects.Contains(AppUserSelect.Phone) ? q.Phone : default(string),
                StatusId = filter.Selects.Contains(AppUserSelect.Status) ? q.StatusId : default(long),
                SexId = filter.Selects.Contains(AppUserSelect.Sex) ? q.SexId : default(long),
                Birthday = filter.Selects.Contains(AppUserSelect.Birthday) ? q.Birthday : default(DateTime?),
                PositionId = filter.Selects.Contains(AppUserSelect.Position) ? q.PositionId : default(long),
                Department = filter.Selects.Contains(AppUserSelect.Department) ? q.Department : default(string),
                OrganizationId = filter.Selects.Contains(AppUserSelect.Organization) ? q.OrganizationId : default(long),
                ProvinceId = filter.Selects.Contains(AppUserSelect.Province) ? q.ProvinceId : default(long),
                Organization = filter.Selects.Contains(AppUserSelect.Organization) && q.Organization != null ? new Organization
                {
                    Id = q.Organization.Id,
                    Code = q.Organization.Code,
                    Name = q.Organization.Name,
                    Address = q.Organization.Address,
                    Phone = q.Organization.Phone,
                    Path = q.Organization.Path,
                    ParentId = q.Organization.ParentId,
                    Email = q.Organization.Email,
                    StatusId = q.Organization.StatusId,
                    Level = q.Organization.Level
                } : null,
                Province = filter.Selects.Contains(AppUserSelect.Province) && q.Province != null ? new Province
                {
                    Id = q.Province.Id,
                    Code = q.Province.Code,
                    Name = q.Province.Name,
                    Priority = q.Province.Priority,
                    StatusId = q.Province.StatusId,
                } : null,
                Position = filter.Selects.Contains(AppUserSelect.Position) && q.Position != null ? new Position
                {
                    Id = q.Position.Id,
                    Code = q.Position.Code,
                    Name = q.Position.Name,
                    StatusId = q.Position.StatusId,
                } : null,
                Status = filter.Selects.Contains(AppUserSelect.Status) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                } : null,
                Sex = filter.Selects.Contains(AppUserSelect.Sex) && q.Sex != null ? new Sex
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                } : null,
                RowId = filter.Selects.Contains(AppUserSelect.RowId) ? q.RowId : default(Guid)
            }).ToListAsync();
            return AppUsers;
        }

        public async Task<int> Count(AppUserFilter filter)
        {
            IQueryable<AppUserDAO> AppUsers = DataContext.AppUser;
            AppUsers = DynamicFilter(AppUsers, filter);
            return await AppUsers.CountAsync();
        }

        public async Task<List<AppUser>> List(AppUserFilter filter)
        {
            if (filter == null) return new List<AppUser>();
            IQueryable<AppUserDAO> AppUserDAOs = DataContext.AppUser;
            AppUserDAOs = DynamicFilter(AppUserDAOs, filter);
            AppUserDAOs = DynamicOrder(AppUserDAOs, filter);
            List<AppUser> AppUsers = await DynamicSelect(AppUserDAOs, filter);
            return AppUsers;
        }
        public async Task<List<AppUser>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };
            List<AppUser> AppUsers = await DataContext.AppUser.AsNoTracking()
                .Where(x => x.Id, IdFilter)
                .Select(x => new AppUser()
                {
                    Id = x.Id,
                    Username = x.Username,
                    DisplayName = x.DisplayName,
                    Address = x.Address,
                    Avatar = x.Avatar,
                    Birthday = x.Birthday,
                    Email = x.Email,
                    Phone = x.Phone,
                    StatusId = x.StatusId,
                    SexId = x.SexId,
                    Department = x.Department,
                    OrganizationId = x.OrganizationId,
                    RowId = x.RowId,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    DeletedAt = x.DeletedAt,
                    Organization = x.Organization == null ? null : new Organization
                    {
                        Id = x.Organization.Id,
                        Code = x.Organization.Code,
                        Name = x.Organization.Name,
                        Path = x.Organization.Path,
                    },
                    Status = x.Status == null ? null : new Status
                    {
                        Id = x.Status.Id,
                        Code = x.Status.Code,
                        Name = x.Status.Name,
                    },
                    Sex = x.Sex == null ? null : new Sex
                    {
                        Id = x.Sex.Id,
                        Code = x.Sex.Code,
                        Name = x.Sex.Name,
                    }
                }).ToListAsync();
            var AppUserRoleMappings = await DataContext.AppUserRoleMapping
                .Where(x => x.AppUserId, IdFilter)
                .Select(x => new AppUserRoleMapping
                {
                    AppUserId = x.AppUserId,
                    RoleId = x.RoleId,
                    Role = x.Role == null ? null : new Role
                    {
                        Id = x.Role.Id,
                        Name = x.Role.Name,
                        Code = x.Role.Code,
                    },
                }).ToListAsync();

            foreach (var AppUser in AppUsers)
            {
                AppUser.AppUserRoleMappings = AppUserRoleMappings.Where(x => x.AppUserId == AppUser.Id).ToList();
            }
            return AppUsers;
        }
        public async Task<AppUser> Get(long Id)
        {
            AppUser AppUser = await DataContext.AppUser.Where(x => x.Id == Id).Select(x => new AppUser()
            {
                Id = x.Id,
                Username = x.Username,
                DisplayName = x.DisplayName,
                Address = x.Address,
                Avatar = x.Avatar,
                Birthday = x.Birthday,
                Email = x.Email,
                Phone = x.Phone,
                StatusId = x.StatusId,
                SexId = x.SexId,
                PositionId = x.PositionId,
                Department = x.Department,
                OrganizationId = x.OrganizationId,
                ProvinceId = x.ProvinceId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                RowId = x.RowId,
                Organization = x.Organization == null ? null : new Organization
                {
                    Id = x.Organization.Id,
                    Code = x.Organization.Code,
                    Name = x.Organization.Name,
                    Address = x.Organization.Address,
                    Phone = x.Organization.Phone,
                    Path = x.Organization.Path,
                    ParentId = x.Organization.ParentId,
                    Email = x.Organization.Email,
                    StatusId = x.Organization.StatusId,
                    Level = x.Organization.Level
                },
                Position = x.Position == null ? null : new Position
                {
                    Id = x.Position.Id,
                    Code = x.Position.Code,
                    Name = x.Position.Name,
                    StatusId = x.Position.StatusId,
                },
                Province = x.Province == null ? null : new Province
                {
                    Id = x.Province.Id,
                    Code = x.Province.Code,
                    Name = x.Province.Name,
                    Priority = x.Province.Priority,
                    StatusId = x.Province.StatusId,
                },
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                },
                Sex = x.Sex == null ? null : new Sex
                {
                    Id = x.Sex.Id,
                    Code = x.Sex.Code,
                    Name = x.Sex.Name,
                }
            }).FirstOrDefaultAsync();

            if (AppUser == null)
                return null;
            AppUser.AppUserRoleMappings = await DataContext.AppUserRoleMapping
                .Where(x => x.AppUserId == AppUser.Id)
                .Select(x => new AppUserRoleMapping
                {
                    AppUserId = x.AppUserId,
                    RoleId = x.RoleId,
                    Role = new Role
                    {
                        Id = x.Role.Id,
                        Code = x.Role.Code,
                        Name = x.Role.Name,
                    },
                }).ToListAsync();
            AppUser.AppUserStoreMappings = await DataContext.AppUserStoreMapping
                .Where(x => x.AppUserId == AppUser.Id)
                .Select(x => new AppUserStoreMapping
                {
                    AppUserId = x.AppUserId,
                    StoreId = x.StoreId,
                    Store = new Store
                    {
                        Id = x.Store.Id,
                        Code = x.Store.Code,
                        CodeDraft = x.Store.CodeDraft,
                        Name = x.Store.Name,
                        Address = x.Store.Address,
                        StoreTypeId = x.Store.StoreTypeId,
                        OrganizationId = x.Store.OrganizationId,
                        Organization = x.Store.Organization == null ? null : new Organization
                        {
                            Id = x.Store.Organization.Id,
                            Name = x.Store.Organization.Name,
                        },
                        StoreType = x.Store.StoreType == null ? null : new StoreType
                        {
                            Id = x.Store.StoreType.Id,
                            Name = x.Store.StoreType.Name,
                        },
                    },
                }).ToListAsync();

            return AppUser;
        }

        public async Task<bool> Update(AppUser AppUser)
        {
            AppUserDAO AppUserDAO = DataContext.AppUser.Where(x => x.Id == AppUser.Id).FirstOrDefault();
            if (AppUserDAO == null)
                return false;
            await DataContext.AppUserStoreMapping.Where(x => x.AppUserId == AppUser.Id).DeleteFromQueryAsync();
            if (AppUser.AppUserStoreMappings != null)
            {
                List<AppUserStoreMappingDAO> AppUserStoreMappingDAOs = new List<AppUserStoreMappingDAO>();
                foreach (var AppUserStoreMapping in AppUser.AppUserStoreMappings)
                {
                    AppUserStoreMappingDAO AppUserStoreMappingDAO = new AppUserStoreMappingDAO
                    {
                        AppUserId = AppUser.Id,
                        StoreId = AppUserStoreMapping.StoreId
                    };
                    AppUserStoreMappingDAOs.Add(AppUserStoreMappingDAO);
                }
                await DataContext.AppUserStoreMapping.BulkMergeAsync(AppUserStoreMappingDAOs);
            }

            await DataContext.AppUserRoleMapping.Where(x => x.AppUserId == AppUser.Id).DeleteFromQueryAsync();
            if (AppUser.AppUserRoleMappings != null)
            {
                List<AppUserRoleMappingDAO> AppUserRoleMappingDAOs = new List<AppUserRoleMappingDAO>();
                foreach (var AppUserRoleMapping in AppUser.AppUserRoleMappings)
                {
                    AppUserRoleMappingDAO AppUserRoleMappingDAO = new AppUserRoleMappingDAO
                    {
                        AppUserId = AppUser.Id,
                        RoleId = AppUserRoleMapping.RoleId
                    };
                    AppUserRoleMappingDAOs.Add(AppUserRoleMappingDAO);
                }
                await DataContext.AppUserRoleMapping.BulkMergeAsync(AppUserRoleMappingDAOs);
            }

            AppUserDAO.UpdatedAt = StaticParams.DateTimeNow;
            await DataContext.SaveChangesAsync();
            return true;
        }


        public async Task<bool> BulkMergeERouteScope(List<AppUserStoreMapping> AppUserStoreMappings, List<long> AppUserIds)
        {
            await DataContext.AppUserStoreMapping.Where(x => AppUserIds.Contains(x.AppUserId)).DeleteFromQueryAsync();
            List<AppUserStoreMappingDAO> AppUserStoreMappingDAOs = AppUserStoreMappings.Select(x => new AppUserStoreMappingDAO
            {
                AppUserId = x.AppUserId,
                StoreId = x.StoreId
            }).ToList();

            await DataContext.AppUserStoreMapping.BulkMergeAsync(AppUserStoreMappingDAOs);
            return true;
        }

        public async Task<bool> BulkMerge(List<AppUser> AppUsers)
        {
            List<AppUserDAO> AppUserDAOs = new List<AppUserDAO>();
            foreach (AppUser AppUser in AppUsers)
            {
                AppUserDAO AppUserDAO = new AppUserDAO();
                AppUserDAOs.Add(AppUserDAO);
                AppUserDAO.Address = AppUser.Address;
                AppUserDAO.Avatar = AppUser.Avatar;
                AppUserDAO.CreatedAt = AppUser.CreatedAt;
                AppUserDAO.UpdatedAt = AppUser.UpdatedAt;
                AppUserDAO.DeletedAt = AppUser.DeletedAt;
                AppUserDAO.Department = AppUser.Department;
                AppUserDAO.DisplayName = AppUser.DisplayName;
                AppUserDAO.Email = AppUser.Email;
                AppUserDAO.Id = AppUser.Id;
                AppUserDAO.OrganizationId = AppUser.OrganizationId;
                AppUserDAO.Phone = AppUser.Phone;
                AppUserDAO.PositionId = AppUser.PositionId;
                AppUserDAO.ProvinceId = AppUser.ProvinceId;
                AppUserDAO.RowId = AppUser.RowId;
                AppUserDAO.StatusId = AppUser.StatusId;
                AppUserDAO.Username = AppUser.Username;
                AppUserDAO.SexId = AppUser.SexId;
                AppUserDAO.Birthday = AppUser.Birthday;
            }
            await DataContext.BulkMergeAsync(AppUserDAOs);
            return true;
        }
    }
}
