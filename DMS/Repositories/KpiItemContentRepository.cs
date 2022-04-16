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
    public interface IKpiItemContentRepository
    {
        Task<int> Count(KpiItemContentFilter KpiItemContentFilter);
        Task<int> CountAll(KpiItemContentFilter KpiItemContentFilter);
        Task<List<KpiItemContent>> List(KpiItemContentFilter KpiItemContentFilter);
        Task<KpiItemContent> Get(long Id);
        Task<bool> Create(KpiItemContent KpiItemContent);
        Task<bool> Update(KpiItemContent KpiItemContent);
        Task<bool> Delete(KpiItemContent KpiItemContent);
        Task<bool> BulkMerge(List<KpiItemContent> KpiItemContents);
        Task<bool> BulkDelete(List<KpiItemContent> KpiItemContents);
    }
    public class KpiItemContentRepository : IKpiItemContentRepository
    {
        private DataContext DataContext;
        public KpiItemContentRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<KpiItemContentDAO> DynamicFilter(IQueryable<KpiItemContentDAO> query, KpiItemContentFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.KpiItemId, filter.KpiItemId);
            query = query.Where(q => q.ItemId, filter.ItemId);
            return query;
        }

        private IQueryable<KpiItemContentDAO> OrFilter(IQueryable<KpiItemContentDAO> query, KpiItemContentFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<KpiItemContentDAO> initQuery = query.Where(q => false);
            foreach (KpiItemContentFilter KpiItemContentFilter in filter.OrFilter)
            {
                IQueryable<KpiItemContentDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, KpiItemContentFilter.Id);
                queryable = queryable.Where(q => q.KpiItemId, KpiItemContentFilter.KpiItemId);
                queryable = queryable.Where(q => q.ItemId, KpiItemContentFilter.ItemId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<KpiItemContentDAO>
            DynamicOrder(IQueryable<KpiItemContentDAO>
                query, KpiItemContentFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case KpiItemContentOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case KpiItemContentOrder.KpiItem:
                            query = query.OrderBy(q => q.KpiItemId);
                            break;
                        case KpiItemContentOrder.Item:
                            query = query.OrderBy(q => q.ItemId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case KpiItemContentOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case KpiItemContentOrder.KpiItem:
                            query = query.OrderByDescending(q => q.KpiItemId);
                            break;
                        case KpiItemContentOrder.Item:
                            query = query.OrderByDescending(q => q.ItemId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<KpiItemContent>> DynamicSelect(IQueryable<KpiItemContentDAO> query, KpiItemContentFilter filter)
        {
            List<KpiItemContent> KpiItemContents = await query.Select(q => new KpiItemContent()
            {
                Id = filter.Selects.Contains(KpiItemContentSelect.Id) ? q.Id : default(long),
                KpiItemId = filter.Selects.Contains(KpiItemContentSelect.KpiItem) ? q.KpiItemId : default(long),
                ItemId = filter.Selects.Contains(KpiItemContentSelect.Item) ? q.ItemId : default(long),
                Item = filter.Selects.Contains(KpiItemContentSelect.Item) && q.Item != null ? new Item
                {
                    Id = q.Item.Id,
                    ProductId = q.Item.ProductId,
                    Code = q.Item.Code,
                    Name = q.Item.Name,
                    ScanCode = q.Item.ScanCode,
                    SalePrice = q.Item.SalePrice,
                    RetailPrice = q.Item.RetailPrice,
                    StatusId = q.Item.StatusId,
                    Used = q.Item.Used,
                } : null,
                KpiItem = filter.Selects.Contains(KpiItemContentSelect.KpiItem) && q.KpiItem != null ? new KpiItem
                {
                    Id = q.KpiItem.Id,
                    OrganizationId = q.KpiItem.OrganizationId,
                    KpiYearId = q.KpiItem.KpiYearId,
                    KpiPeriodId = q.KpiItem.KpiPeriodId,
                    StatusId = q.KpiItem.StatusId,
                    EmployeeId = q.KpiItem.EmployeeId,
                    CreatorId = q.KpiItem.CreatorId,
                } : null,
            }).ToListWithNoLockAsync();

            var KpiItemContentIds = KpiItemContents.Select(x => x.Id).ToList();
            IdFilter KpiItemContentIdFilter = new IdFilter { In = KpiItemContentIds };
            List<KpiItemContentKpiCriteriaItemMapping> KpiItemContentKpiItemCriteriaMappings = await DataContext.KpiItemContentKpiCriteriaItemMapping
                .Where(x => x.KpiItemContentId, KpiItemContentIdFilter)
                .Select(x => new KpiItemContentKpiCriteriaItemMapping
                {
                    KpiCriteriaItemId = x.KpiCriteriaItemId,
                    KpiItemContentId = x.KpiItemContentId,
                    Value = x.Value,
                    KpiItemContent = new KpiItemContent
                    {
                        Id = x.KpiItemContent.Id,
                        ItemId = x.KpiItemContent.ItemId,
                        KpiItemId = x.KpiItemContent.KpiItemId,
                        Item = new Item
                        {
                            Id = x.KpiItemContent.Item.Id,
                            ProductId = x.KpiItemContent.Item.ProductId,
                            Code = x.KpiItemContent.Item.Code,
                            Name = x.KpiItemContent.Item.Name,
                            ScanCode = x.KpiItemContent.Item.ScanCode,
                            SalePrice = x.KpiItemContent.Item.SalePrice,
                            RetailPrice = x.KpiItemContent.Item.RetailPrice,
                            StatusId = x.KpiItemContent.Item.StatusId,
                        },
                    }
                }).ToListWithNoLockAsync();
            foreach (KpiItemContent KpiItemContent in KpiItemContents)
            {
                KpiItemContent.KpiItemContentKpiCriteriaItemMappings = KpiItemContentKpiItemCriteriaMappings.Where(x => x.KpiItemContentId == KpiItemContent.Id).ToList();
            }

            return KpiItemContents;
        }

        public async Task<int> Count(KpiItemContentFilter filter)
        {
            IQueryable<KpiItemContentDAO> KpiItemContents = DataContext.KpiItemContent.AsNoTracking();
            KpiItemContents = DynamicFilter(KpiItemContents, filter);
            KpiItemContents = OrFilter(KpiItemContents, filter);
            return await KpiItemContents.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(KpiItemContentFilter filter)
        {
            IQueryable<KpiItemContentDAO> KpiItemContents = DataContext.KpiItemContent.AsNoTracking();
            KpiItemContents = DynamicFilter(KpiItemContents, filter);
            return await KpiItemContents.CountWithNoLockAsync();
        }

        public async Task<List<KpiItemContent>> List(KpiItemContentFilter filter)
        {
            if (filter == null) return new List<KpiItemContent>();
            IQueryable<KpiItemContentDAO> KpiItemContentDAOs = DataContext.KpiItemContent.AsNoTracking();
            KpiItemContentDAOs = DynamicFilter(KpiItemContentDAOs, filter);
            KpiItemContentDAOs = OrFilter(KpiItemContentDAOs, filter);
            KpiItemContentDAOs = DynamicOrder(KpiItemContentDAOs, filter);
            List<KpiItemContent> KpiItemContents = await DynamicSelect(KpiItemContentDAOs, filter);
            return KpiItemContents;
        }

        public async Task<KpiItemContent> Get(long Id)
        {
            KpiItemContent KpiItemContent = await DataContext.KpiItemContent.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new KpiItemContent()
            {
                Id = x.Id,
                KpiItemId = x.KpiItemId,
                ItemId = x.ItemId,
                Item = x.Item == null ? null : new Item
                {
                    Id = x.Item.Id,
                    ProductId = x.Item.ProductId,
                    Code = x.Item.Code,
                    Name = x.Item.Name,
                    ScanCode = x.Item.ScanCode,
                    SalePrice = x.Item.SalePrice,
                    RetailPrice = x.Item.RetailPrice,
                    StatusId = x.Item.StatusId,
                    Used = x.Item.Used,
                },
                KpiItem = x.KpiItem == null ? null : new KpiItem
                {
                    Id = x.KpiItem.Id,
                    OrganizationId = x.KpiItem.OrganizationId,
                    KpiYearId = x.KpiItem.KpiYearId,
                    KpiPeriodId = x.KpiItem.KpiPeriodId,
                    StatusId = x.KpiItem.StatusId,
                    EmployeeId = x.KpiItem.EmployeeId,
                    CreatorId = x.KpiItem.CreatorId,
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (KpiItemContent == null)
                return null;

            return KpiItemContent;
        }
        public async Task<bool> Create(KpiItemContent KpiItemContent)
        {
            KpiItemContentDAO KpiItemContentDAO = new KpiItemContentDAO();
            KpiItemContentDAO.Id = KpiItemContent.Id;
            KpiItemContentDAO.KpiItemId = KpiItemContent.KpiItemId;
            KpiItemContentDAO.ItemId = KpiItemContent.ItemId;
            DataContext.KpiItemContent.Add(KpiItemContentDAO);
            await DataContext.SaveChangesAsync();
            KpiItemContent.Id = KpiItemContentDAO.Id;
            await SaveReference(KpiItemContent);
            return true;
        }

        public async Task<bool> Update(KpiItemContent KpiItemContent)
        {
            KpiItemContentDAO KpiItemContentDAO = DataContext.KpiItemContent.Where(x => x.Id == KpiItemContent.Id).FirstOrDefault();
            if (KpiItemContentDAO == null)
                return false;
            KpiItemContentDAO.Id = KpiItemContent.Id;
            KpiItemContentDAO.KpiItemId = KpiItemContent.KpiItemId;
            KpiItemContentDAO.ItemId = KpiItemContent.ItemId;
            await DataContext.SaveChangesAsync();
            await SaveReference(KpiItemContent);
            return true;
        }

        public async Task<bool> Delete(KpiItemContent KpiItemContent)
        {
            await DataContext.KpiItemContent.Where(x => x.Id == KpiItemContent.Id).DeleteFromQueryAsync();
            return true;
        }

        public async Task<bool> BulkMerge(List<KpiItemContent> KpiItemContents)
        {
            List<KpiItemContentDAO> KpiItemContentDAOs = new List<KpiItemContentDAO>();
            foreach (KpiItemContent KpiItemContent in KpiItemContents)
            {
                KpiItemContentDAO KpiItemContentDAO = new KpiItemContentDAO();
                KpiItemContentDAO.Id = KpiItemContent.Id;
                KpiItemContentDAO.KpiItemId = KpiItemContent.KpiItemId;
                KpiItemContentDAO.ItemId = KpiItemContent.ItemId;
                KpiItemContentDAOs.Add(KpiItemContentDAO);
            }
            await DataContext.BulkMergeAsync(KpiItemContentDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<KpiItemContent> KpiItemContents)
        {
            List<long> Ids = KpiItemContents.Select(x => x.Id).ToList();
            await DataContext.KpiItemContent
                .Where(x => Ids.Contains(x.Id))
                .DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(KpiItemContent KpiItemContent)
        {
        }

    }
}
