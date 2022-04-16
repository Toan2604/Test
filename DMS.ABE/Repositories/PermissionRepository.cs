using DMS.ABE.Common;
using TrueSight.Common;
using DMS.ABE.Entities;
using DMS.ABE.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.ABE.Repositories
{
    public interface IPermissionRepository
    {
        Task<int> Count(PermissionFilter PermissionFilter);
        Task<List<Permission>> List(PermissionFilter PermissionFilter);
        Task<Permission> Get(long Id);
        Task<bool> Create(Permission Permission);
        Task<bool> Update(Permission Permission);
        Task<bool> Delete(Permission Permission);
        Task<bool> BulkMerge(List<Permission> Permissions);
        Task<bool> BulkDelete(List<Permission> Permissions);
      
    }
    public class PermissionRepository : IPermissionRepository
    {
        private DataContext DataContext;
        public PermissionRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<PermissionDAO> DynamicFilter(IQueryable<PermissionDAO> query, PermissionFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.RoleId, filter.RoleId);
            query = query.Where(q => q.MenuId, filter.MenuId);
            query = query.Where(q => q.StatusId, filter.StatusId);
            query = OrFilter(query, filter);
            return query;
        }

        private IQueryable<PermissionDAO> OrFilter(IQueryable<PermissionDAO> query, PermissionFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<PermissionDAO> initQuery = query.Where(q => false);
            foreach (PermissionFilter PermissionFilter in filter.OrFilter)
            {
                IQueryable<PermissionDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, PermissionFilter.Id);
                queryable = queryable.Where(q => q.Code, PermissionFilter.Code);
                queryable = queryable.Where(q => q.Name, PermissionFilter.Name);
                queryable = queryable.Where(q => q.RoleId, PermissionFilter.RoleId);
                queryable = queryable.Where(q => q.MenuId, PermissionFilter.MenuId);
                queryable = queryable.Where(q => q.StatusId, PermissionFilter.StatusId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<PermissionDAO> DynamicOrder(IQueryable<PermissionDAO> query, PermissionFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case PermissionOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case PermissionOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case PermissionOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case PermissionOrder.Role:
                            query = query.OrderBy(q => q.RoleId);
                            break;
                        case PermissionOrder.Menu:
                            query = query.OrderBy(q => q.MenuId);
                            break;
                        case PermissionOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case PermissionOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case PermissionOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case PermissionOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case PermissionOrder.Role:
                            query = query.OrderByDescending(q => q.RoleId);
                            break;
                        case PermissionOrder.Menu:
                            query = query.OrderByDescending(q => q.MenuId);
                            break;
                        case PermissionOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<Permission>> DynamicSelect(IQueryable<PermissionDAO> query, PermissionFilter filter)
        {
            List<Permission> Permissions = await query.Select(q => new Permission()
            {
                Id = filter.Selects.Contains(PermissionSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(PermissionSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(PermissionSelect.Name) ? q.Name : default(string),
                RoleId = filter.Selects.Contains(PermissionSelect.Role) ? q.RoleId : default(long),
                MenuId = filter.Selects.Contains(PermissionSelect.Menu) ? q.MenuId : default(long),
                StatusId = filter.Selects.Contains(PermissionSelect.Status) ? q.StatusId : default(long),
                Menu = filter.Selects.Contains(PermissionSelect.Menu) && q.Menu != null ? new Menu
                {
                    Id = q.Menu.Id,
                    Code = q.Menu.Code,
                    Name = q.Menu.Name,
                    Path = q.Menu.Path,
                    IsDeleted = q.Menu.IsDeleted,
                } : null,
                Role = filter.Selects.Contains(PermissionSelect.Role) && q.Role != null ? new Role
                {
                    Id = q.Role.Id,
                    Code = q.Role.Code,
                    Name = q.Role.Name,
                    StatusId = q.StatusId
                } : null,
            }).ToListAsync();
            if (filter.Selects.Contains(PermissionSelect.PermissionContent))
            {
                List<long> Ids = Permissions.Select(x => x.Id).ToList();
                List<PermissionContent> PermissionContents = await DataContext.PermissionContent
                    .Where(x => Ids.Contains(x.PermissionId))
                    .Select(x => new PermissionContent
                    {
                        PermissionId = x.PermissionId,
                        FieldId = x.FieldId,
                        PermissionOperatorId = x.PermissionOperatorId,
                        Value = x.Value,
                        Id = x.Id
                    }).ToListAsync();
                List<PermissionActionMapping> PermissionActionMappings = await DataContext.PermissionActionMapping
                    .Where(x => Ids.Contains(x.PermissionId))
                    .Select(x => new PermissionActionMapping
                    {
                        ActionId = x.ActionId,
                        PermissionId = x.PermissionId,
                    }).ToListAsync();
                foreach (Permission Permission in Permissions)
                {
                    Permission.PermissionContents = PermissionContents
                        .Where(x => x.PermissionId == Permission.Id)
                        .ToList();
                    Permission.PermissionActionMappings = PermissionActionMappings
                        .Where(x => x.PermissionId == Permission.Id)
                        .ToList();
                }
            }

            return Permissions;
        }

        public async Task<int> Count(PermissionFilter filter)
        {
            IQueryable<PermissionDAO> Permissions = DataContext.Permission;
            Permissions = DynamicFilter(Permissions, filter);
            return await Permissions.CountAsync();
        }

        public async Task<List<Permission>> List(PermissionFilter filter)
        {
            if (filter == null) return new List<Permission>();
            IQueryable<PermissionDAO> PermissionDAOs = DataContext.Permission.AsNoTracking();
            PermissionDAOs = DynamicFilter(PermissionDAOs, filter);
            PermissionDAOs = DynamicOrder(PermissionDAOs, filter);
            List<Permission> Permissions = await DynamicSelect(PermissionDAOs, filter);
            return Permissions;
        }

        public async Task<Permission> Get(long Id)
        {
            Permission Permission = await DataContext.Permission.AsNoTracking()
                .Where(x => x.Id == Id).Select(x => new Permission()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    RoleId = x.RoleId,
                    MenuId = x.MenuId,
                    StatusId = x.StatusId,
                    Status = x.Status == null ? null : new Status
                    {
                        Id = x.Status.Id,
                        Code = x.Status.Code,
                        Name = x.Status.Name,
                    },
                    Menu = x.Menu == null ? null : new Menu
                    {
                        Id = x.Menu.Id,
                        Code = x.Menu.Code,
                        Name = x.Menu.Name,
                        Path = x.Menu.Path,
                        IsDeleted = x.Menu.IsDeleted,
                    },
                    Role = x.Role == null ? null : new Role
                    {
                        Id = x.Role.Id,
                        Code = x.Role.Code,
                        Name = x.Role.Name,
                        StatusId = x.Role.StatusId,
                    },
                }).FirstOrDefaultAsync();

            if (Permission == null)
                return null;
            Permission.PermissionContents = await DataContext.PermissionContent
                .Where(x => x.PermissionId == Permission.Id)
                .Select(x => new PermissionContent
                {
                    Id = x.Id,
                    PermissionId = x.PermissionId,
                    FieldId = x.FieldId,
                    PermissionOperatorId = x.PermissionOperatorId,
                    Value = x.Value,
                    Field = new Field
                    {
                        Id = x.Field.Id,
                        Name = x.Field.Name,
                        FieldTypeId = x.Field.FieldTypeId,
                        MenuId = x.Field.MenuId,
                        IsDeleted = x.Field.IsDeleted,
                    },
                    PermissionOperator = new PermissionOperator
                    {
                        Id = x.PermissionOperator.Id,
                        Code = x.PermissionOperator.Code,
                        Name = x.PermissionOperator.Name,
                    },
                }).ToListAsync();

            Permission.PermissionActionMappings = await DataContext.PermissionActionMapping
                .Where(x => x.PermissionId == Permission.Id)
                .Select(x => new PermissionActionMapping
                {
                    PermissionId = x.PermissionId,
                    ActionId = x.ActionId,
                    Action = x.Action == null ? null : new Entities.Action
                    {
                        Id = x.Action.Id,
                        Name = x.Action.Name,
                        MenuId = x.Action.MenuId,
                    },
                }).ToListAsync();

            return Permission;
        }
        public async Task<bool> Create(Permission Permission)
        {
            PermissionDAO PermissionDAO = new PermissionDAO();
            PermissionDAO.Code = Permission.Code;
            PermissionDAO.Name = Permission.Name;
            PermissionDAO.RoleId = Permission.RoleId;
            PermissionDAO.MenuId = Permission.MenuId;
            PermissionDAO.StatusId = Permission.StatusId;
            DataContext.Permission.Add(PermissionDAO);
            await DataContext.SaveChangesAsync();
            Permission.Id = PermissionDAO.Id;
            await SaveReference(Permission);
            return true;
        }

        public async Task<bool> Update(Permission Permission)
        {
            PermissionDAO PermissionDAO = DataContext.Permission.Where(x => x.Id == Permission.Id).FirstOrDefault();
            if (PermissionDAO == null)
                return false;
            PermissionDAO.Id = Permission.Id;
            PermissionDAO.Code = Permission.Code;
            PermissionDAO.Name = Permission.Name;
            PermissionDAO.RoleId = Permission.RoleId;
            PermissionDAO.MenuId = Permission.MenuId;
            PermissionDAO.StatusId = Permission.StatusId;
            await DataContext.SaveChangesAsync();
            await SaveReference(Permission);
            return true;
        }

        public async Task<bool> Delete(Permission Permission)
        {
            await DataContext.PermissionContent.Where(x => x.PermissionId == Permission.Id).DeleteFromQueryAsync();
            await DataContext.PermissionActionMapping.Where(x => x.PermissionId == Permission.Id).DeleteFromQueryAsync();
            await DataContext.Permission.Where(x => x.Id == Permission.Id).DeleteFromQueryAsync();
            return true;
        }

        public async Task<bool> BulkMerge(List<Permission> Permissions)
        {
            List<PermissionDAO> PermissionDAOs = new List<PermissionDAO>();
            foreach (Permission Permission in Permissions)
            {
                PermissionDAO PermissionDAO = new PermissionDAO();
                PermissionDAO.Code = Permission.Code;
                PermissionDAO.Name = Permission.Name;
                PermissionDAO.RoleId = Permission.RoleId;
                PermissionDAO.MenuId = Permission.MenuId;
                PermissionDAO.StatusId = Permission.StatusId;
                PermissionDAOs.Add(PermissionDAO);
            }
            await DataContext.BulkMergeAsync(PermissionDAOs);
            foreach (Permission Permission in Permissions)
            {
                PermissionDAO PermissionDAO = PermissionDAOs.Where(p => p.Code == Permission.Code).FirstOrDefault();
                Permission.Id = PermissionDAO.Id;
                Permission.PermissionActionMappings.ForEach(x => x.PermissionId = PermissionDAO.Id);
                Permission.PermissionContents.ForEach(x => x.PermissionId = PermissionDAO.Id);
            }
            List<PermissionActionMapping> PermissionActionMappings = Permissions.SelectMany(x => x.PermissionActionMappings).ToList();
            List<PermissionContent> PermissionContents = Permissions.SelectMany(x => x.PermissionContents).ToList();
            List<PermissionActionMappingDAO> PermissionActionMappingDAOs = PermissionActionMappings.Select(x => new PermissionActionMappingDAO
            {
                ActionId = x.ActionId,
                PermissionId = x.PermissionId,
            }).ToList();
            List<PermissionContentDAO> PermissionContentDAOs = PermissionContents.Select(x => new PermissionContentDAO
            {
                FieldId = x.FieldId,
                PermissionOperatorId = x.PermissionOperatorId,
                Value = x.Value,
                PermissionId = x.PermissionId,
            }).ToList();
            await DataContext.BulkMergeAsync(PermissionActionMappingDAOs);
            await DataContext.BulkMergeAsync(PermissionContentDAOs);

            return true;
        }

        public async Task<bool> BulkDelete(List<Permission> Permissions)
        {
            List<long> Ids = Permissions.Select(x => x.Id).ToList();
            await DataContext.Permission
                .Where(x => Ids.Contains(x.Id)).DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(Permission Permission)
        {
            await DataContext.PermissionContent
                .Where(x => x.PermissionId == Permission.Id)
                .DeleteFromQueryAsync();
            List<PermissionContentDAO> PermissionContentDAOs = new List<PermissionContentDAO>();
            if (Permission.PermissionContents != null)
            {
                foreach (PermissionContent PermissionContent in Permission.PermissionContents)
                {
                    PermissionContentDAO PermissionContentDAO = new PermissionContentDAO();
                    PermissionContentDAO.PermissionId = Permission.Id;
                    PermissionContentDAO.FieldId = PermissionContent.FieldId;
                    PermissionContentDAO.PermissionOperatorId = PermissionContent.PermissionOperatorId;
                    PermissionContentDAO.Value = PermissionContent.Value;
                    PermissionContentDAOs.Add(PermissionContentDAO);
                }
                await DataContext.PermissionContent.BulkInsertAsync(PermissionContentDAOs);
            }
            await DataContext.PermissionActionMapping
                .Where(x => x.PermissionId == Permission.Id)
                .DeleteFromQueryAsync();
            List<PermissionActionMappingDAO> PermissionActionMappingDAOs = new List<PermissionActionMappingDAO>();
            if (Permission.PermissionActionMappings != null)
            {
                foreach (PermissionActionMapping PermissionPageMapping in Permission.PermissionActionMappings)
                {
                    PermissionActionMappingDAO PermissionActionMappingDAO = new PermissionActionMappingDAO();
                    PermissionActionMappingDAO.PermissionId = Permission.Id;
                    PermissionActionMappingDAO.ActionId = PermissionPageMapping.ActionId;
                    PermissionActionMappingDAOs.Add(PermissionActionMappingDAO);
                }
                await DataContext.PermissionActionMapping.BulkMergeAsync(PermissionActionMappingDAOs);
            }
        }
    }
}
