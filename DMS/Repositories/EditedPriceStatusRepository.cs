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
    public interface IEditedPriceStatusRepository
    {
        Task<int> Count(EditedPriceStatusFilter EditedPriceStatusFilter);
        Task<int> CountAll(EditedPriceStatusFilter EditedPriceStatusFilter);
        Task<List<EditedPriceStatus>> List(EditedPriceStatusFilter EditedPriceStatusFilter);
        Task<EditedPriceStatus> Get(long Id);
    }
    public class EditedPriceStatusRepository : IEditedPriceStatusRepository
    {
        private DataContext DataContext;
        public EditedPriceStatusRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<EditedPriceStatusDAO> DynamicFilter(IQueryable<EditedPriceStatusDAO> query, EditedPriceStatusFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            return query;
        }

        private IQueryable<EditedPriceStatusDAO> OrFilter(IQueryable<EditedPriceStatusDAO> query, EditedPriceStatusFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<EditedPriceStatusDAO> initQuery = query.Where(q => false);
            foreach (EditedPriceStatusFilter EditedPriceStatusFilter in filter.OrFilter)
            {
                IQueryable<EditedPriceStatusDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, EditedPriceStatusFilter.Id);
                queryable = queryable.Where(q => q.Code, EditedPriceStatusFilter.Code);
                queryable = queryable.Where(q => q.Name, EditedPriceStatusFilter.Name);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<EditedPriceStatusDAO> DynamicOrder(IQueryable<EditedPriceStatusDAO> query, EditedPriceStatusFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case EditedPriceStatusOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case EditedPriceStatusOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case EditedPriceStatusOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case EditedPriceStatusOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case EditedPriceStatusOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case EditedPriceStatusOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<EditedPriceStatus>> DynamicSelect(IQueryable<EditedPriceStatusDAO> query, EditedPriceStatusFilter filter)
        {
            List<EditedPriceStatus> EditedPriceStatuses = await query.Select(q => new EditedPriceStatus()
            {
                Id = filter.Selects.Contains(EditedPriceStatusSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(EditedPriceStatusSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(EditedPriceStatusSelect.Name) ? q.Name : default(string),
            }).ToListWithNoLockAsync();
            return EditedPriceStatuses;
        }

        public async Task<int> Count(EditedPriceStatusFilter filter)
        {
            IQueryable<EditedPriceStatusDAO> EditedPriceStatuses = DataContext.EditedPriceStatus.AsNoTracking();
            EditedPriceStatuses = DynamicFilter(EditedPriceStatuses, filter);
            EditedPriceStatuses = OrFilter(EditedPriceStatuses, filter);
            return await EditedPriceStatuses.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(EditedPriceStatusFilter filter)
        {
            IQueryable<EditedPriceStatusDAO> EditedPriceStatuses = DataContext.EditedPriceStatus.AsNoTracking();
            EditedPriceStatuses = DynamicFilter(EditedPriceStatuses, filter);
            return await EditedPriceStatuses.CountWithNoLockAsync();
        }

        public async Task<List<EditedPriceStatus>> List(EditedPriceStatusFilter filter)
        {
            if (filter == null) return new List<EditedPriceStatus>();
            IQueryable<EditedPriceStatusDAO> EditedPriceStatusDAOs = DataContext.EditedPriceStatus.AsNoTracking();
            EditedPriceStatusDAOs = DynamicFilter(EditedPriceStatusDAOs, filter);
            EditedPriceStatusDAOs = OrFilter(EditedPriceStatusDAOs, filter);
            EditedPriceStatusDAOs = DynamicOrder(EditedPriceStatusDAOs, filter);
            List<EditedPriceStatus> EditedPriceStatuses = await DynamicSelect(EditedPriceStatusDAOs, filter);
            return EditedPriceStatuses;
        }

        public async Task<EditedPriceStatus> Get(long Id)
        {
            EditedPriceStatus EditedPriceStatus = await DataContext.EditedPriceStatus.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new EditedPriceStatus()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).FirstOrDefaultWithNoLockAsync();

            if (EditedPriceStatus == null)
                return null;

            return EditedPriceStatus;
        }
    }
}
