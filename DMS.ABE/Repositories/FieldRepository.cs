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
    public interface IFieldRepository
    {
        Task<int> Count(FieldFilter FieldFilter);
        Task<List<Field>> List(FieldFilter FieldFilter);
        Task<Field> Get(long Id);
    }
    public class FieldRepository : IFieldRepository
    {
        private DataContext DataContext;
        public FieldRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<FieldDAO> DynamicFilter(IQueryable<FieldDAO> query, FieldFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.MenuId, filter.MenuId);
            query = query.Where(q => q.FieldTypeId, filter.FieldTypeId);
            query = OrFilter(query, filter);
            return query;
        }

        private IQueryable<FieldDAO> OrFilter(IQueryable<FieldDAO> query, FieldFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<FieldDAO> initQuery = query.Where(q => false);
            foreach (FieldFilter FieldFilter in filter.OrFilter)
            {
                IQueryable<FieldDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, FieldFilter.Id);
                queryable = queryable.Where(q => q.FieldTypeId, filter.FieldTypeId);
                queryable = queryable.Where(q => q.MenuId, filter.MenuId);
                queryable = queryable.Where(q => q.Name, FieldFilter.Name);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<FieldDAO> DynamicOrder(IQueryable<FieldDAO> query, FieldFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case FieldOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case FieldOrder.Menu:
                            query = query.OrderBy(q => q.Menu.Name);
                            break;
                        case FieldOrder.FieldType:
                            query = query.OrderBy(q => q.FieldType.Name);
                            break;
                        case FieldOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case FieldOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case FieldOrder.Menu:
                            query = query.OrderByDescending(q => q.Menu.Name);
                            break;
                        case FieldOrder.FieldType:
                            query = query.OrderByDescending(q => q.FieldType.Name);
                            break;
                        case FieldOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<Field>> DynamicSelect(IQueryable<FieldDAO> query, FieldFilter filter)
        {
            List<Field> Fields = await query.Select(q => new Field()
            {
                Id = filter.Selects.Contains(FieldSelect.Id) ? q.Id : default(long),
                FieldTypeId = filter.Selects.Contains(FieldSelect.FieldType) ? q.FieldTypeId : default(long),
                Name = filter.Selects.Contains(FieldSelect.Name) ? q.Name : default(string),
            }).ToListAsync();
            return Fields;
        }

        public async Task<int> Count(FieldFilter filter)
        {
            IQueryable<FieldDAO> Fields = DataContext.Field.AsNoTracking();
            Fields = DynamicFilter(Fields, filter);
            return await Fields.CountAsync();
        }

        public async Task<List<Field>> List(FieldFilter filter)
        {
            if (filter == null) return new List<Field>();
            IQueryable<FieldDAO> FieldDAOs = DataContext.Field.AsNoTracking();
            FieldDAOs = DynamicFilter(FieldDAOs, filter);
            FieldDAOs = DynamicOrder(FieldDAOs, filter);
            List<Field> Fields = await DynamicSelect(FieldDAOs, filter);
            return Fields;
        }

        public async Task<Field> Get(long Id)
        {
            Field Field = await DataContext.Field.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new Field()
            {
                Id = x.Id,
                FieldTypeId = x.FieldTypeId,
                MenuId = x.MenuId,
                Name = x.Name,
            }).FirstOrDefaultAsync();

            if (Field == null)
                return null;

            return Field;
        }
    }
}
