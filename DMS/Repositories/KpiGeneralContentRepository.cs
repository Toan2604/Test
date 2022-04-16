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
    public interface IKpiGeneralContentRepository
    {
        Task<int> Count(KpiGeneralContentFilter KpiGeneralContentFilter);
        Task<int> CountAll(KpiGeneralContentFilter KpiGeneralContentFilter);
        Task<List<KpiGeneralContent>> List(KpiGeneralContentFilter KpiGeneralContentFilter);
        Task<KpiGeneralContent> Get(long Id);
        Task<bool> Create(KpiGeneralContent KpiGeneralContent);
        Task<bool> Update(KpiGeneralContent KpiGeneralContent);
        Task<bool> Delete(KpiGeneralContent KpiGeneralContent);
        Task<bool> BulkMerge(List<KpiGeneralContent> KpiGeneralContents);
        Task<bool> BulkDelete(List<KpiGeneralContent> KpiGeneralContents);
    }
    public class KpiGeneralContentRepository : IKpiGeneralContentRepository
    {
        private DataContext DataContext;
        public KpiGeneralContentRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<KpiGeneralContentDAO> DynamicFilter(IQueryable<KpiGeneralContentDAO> query, KpiGeneralContentFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.KpiGeneralId, filter.KpiGeneralId);
            query = query.Where(q => q.KpiCriteriaGeneralId, filter.KpiCriteriaGeneralId);
            query = query.Where(q => q.StatusId, filter.StatusId);
            return query;
        }

        private IQueryable<KpiGeneralContentDAO> OrFilter(IQueryable<KpiGeneralContentDAO> query, KpiGeneralContentFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<KpiGeneralContentDAO> initQuery = query.Where(q => false);
            foreach (KpiGeneralContentFilter KpiGeneralContentFilter in filter.OrFilter)
            {
                IQueryable<KpiGeneralContentDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, KpiGeneralContentFilter.Id);
                queryable = queryable.Where(q => q.KpiGeneralId, KpiGeneralContentFilter.KpiGeneralId);
                queryable = queryable.Where(q => q.KpiCriteriaGeneralId, KpiGeneralContentFilter.KpiCriteriaGeneralId);
                queryable = queryable.Where(q => q.StatusId, KpiGeneralContentFilter.StatusId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<KpiGeneralContentDAO> DynamicOrder(IQueryable<KpiGeneralContentDAO> query, KpiGeneralContentFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case KpiGeneralContentOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case KpiGeneralContentOrder.KpiGeneral:
                            query = query.OrderBy(q => q.KpiGeneralId);
                            break;
                        case KpiGeneralContentOrder.KpiCriteriaGeneral:
                            query = query.OrderBy(q => q.KpiCriteriaGeneralId);
                            break;
                        case KpiGeneralContentOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case KpiGeneralContentOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case KpiGeneralContentOrder.KpiGeneral:
                            query = query.OrderByDescending(q => q.KpiGeneralId);
                            break;
                        case KpiGeneralContentOrder.KpiCriteriaGeneral:
                            query = query.OrderByDescending(q => q.KpiCriteriaGeneralId);
                            break;
                        case KpiGeneralContentOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<KpiGeneralContent>> DynamicSelect(IQueryable<KpiGeneralContentDAO> query, KpiGeneralContentFilter filter)
        {
            List<KpiGeneralContent> KpiGeneralContents = await query.Select(q => new KpiGeneralContent()
            {
                Id = filter.Selects.Contains(KpiGeneralContentSelect.Id) ? q.Id : default(long),
                KpiGeneralId = filter.Selects.Contains(KpiGeneralContentSelect.KpiGeneral) ? q.KpiGeneralId : default(long),
                KpiCriteriaGeneralId = filter.Selects.Contains(KpiGeneralContentSelect.KpiCriteriaGeneral) ? q.KpiCriteriaGeneralId : default(long),
                StatusId = filter.Selects.Contains(KpiGeneralContentSelect.Status) ? q.StatusId : default(long),
                KpiCriteriaGeneral = filter.Selects.Contains(KpiGeneralContentSelect.KpiCriteriaGeneral) && q.KpiCriteriaGeneral != null ? new KpiCriteriaGeneral
                {
                    Id = q.KpiCriteriaGeneral.Id,
                    Code = q.KpiCriteriaGeneral.Code,
                    Name = q.KpiCriteriaGeneral.Name,
                } : null,
                KpiGeneral = filter.Selects.Contains(KpiGeneralContentSelect.KpiGeneral) && q.KpiGeneral != null ? new KpiGeneral
                {
                    Id = q.KpiGeneral.Id,
                    OrganizationId = q.KpiGeneral.OrganizationId,
                    EmployeeId = q.KpiGeneral.EmployeeId,
                    KpiYearId = q.KpiGeneral.KpiYearId,
                    StatusId = q.KpiGeneral.StatusId,
                    CreatorId = q.KpiGeneral.CreatorId,
                } : null,
                Status = filter.Selects.Contains(KpiGeneralContentSelect.Status) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                } : null,
            }).ToListWithNoLockAsync();

            var KpiGeneralContentIds = KpiGeneralContents.Select(x => x.Id).ToList();
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
            foreach (KpiGeneralContent KpiGeneralContent in KpiGeneralContents)
            {
                KpiGeneralContent.KpiGeneralContentKpiPeriodMappings = KpiGeneralContentKpiPeriodMappings.Where(x => x.KpiGeneralContentId == KpiGeneralContent.Id).ToList();
            }
            return KpiGeneralContents;
        }

        public async Task<int> Count(KpiGeneralContentFilter filter)
        {
            IQueryable<KpiGeneralContentDAO> KpiGeneralContents = DataContext.KpiGeneralContent.AsNoTracking();
            KpiGeneralContents = DynamicFilter(KpiGeneralContents, filter);
            KpiGeneralContents = OrFilter(KpiGeneralContents, filter);
            return await KpiGeneralContents.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(KpiGeneralContentFilter filter)
        {
            IQueryable<KpiGeneralContentDAO> KpiGeneralContents = DataContext.KpiGeneralContent.AsNoTracking();
            KpiGeneralContents = DynamicFilter(KpiGeneralContents, filter);
            return await KpiGeneralContents.CountWithNoLockAsync();
        }
        public async Task<List<KpiGeneralContent>> List(KpiGeneralContentFilter filter)
        {
            if (filter == null) return new List<KpiGeneralContent>();
            IQueryable<KpiGeneralContentDAO> KpiGeneralContentDAOs = DataContext.KpiGeneralContent.AsNoTracking();
            KpiGeneralContentDAOs = DynamicFilter(KpiGeneralContentDAOs, filter);
            KpiGeneralContentDAOs = OrFilter(KpiGeneralContentDAOs, filter);
            KpiGeneralContentDAOs = DynamicOrder(KpiGeneralContentDAOs, filter);
            List<KpiGeneralContent> KpiGeneralContents = await DynamicSelect(KpiGeneralContentDAOs, filter);
            return KpiGeneralContents;
        }

        public async Task<KpiGeneralContent> Get(long Id)
        {
            KpiGeneralContent KpiGeneralContent = await DataContext.KpiGeneralContent.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new KpiGeneralContent()
            {
                Id = x.Id,
                KpiGeneralId = x.KpiGeneralId,
                KpiCriteriaGeneralId = x.KpiCriteriaGeneralId,
                StatusId = x.StatusId,
                KpiCriteriaGeneral = x.KpiCriteriaGeneral == null ? null : new KpiCriteriaGeneral
                {
                    Id = x.KpiCriteriaGeneral.Id,
                    Code = x.KpiCriteriaGeneral.Code,
                    Name = x.KpiCriteriaGeneral.Name,
                },
                KpiGeneral = x.KpiGeneral == null ? null : new KpiGeneral
                {
                    Id = x.KpiGeneral.Id,
                    OrganizationId = x.KpiGeneral.OrganizationId,
                    EmployeeId = x.KpiGeneral.EmployeeId,
                    KpiYearId = x.KpiGeneral.KpiYearId,
                    StatusId = x.KpiGeneral.StatusId,
                    CreatorId = x.KpiGeneral.CreatorId,
                },
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (KpiGeneralContent == null)
                return null;

            return KpiGeneralContent;
        }
        public async Task<bool> Create(KpiGeneralContent KpiGeneralContent)
        {
            KpiGeneralContentDAO KpiGeneralContentDAO = new KpiGeneralContentDAO();
            KpiGeneralContentDAO.Id = KpiGeneralContent.Id;
            KpiGeneralContentDAO.KpiGeneralId = KpiGeneralContent.KpiGeneralId;
            KpiGeneralContentDAO.KpiCriteriaGeneralId = KpiGeneralContent.KpiCriteriaGeneralId;
            KpiGeneralContentDAO.StatusId = KpiGeneralContent.StatusId;
            DataContext.KpiGeneralContent.Add(KpiGeneralContentDAO);
            await DataContext.SaveChangesAsync();
            KpiGeneralContent.Id = KpiGeneralContentDAO.Id;
            await SaveReference(KpiGeneralContent);
            return true;
        }

        public async Task<bool> Update(KpiGeneralContent KpiGeneralContent)
        {
            KpiGeneralContentDAO KpiGeneralContentDAO = DataContext.KpiGeneralContent.Where(x => x.Id == KpiGeneralContent.Id).FirstOrDefault();
            if (KpiGeneralContentDAO == null)
                return false;
            KpiGeneralContentDAO.Id = KpiGeneralContent.Id;
            KpiGeneralContentDAO.KpiGeneralId = KpiGeneralContent.KpiGeneralId;
            KpiGeneralContentDAO.KpiCriteriaGeneralId = KpiGeneralContent.KpiCriteriaGeneralId;
            KpiGeneralContentDAO.StatusId = KpiGeneralContent.StatusId;
            await DataContext.SaveChangesAsync();
            await SaveReference(KpiGeneralContent);
            return true;
        }

        public async Task<bool> Delete(KpiGeneralContent KpiGeneralContent)
        {
            await DataContext.KpiGeneralContent.Where(x => x.Id == KpiGeneralContent.Id).DeleteFromQueryAsync();
            return true;
        }

        public async Task<bool> BulkMerge(List<KpiGeneralContent> KpiGeneralContents)
        {
            List<KpiGeneralContentDAO> KpiGeneralContentDAOs = new List<KpiGeneralContentDAO>();
            foreach (KpiGeneralContent KpiGeneralContent in KpiGeneralContents)
            {
                KpiGeneralContentDAO KpiGeneralContentDAO = new KpiGeneralContentDAO();
                KpiGeneralContentDAO.Id = KpiGeneralContent.Id;
                KpiGeneralContentDAO.KpiGeneralId = KpiGeneralContent.KpiGeneralId;
                KpiGeneralContentDAO.KpiCriteriaGeneralId = KpiGeneralContent.KpiCriteriaGeneralId;
                KpiGeneralContentDAO.StatusId = KpiGeneralContent.StatusId;
                KpiGeneralContentDAOs.Add(KpiGeneralContentDAO);
            }
            await DataContext.BulkMergeAsync(KpiGeneralContentDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<KpiGeneralContent> KpiGeneralContents)
        {
            List<long> Ids = KpiGeneralContents.Select(x => x.Id).ToList();
            await DataContext.KpiGeneralContent
                .Where(x => Ids.Contains(x.Id))
                .DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(KpiGeneralContent KpiGeneralContent)
        {
        }

    }
}
