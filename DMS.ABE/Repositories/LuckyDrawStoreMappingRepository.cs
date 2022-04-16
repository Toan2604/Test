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
    public interface ILuckyDrawStoreMappingRepository
    {
        Task<int> Count(LuckyDrawStoreMappingFilter LuckyDrawStoreMappingFilter);
        Task<List<LuckyDrawStoreMapping>> List(LuckyDrawStoreMappingFilter LuckyDrawStoreMappingFilter);
    }
    public class LuckyDrawStoreMappingRepository : ILuckyDrawStoreMappingRepository
    {
        private readonly DataContext DataContext;

        public LuckyDrawStoreMappingRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<LuckyDrawStoreMappingDAO> DynamicFilter(IQueryable<LuckyDrawStoreMappingDAO> query, LuckyDrawStoreMappingFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.LuckyDrawId, filter.LuckyDrawId);
            query = query.Where(q => q.StoreId, filter.StoreId);

            return query;
        }

        public async Task<int> Count(LuckyDrawStoreMappingFilter filter)
        {
            IQueryable<LuckyDrawStoreMappingDAO> LuckyDrawStoreMappings = DataContext.LuckyDrawStoreMapping.AsNoTracking();
            LuckyDrawStoreMappings = DynamicFilter(LuckyDrawStoreMappings, filter);
            return await LuckyDrawStoreMappings.CountAsync();
        }

        public async Task<List<LuckyDrawStoreMapping>> List(LuckyDrawStoreMappingFilter filter)
        {
            if (filter == null) return new List<LuckyDrawStoreMapping>();
            IQueryable<LuckyDrawStoreMappingDAO> LuckyDrawStoreMappingDAOs = DataContext.LuckyDrawStoreMapping.AsNoTracking();
            LuckyDrawStoreMappingDAOs = DynamicFilter(LuckyDrawStoreMappingDAOs, filter);
            List<LuckyDrawStoreMapping> LuckyDrawStoreMappings = await LuckyDrawStoreMappingDAOs.Select(x => new LuckyDrawStoreMapping
            {
                LuckyDrawId = x.LuckyDrawId,
                StoreId = x.StoreId,
                LuckyDraw = new LuckyDraw
                {
                    Id = x.LuckyDraw.Id,
                    OrganizationId = x.LuckyDraw.OrganizationId
                }
            }).ToListAsync();
            return LuckyDrawStoreMappings;
        }
    }

}
