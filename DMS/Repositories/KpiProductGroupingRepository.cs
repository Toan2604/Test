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
    public interface IKpiProductGroupingRepository
    {
        Task<int> Count(KpiProductGroupingFilter KpiProductGroupingFilter);
        Task<int> CountAll(KpiProductGroupingFilter KpiProductGroupingFilter);
        Task<List<KpiProductGrouping>> List(KpiProductGroupingFilter KpiProductGroupingFilter);
        Task<List<KpiProductGrouping>> List(List<long> Ids);
        Task<KpiProductGrouping> Get(long Id);
        Task<bool> Create(KpiProductGrouping KpiProductGrouping);
        Task<bool> Update(KpiProductGrouping KpiProductGrouping);
        Task<bool> Delete(KpiProductGrouping KpiProductGrouping);
        Task<bool> BulkMerge(List<KpiProductGrouping> KpiProductGroupings);
        Task<bool> BulkDelete(List<KpiProductGrouping> KpiProductGroupings);
    }
    public class KpiProductGroupingRepository : IKpiProductGroupingRepository
    {
        private DataContext DataContext;
        public KpiProductGroupingRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<KpiProductGroupingDAO> DynamicFilter(IQueryable<KpiProductGroupingDAO> query, KpiProductGroupingFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.CreatedAt, filter.CreatedAt);
            query = query.Where(q => q.UpdatedAt, filter.UpdatedAt);
            query = query.Where(q => q.Id, filter.Id);
            if (filter.OrganizationId != null)
            {
                if (filter.OrganizationId.Equal != null)
                {
                    OrganizationDAO OrganizationDAO = DataContext.Organization
                        .Where(o => o.Id == filter.OrganizationId.Equal.Value).FirstOrDefault();
                    query = query.Where(q => q.Organization.Path.StartsWith(OrganizationDAO.Path));
                }
                if (filter.OrganizationId.NotEqual != null)
                {
                    OrganizationDAO OrganizationDAO = DataContext.Organization
                        .Where(o => o.Id == filter.OrganizationId.NotEqual.Value).FirstOrDefault();
                    query = query.Where(q => !q.Organization.Path.StartsWith(OrganizationDAO.Path));
                }
                if (filter.OrganizationId.In != null)
                {
                    List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                        .Where(o => o.DeletedAt == null && o.StatusId == 1).ToList();
                    List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => filter.OrganizationId.In.Contains(o.Id)).ToList();
                    List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> Ids = Branches.Select(o => o.Id).ToList();
                    IdFilter IdFilter = new IdFilter { In = Ids };
                    query = query.Where(x => x.OrganizationId, IdFilter);
                }
                if (filter.OrganizationId.NotIn != null)
                {
                    List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                        .Where(o => o.DeletedAt == null && o.StatusId == 1).ToList();
                    List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => filter.OrganizationId.NotIn.Contains(o.Id)).ToList();
                    List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> Ids = Branches.Select(o => o.Id).ToList();
                    IdFilter IdFilter = new IdFilter { NotIn = Ids };
                    query = query.Where(x => x.OrganizationId, IdFilter);
                }
            }
            query = query.Where(q => q.KpiYearId, filter.KpiYearId);
            query = query.Where(q => q.KpiPeriodId, filter.KpiPeriodId);
            query = query.Where(q => q.KpiProductGroupingTypeId, filter.KpiProductGroupingTypeId);
            query = query.Where(q => q.StatusId, filter.StatusId);
            query = query.Where(q => q.EmployeeId, filter.EmployeeId);
            query = query.Where(q => q.CreatorId, filter.CreatorId);
            return query;
        }

        private IQueryable<KpiProductGroupingDAO> OrFilter(IQueryable<KpiProductGroupingDAO> query, KpiProductGroupingFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<KpiProductGroupingDAO> initQuery = query.Where(q => false);
            foreach (KpiProductGroupingFilter KpiProductGroupingFilter in filter.OrFilter)
            {
                IQueryable<KpiProductGroupingDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, filter.Id);
                if (KpiProductGroupingFilter.OrganizationId != null)
                {
                    if (KpiProductGroupingFilter.OrganizationId.Equal != null)
                    {
                        OrganizationDAO OrganizationDAO = DataContext.Organization
                            .Where(o => o.Id == KpiProductGroupingFilter.OrganizationId.Equal.Value).FirstOrDefault();
                        queryable = queryable.Where(q => q.Organization.Path.StartsWith(OrganizationDAO.Path));
                    }
                    if (KpiProductGroupingFilter.OrganizationId.NotEqual != null)
                    {
                        OrganizationDAO OrganizationDAO = DataContext.Organization
                            .Where(o => o.Id == KpiProductGroupingFilter.OrganizationId.NotEqual.Value).FirstOrDefault();
                        queryable = queryable.Where(q => !q.Organization.Path.StartsWith(OrganizationDAO.Path));
                    }
                    if (KpiProductGroupingFilter.OrganizationId.In != null)
                    {
                        List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                            .Where(o => o.DeletedAt == null && o.StatusId == 1).ToList();
                        List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => KpiProductGroupingFilter.OrganizationId.In.Contains(o.Id)).ToList();
                        List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> Ids = Branches.Select(o => o.Id).ToList();
                        IdFilter IdFilter = new IdFilter { In = Ids };
                        queryable = queryable.Where(x => x.OrganizationId, IdFilter);
                    }
                    if (KpiProductGroupingFilter.OrganizationId.NotIn != null)
                    {
                        List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                            .Where(o => o.DeletedAt == null && o.StatusId == 1).ToList();
                        List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => KpiProductGroupingFilter.OrganizationId.NotIn.Contains(o.Id)).ToList();
                        List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> Ids = Branches.Select(o => o.Id).ToList();
                        IdFilter IdFilter = new IdFilter { NotIn = Ids };
                        queryable = queryable.Where(x => x.OrganizationId, IdFilter);
                    }
                }
                queryable = queryable.Where(q => q.KpiYearId, filter.KpiYearId);
                queryable = queryable.Where(q => q.KpiPeriodId, filter.KpiPeriodId);
                queryable = queryable.Where(q => q.KpiProductGroupingTypeId, filter.KpiProductGroupingTypeId);
                queryable = queryable.Where(q => q.StatusId, filter.StatusId);
                queryable = queryable.Where(q => q.EmployeeId, filter.EmployeeId);
                queryable = queryable.Where(q => q.CreatorId, filter.CreatorId);
                queryable = queryable.Where(q => q.RowId, filter.RowId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<KpiProductGroupingDAO> DynamicOrder(IQueryable<KpiProductGroupingDAO> query, KpiProductGroupingFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case KpiProductGroupingOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case KpiProductGroupingOrder.Organization:
                            query = query.OrderBy(q => q.OrganizationId);
                            break;
                        case KpiProductGroupingOrder.KpiYear:
                            query = query.OrderBy(q => q.KpiYearId);
                            break;
                        case KpiProductGroupingOrder.KpiPeriod:
                            query = query.OrderBy(q => q.KpiPeriodId);
                            break;
                        case KpiProductGroupingOrder.KpiProductGroupingType:
                            query = query.OrderBy(q => q.KpiProductGroupingTypeId);
                            break;
                        case KpiProductGroupingOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                        case KpiProductGroupingOrder.Employee:
                            query = query.OrderBy(q => q.EmployeeId);
                            break;
                        case KpiProductGroupingOrder.Creator:
                            query = query.OrderBy(q => q.CreatorId);
                            break;
                        case KpiProductGroupingOrder.Row:
                            query = query.OrderBy(q => q.RowId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case KpiProductGroupingOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case KpiProductGroupingOrder.Organization:
                            query = query.OrderByDescending(q => q.OrganizationId);
                            break;
                        case KpiProductGroupingOrder.KpiYear:
                            query = query.OrderByDescending(q => q.KpiYearId);
                            break;
                        case KpiProductGroupingOrder.KpiPeriod:
                            query = query.OrderByDescending(q => q.KpiPeriodId);
                            break;
                        case KpiProductGroupingOrder.KpiProductGroupingType:
                            query = query.OrderByDescending(q => q.KpiProductGroupingTypeId);
                            break;
                        case KpiProductGroupingOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                        case KpiProductGroupingOrder.Employee:
                            query = query.OrderByDescending(q => q.EmployeeId);
                            break;
                        case KpiProductGroupingOrder.Creator:
                            query = query.OrderByDescending(q => q.CreatorId);
                            break;
                        case KpiProductGroupingOrder.Row:
                            query = query.OrderByDescending(q => q.RowId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<KpiProductGrouping>> DynamicSelect(IQueryable<KpiProductGroupingDAO> query, KpiProductGroupingFilter filter)
        {
            List<KpiProductGrouping> KpiProductGroupings = await query.Select(q => new KpiProductGrouping()
            {
                Id = filter.Selects.Contains(KpiProductGroupingSelect.Id) ? q.Id : default(long),
                OrganizationId = filter.Selects.Contains(KpiProductGroupingSelect.Organization) ? q.OrganizationId : default(long),
                KpiYearId = filter.Selects.Contains(KpiProductGroupingSelect.KpiYear) ? q.KpiYearId : default(long),
                KpiPeriodId = filter.Selects.Contains(KpiProductGroupingSelect.KpiPeriod) ? q.KpiPeriodId : default(long),
                KpiProductGroupingTypeId = filter.Selects.Contains(KpiProductGroupingSelect.KpiProductGroupingType) ? q.KpiProductGroupingTypeId : default(long),
                StatusId = filter.Selects.Contains(KpiProductGroupingSelect.Status) ? q.StatusId : default(long),
                EmployeeId = filter.Selects.Contains(KpiProductGroupingSelect.Employee) ? q.EmployeeId : default(long),
                CreatorId = filter.Selects.Contains(KpiProductGroupingSelect.Creator) ? q.CreatorId : default(long),
                RowId = filter.Selects.Contains(KpiProductGroupingSelect.Row) ? q.RowId : default(Guid),
                Creator = filter.Selects.Contains(KpiProductGroupingSelect.Creator) && q.Creator != null ? new AppUser
                {
                    Id = q.Creator.Id,
                    Username = q.Creator.Username,
                    DisplayName = q.Creator.DisplayName,
                    Address = q.Creator.Address,
                    Email = q.Creator.Email,
                    Phone = q.Creator.Phone,
                    SexId = q.Creator.SexId,
                    Birthday = q.Creator.Birthday,
                    Avatar = q.Creator.Avatar,
                    PositionId = q.Creator.PositionId,
                    Department = q.Creator.Department,
                    OrganizationId = q.Creator.OrganizationId,
                    ProvinceId = q.Creator.ProvinceId,
                    StatusId = q.Creator.StatusId,
                    RowId = q.Creator.RowId,
                } : null,
                Employee = filter.Selects.Contains(KpiProductGroupingSelect.Employee) && q.Employee != null ? new AppUser
                {
                    Id = q.Employee.Id,
                    Username = q.Employee.Username,
                    DisplayName = q.Employee.DisplayName,
                    Address = q.Employee.Address,
                    Email = q.Employee.Email,
                    Phone = q.Employee.Phone,
                    SexId = q.Employee.SexId,
                    Birthday = q.Employee.Birthday,
                    Avatar = q.Employee.Avatar,
                    PositionId = q.Employee.PositionId,
                    Department = q.Employee.Department,
                    OrganizationId = q.Employee.OrganizationId,
                    ProvinceId = q.Employee.ProvinceId,
                    StatusId = q.Employee.StatusId,
                    RowId = q.Employee.RowId,
                    Organization = new Organization
                    {
                        Id = q.Employee.Organization.Id,
                        Code = q.Employee.Organization.Code,
                        Name = q.Employee.Organization.Name,
                    }
                } : null,
                KpiPeriod = filter.Selects.Contains(KpiProductGroupingSelect.KpiPeriod) && q.KpiPeriod != null ? new KpiPeriod
                {
                    Id = q.KpiPeriod.Id,
                    Code = q.KpiPeriod.Code,
                    Name = q.KpiPeriod.Name,
                } : null,
                KpiYear = filter.Selects.Contains(KpiProductGroupingSelect.KpiYear) && q.KpiYear != null ? new KpiYear
                {
                    Id = q.KpiYear.Id,
                    Code = q.KpiYear.Code,
                    Name = q.KpiYear.Name,
                } : null,
                KpiProductGroupingType = filter.Selects.Contains(KpiProductGroupingSelect.KpiProductGroupingType) && q.KpiProductGroupingType != null ? new KpiProductGroupingType
                {
                    Id = q.KpiProductGroupingType.Id,
                    Code = q.KpiProductGroupingType.Code,
                    Name = q.KpiProductGroupingType.Name,
                } : null,
                Organization = filter.Selects.Contains(KpiProductGroupingSelect.Organization) && q.Organization != null ? new Organization
                {
                    Id = q.Organization.Id,
                    Code = q.Organization.Code,
                    Name = q.Organization.Name,
                    ParentId = q.Organization.ParentId,
                    Path = q.Organization.Path,
                    Level = q.Organization.Level,
                    StatusId = q.Organization.StatusId,
                    Phone = q.Organization.Phone,
                    Email = q.Organization.Email,
                    Address = q.Organization.Address,
                } : null,
                Status = filter.Selects.Contains(KpiProductGroupingSelect.Status) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                } : null,
                CreatedAt = q.CreatedAt,
                UpdatedAt = q.UpdatedAt,
            }).ToListWithNoLockAsync();
            return KpiProductGroupings;
        }

        public async Task<int> Count(KpiProductGroupingFilter filter)
        {
            IQueryable<KpiProductGroupingDAO> KpiProductGroupings = DataContext.KpiProductGrouping.AsNoTracking();
            KpiProductGroupings = DynamicFilter(KpiProductGroupings, filter);
            KpiProductGroupings = OrFilter(KpiProductGroupings, filter);
            return await KpiProductGroupings.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(KpiProductGroupingFilter filter)
        {
            IQueryable<KpiProductGroupingDAO> KpiProductGroupings = DataContext.KpiProductGrouping.AsNoTracking();
            KpiProductGroupings = DynamicFilter(KpiProductGroupings, filter);
            return await KpiProductGroupings.CountWithNoLockAsync();
        }
        public async Task<List<KpiProductGrouping>> List(KpiProductGroupingFilter filter)
        {
            if (filter == null) return new List<KpiProductGrouping>();
            IQueryable<KpiProductGroupingDAO> KpiProductGroupingDAOs = DataContext.KpiProductGrouping.AsNoTracking();
            KpiProductGroupingDAOs = DynamicFilter(KpiProductGroupingDAOs, filter);
            KpiProductGroupingDAOs = OrFilter(KpiProductGroupingDAOs, filter);
            KpiProductGroupingDAOs = DynamicOrder(KpiProductGroupingDAOs, filter);
            List<KpiProductGrouping> KpiProductGroupings = await DynamicSelect(KpiProductGroupingDAOs, filter);
            return KpiProductGroupings;
        }

        public async Task<List<KpiProductGrouping>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };
            List<KpiProductGrouping> KpiProductGroupings = await DataContext.KpiProductGrouping.AsNoTracking()
            .Where(x => x.Id, IdFilter)
            .Select(x => new KpiProductGrouping()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                Id = x.Id,
                OrganizationId = x.OrganizationId,
                KpiYearId = x.KpiYearId,
                KpiPeriodId = x.KpiPeriodId,
                KpiProductGroupingTypeId = x.KpiProductGroupingTypeId,
                StatusId = x.StatusId,
                EmployeeId = x.EmployeeId,
                CreatorId = x.CreatorId,
                RowId = x.RowId,
                Creator = x.Creator == null ? null : new AppUser
                {
                    Id = x.Creator.Id,
                    Username = x.Creator.Username,
                    DisplayName = x.Creator.DisplayName,
                    Address = x.Creator.Address,
                    Email = x.Creator.Email,
                    Phone = x.Creator.Phone,
                    SexId = x.Creator.SexId,
                    Birthday = x.Creator.Birthday,
                    Avatar = x.Creator.Avatar,
                    PositionId = x.Creator.PositionId,
                    Department = x.Creator.Department,
                    OrganizationId = x.Creator.OrganizationId,
                    ProvinceId = x.Creator.ProvinceId,
                    StatusId = x.Creator.StatusId,
                    RowId = x.Creator.RowId,
                },
                Employee = x.Employee == null ? null : new AppUser
                {
                    Id = x.Employee.Id,
                    Username = x.Employee.Username,
                    DisplayName = x.Employee.DisplayName,
                    Address = x.Employee.Address,
                    Email = x.Employee.Email,
                    Phone = x.Employee.Phone,
                    SexId = x.Employee.SexId,
                    Birthday = x.Employee.Birthday,
                    Avatar = x.Employee.Avatar,
                    PositionId = x.Employee.PositionId,
                    Department = x.Employee.Department,
                    OrganizationId = x.Employee.OrganizationId,
                    ProvinceId = x.Employee.ProvinceId,
                    StatusId = x.Employee.StatusId,
                    RowId = x.Employee.RowId,
                },
                KpiPeriod = x.KpiPeriod == null ? null : new KpiPeriod
                {
                    Id = x.KpiPeriod.Id,
                    Code = x.KpiPeriod.Code,
                    Name = x.KpiPeriod.Name,
                },
                KpiProductGroupingType = x.KpiProductGroupingType == null ? null : new KpiProductGroupingType
                {
                    Id = x.KpiProductGroupingType.Id,
                    Code = x.KpiProductGroupingType.Code,
                    Name = x.KpiProductGroupingType.Name,
                },
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                },
            }).ToListWithNoLockAsync();


            return KpiProductGroupings;
        }

        public async Task<KpiProductGrouping> Get(long Id)
        {
            KpiProductGrouping KpiProductGrouping = await DataContext.KpiProductGrouping.AsNoTracking()
            .Where(x => x.Id == Id)
            .Where(x => x.DeletedAt == null)
            .Select(x => new KpiProductGrouping()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Id = x.Id,
                OrganizationId = x.OrganizationId,
                KpiYearId = x.KpiYearId,
                KpiPeriodId = x.KpiPeriodId,
                KpiProductGroupingTypeId = x.KpiProductGroupingTypeId,
                StatusId = x.StatusId,
                EmployeeId = x.EmployeeId,
                CreatorId = x.CreatorId,
                RowId = x.RowId,
                Creator = x.Creator == null ? null : new AppUser
                {
                    Id = x.Creator.Id,
                    Username = x.Creator.Username,
                    DisplayName = x.Creator.DisplayName,
                },
                Employee = x.Employee == null ? null : new AppUser
                {
                    Id = x.Employee.Id,
                    Username = x.Employee.Username,
                    DisplayName = x.Employee.DisplayName,
                },
                KpiPeriod = x.KpiPeriod == null ? null : new KpiPeriod
                {
                    Id = x.KpiPeriod.Id,
                    Code = x.KpiPeriod.Code,
                    Name = x.KpiPeriod.Name,
                },
                KpiYear = x.KpiYear == null ? null : new KpiYear
                {
                    Id = x.KpiYear.Id,
                    Code = x.KpiYear.Code,
                    Name = x.KpiYear.Name,
                },
                KpiProductGroupingType = x.KpiProductGroupingType == null ? null : new KpiProductGroupingType
                {
                    Id = x.KpiProductGroupingType.Id,
                    Code = x.KpiProductGroupingType.Code,
                    Name = x.KpiProductGroupingType.Name,
                },
                Organization = x.Organization == null ? null : new Organization
                {
                    Id = x.Organization.Id,
                    Code = x.Organization.Code,
                    Name = x.Organization.Name,
                    ParentId = x.Organization.ParentId,
                    Path = x.Organization.Path,
                    Level = x.Organization.Level,
                    StatusId = x.Organization.StatusId,
                    Phone = x.Organization.Phone,
                    Email = x.Organization.Email,
                    Address = x.Organization.Address,
                },
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (KpiProductGrouping == null)
                return null;

            KpiProductGrouping.KpiProductGroupingContents = await DataContext.KpiProductGroupingContent.AsNoTracking()
                .Where(x => x.KpiProductGroupingId == KpiProductGrouping.Id)
                .Select(c => new KpiProductGroupingContent
                {
                    Id = c.Id,
                    KpiProductGroupingId = c.ProductGroupingId,
                    ProductGroupingId = c.ProductGroupingId,
                    SelectAllCurrentItem = c.SelectAllCurrentItem,
                    RowId = c.RowId,
                    ProductGrouping = new ProductGrouping
                    {
                        Id = c.ProductGrouping.Id,
                        Code = c.ProductGrouping.Code,
                        Name = c.ProductGrouping.Name,
                        Description = c.ProductGrouping.Description,
                        ParentId = c.ProductGrouping.ParentId,
                        Path = c.ProductGrouping.Path,
                        Level = c.ProductGrouping.Level,
                    }
                })
                .ToListWithNoLockAsync();

            var ContentIds = KpiProductGrouping.KpiProductGroupingContents
                .Select(x => x.Id)
                .ToList();
            IdFilter KpiProductGroupingContentIdFilter = new IdFilter { In = ContentIds };
            List<KpiProductGroupingContentCriteriaMapping> KpiProductGroupingContentCriteriaMappings = await DataContext.KpiProductGroupingContentCriteriaMapping.AsNoTracking()
                .Where(x => x.KpiProductGroupingContentId, KpiProductGroupingContentIdFilter)
                .Select(x => new KpiProductGroupingContentCriteriaMapping
                {
                    KpiProductGroupingContentId = x.KpiProductGroupingContentId,
                    KpiProductGroupingCriteriaId = x.KpiProductGroupingCriteriaId,
                    Value = x.Value,
                    KpiProductGroupingContent = new KpiProductGroupingContent
                    {
                        Id = x.KpiProductGroupingContent.Id,
                        KpiProductGroupingId = x.KpiProductGroupingContent.KpiProductGroupingId,
                        ProductGroupingId = x.KpiProductGroupingContent.ProductGroupingId,
                    }
                }).ToListWithNoLockAsync();

            List<KpiProductGroupingContentItemMapping> KpiProductGroupingContentItemMappings = await DataContext.KpiProductGroupingContentItemMapping.AsNoTracking()
                .Where(x => x.KpiProductGroupingContentId, KpiProductGroupingContentIdFilter)
                .Select(x => new KpiProductGroupingContentItemMapping
                {
                    KpiProductGroupingContentId = x.KpiProductGroupingContentId,
                    ItemId = x.ItemId,
                    Item = new Item
                    {
                        Id = x.Item.Id,
                        Code = x.Item.Code,
                        Name = x.Item.Name,
                        ProductId = x.Item.ProductId,
                        StatusId = x.Item.StatusId,
                    }
                }).ToListWithNoLockAsync();
            foreach (KpiProductGroupingContent KpiProductGroupingContent in KpiProductGrouping.KpiProductGroupingContents)
            {
                KpiProductGroupingContent.KpiProductGroupingContentCriteriaMappings = KpiProductGroupingContentCriteriaMappings.Where(x => x.KpiProductGroupingContentId == KpiProductGroupingContent.Id).ToList();
                KpiProductGroupingContent.KpiProductGroupingContentItemMappings = KpiProductGroupingContentItemMappings.Where(x => x.KpiProductGroupingContentId == KpiProductGroupingContent.Id).ToList();
            }
            return KpiProductGrouping;
        }
        public async Task<bool> Create(KpiProductGrouping KpiProductGrouping)
        {
            KpiProductGroupingDAO KpiProductGroupingDAO = new KpiProductGroupingDAO();
            KpiProductGroupingDAO.Id = KpiProductGrouping.Id;
            KpiProductGroupingDAO.OrganizationId = KpiProductGrouping.OrganizationId;
            KpiProductGroupingDAO.KpiYearId = KpiProductGrouping.KpiYearId;
            KpiProductGroupingDAO.KpiPeriodId = KpiProductGrouping.KpiPeriodId;
            KpiProductGroupingDAO.KpiProductGroupingTypeId = KpiProductGrouping.KpiProductGroupingTypeId;
            KpiProductGroupingDAO.StatusId = KpiProductGrouping.StatusId;
            KpiProductGroupingDAO.EmployeeId = KpiProductGrouping.EmployeeId;
            KpiProductGroupingDAO.CreatorId = KpiProductGrouping.CreatorId;
            KpiProductGroupingDAO.RowId = Guid.NewGuid();
            KpiProductGroupingDAO.CreatedAt = StaticParams.DateTimeNow;
            KpiProductGroupingDAO.UpdatedAt = StaticParams.DateTimeNow;
            DataContext.KpiProductGrouping.Add(KpiProductGroupingDAO);
            await DataContext.SaveChangesAsync();
            KpiProductGrouping.Id = KpiProductGroupingDAO.Id;
            KpiProductGrouping.RowId = KpiProductGroupingDAO.RowId;
            await SaveReference(KpiProductGrouping);
            return true;
        }

        public async Task<bool> Update(KpiProductGrouping KpiProductGrouping)
        {
            KpiProductGroupingDAO KpiProductGroupingDAO = DataContext.KpiProductGrouping.Where(x => x.Id == KpiProductGrouping.Id).FirstOrDefault();
            if (KpiProductGroupingDAO == null)
                return false;
            KpiProductGroupingDAO.Id = KpiProductGrouping.Id;
            KpiProductGroupingDAO.OrganizationId = KpiProductGrouping.OrganizationId;
            KpiProductGroupingDAO.KpiYearId = KpiProductGrouping.KpiYearId;
            KpiProductGroupingDAO.KpiPeriodId = KpiProductGrouping.KpiPeriodId;
            KpiProductGroupingDAO.KpiProductGroupingTypeId = KpiProductGrouping.KpiProductGroupingTypeId;
            KpiProductGroupingDAO.StatusId = KpiProductGrouping.StatusId;
            KpiProductGroupingDAO.EmployeeId = KpiProductGrouping.EmployeeId;
            KpiProductGroupingDAO.CreatorId = KpiProductGrouping.CreatorId;
            KpiProductGroupingDAO.RowId = KpiProductGrouping.RowId;
            KpiProductGroupingDAO.UpdatedAt = StaticParams.DateTimeNow;
            await DataContext.SaveChangesAsync();
            await SaveReference(KpiProductGrouping);
            return true;
        }

        public async Task<bool> Delete(KpiProductGrouping KpiProductGrouping)
        {
            await DeleteReference(new List<KpiProductGrouping>
            {
                KpiProductGrouping
            });
            await DataContext.KpiProductGrouping
                .Where(x => x.Id == KpiProductGrouping.Id).UpdateFromQueryAsync(x => new KpiProductGroupingDAO { DeletedAt = StaticParams.DateTimeNow, UpdatedAt = StaticParams.DateTimeNow });

            return true;
        }

        public async Task<bool> BulkMerge(List<KpiProductGrouping> KpiProductGroupings)
        {
            try
            {
                List<KpiProductGroupingDAO> KpiProductGroupingDAOs = new List<KpiProductGroupingDAO>();
                foreach (KpiProductGrouping KpiProductGrouping in KpiProductGroupings)
                {
                    KpiProductGroupingDAO KpiProductGroupingDAO = new KpiProductGroupingDAO();
                    KpiProductGroupingDAO.Id = KpiProductGrouping.Id;
                    KpiProductGroupingDAO.OrganizationId = KpiProductGrouping.OrganizationId;
                    KpiProductGroupingDAO.KpiYearId = KpiProductGrouping.KpiYearId;
                    KpiProductGroupingDAO.KpiPeriodId = KpiProductGrouping.KpiPeriodId;
                    KpiProductGroupingDAO.KpiProductGroupingTypeId = KpiProductGrouping.KpiProductGroupingTypeId;
                    KpiProductGroupingDAO.StatusId = KpiProductGrouping.StatusId;
                    KpiProductGroupingDAO.EmployeeId = KpiProductGrouping.EmployeeId;
                    KpiProductGroupingDAO.CreatorId = KpiProductGrouping.CreatorId;
                    KpiProductGroupingDAO.RowId = KpiProductGrouping.RowId;
                    KpiProductGroupingDAO.CreatedAt = KpiProductGrouping.CreatedAt;
                    KpiProductGroupingDAO.UpdatedAt = StaticParams.DateTimeNow;
                    KpiProductGroupingDAOs.Add(KpiProductGroupingDAO);
                }
                await DataContext.BulkMergeAsync(KpiProductGroupingDAOs);
                await DeleteReference(KpiProductGroupings); // xóa references

                List<KpiProductGroupingContentDAO> KpiProductGroupingContentDAOs = new List<KpiProductGroupingContentDAO>();
                List<KpiProductGroupingContentCriteriaMappingDAO> KpiProductGroupingContentCriteriaMappingDAOs = new List<KpiProductGroupingContentCriteriaMappingDAO>();
                List<KpiProductGroupingContentItemMappingDAO> KpiProductGroupingContentItemMappingDAOs = new List<KpiProductGroupingContentItemMappingDAO>();
                foreach (var KpiProductGrouping in KpiProductGroupings)
                {
                    foreach (var content in KpiProductGrouping.KpiProductGroupingContents)
                    {
                        if (content.SelectAllCurrentItem && content.SelectAllCurrentItem == true)
                            content.KpiProductGroupingContentItemMappings = new List<KpiProductGroupingContentItemMapping>();
                    }
                    KpiProductGrouping.Id = KpiProductGroupingDAOs.Where(x => x.RowId == KpiProductGrouping.RowId)
                        .Select(x => x.Id)
                        .FirstOrDefault();
                    if (KpiProductGrouping.KpiProductGroupingContents != null && KpiProductGrouping.KpiProductGroupingContents.Any())
                    {
                        List<KpiProductGroupingContentDAO> ListContent = KpiProductGrouping.KpiProductGroupingContents.Select(x => new KpiProductGroupingContentDAO
                        {
                            KpiProductGroupingId = KpiProductGrouping.Id,
                            ProductGroupingId = x.ProductGroupingId,
                            SelectAllCurrentItem = x.SelectAllCurrentItem,
                            KpiProductGroupingContentCriteriaMappings = x.KpiProductGroupingContentCriteriaMappings.Select(a => new KpiProductGroupingContentCriteriaMappingDAO
                            {
                                KpiProductGroupingCriteriaId = a.KpiProductGroupingCriteriaId,
                                KpiProductGroupingContentId = a.KpiProductGroupingContentId,
                                Value = a.Value,
                            }).ToList(),
                            KpiProductGroupingContentItemMappings = x.KpiProductGroupingContentItemMappings.Select(i => new KpiProductGroupingContentItemMappingDAO
                            {
                                KpiProductGroupingContentId = i.KpiProductGroupingContentId,
                                ItemId = i.ItemId,
                            }).ToList(),
                            RowId = Guid.NewGuid(),
                        }).ToList();
                        KpiProductGroupingContentDAOs.AddRange(ListContent);
                    }
                }
                await DataContext.KpiProductGroupingContent.BulkMergeAsync(KpiProductGroupingContentDAOs);

                foreach (KpiProductGroupingContentDAO KpiProductGroupingContentDAO in KpiProductGroupingContentDAOs)
                {
                    KpiProductGroupingContentDAO.Id = KpiProductGroupingContentDAOs
                        .Where(x => x.RowId == KpiProductGroupingContentDAO.RowId).Select(x => x.Id)
                        .FirstOrDefault();
                    if (KpiProductGroupingContentDAO.KpiProductGroupingContentCriteriaMappings != null && KpiProductGroupingContentDAO.KpiProductGroupingContentCriteriaMappings.Any())
                    {
                        var ListCriteriaMappings = KpiProductGroupingContentDAO.KpiProductGroupingContentCriteriaMappings
                            .Select(x => new KpiProductGroupingContentCriteriaMappingDAO
                            {
                                KpiProductGroupingContentId = KpiProductGroupingContentDAO.Id,
                                KpiProductGroupingCriteriaId = x.KpiProductGroupingCriteriaId,
                                Value = x.Value,
                            })
                            .ToList();
                        KpiProductGroupingContentCriteriaMappingDAOs.AddRange(ListCriteriaMappings);
                    }
                    if (KpiProductGroupingContentDAO.KpiProductGroupingContentItemMappings != null && KpiProductGroupingContentDAO.KpiProductGroupingContentItemMappings.Any())
                    {
                        var ListItemMappings = KpiProductGroupingContentDAO.KpiProductGroupingContentItemMappings
                            .Select(x => new KpiProductGroupingContentItemMappingDAO
                            {
                                KpiProductGroupingContentId = KpiProductGroupingContentDAO.Id,
                                ItemId = x.ItemId,
                            }).ToList();
                        foreach (var ListItemMapping in ListItemMappings)
                        {
                            KpiProductGroupingContentItemMappingDAO KpiProductGroupingContentItemMappingDAO = KpiProductGroupingContentItemMappingDAOs
                                .Where(x => x.KpiProductGroupingContentId == ListItemMapping.KpiProductGroupingContentId &&
                                        x.ItemId == ListItemMapping.ItemId).FirstOrDefault();
                            if (KpiProductGroupingContentItemMappingDAO == null)
                            {
                                KpiProductGroupingContentItemMappingDAO = new KpiProductGroupingContentItemMappingDAO
                                {
                                    KpiProductGroupingContentId = ListItemMapping.KpiProductGroupingContentId,
                                    ItemId = ListItemMapping.ItemId,
                                };
                                KpiProductGroupingContentItemMappingDAOs.Add(KpiProductGroupingContentItemMappingDAO);
                            }
                        }
                    }
                }
                await DataContext.KpiProductGroupingContentCriteriaMapping.BulkMergeAsync(KpiProductGroupingContentCriteriaMappingDAOs);
                await DataContext.KpiProductGroupingContentItemMapping.BulkMergeAsync(KpiProductGroupingContentItemMappingDAOs);

                return true;
            }
            catch (Exception Exception)
            {

                throw Exception;
            }
        }

        public async Task<bool> BulkDelete(List<KpiProductGrouping> KpiProductGroupings)
        {
            List<long> Ids = KpiProductGroupings.Select(x => x.Id).ToList();
            IdFilter IdFilter = new IdFilter { In = Ids };
            await DeleteReference(KpiProductGroupings);
            await DataContext.KpiProductGrouping
                .Where(x => IdFilter.In.Contains(x.Id))
                .UpdateFromQueryAsync(x => new KpiProductGroupingDAO { DeletedAt = StaticParams.DateTimeNow, UpdatedAt = StaticParams.DateTimeNow });
            return true;
        }

        private async Task SaveReference(KpiProductGrouping KpiProductGrouping)
        {
            try
            {
                await DeleteReference(new List<KpiProductGrouping> {
                        KpiProductGrouping
                    });

                #region thêm references mới
                List<KpiProductGroupingContentDAO> KpiProductGroupingContentDAOs = new List<KpiProductGroupingContentDAO>();
                List<KpiProductGroupingContentCriteriaMappingDAO> KpiProductGroupingContentCriteriaMappingDAOs = new List<KpiProductGroupingContentCriteriaMappingDAO>();
                List<KpiProductGroupingContentItemMappingDAO> KpiProductGroupingContentItemMappingDAOs = new List<KpiProductGroupingContentItemMappingDAO>();
                if (KpiProductGrouping.KpiProductGroupingContents != null && KpiProductGrouping.KpiProductGroupingContents.Any())
                {
                    KpiProductGrouping.KpiProductGroupingContents.ForEach(x => x.RowId = Guid.NewGuid());
                    foreach (var KpiProductGroupingContent in KpiProductGrouping.KpiProductGroupingContents)
                    {
                        KpiProductGroupingContentDAO KpiProductGroupingContentDAO = new KpiProductGroupingContentDAO();
                        KpiProductGroupingContentDAO.Id = KpiProductGroupingContent.Id;
                        KpiProductGroupingContentDAO.KpiProductGroupingId = KpiProductGrouping.Id;
                        KpiProductGroupingContentDAO.ProductGroupingId = KpiProductGroupingContent.ProductGroupingId;
                        KpiProductGroupingContentDAO.SelectAllCurrentItem = KpiProductGroupingContent.SelectAllCurrentItem;
                        KpiProductGroupingContentDAO.RowId = KpiProductGroupingContent.RowId;
                        KpiProductGroupingContentDAOs.Add(KpiProductGroupingContentDAO);
                    }
                    await DataContext.KpiProductGroupingContent.BulkMergeAsync(KpiProductGroupingContentDAOs);

                    foreach (var KpiProductGroupingContent in KpiProductGrouping.KpiProductGroupingContents)
                    {
                        KpiProductGroupingContent.Id = KpiProductGroupingContentDAOs
                            .Where(x => x.RowId == KpiProductGroupingContent.RowId)
                            .Select(x => x.Id).FirstOrDefault(); // khi tạo mới thì các content chưa có id mặc định nên phải where theo RowId để tìm ra Id
                        if (KpiProductGroupingContent.KpiProductGroupingContentCriteriaMappings != null)
                        {
                            foreach (var KpiProductGroupingContentCriteriaMapping in KpiProductGroupingContent.KpiProductGroupingContentCriteriaMappings)
                            {
                                KpiProductGroupingContentCriteriaMappingDAO KpiProductGroupingContentCriteriaMappingDAO = new KpiProductGroupingContentCriteriaMappingDAO();
                                KpiProductGroupingContentCriteriaMappingDAO.KpiProductGroupingContentId = KpiProductGroupingContent.Id;
                                KpiProductGroupingContentCriteriaMappingDAO.KpiProductGroupingCriteriaId = KpiProductGroupingContentCriteriaMapping.KpiProductGroupingCriteriaId;
                                KpiProductGroupingContentCriteriaMappingDAO.Value = KpiProductGroupingContentCriteriaMapping.Value;
                                KpiProductGroupingContentCriteriaMappingDAOs.Add(KpiProductGroupingContentCriteriaMappingDAO);
                            }
                        }
                        if (KpiProductGroupingContent.SelectAllCurrentItem == false && KpiProductGroupingContent.KpiProductGroupingContentItemMappings != null)
                        {
                            foreach (var KpiProductGroupingContentItemMapping in KpiProductGroupingContent.KpiProductGroupingContentItemMappings)
                            {
                                KpiProductGroupingContentItemMappingDAO KpiProductGroupingContentItemMappingDAO = new KpiProductGroupingContentItemMappingDAO();
                                KpiProductGroupingContentItemMappingDAO.KpiProductGroupingContentId = KpiProductGroupingContent.Id;
                                KpiProductGroupingContentItemMappingDAO.ItemId = KpiProductGroupingContentItemMapping.ItemId;
                                KpiProductGroupingContentItemMappingDAOs.Add(KpiProductGroupingContentItemMappingDAO);
                            }
                        }
                    }
                    await DataContext.KpiProductGroupingContentCriteriaMapping.BulkMergeAsync(KpiProductGroupingContentCriteriaMappingDAOs); // thêm mapping content và chỉ tiêu kpi
                    await DataContext.KpiProductGroupingContentItemMapping.BulkMergeAsync(KpiProductGroupingContentItemMappingDAOs); // thêm mapping content và Item
                }
                #endregion
            }
            catch (Exception Exception)
            {

                throw Exception;
            }
        }

        private async Task DeleteReference(List<KpiProductGrouping> KpiProductGroupings)
        {
            try
            {
                List<long> Ids = KpiProductGroupings.Select(x => x.Id).ToList();
                IdFilter IdFilter = new IdFilter { In = Ids };
                await DataContext.KpiProductGroupingContentCriteriaMapping
                    .Where(x => IdFilter.In.Contains(x.KpiProductGroupingContent.KpiProductGroupingId))
                    .DeleteFromQueryAsync();
                await DataContext.KpiProductGroupingContentItemMapping
                    .Where(x => IdFilter.In.Contains(x.KpiProductGroupingContent.KpiProductGroupingId))
                    .DeleteFromQueryAsync();
                await DataContext.KpiProductGroupingContent
                    .Where(x => IdFilter.In.Contains(x.KpiProductGroupingId))
                    .DeleteFromQueryAsync();
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

    }
}
