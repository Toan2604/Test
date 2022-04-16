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
    public interface IOrganizationRepository
    {
        Task<int> Count(OrganizationFilter OrganizationFilter);
        Task<int> CountAll(OrganizationFilter OrganizationFilter);
        Task<List<Organization>> List(OrganizationFilter OrganizationFilter);
        Task<List<Organization>> List(List<long> Ids);
        Task<Organization> Get(long Id);
        Task<bool> BulkMerge(List<Organization> Organizations);
        Task<bool> UpdateIsDisplay(Organization Organization);
    }
    public class OrganizationRepository : IOrganizationRepository
    {
        private DataContext DataContext;
        public OrganizationRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<OrganizationDAO> DynamicFilter(IQueryable<OrganizationDAO> query, OrganizationFilter filter)
        {
            query = query.Where(x => x.DeletedAt == null);
            if (filter == null)
                return query.Where(q => false);
            if (filter.Id != null)
            {
                if (filter.Id.Equal != null)
                {
                    OrganizationDAO OrganizationDAO = DataContext.Organization
                        .Where(o => o.Id == filter.Id.Equal.Value).FirstOrDefaultWithNoLock();
                    if (OrganizationDAO == null)
                        return query.Where(q => false);
                    else
                        query = query.Where(q => q.Path.StartsWith(OrganizationDAO.Path));
                }
                if (filter.Id.NotEqual != null)
                {
                    OrganizationDAO OrganizationDAO = DataContext.Organization
                        .Where(o => o.Id == filter.Id.NotEqual.Value).FirstOrDefaultWithNoLock();
                    if (OrganizationDAO == null)
                        return query.Where(q => false);
                    else
                        query = query.Where(q => !q.Path.StartsWith(OrganizationDAO.Path));
                }
                if (filter.Id.In != null)
                {
                    List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                       .Where(o => o.DeletedAt == null && o.StatusId == 1).ToListWithNoLock();
                    List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => filter.Id.In.Contains(o.Id)).ToList();
                    List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> Ids = Branches.Select(o => o.Id).ToList();
                    IdFilter IdFilter = new IdFilter { In = Ids };
                    query = query.Where(x => x.Id, IdFilter);
                }
                if (filter.Id.NotIn != null)
                {
                    List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                       .Where(o => o.DeletedAt == null && o.StatusId == 1).ToListWithNoLock();
                    List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => filter.Id.NotIn.Contains(o.Id)).ToList();
                    List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> Ids = Branches.Select(o => o.Id).ToList();
                    IdFilter IdFilter = new IdFilter { NotIn = Ids };
                    query = query.Where(x => x.Id, IdFilter);
                }
            }
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            if (filter.ParentId != null)
            {
                if (filter.ParentId.Equal != null)
                {
                    OrganizationDAO OrganizationDAO = DataContext.Organization
                        .Where(o => o.Id == filter.ParentId.Equal.Value).FirstOrDefaultWithNoLock();
                    query = query.Where(q => q.Path.StartsWith(OrganizationDAO.Path));
                }
                if (filter.ParentId.NotEqual != null)
                {
                    OrganizationDAO OrganizationDAO = DataContext.Organization
                        .Where(o => o.Id == filter.ParentId.NotEqual.Value).FirstOrDefaultWithNoLock();
                    query = query.Where(q => !q.Path.StartsWith(OrganizationDAO.Path));
                }
                if (filter.ParentId.In != null)
                {
                    List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                       .Where(o => o.DeletedAt == null && o.StatusId == 1).ToListWithNoLock();
                    List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => filter.ParentId.In.Contains(o.Id)).ToList();
                    List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> Ids = Branches.Select(o => o.Id).ToList();
                    IdFilter IdFilter = new IdFilter { In = Ids };
                    query = query.Where(q => q.ParentId.HasValue).Where(x => x.ParentId, IdFilter);
                }
                if (filter.ParentId.NotIn != null)
                {
                    List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                       .Where(o => o.DeletedAt == null && o.StatusId == 1).ToListWithNoLock();
                    List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => filter.ParentId.NotIn.Contains(o.Id)).ToList();
                    List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> Ids = Branches.Select(o => o.Id).ToList();
                    IdFilter IdFilter = new IdFilter { NotIn = Ids };
                    query = query.Where(q => q.ParentId.HasValue).Where(x => x.ParentId, IdFilter);
                }
            }
            query = query.Where(q => q.Path, filter.Path);
            query = query.Where(q => q.Level, filter.Level);
            query = query.Where(q => q.StatusId, filter.StatusId);
            query = query.Where(q => q.Phone, filter.Phone);
            query = query.Where(q => q.Email, filter.Email);
            query = query.Where(q => q.Address, filter.Address);
            query = query.Where(q => q.UpdatedAt, filter.UpdatedAt);
            if (filter.IsDisplay.HasValue)
                query = query.Where(q => q.IsDisplay == filter.IsDisplay);
            return query;
        }

        private IQueryable<OrganizationDAO> OrFilter(IQueryable<OrganizationDAO> query, OrganizationFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<OrganizationDAO> initQuery = query.Where(q => false);
            foreach (OrganizationFilter OrganizationFilter in filter.OrFilter)
            {
                IQueryable<OrganizationDAO> queryable = query;
                queryable = queryable.Where(x => x.DeletedAt == null);
                if (OrganizationFilter.Id != null)
                {
                    if (OrganizationFilter.Id.Equal != null)
                    {
                        OrganizationDAO OrganizationDAO = DataContext.Organization
                            .Where(o => o.Id == OrganizationFilter.Id.Equal.Value).FirstOrDefaultWithNoLock();
                        queryable = queryable.Where(q => q.Path.StartsWith(OrganizationDAO.Path));
                    }
                    if (OrganizationFilter.Id.NotEqual != null)
                    {
                        OrganizationDAO OrganizationDAO = DataContext.Organization
                            .Where(o => o.Id == OrganizationFilter.Id.NotEqual.Value).FirstOrDefaultWithNoLock();
                        queryable = queryable.Where(q => !q.Path.StartsWith(OrganizationDAO.Path));
                    }
                    if (OrganizationFilter.Id.In != null)
                    {
                        List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                           .Where(o => o.DeletedAt == null && o.StatusId == 1).ToListWithNoLock();
                        List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => OrganizationFilter.Id.In.Contains(o.Id)).ToList();
                        List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> Ids = Branches.Select(o => o.Id).ToList();
                        IdFilter IdFilter = new IdFilter { In = Ids };
                        queryable = queryable.Where(x => x.Id, IdFilter);
                    }
                    if (OrganizationFilter.Id.NotIn != null)
                    {
                        List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                           .Where(o => o.DeletedAt == null && o.StatusId == 1).ToListWithNoLock();
                        List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => OrganizationFilter.Id.NotIn.Contains(o.Id)).ToList();
                        List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> Ids = Branches.Select(o => o.Id).ToList();
                        IdFilter IdFilter = new IdFilter { NotIn = Ids };
                        queryable = queryable.Where(x => x.Id, IdFilter);
                    }
                }
                queryable = queryable.Where(q => q.Code, OrganizationFilter.Code);
                queryable = queryable.Where(q => q.Name, OrganizationFilter.Name);
                if (OrganizationFilter.ParentId != null)
                {
                    if (OrganizationFilter.ParentId.Equal != null)
                    {
                        OrganizationDAO OrganizationDAO = DataContext.Organization
                            .Where(o => o.Id == OrganizationFilter.ParentId.Equal.Value).FirstOrDefaultWithNoLock();
                        queryable = queryable.Where(q => q.Path.StartsWith(OrganizationDAO.Path));
                    }
                    if (OrganizationFilter.ParentId.NotEqual != null)
                    {
                        OrganizationDAO OrganizationDAO = DataContext.Organization
                            .Where(o => o.Id == OrganizationFilter.ParentId.NotEqual.Value).FirstOrDefaultWithNoLock();
                        queryable = queryable.Where(q => !q.Path.StartsWith(OrganizationDAO.Path));
                    }
                    if (OrganizationFilter.ParentId.In != null)
                    {
                        List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                           .Where(o => o.DeletedAt == null && o.StatusId == 1).ToListWithNoLock();
                        List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => OrganizationFilter.Id.In.Contains(o.Id)).ToList();
                        List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> Ids = Branches.Select(o => o.Id).ToList();
                        IdFilter IdFilter = new IdFilter { In = Ids };
                        queryable = queryable.Where(q => q.ParentId.HasValue).Where(x => x.ParentId, IdFilter);
                    }
                    if (OrganizationFilter.ParentId.NotIn != null)
                    {
                        List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                           .Where(o => o.DeletedAt == null && o.StatusId == 1).ToListWithNoLock();
                        List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => OrganizationFilter.ParentId.NotIn.Contains(o.Id)).ToList();
                        List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> Ids = Branches.Select(o => o.Id).ToList();
                        IdFilter IdFilter = new IdFilter { NotIn = Ids };
                        queryable = queryable.Where(q => q.ParentId.HasValue).Where(x => x.ParentId, IdFilter);
                    }
                }
                queryable = queryable.Where(q => q.Path, OrganizationFilter.Path);
                queryable = queryable.Where(q => q.Level, OrganizationFilter.Level);
                queryable = queryable.Where(q => q.StatusId, OrganizationFilter.StatusId);
                queryable = queryable.Where(q => q.Phone, OrganizationFilter.Phone);
                queryable = queryable.Where(q => q.Email, OrganizationFilter.Email);
                queryable = queryable.Where(q => q.Address, OrganizationFilter.Address);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<OrganizationDAO> DynamicOrder(IQueryable<OrganizationDAO> query, OrganizationFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case OrganizationOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case OrganizationOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case OrganizationOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case OrganizationOrder.Parent:
                            query = query.OrderBy(q => q.ParentId);
                            break;
                        case OrganizationOrder.Path:
                            query = query.OrderBy(q => q.Path);
                            break;
                        case OrganizationOrder.Level:
                            query = query.OrderBy(q => q.Level);
                            break;
                        case OrganizationOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                        case OrganizationOrder.Phone:
                            query = query.OrderBy(q => q.Phone);
                            break;
                        case OrganizationOrder.Address:
                            query = query.OrderBy(q => q.Address);
                            break;
                        case OrganizationOrder.Email:
                            query = query.OrderBy(q => q.Email);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case OrganizationOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case OrganizationOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case OrganizationOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case OrganizationOrder.Parent:
                            query = query.OrderByDescending(q => q.ParentId);
                            break;
                        case OrganizationOrder.Path:
                            query = query.OrderByDescending(q => q.Path);
                            break;
                        case OrganizationOrder.Level:
                            query = query.OrderByDescending(q => q.Level);
                            break;
                        case OrganizationOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                        case OrganizationOrder.Phone:
                            query = query.OrderByDescending(q => q.Phone);
                            break;
                        case OrganizationOrder.Address:
                            query = query.OrderByDescending(q => q.Address);
                            break;
                        case OrganizationOrder.Email:
                            query = query.OrderByDescending(q => q.Email);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<Organization>> DynamicSelect(IQueryable<OrganizationDAO> query, OrganizationFilter filter)
        {
            List<Organization> Organizations = await query.Select(q => new Organization()
            {
                Id = filter.Selects.Contains(OrganizationSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(OrganizationSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(OrganizationSelect.Name) ? q.Name : default(string),
                ParentId = filter.Selects.Contains(OrganizationSelect.Parent) ? q.ParentId : default(long?),
                Path = filter.Selects.Contains(OrganizationSelect.Path) ? q.Path : default(string),
                Level = filter.Selects.Contains(OrganizationSelect.Level) ? q.Level : default(long),
                StatusId = filter.Selects.Contains(OrganizationSelect.Status) ? q.StatusId : default(long),
                Phone = filter.Selects.Contains(OrganizationSelect.Phone) ? q.Phone : default(string),
                Address = filter.Selects.Contains(OrganizationSelect.Address) ? q.Address : default(string),
                Email = filter.Selects.Contains(OrganizationSelect.Email) ? q.Email : default(string),
                IsDisplay = filter.Selects.Contains(OrganizationSelect.IsDisplay) ? q.IsDisplay : default(bool),
                UpdatedAt = filter.Selects.Contains(OrganizationSelect.UpdatedAt) ? q.UpdatedAt : default(DateTime),
                CreatedAt = filter.Selects.Contains(OrganizationSelect.CreatedAt) ? q.CreatedAt : default(DateTime),
                DeletedAt = filter.Selects.Contains(OrganizationSelect.DeletedAt) ? q.DeletedAt : null,
                Parent = filter.Selects.Contains(OrganizationSelect.Parent) && q.Parent != null ? new Organization
                {
                    Id = q.Parent.Id,
                    Code = q.Parent.Code,
                    Name = q.Parent.Name,
                    ParentId = q.Parent.ParentId,
                    Path = q.Parent.Path,
                    Level = q.Parent.Level,
                    StatusId = q.Parent.StatusId,
                    Phone = q.Parent.Phone,
                    Address = q.Parent.Address,
                    IsDisplay = q.Parent.IsDisplay,
                    Email = q.Parent.Email,
                } : null,
                Status = filter.Selects.Contains(OrganizationSelect.Status) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                } : null,
                RowId = filter.Selects.Contains(OrganizationSelect.RowId) ? q.RowId : default(Guid)
            }).ToListWithNoLockAsync();
            return Organizations;
        }

        public async Task<int> Count(OrganizationFilter filter)
        {
            IQueryable<OrganizationDAO> Organizations = DataContext.Organization;
            Organizations = DynamicFilter(Organizations, filter);
            Organizations = OrFilter(Organizations, filter);
            return await Organizations.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(OrganizationFilter filter)
        {
            IQueryable<OrganizationDAO> Organizations = DataContext.Organization;
            Organizations = DynamicFilter(Organizations, filter);
            return await Organizations.CountWithNoLockAsync();
        }

        public async Task<List<Organization>> List(OrganizationFilter filter)
        {
            if (filter == null) return new List<Organization>();
            IQueryable<OrganizationDAO> OrganizationDAOs = DataContext.Organization.AsNoTracking();
            OrganizationDAOs = DynamicFilter(OrganizationDAOs, filter);
            OrganizationDAOs = OrFilter(OrganizationDAOs, filter);
            OrganizationDAOs = DynamicOrder(OrganizationDAOs, filter);
            List<Organization> Organizations = await DynamicSelect(OrganizationDAOs, filter);
            return Organizations;
        }

        public async Task<List<Organization>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };
            List<Organization> Organizations = await DataContext.Organization.AsNoTracking()
            .Where(x => x.Id, IdFilter)
            .Select(x => new Organization()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                ParentId = x.ParentId,
                Path = x.Path,
                Level = x.Level,
                StatusId = x.StatusId,
                Phone = x.Phone,
                Email = x.Email,
                Address = x.Address,
                RowId = x.RowId,
                Parent = x.Parent == null ? null : new Organization
                {
                    Id = x.Parent.Id,
                    Code = x.Parent.Code,
                    Name = x.Parent.Name,
                    ParentId = x.Parent.ParentId,
                    Path = x.Parent.Path,
                    Level = x.Parent.Level,
                    StatusId = x.Parent.StatusId,
                    Phone = x.Parent.Phone,
                    Email = x.Parent.Email,
                    Address = x.Parent.Address,
                    RowId = x.Parent.RowId,
                },
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                },
            }).ToListWithNoLockAsync();

            var AppUsers = await DataContext.AppUser.Where(x => x.OrganizationId, IdFilter).ToListWithNoLockAsync();
            foreach (var Organization in Organizations)
            {
                Organization.AppUsers = AppUsers.Where(x => x.OrganizationId == Organization.Id).Select(x => new AppUser
                {
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    DeletedAt = x.DeletedAt,
                    Id = x.Id,
                    Username = x.Username,
                    DisplayName = x.DisplayName,
                    Address = x.Address,
                    Email = x.Email,
                    Phone = x.Phone,
                    SexId = x.SexId,
                    Birthday = x.Birthday,
                    Avatar = x.Avatar,
                    Department = x.Department,
                    OrganizationId = x.OrganizationId,
                    StatusId = x.StatusId,
                    RowId = x.RowId,
                }).ToList();
            }
            return Organizations;
        }

        public async Task<Organization> Get(long Id)
        {
            Organization Organization = await DataContext.Organization
                .AsNoTracking()
                .Where(x => x.Id == Id).Select(x => new Organization()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    ParentId = x.ParentId,
                    Path = x.Path,
                    Level = x.Level,
                    StatusId = x.StatusId,
                    Phone = x.Phone,
                    Address = x.Address,
                    Email = x.Email,
                    IsDisplay = x.IsDisplay,
                    Parent = x.Parent == null ? null : new Organization
                    {
                        Id = x.Parent.Id,
                        Code = x.Parent.Code,
                        Name = x.Parent.Name,
                        ParentId = x.Parent.ParentId,
                        Path = x.Parent.Path,
                        Level = x.Parent.Level,
                        StatusId = x.Parent.StatusId,
                        Phone = x.Parent.Phone,
                        Address = x.Parent.Address,
                        Email = x.Parent.Email,
                    },
                    Status = x.Status == null ? null : new Status
                    {
                        Id = x.Status.Id,
                        Code = x.Status.Code,
                        Name = x.Status.Name,
                    },
                }).FirstOrDefaultWithNoLockAsync();

            if (Organization == null)
                return null;
            Organization.AppUsers = await DataContext.AppUser
                .Where(x => x.OrganizationId == Organization.Id)
                .Select(x => new AppUser
                {
                    Id = x.Id,
                    Username = x.Username,
                    DisplayName = x.DisplayName,
                    Address = x.Address,
                    Email = x.Email,
                    Phone = x.Phone,
                    Department = x.Department,
                    OrganizationId = x.OrganizationId,
                    SexId = x.SexId,
                    StatusId = x.StatusId,
                    Sex = new Sex
                    {
                        Id = x.Sex.Id,
                        Code = x.Sex.Code,
                        Name = x.Sex.Name,
                    },
                    Status = new Status
                    {
                        Id = x.Status.Id,
                        Code = x.Status.Code,
                        Name = x.Status.Name,
                    },
                }).ToListWithNoLockAsync();

            return Organization;
        }

        public async Task<bool> UpdateIsDisplay(Organization Organization)
        {
            var Path = await DataContext.Organization.Where(x => x.Id == Organization.Id).Select(x => x.Path).FirstOrDefaultWithNoLockAsync();
            if (!string.IsNullOrWhiteSpace(Path))
            {
                await DataContext.Organization.Where(x => x.Path.StartsWith(Path)).UpdateFromQueryAsync(x => new OrganizationDAO
                {
                    IsDisplay = Organization.IsDisplay,
                    UpdatedAt = StaticParams.DateTimeNow
                });
            }
            await DataContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> BulkMerge(List<Organization> Organizations)
        {
            var AppUsers = Organizations.Where(x => x.AppUsers != null).SelectMany(x => x.AppUsers).ToList();
            var AppUserIds = AppUsers.Select(x => x.Id).ToList();
            IdFilter AppUserIdFilter = new IdFilter { In = AppUserIds };
            var AppUserDAOs = await DataContext.AppUser
                .Where(x => x.Id, AppUserIdFilter)
                .ToListWithNoLockAsync();

            var OrganizationInDBs = await DataContext.Organization.Where(x => x.DeletedAt == null).ToListWithNoLockAsync();
            List<OrganizationDAO> OrganizationDAOs = new List<OrganizationDAO>();
            foreach (var Organization in Organizations)
            {
                var oldData = OrganizationInDBs.Where(x => x.Id == Organization.Id).FirstOrDefault();
                if (oldData == null)
                {
                    OrganizationDAO OrganizationDAO = new OrganizationDAO
                    {
                        Id = Organization.Id,
                        Code = Organization.Code,
                        Name = Organization.Name,
                        Address = Organization.Address,
                        CreatedAt = Organization.CreatedAt,
                        UpdatedAt = Organization.UpdatedAt,
                        DeletedAt = Organization.DeletedAt,
                        Email = Organization.Email,
                        Level = Organization.Level,
                        ParentId = Organization.ParentId,
                        Path = Organization.Path,
                        Phone = Organization.Phone,
                        RowId = Organization.RowId,
                        StatusId = Organization.StatusId,
                        IsDisplay = true
                    };
                    OrganizationDAOs.Add(OrganizationDAO);
                }
                else
                {
                    oldData.Code = Organization.Code;
                    oldData.Name = Organization.Name;
                    oldData.Address = Organization.Address;
                    oldData.CreatedAt = Organization.CreatedAt;
                    oldData.UpdatedAt = Organization.UpdatedAt;
                    oldData.DeletedAt = Organization.DeletedAt;
                    oldData.Email = Organization.Email;
                    oldData.Level = Organization.Level;
                    oldData.ParentId = Organization.ParentId;
                    oldData.Path = Organization.Path;
                    oldData.Phone = Organization.Phone;
                    oldData.StatusId = Organization.StatusId;
                    OrganizationDAOs.Add(oldData);
                }
            }

            await DataContext.Organization.BulkMergeAsync(OrganizationDAOs);

            foreach (var AppUserDAO in AppUserDAOs)
            {
                AppUserDAO.OrganizationId = AppUsers.Where(x => x.Id == AppUserDAO.Id).Select(x => x.OrganizationId).FirstOrDefault();
            }
            await DataContext.SaveChangesAsync();
            return true;
        }
    }
}
