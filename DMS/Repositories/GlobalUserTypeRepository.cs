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
    public interface IGlobalUserTypeRepository
    {
        Task<int> Count(GlobalUserTypeFilter GlobalUserTypeFilter);
        Task<List<GlobalUserType>> List(GlobalUserTypeFilter GlobalUserTypeFilter);
        Task<List<GlobalUserType>> List(List<long> Ids);
        Task<GlobalUserType> Get(long Id);
        Task<bool> BulkMerge(List<GlobalUserType> GlobalUserTypes);
    }
    public class GlobalUserTypeRepository : IGlobalUserTypeRepository
    {
        private DataContext DataContext;
        public GlobalUserTypeRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<GlobalUserTypeDAO> DynamicFilter(IQueryable<GlobalUserTypeDAO> query, GlobalUserTypeFilter filter)
        {
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            return query;
        }

        private IQueryable<GlobalUserTypeDAO> DynamicOrder(IQueryable<GlobalUserTypeDAO> query, GlobalUserTypeFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case GlobalUserTypeOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case GlobalUserTypeOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case GlobalUserTypeOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case GlobalUserTypeOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case GlobalUserTypeOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case GlobalUserTypeOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<GlobalUserType>> DynamicSelect(IQueryable<GlobalUserTypeDAO> query, GlobalUserTypeFilter filter)
        {
            List<GlobalUserType> CallCategories = await query.Select(q => new GlobalUserType()
            {
                Id = filter.Selects.Contains(GlobalUserTypeSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(GlobalUserTypeSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(GlobalUserTypeSelect.Name) ? q.Name : default(string),
            }).ToListWithNoLockAsync();
            return CallCategories;
        }

        public async Task<int> Count(GlobalUserTypeFilter filter)
        {
            IQueryable<GlobalUserTypeDAO> CallCategories = DataContext.GlobalUserType.AsNoTracking();
            CallCategories = DynamicFilter(CallCategories, filter);
            return await CallCategories.CountWithNoLockAsync();
        }

        public async Task<List<GlobalUserType>> List(GlobalUserTypeFilter filter)
        {
            if (filter == null) return new List<GlobalUserType>();
            IQueryable<GlobalUserTypeDAO> GlobalUserTypeDAOs = DataContext.GlobalUserType.AsNoTracking();
            GlobalUserTypeDAOs = DynamicFilter(GlobalUserTypeDAOs, filter);
            GlobalUserTypeDAOs = DynamicOrder(GlobalUserTypeDAOs, filter);
            List<GlobalUserType> GlobalUserTypes = await DynamicSelect(GlobalUserTypeDAOs, filter);
            return GlobalUserTypes;
        }

        public async Task<List<GlobalUserType>> List(List<long> Ids)
        {
            List<GlobalUserType> GlobalUserTypes = await DataContext.GlobalUserType.AsNoTracking()
            .Where(x => Ids.Contains(x.Id)).Select(x => new GlobalUserType()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).ToListWithNoLockAsync();

            return GlobalUserTypes;
        }

        public async Task<GlobalUserType> Get(long Id)
        {
            GlobalUserType GlobalUserType = await DataContext.GlobalUserType.AsNoTracking()
            .Where(x => x.Id == Id)
            .Select(x => new GlobalUserType()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).FirstOrDefaultWithNoLockAsync();

            if (GlobalUserType == null)
                return null;

            return GlobalUserType;
        }

        public async Task<bool> BulkMerge(List<GlobalUserType> GlobalUserTypes)
        {
            List<GlobalUserTypeDAO> GlobalUserTypeDAOs = GlobalUserTypes.Select(x => new GlobalUserTypeDAO
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).ToList();
            await DataContext.BulkMergeAsync(GlobalUserTypeDAOs);
            return true;
        }
    }
}
