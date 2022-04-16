using TrueSight.Common;
using DMS.ABE.Common;
using DMS.ABE.Helpers;
using DMS.ABE.Entities;
using DMS.ABE.Models;
using DMS.ABE.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thinktecture;
using Thinktecture.EntityFrameworkCore.TempTables;

namespace DMS.ABE.Repositories
{
    public interface ILuckyDrawNumberRepository
    {
        Task<int> CountAll(LuckyDrawNumberFilter LuckyDrawNumberFilter);
        Task<int> Count(LuckyDrawNumberFilter LuckyDrawNumberFilter);
        Task<List<LuckyDrawNumber>> List(LuckyDrawNumberFilter LuckyDrawNumberFilter);
        Task<List<LuckyDrawNumber>> List(List<long> Ids);
        Task<LuckyDrawNumber> Get(long Id);
        Task<bool> Used(List<long> Ids);
    }
    public class LuckyDrawNumberRepository : ILuckyDrawNumberRepository
    {
        private DataContext DataContext;
        public LuckyDrawNumberRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private async Task<IQueryable<LuckyDrawNumberDAO>> DynamicFilter(IQueryable<LuckyDrawNumberDAO> query, LuckyDrawNumberFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Used, filter.Used);
            query = query.Where(q => q.LuckyDrawStructureId, filter.LuckyDrawStructureId);
            return query;
        }

