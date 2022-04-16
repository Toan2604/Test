using DMS.Entities;
using DMS.Helpers;
using DMS.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Repositories
{
    public interface ILuckyDrawStoreGroupingMappingRepository
    {
        Task<int> Count(LuckyDrawStoreGroupingMappingFilter LuckyDrawStoreGroupingMappingFilter);
        Task<List<LuckyDrawStoreGroupingMapping>> List(LuckyDrawStoreGroupingMappingFilter LuckyDrawStoreGroupingMappingFilter);
    }
    public class LuckyDrawStoreGroupingMappingRepository : ILuckyDrawStoreGroupingMappingRepository
    {
        private readonly DataContext DataContext;

        public LuckyDrawStoreGroupingMappingRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<LuckyDrawStoreGroupingMappingDAO> DynamicFilter(IQueryable<LuckyDrawStoreGroupingMappingDAO> query, LuckyDrawStoreGroupingMappingFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.LuckyDrawId, filter.LuckyDrawId);
            query = query.Where(q => q.StoreGroupingId, filter.StoreGroupingId);

            return query;
        }

        public async Task<int> Count(LuckyDrawStoreGroupingMappingFilter filter)
        {
            IQueryable<LuckyDrawStoreGroupingMappingDAO> LuckyDrawStoreGroupingMappings = DataContext.LuckyDrawStoreGroupingMapping.AsNoTracking();
            LuckyDrawStoreGroupingMappings = DynamicFilter(LuckyDrawStoreGroupingMappings, filter);
            return await LuckyDrawStoreGroupingMappings.CountWithNoLockAsync();
        }

        public async Task<List<LuckyDrawStoreGroupingMapping>> List(LuckyDrawStoreGroupingMappingFilter filter)
        {
            if (filter == null) return new List<LuckyDrawStoreGroupingMapping>();
            IQueryable<LuckyDrawStoreGroupingMappingDAO> LuckyDrawStoreGroupingMappingDAOs = DataContext.LuckyDrawStoreGroupingMapping.AsNoTracking();
            LuckyDrawStoreGroupingMappingDAOs = DynamicFilter(LuckyDrawStoreGroupingMappingDAOs, filter);
            List<LuckyDrawStoreGroupingMapping> LuckyDrawStoreGroupingMappings = await LuckyDrawStoreGroupingMappingDAOs.Select(x => new LuckyDrawStoreGroupingMapping
            {
                LuckyDrawId = x.LuckyDrawId,
                StoreGroupingId = x.StoreGroupingId,
                LuckyDraw = new LuckyDraw
                {
                    Id = x.LuckyDraw.Id,
                    OrganizationId = x.LuckyDraw.OrganizationId
                }
            }).ToListWithNoLockAsync();
            return LuckyDrawStoreGroupingMappings;
        }
    }

}
