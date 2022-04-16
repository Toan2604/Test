using DMS.ABE.Entities;
using DMS.ABE.Helpers;
using DMS.ABE.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.ABE.Repositories
{
    public interface ILuckyDrawStoreTypeMappingRepository
    {
        Task<int> Count(LuckyDrawStoreTypeMappingFilter LuckyDrawStoreTypeMappingFilter);
        Task<List<LuckyDrawStoreTypeMapping>> List(LuckyDrawStoreTypeMappingFilter LuckyDrawStoreTypeMappingFilter);
    }
    public class LuckyDrawStoreTypeMappingRepository : ILuckyDrawStoreTypeMappingRepository
    {
        private readonly DataContext DataContext;

        public LuckyDrawStoreTypeMappingRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<LuckyDrawStoreTypeMappingDAO> DynamicFilter(IQueryable<LuckyDrawStoreTypeMappingDAO> query, LuckyDrawStoreTypeMappingFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.LuckyDrawId, filter.LuckyDrawId);
            query = query.Where(q => q.StoreTypeId, filter.StoreTypeId);

            return query;
        }

        public async Task<int> Count(LuckyDrawStoreTypeMappingFilter filter)
        {
            IQueryable<LuckyDrawStoreTypeMappingDAO> LuckyDrawStoreTypeMappings = DataContext.LuckyDrawStoreTypeMapping.AsNoTracking();
            LuckyDrawStoreTypeMappings = DynamicFilter(LuckyDrawStoreTypeMappings, filter);
            return await LuckyDrawStoreTypeMappings.CountAsync();
        }

        public async Task<List<LuckyDrawStoreTypeMapping>> List(LuckyDrawStoreTypeMappingFilter filter)
        {
            if (filter == null) return new List<LuckyDrawStoreTypeMapping>();
            IQueryable<LuckyDrawStoreTypeMappingDAO> LuckyDrawStoreTypeMappingDAOs = DataContext.LuckyDrawStoreTypeMapping.AsNoTracking();
            LuckyDrawStoreTypeMappingDAOs = DynamicFilter(LuckyDrawStoreTypeMappingDAOs, filter);
            List<LuckyDrawStoreTypeMapping> LuckyDrawStoreTypeMappings = await LuckyDrawStoreTypeMappingDAOs.Select(x => new LuckyDrawStoreTypeMapping
            {
                LuckyDrawId = x.LuckyDrawId,
                StoreTypeId = x.StoreTypeId,
                LuckyDraw = new LuckyDraw
                {
                    Id = x.LuckyDraw.Id,
                    OrganizationId = x.LuckyDraw.OrganizationId
                }
            }).ToListAsync();
            return LuckyDrawStoreTypeMappings;
        }
    }

}
