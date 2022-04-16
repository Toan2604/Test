using TrueSight.Common;
using DMS.Common;
using DMS.Helpers;
using DMS.Entities;
using DMS.Models;
using DMS.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thinktecture;
using Thinktecture.EntityFrameworkCore.TempTables;

namespace DMS.Repositories
{
    public interface IDirectSalesOrderSourceTypeRepository
    {
        Task<int> Count(DirectSalesOrderSourceTypeFilter DirectSalesOrderSourceTypeFilter);
        Task<List<DirectSalesOrderSourceType>> List(DirectSalesOrderSourceTypeFilter DirectSalesOrderSourceTypeFilter);
    }
    public class DirectSalesOrderSourceTypeRepository : IDirectSalesOrderSourceTypeRepository
    {
        private DataContext DataContext;
        public DirectSalesOrderSourceTypeRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private async Task<IQueryable<DirectSalesOrderSourceTypeDAO>> DynamicFilter(IQueryable<DirectSalesOrderSourceTypeDAO> query, DirectSalesOrderSourceTypeFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            return query;
        }

        private async Task<IQueryable<DirectSalesOrderSourceTypeDAO>> OrFilter(IQueryable<DirectSalesOrderSourceTypeDAO> query, DirectSalesOrderSourceTypeFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<DirectSalesOrderSourceTypeDAO> initQuery = query.Where(q => false);
            foreach (DirectSalesOrderSourceTypeFilter DirectSalesOrderSourceTypeFilter in filter.OrFilter)
            {
                IQueryable<DirectSalesOrderSourceTypeDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, DirectSalesOrderSourceTypeFilter.Id);
                queryable = queryable.Where(q => q.Code, DirectSalesOrderSourceTypeFilter.Code);
                queryable = queryable.Where(q => q.Name, DirectSalesOrderSourceTypeFilter.Name);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }    

        private IQueryable<DirectSalesOrderSourceTypeDAO> DynamicOrder(IQueryable<DirectSalesOrderSourceTypeDAO> query, DirectSalesOrderSourceTypeFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case DirectSalesOrderSourceTypeOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case DirectSalesOrderSourceTypeOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case DirectSalesOrderSourceTypeOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case DirectSalesOrderSourceTypeOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case DirectSalesOrderSourceTypeOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case DirectSalesOrderSourceTypeOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<DirectSalesOrderSourceType>> DynamicSelect(IQueryable<DirectSalesOrderSourceTypeDAO> query, DirectSalesOrderSourceTypeFilter filter)
        {
            List<DirectSalesOrderSourceType> DirectSalesOrderSourceTypes = await query.Select(q => new DirectSalesOrderSourceType()
            {
                Id = filter.Selects.Contains(DirectSalesOrderSourceTypeSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(DirectSalesOrderSourceTypeSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(DirectSalesOrderSourceTypeSelect.Name) ? q.Name : default(string),
            }).ToListAsync();
            return DirectSalesOrderSourceTypes;
        }

        public async Task<int> Count(DirectSalesOrderSourceTypeFilter filter)
        {
            IQueryable<DirectSalesOrderSourceTypeDAO> DirectSalesOrderSourceTypeDAOs = DataContext.DirectSalesOrderSourceType.AsNoTracking();
            DirectSalesOrderSourceTypeDAOs = await DynamicFilter(DirectSalesOrderSourceTypeDAOs, filter);
            DirectSalesOrderSourceTypeDAOs = await OrFilter(DirectSalesOrderSourceTypeDAOs, filter);
            return await DirectSalesOrderSourceTypeDAOs.CountAsync();
        }

        public async Task<List<DirectSalesOrderSourceType>> List(DirectSalesOrderSourceTypeFilter filter)
        {
            if (filter == null) return new List<DirectSalesOrderSourceType>();
            IQueryable<DirectSalesOrderSourceTypeDAO> DirectSalesOrderSourceTypeDAOs = DataContext.DirectSalesOrderSourceType.AsNoTracking();
            DirectSalesOrderSourceTypeDAOs = await DynamicFilter(DirectSalesOrderSourceTypeDAOs, filter);
            DirectSalesOrderSourceTypeDAOs = await OrFilter(DirectSalesOrderSourceTypeDAOs, filter);
            DirectSalesOrderSourceTypeDAOs = DynamicOrder(DirectSalesOrderSourceTypeDAOs, filter);
            List<DirectSalesOrderSourceType> DirectSalesOrderSourceTypes = await DynamicSelect(DirectSalesOrderSourceTypeDAOs, filter);
            return DirectSalesOrderSourceTypes;
        }

        
    }
}