        private async Task<IQueryable<LuckyDrawNumberDAO>> OrFilter(IQueryable<LuckyDrawNumberDAO> query, LuckyDrawNumberFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<LuckyDrawNumberDAO> initQuery = query.Where(q => false);
            foreach (LuckyDrawNumberFilter LuckyDrawNumberFilter in filter.OrFilter)
            {
                IQueryable<LuckyDrawNumberDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, LuckyDrawNumberFilter.Id);
                queryable = queryable.Where(q => q.Used, LuckyDrawNumberFilter.Used);
                queryable = queryable.Where(q => q.LuckyDrawStructureId, LuckyDrawNumberFilter.LuckyDrawStructureId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }    

        private IQueryable<LuckyDrawNumberDAO> DynamicOrder(IQueryable<LuckyDrawNumberDAO> query, LuckyDrawNumberFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case LuckyDrawNumberOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case LuckyDrawNumberOrder.LuckyDrawStructure:
                            query = query.OrderBy(q => q.LuckyDrawStructureId);
                            break;
                        case LuckyDrawNumberOrder.Used:
                            query = query.OrderBy(q => q.Used);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case LuckyDrawNumberOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case LuckyDrawNumberOrder.LuckyDrawStructure:
                            query = query.OrderByDescending(q => q.LuckyDrawStructureId);
                            break;
                        case LuckyDrawNumberOrder.Used:
                            query = query.OrderByDescending(q => q.Used);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<LuckyDrawNumber>> DynamicSelect(IQueryable<LuckyDrawNumberDAO> query, LuckyDrawNumberFilter filter)
        {
            List<LuckyDrawNumber> LuckyDrawNumbers = await query.Select(q => new LuckyDrawNumber()
            {
                Id = filter.Selects.Contains(LuckyDrawNumberSelect.Id) ? q.Id : default(long),
                LuckyDrawStructureId = filter.Selects.Contains(LuckyDrawNumberSelect.LuckyDrawStructure) ? q.LuckyDrawStructureId : default(long),
                Used = filter.Selects.Contains(LuckyDrawNumberSelect.Used) ? q.Used : default(bool),
                LuckyDrawStructure = filter.Selects.Contains(LuckyDrawNumberSelect.LuckyDrawStructure) && q.LuckyDrawStructure != null ? new LuckyDrawStructure
                {
                    Id = q.LuckyDrawStructure.Id,
                    LuckyDrawId = q.LuckyDrawStructure.LuckyDrawId,
                    Name = q.LuckyDrawStructure.Name,
                    Value = q.LuckyDrawStructure.Value,
                    Quantity = q.LuckyDrawStructure.Quantity,
                } : null,
            }).ToListAsync();
            return LuckyDrawNumbers;
        }

        public async Task<int> CountAll(LuckyDrawNumberFilter filter)
        {
            IQueryable<LuckyDrawNumberDAO> LuckyDrawNumberDAOs = DataContext.LuckyDrawNumber.AsNoTracking();
            LuckyDrawNumberDAOs = await DynamicFilter(LuckyDrawNumberDAOs, filter);
            return await LuckyDrawNumberDAOs.CountAsync();
        }

        public async Task<int> Count(LuckyDrawNumberFilter filter)
        {
            IQueryable<LuckyDrawNumberDAO> LuckyDrawNumberDAOs = DataContext.LuckyDrawNumber.AsNoTracking();
            LuckyDrawNumberDAOs = await DynamicFilter(LuckyDrawNumberDAOs, filter);
            LuckyDrawNumberDAOs = await OrFilter(LuckyDrawNumberDAOs, filter);
            return await LuckyDrawNumberDAOs.CountAsync();
        }

        public async Task<List<LuckyDrawNumber>> List(LuckyDrawNumberFilter filter)
        {
            if (filter == null) return new List<LuckyDrawNumber>();
            IQueryable<LuckyDrawNumberDAO> LuckyDrawNumberDAOs = DataContext.LuckyDrawNumber.AsNoTracking();
            LuckyDrawNumberDAOs = await DynamicFilter(LuckyDrawNumberDAOs, filter);
            LuckyDrawNumberDAOs = await OrFilter(LuckyDrawNumberDAOs, filter);
            LuckyDrawNumberDAOs = DynamicOrder(LuckyDrawNumberDAOs, filter);
            List<LuckyDrawNumber> LuckyDrawNumbers = await DynamicSelect(LuckyDrawNumberDAOs, filter);
            return LuckyDrawNumbers;
        }

        public async Task<List<LuckyDrawNumber>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };

            IQueryable<LuckyDrawNumberDAO> query = DataContext.LuckyDrawNumber.AsNoTracking();
            query = query.Where(q => q.Id, IdFilter);
            List<LuckyDrawNumber> LuckyDrawNumbers = await query.AsNoTracking()
            .Select(x => new LuckyDrawNumber()
            {
                Id = x.Id,
                LuckyDrawStructureId = x.LuckyDrawStructureId,
                Used = x.Used,
                LuckyDrawStructure = x.LuckyDrawStructure == null ? null : new LuckyDrawStructure
                {
                    Id = x.LuckyDrawStructure.Id,
                    LuckyDrawId = x.LuckyDrawStructure.LuckyDrawId,
                    Name = x.LuckyDrawStructure.Name,
                    Value = x.LuckyDrawStructure.Value,
                    Quantity = x.LuckyDrawStructure.Quantity,
                },
            }).ToListAsync();
            

            return LuckyDrawNumbers;
        }

        public async Task<LuckyDrawNumber> Get(long Id)
        {
            LuckyDrawNumber LuckyDrawNumber = await DataContext.LuckyDrawNumber.AsNoTracking()
            .Where(x => x.Id == Id)
            .Select(x => new LuckyDrawNumber()
            {
                Id = x.Id,
                LuckyDrawStructureId = x.LuckyDrawStructureId,
                Used = x.Used,
                LuckyDrawStructure = x.LuckyDrawStructure == null ? null : new LuckyDrawStructure
                {
                    Id = x.LuckyDrawStructure.Id,
                    LuckyDrawId = x.LuckyDrawStructure.LuckyDrawId,
                    Name = x.LuckyDrawStructure.Name,
                    Value = x.LuckyDrawStructure.Value,
                    Quantity = x.LuckyDrawStructure.Quantity,
                },
            }).FirstOrDefaultAsync();

            if (LuckyDrawNumber == null)
                return null;

            return LuckyDrawNumber;
        }
               
        public async Task<bool> Used(List<long> Ids)
        {
            await DataContext.LuckyDrawNumber
                .WhereBulkContains(Ids, x => x.Id)
                .UpdateFromQueryAsync(x => new LuckyDrawNumberDAO { Used = true });
            return true;
        }
    }
}
