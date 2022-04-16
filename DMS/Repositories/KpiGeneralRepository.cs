using DMS.Entities;
using DMS.Enums;
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
    public interface IKpiGeneralRepository
    {
        Task<int> Count(KpiGeneralFilter KpiGeneralFilter);
        Task<int> CountAll(KpiGeneralFilter KpiGeneralFilter);
        Task<List<KpiGeneral>> List(KpiGeneralFilter KpiGeneralFilter);
        Task<List<KpiGeneral>> List(List<long> Ids);
        Task<KpiGeneral> Get(long Id);
        Task<bool> Create(KpiGeneral KpiGeneral);
        Task<bool> Update(KpiGeneral KpiGeneral);
        Task<bool> Delete(KpiGeneral KpiGeneral);
        Task<bool> BulkMerge(List<KpiGeneral> KpiGenerals);
        Task<bool> BulkDelete(List<KpiGeneral> KpiGenerals);
    }
    public class KpiGeneralRepository : IKpiGeneralRepository
    {
        private DataContext DataContext;
        public KpiGeneralRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<KpiGeneralDAO> DynamicFilter(IQueryable<KpiGeneralDAO> query, KpiGeneralFilter filter)
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
                    query = query.Where(q => q.Employee.Organization.Path.StartsWith(OrganizationDAO.Path));
                }
                if (filter.OrganizationId.NotEqual != null)
                {
                    OrganizationDAO OrganizationDAO = DataContext.Organization
                        .Where(o => o.Id == filter.OrganizationId.NotEqual.Value).FirstOrDefault();
                    query = query.Where(q => !q.Employee.Organization.Path.StartsWith(OrganizationDAO.Path));
                }
                if (filter.OrganizationId.In != null)
                {
                    List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                        .Where(o => o.DeletedAt == null && o.StatusId == 1).ToList();
                    List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => filter.OrganizationId.In.Contains(o.Id)).ToList();
                    List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> Ids = Branches.Select(o => o.Id).ToList();
                    IdFilter IdFilter = new IdFilter { In = Ids };
                    query = query.Where(x => x.Employee.OrganizationId, IdFilter);
                }
                if (filter.OrganizationId.NotIn != null)
                {
                    List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                        .Where(o => o.DeletedAt == null && o.StatusId == 1).ToList();
                    List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => filter.OrganizationId.NotIn.Contains(o.Id)).ToList();
                    List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                    List<long> Ids = Branches.Select(o => o.Id).ToList();
                    IdFilter IdFilter = new IdFilter { NotIn = Ids };
                    query = query.Where(x => x.Employee.OrganizationId, IdFilter);
                }
            }
            query = query.Where(q => q.EmployeeId, filter.AppUserId);
            query = query.Where(q => q.KpiYearId, filter.KpiYearId);
            query = query.Where(q => q.StatusId, filter.StatusId);
            query = query.Where(q => q.CreatorId, filter.CreatorId);
            return query;
        }

        private IQueryable<KpiGeneralDAO> OrFilter(IQueryable<KpiGeneralDAO> query, KpiGeneralFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<KpiGeneralDAO> initQuery = query.Where(q => false);
            foreach (KpiGeneralFilter KpiGeneralFilter in filter.OrFilter)
            {
                IQueryable<KpiGeneralDAO> queryable = query;
                if (KpiGeneralFilter.Id != null)
                    queryable = queryable.Where(q => q.Id, KpiGeneralFilter.Id);
                if (KpiGeneralFilter.OrganizationId != null)
                {
                    if (KpiGeneralFilter.OrganizationId.Equal != null)
                    {
                        OrganizationDAO OrganizationDAO = DataContext.Organization
                            .Where(o => o.Id == KpiGeneralFilter.OrganizationId.Equal.Value).FirstOrDefault();
                        queryable = queryable.Where(q => q.Employee.Organization.Path.StartsWith(OrganizationDAO.Path));
                    }
                    if (KpiGeneralFilter.OrganizationId.NotEqual != null)
                    {
                        OrganizationDAO OrganizationDAO = DataContext.Organization
                            .Where(o => o.Id == KpiGeneralFilter.OrganizationId.NotEqual.Value).FirstOrDefault();
                        queryable = queryable.Where(q => !q.Employee.Organization.Path.StartsWith(OrganizationDAO.Path));
                    }
                    if (KpiGeneralFilter.OrganizationId.In != null)
                    {
                        List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                            .Where(o => o.DeletedAt == null && o.StatusId == 1).ToList();
                        List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => KpiGeneralFilter.OrganizationId.In.Contains(o.Id)).ToList();
                        List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> Ids = Branches.Select(o => o.Id).ToList();
                        IdFilter IdFilter = new IdFilter { In = Ids };
                        queryable = queryable.Where(x => x.Employee.OrganizationId, IdFilter);
                    }
                    if (KpiGeneralFilter.OrganizationId.NotIn != null)
                    {
                        List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
                            .Where(o => o.DeletedAt == null && o.StatusId == 1).ToList();
                        List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => KpiGeneralFilter.OrganizationId.NotIn.Contains(o.Id)).ToList();
                        List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
                        List<long> Ids = Branches.Select(o => o.Id).ToList();
                        IdFilter IdFilter = new IdFilter { NotIn = Ids };
                        queryable = queryable.Where(x => x.Employee.OrganizationId, IdFilter);
                    }
                }
                queryable = queryable.Where(q => q.EmployeeId, KpiGeneralFilter.AppUserId);
                queryable = queryable.Where(q => q.KpiYearId, KpiGeneralFilter.KpiYearId);
                queryable = queryable.Where(q => q.StatusId, KpiGeneralFilter.StatusId);
                queryable = queryable.Where(q => q.CreatorId, KpiGeneralFilter.CreatorId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<KpiGeneralDAO> DynamicOrder(IQueryable<KpiGeneralDAO> query, KpiGeneralFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case KpiGeneralOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case KpiGeneralOrder.Organization:
                            query = query.OrderBy(q => q.OrganizationId);
                            break;
                        case KpiGeneralOrder.Employee:
                            query = query.OrderBy(q => q.EmployeeId);
                            break;
                        case KpiGeneralOrder.KpiYear:
                            query = query.OrderBy(q => q.KpiYearId);
                            break;
                        case KpiGeneralOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                        case KpiGeneralOrder.Creator:
                            query = query.OrderBy(q => q.CreatorId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case KpiGeneralOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case KpiGeneralOrder.Organization:
                            query = query.OrderByDescending(q => q.OrganizationId);
                            break;
                        case KpiGeneralOrder.Employee:
                            query = query.OrderByDescending(q => q.EmployeeId);
                            break;
                        case KpiGeneralOrder.KpiYear:
                            query = query.OrderByDescending(q => q.KpiYearId);
                            break;
                        case KpiGeneralOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                        case KpiGeneralOrder.Creator:
                            query = query.OrderByDescending(q => q.CreatorId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<KpiGeneral>> DynamicSelect(IQueryable<KpiGeneralDAO> query, KpiGeneralFilter filter)
        {
            List<KpiGeneral> KpiGenerals = await query.Select(q => new KpiGeneral()
            {
                Id = filter.Selects.Contains(KpiGeneralSelect.Id) ? q.Id : default(long),
                OrganizationId = filter.Selects.Contains(KpiGeneralSelect.Organization) ? q.OrganizationId : default(long),
                EmployeeId = filter.Selects.Contains(KpiGeneralSelect.Employee) ? q.EmployeeId : default(long),
                KpiYearId = filter.Selects.Contains(KpiGeneralSelect.KpiYear) ? q.KpiYearId : default(long),
                StatusId = filter.Selects.Contains(KpiGeneralSelect.Status) ? q.StatusId : default(long),
                CreatorId = filter.Selects.Contains(KpiGeneralSelect.Creator) ? q.CreatorId : default(long),
                Creator = filter.Selects.Contains(KpiGeneralSelect.Creator) && q.Creator != null ? new AppUser
                {
                    Id = q.Creator.Id,
                    Username = q.Creator.Username,
                    DisplayName = q.Creator.DisplayName,
                    Address = q.Creator.Address,
                    Email = q.Creator.Email,
                    Phone = q.Creator.Phone,
                    PositionId = q.Creator.PositionId,
                    Department = q.Creator.Department,
                    OrganizationId = q.Creator.OrganizationId,
                    StatusId = q.Creator.StatusId,
                    Avatar = q.Creator.Avatar,
                    ProvinceId = q.Creator.ProvinceId,
                    SexId = q.Creator.SexId,
                    Birthday = q.Creator.Birthday,
                } : null,
                Employee = filter.Selects.Contains(KpiGeneralSelect.Employee) && q.Employee != null ? new AppUser
                {
                    Id = q.Employee.Id,
                    Username = q.Employee.Username,
                    DisplayName = q.Employee.DisplayName,
                    Address = q.Employee.Address,
                    Email = q.Employee.Email,
                    Phone = q.Employee.Phone,
                    PositionId = q.Employee.PositionId,
                    Department = q.Employee.Department,
                    OrganizationId = q.Employee.OrganizationId,
                    StatusId = q.Employee.StatusId,
                    Avatar = q.Employee.Avatar,
                    ProvinceId = q.Employee.ProvinceId,
                    SexId = q.Employee.SexId,
                    Birthday = q.Employee.Birthday,
                } : null,
                KpiYear = filter.Selects.Contains(KpiGeneralSelect.KpiYear) && q.KpiYear != null ? new KpiYear
                {
                    Id = q.KpiYear.Id,
                    Code = q.KpiYear.Code,
                    Name = q.KpiYear.Name,
                } : null,
                Organization = filter.Selects.Contains(KpiGeneralSelect.Organization) && q.Organization != null ? new Organization
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
                Status = filter.Selects.Contains(KpiGeneralSelect.Status) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                } : null,
                CreatedAt = q.CreatedAt,
                UpdatedAt = q.UpdatedAt,
                RowId = q.RowId,
            }).ToListWithNoLockAsync();
            return KpiGenerals;
        }

        public async Task<int> Count(KpiGeneralFilter filter)
        {
            IQueryable<KpiGeneralDAO> KpiGenerals = DataContext.KpiGeneral.AsNoTracking();
            KpiGenerals = DynamicFilter(KpiGenerals, filter);
            KpiGenerals = OrFilter(KpiGenerals, filter);
            return await KpiGenerals.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(KpiGeneralFilter filter)
        {
            IQueryable<KpiGeneralDAO> KpiGenerals = DataContext.KpiGeneral.AsNoTracking();
            KpiGenerals = DynamicFilter(KpiGenerals, filter);
            return await KpiGenerals.CountWithNoLockAsync();
        }

        public async Task<List<KpiGeneral>> List(KpiGeneralFilter filter)
        {
            if (filter == null) return new List<KpiGeneral>();
            IQueryable<KpiGeneralDAO> KpiGeneralDAOs = DataContext.KpiGeneral.AsNoTracking();
            KpiGeneralDAOs = DynamicFilter(KpiGeneralDAOs, filter);
            KpiGeneralDAOs = OrFilter(KpiGeneralDAOs, filter);
            KpiGeneralDAOs = DynamicOrder(KpiGeneralDAOs, filter);
            List<KpiGeneral> KpiGenerals = await DynamicSelect(KpiGeneralDAOs, filter);
            return KpiGenerals;
        }

        public async Task<List<KpiGeneral>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };
            List<KpiGeneral> KpiGenerals = await DataContext.KpiGeneral.AsNoTracking()
                .Where(x => x.Id, IdFilter)
                .Select(x => new KpiGeneral
                {
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    Id = x.Id,
                    OrganizationId = x.OrganizationId,
                    EmployeeId = x.EmployeeId,
                    KpiYearId = x.KpiYearId,
                    StatusId = x.StatusId,
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
                        PositionId = x.Creator.PositionId,
                        Department = x.Creator.Department,
                        OrganizationId = x.Creator.OrganizationId,
                        StatusId = x.Creator.StatusId,
                        Avatar = x.Creator.Avatar,
                        ProvinceId = x.Creator.ProvinceId,
                        SexId = x.Creator.SexId,
                        Birthday = x.Creator.Birthday,
                    },
                    Employee = x.Employee == null ? null : new AppUser
                    {
                        Id = x.Employee.Id,
                        Username = x.Employee.Username,
                        DisplayName = x.Employee.DisplayName,
                        Address = x.Employee.Address,
                        Email = x.Employee.Email,
                        Phone = x.Employee.Phone,
                        PositionId = x.Employee.PositionId,
                        Department = x.Employee.Department,
                        OrganizationId = x.Employee.OrganizationId,
                        StatusId = x.Employee.StatusId,
                        Avatar = x.Employee.Avatar,
                        ProvinceId = x.Employee.ProvinceId,
                        SexId = x.Employee.SexId,
                        Birthday = x.Employee.Birthday,
                    },
                    KpiYear = x.KpiYear == null ? null : new KpiYear
                    {
                        Id = x.KpiYear.Id,
                        Code = x.KpiYear.Code,
                        Name = x.KpiYear.Name,
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
                }).ToListWithNoLockAsync();

            List<KpiGeneralContent> KpiGeneralContents = await DataContext.KpiGeneralContent.AsNoTracking()
                .Where(x => x.KpiGeneralId, IdFilter)
                .Select(x => new KpiGeneralContent
                {
                    Id = x.Id,
                    KpiGeneralId = x.KpiGeneralId,
                    KpiCriteriaGeneralId = x.KpiCriteriaGeneralId,
                    StatusId = x.StatusId,
                    Status = x.Status == null ? null : new Status
                    {
                        Id = x.Status.Id,
                        Name = x.Status.Name,
                        Code = x.Status.Code,
                    },
                    KpiCriteriaGeneral = new KpiCriteriaGeneral
                    {
                        Id = x.KpiCriteriaGeneral.Id,
                        Code = x.KpiCriteriaGeneral.Code,
                        Name = x.KpiCriteriaGeneral.Name,
                    },
                }).ToListWithNoLockAsync();

            var KpiGeneralContentIds = KpiGeneralContents.Select(x => x.Id).ToList();
            IdFilter KpiGeneralContentIdFilter = new IdFilter { In = KpiGeneralContentIds };


            List<KpiGeneralContentKpiPeriodMapping> KpiGeneralContentKpiPeriodMappings = await DataContext.KpiGeneralContentKpiPeriodMapping.AsNoTracking()
                .Where(x => x.KpiGeneralContentId, KpiGeneralContentIdFilter)
                .Select(mapping => new KpiGeneralContentKpiPeriodMapping
                {
                    KpiGeneralContentId = mapping.KpiGeneralContentId,
                    KpiPeriodId = mapping.KpiPeriodId,
                    Value = mapping.Value,
                    KpiGeneralContent = mapping.KpiGeneralContent == null ? null : new KpiGeneralContent
                    {
                        Id = mapping.KpiGeneralContent.Id,
                        KpiCriteriaGeneralId = mapping.KpiGeneralContent.KpiCriteriaGeneralId,
                    },
                    KpiPeriod = mapping.KpiPeriod == null ? null : new KpiPeriod
                    {
                        Id = mapping.KpiPeriod.Id,
                        Code = mapping.KpiPeriod.Code,
                        Name = mapping.KpiPeriod.Name,
                    }
                }).ToListWithNoLockAsync();
            foreach (KpiGeneralContent KpiGeneralContent in KpiGeneralContents)
            {
                KpiGeneralContent.KpiGeneralContentKpiPeriodMappings = KpiGeneralContentKpiPeriodMappings.Where(x => x.KpiGeneralContentId == KpiGeneralContent.Id).ToList();
            }

            foreach (KpiGeneral KpiGeneral in KpiGenerals)
            {
                KpiGeneral.KpiGeneralContents = KpiGeneralContents.Where(x => x.KpiGeneralId == KpiGeneral.Id).ToList();
            }
            return KpiGenerals;
        }

        public async Task<KpiGeneral> Get(long Id)
        {
            KpiGeneral KpiGeneral = await DataContext.KpiGeneral.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new KpiGeneral()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Id = x.Id,
                OrganizationId = x.OrganizationId,
                EmployeeId = x.EmployeeId,
                KpiYearId = x.KpiYearId,
                StatusId = x.StatusId,
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
                    PositionId = x.Creator.PositionId,
                    Department = x.Creator.Department,
                    OrganizationId = x.Creator.OrganizationId,
                    StatusId = x.Creator.StatusId,
                    Avatar = x.Creator.Avatar,
                    ProvinceId = x.Creator.ProvinceId,
                    SexId = x.Creator.SexId,
                    Birthday = x.Creator.Birthday,
                },
                Employee = x.Employee == null ? null : new AppUser
                {
                    Id = x.Employee.Id,
                    Username = x.Employee.Username,
                    DisplayName = x.Employee.DisplayName,
                    Address = x.Employee.Address,
                    Email = x.Employee.Email,
                    Phone = x.Employee.Phone,
                    PositionId = x.Employee.PositionId,
                    Department = x.Employee.Department,
                    OrganizationId = x.Employee.OrganizationId,
                    StatusId = x.Employee.StatusId,
                    Avatar = x.Employee.Avatar,
                    ProvinceId = x.Employee.ProvinceId,
                    SexId = x.Employee.SexId,
                    Birthday = x.Employee.Birthday,
                },
                KpiYear = x.KpiYear == null ? null : new KpiYear
                {
                    Id = x.KpiYear.Id,
                    Code = x.KpiYear.Code,
                    Name = x.KpiYear.Name,
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

            if (KpiGeneral == null)
                return null;
            KpiGeneral.KpiGeneralContents = await DataContext.KpiGeneralContent.AsNoTracking()
                .Where(x => x.KpiGeneralId == KpiGeneral.Id && x.KpiCriteriaGeneral.StatusId == StatusEnum.ACTIVE.Id)
                .Select(x => new KpiGeneralContent
                {
                    Id = x.Id,
                    KpiGeneralId = x.KpiGeneralId,
                    KpiCriteriaGeneralId = x.KpiCriteriaGeneralId,
                    StatusId = x.StatusId,
                    Status = x.Status == null ? null : new Status
                    {
                        Id = x.Status.Id,
                        Name = x.Status.Name,
                        Code = x.Status.Code,
                    },
                    KpiCriteriaGeneral = new KpiCriteriaGeneral
                    {
                        Id = x.KpiCriteriaGeneral.Id,
                        Code = x.KpiCriteriaGeneral.Code,
                        Name = x.KpiCriteriaGeneral.Name,
                    },
                }).ToListWithNoLockAsync();
            var KpiGeneralContentIds = KpiGeneral.KpiGeneralContents.Select(x => x.Id).ToList();
            IdFilter KpiGeneralContentIdFilter = new IdFilter { In = KpiGeneralContentIds };
            List<KpiGeneralContentKpiPeriodMapping> KpiGeneralContentKpiPeriodMappings = await DataContext.KpiGeneralContentKpiPeriodMapping
                .Where(x => x.KpiGeneralContentId, KpiGeneralContentIdFilter)
                .Select(x => new KpiGeneralContentKpiPeriodMapping
                {
                    KpiGeneralContentId = x.KpiGeneralContentId,
                    KpiPeriodId = x.KpiPeriodId,
                    Value = x.Value,
                    KpiGeneralContent = x.KpiGeneralContent == null ? null : new KpiGeneralContent
                    {
                        Id = x.KpiGeneralContent.Id,
                        KpiCriteriaGeneralId = x.KpiGeneralContent.KpiCriteriaGeneralId,
                    }
                }).ToListWithNoLockAsync();
            foreach (KpiGeneralContent KpiGeneralContent in KpiGeneral.KpiGeneralContents)
            {
                KpiGeneralContent.KpiGeneralContentKpiPeriodMappings = KpiGeneralContentKpiPeriodMappings.Where(x => x.KpiGeneralContentId == KpiGeneralContent.Id).ToList();
            }
            return KpiGeneral;
        }
        public async Task<bool> Create(KpiGeneral KpiGeneral)
        {
            KpiGeneralDAO KpiGeneralDAO = new KpiGeneralDAO();
            KpiGeneralDAO.Id = KpiGeneral.Id;
            KpiGeneralDAO.OrganizationId = KpiGeneral.OrganizationId;
            KpiGeneralDAO.EmployeeId = KpiGeneral.EmployeeId;
            KpiGeneralDAO.KpiYearId = KpiGeneral.KpiYearId;
            KpiGeneralDAO.StatusId = KpiGeneral.StatusId;
            KpiGeneralDAO.CreatorId = KpiGeneral.CreatorId;
            KpiGeneralDAO.CreatedAt = StaticParams.DateTimeNow;
            KpiGeneralDAO.UpdatedAt = StaticParams.DateTimeNow;
            DataContext.KpiGeneral.Add(KpiGeneralDAO);
            await DataContext.SaveChangesAsync();
            KpiGeneral.Id = KpiGeneralDAO.Id;
            await SaveReference(KpiGeneral);
            return true;
        }

        public async Task<bool> Update(KpiGeneral KpiGeneral)
        {
            KpiGeneralDAO KpiGeneralDAO = DataContext.KpiGeneral.Where(x => x.Id == KpiGeneral.Id).FirstOrDefault();
            if (KpiGeneralDAO == null)
                return false;
            KpiGeneralDAO.Id = KpiGeneral.Id;
            KpiGeneralDAO.OrganizationId = KpiGeneral.OrganizationId;
            KpiGeneralDAO.EmployeeId = KpiGeneral.EmployeeId;
            KpiGeneralDAO.KpiYearId = KpiGeneral.KpiYearId;
            KpiGeneralDAO.StatusId = KpiGeneral.StatusId;
            KpiGeneralDAO.CreatorId = KpiGeneral.CreatorId;
            KpiGeneralDAO.UpdatedAt = StaticParams.DateTimeNow;
            await DataContext.SaveChangesAsync();
            await SaveReference(KpiGeneral);
            return true;
        }

        public async Task<bool> Delete(KpiGeneral KpiGeneral)
        {
            await DataContext.KpiGeneral.Where(x => x.Id == KpiGeneral.Id).UpdateFromQueryAsync(x => new KpiGeneralDAO { DeletedAt = StaticParams.DateTimeNow });
            return true;
        }

        public async Task<bool> BulkMerge(List<KpiGeneral> KpiGenerals)
        {
            List<KpiGeneralDAO> KpiGeneralDAOs = new List<KpiGeneralDAO>();
            foreach (KpiGeneral KpiGeneral in KpiGenerals)
            {
                KpiGeneralDAO KpiGeneralDAO = new KpiGeneralDAO();
                KpiGeneralDAO.Id = KpiGeneral.Id;
                KpiGeneralDAO.OrganizationId = KpiGeneral.OrganizationId;
                KpiGeneralDAO.EmployeeId = KpiGeneral.EmployeeId;
                KpiGeneralDAO.KpiYearId = KpiGeneral.KpiYearId;
                KpiGeneralDAO.StatusId = KpiGeneral.StatusId;
                KpiGeneralDAO.CreatorId = KpiGeneral.CreatorId;
                KpiGeneralDAO.CreatedAt = StaticParams.DateTimeNow;
                KpiGeneralDAO.UpdatedAt = StaticParams.DateTimeNow;
                KpiGeneralDAO.RowId = KpiGeneral.RowId;
                KpiGeneralDAOs.Add(KpiGeneralDAO);
            }
            await DataContext.BulkMergeAsync(KpiGeneralDAOs);

            var KpiGeneralIds = KpiGeneralDAOs.Select(x => x.Id).ToList();
            IdFilter KpiGeneralIdFilter = new IdFilter { In = KpiGeneralIds };
            //await DataContext.KpiGeneralContentKpiPeriodMapping
            //    .Where(x => KpiGeneralIds.Contains(x.KpiGeneralContent.KpiGeneralId))
            await DataContext.KpiGeneralContentKpiPeriodMapping
                .Where(x => KpiGeneralIdFilter.In.Contains(x.KpiGeneralContent.KpiGeneralId))
                .DeleteFromQueryAsync();
            await DataContext.KpiGeneralContent
                .Where(x => KpiGeneralIdFilter.In.Contains(x.KpiGeneralId))
                .DeleteFromQueryAsync();

            var KpiGeneralContentDAOs = new List<KpiGeneralContentDAO>();
            foreach (var KpiGeneral in KpiGenerals)
            {
                KpiGeneral.Id = KpiGeneralDAOs.Where(x => x.RowId == KpiGeneral.RowId).Select(x => x.Id).FirstOrDefault();
                if (KpiGeneral.KpiGeneralContents != null && KpiGeneral.KpiGeneralContents.Any())
                {
                    var listContent = KpiGeneral.KpiGeneralContents.Select(x => new KpiGeneralContentDAO
                    {
                        KpiCriteriaGeneralId = x.KpiCriteriaGeneralId, // KpiCriteriaGeneralId da co san map tu frontend xuong
                        KpiGeneralId = KpiGeneral.Id,
                        KpiGeneralContentKpiPeriodMappings = x.KpiGeneralContentKpiPeriodMappings.Select(x => new KpiGeneralContentKpiPeriodMappingDAO
                        {
                            KpiPeriodId = x.KpiPeriodId, // map tu du lieu bang mapping sang bang KpiGeneral sang KpiGeneralContentDAOs
                            Value = x.Value,
                            KpiGeneralContentId = 0, // se duoc gan luc sau
                        }).ToList(),
                        RowId = Guid.NewGuid(),
                        StatusId = x.StatusId,
                    }).ToList();
                    KpiGeneralContentDAOs.AddRange(listContent);
                }
            }
            await DataContext.BulkMergeAsync(KpiGeneralContentDAOs);


            var KpiGeneralContentKpiPeriodMappingDAOs = new List<KpiGeneralContentKpiPeriodMappingDAO>();
            foreach (var KpiGeneralContent in KpiGeneralContentDAOs)
            {
                KpiGeneralContent.Id = KpiGeneralContentDAOs.Where(x => x.RowId == KpiGeneralContent.RowId).Select(x => x.Id).FirstOrDefault(); // get Id dua vao RowId
                if (KpiGeneralContent.KpiGeneralContentKpiPeriodMappings != null && KpiGeneralContent.KpiGeneralContentKpiPeriodMappings.Any())
                {
                    var listMappings = KpiGeneralContent.KpiGeneralContentKpiPeriodMappings.Select(x => new KpiGeneralContentKpiPeriodMappingDAO
                    {
                        KpiPeriodId = x.KpiPeriodId,
                        Value = x.Value,
                        KpiGeneralContentId = KpiGeneralContent.Id
                    }).ToList();
                    KpiGeneralContentKpiPeriodMappingDAOs.AddRange(listMappings);
                }
            }

            await DataContext.BulkMergeAsync(KpiGeneralContentKpiPeriodMappingDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<KpiGeneral> KpiGenerals)
        {
            List<long> Ids = KpiGenerals.Select(x => x.Id).ToList();
            IdFilter IdFilter = new IdFilter { In = Ids };

            await DataContext.KpiGeneral
                .Where(x => x.Id, IdFilter)
                .UpdateFromQueryAsync(x => new KpiGeneralDAO { DeletedAt = StaticParams.DateTimeNow });
            return true;
        }

        private async Task SaveReference(KpiGeneral KpiGeneral)
        {
            await DataContext.KpiGeneralContentKpiPeriodMapping
                .Where(x => x.KpiGeneralContent.KpiGeneralId == KpiGeneral.Id)
                .DeleteFromQueryAsync();
            await DataContext.KpiGeneralContent
                .Where(x => x.KpiGeneralId == KpiGeneral.Id)
                .DeleteFromQueryAsync();
            List<KpiGeneralContentDAO> KpiGeneralContentDAOs = new List<KpiGeneralContentDAO>();
            List<KpiGeneralContentKpiPeriodMappingDAO> KpiGeneralContentKpiCriteriaItemMappingDAOs = new List<KpiGeneralContentKpiPeriodMappingDAO>();
            if (KpiGeneral.KpiGeneralContents != null)
            {
                KpiGeneral.KpiGeneralContents.ForEach(x => x.RowId = Guid.NewGuid());
                foreach (KpiGeneralContent KpiGeneralContent in KpiGeneral.KpiGeneralContents)
                {
                    KpiGeneralContentDAO KpiGeneralContentDAO = new KpiGeneralContentDAO();
                    KpiGeneralContentDAO.Id = KpiGeneralContent.Id;
                    KpiGeneralContentDAO.KpiGeneralId = KpiGeneral.Id;
                    KpiGeneralContentDAO.KpiCriteriaGeneralId = KpiGeneralContent.KpiCriteriaGeneralId;
                    KpiGeneralContentDAO.RowId = KpiGeneralContent.RowId;
                    KpiGeneralContentDAO.StatusId = KpiGeneralContent.StatusId;
                    KpiGeneralContentDAOs.Add(KpiGeneralContentDAO);
                }
                await DataContext.KpiGeneralContent.BulkMergeAsync(KpiGeneralContentDAOs);


                foreach (KpiGeneralContent KpiGeneralContent in KpiGeneral.KpiGeneralContents)
                {
                    KpiGeneralContent.Id = KpiGeneralContentDAOs.Where(x => x.RowId == KpiGeneralContent.RowId).Select(x => x.Id).FirstOrDefault();
                    if (KpiGeneralContent.KpiGeneralContentKpiPeriodMappings != null)
                    {
                        foreach (KpiGeneralContentKpiPeriodMapping KpiGeneralContentKpiPeriodMapping in KpiGeneralContent.KpiGeneralContentKpiPeriodMappings)
                        {
                            KpiGeneralContentKpiPeriodMappingDAO KpiGeneralContentKpiCriteriaItemMappingDAO = new KpiGeneralContentKpiPeriodMappingDAO
                            {
                                KpiGeneralContentId = KpiGeneralContent.Id,
                                KpiPeriodId = KpiGeneralContentKpiPeriodMapping.KpiPeriodId,
                                Value = KpiGeneralContentKpiPeriodMapping.Value
                            };
                            KpiGeneralContentKpiCriteriaItemMappingDAOs.Add(KpiGeneralContentKpiCriteriaItemMappingDAO);
                        }
                    }
                }

                await DataContext.KpiGeneralContentKpiPeriodMapping.BulkMergeAsync(KpiGeneralContentKpiCriteriaItemMappingDAOs);
            }
        }

    }
}
