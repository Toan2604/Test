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
    public interface IStoreUserFavoriteProductMappingRepository
    {

        Task<bool> Update(long AppUserId, long StoreId);
        Task<bool> Delete(long? AppUserId, long StoreId);
        Task<List<StoreUserFavoriteProductMapping>> List(StoreUserFavoriteProductMappingFilter StoreUserFavoriteProductMappingFilter);
        Task<int> Count(StoreUserFavoriteProductMappingFilter StoreUserFavoriteProductMappingFilter);
    }
    public class StoreUserFavoriteProductMappingRepository : IStoreUserFavoriteProductMappingRepository
    {
        private DataContext DataContext;
        public StoreUserFavoriteProductMappingRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<StoreUserFavoriteProductMappingDAO> DynamicFilter(IQueryable<StoreUserFavoriteProductMappingDAO> query, StoreUserFavoriteProductMappingFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.StoreUserId, filter.StoreUserId);
            query = query.Where(q => q.FavoriteProductId, filter.FavoriteProductId);
            query = OrFilter(query, filter);
            return query;
        }

        private IQueryable<StoreUserFavoriteProductMappingDAO> OrFilter(IQueryable<StoreUserFavoriteProductMappingDAO> query, StoreUserFavoriteProductMappingFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<StoreUserFavoriteProductMappingDAO> initQuery = query.Where(q => false);
            foreach (StoreUserFavoriteProductMappingFilter StoreUserFavoriteProductMappingFilter in filter.OrFilter)
            {
                IQueryable<StoreUserFavoriteProductMappingDAO> queryable = query;
                queryable = queryable.Where(q => q.StoreUserId, StoreUserFavoriteProductMappingFilter.StoreUserId);
                queryable = queryable.Where(q => q.FavoriteProductId, StoreUserFavoriteProductMappingFilter.FavoriteProductId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<StoreUserFavoriteProductMappingDAO> DynamicOrder(IQueryable<StoreUserFavoriteProductMappingDAO> query, StoreUserFavoriteProductMappingFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case StoreUserFavoriteProductMappingOrder.StoreUser:
                            query = query.OrderBy(q => q.StoreUserId);
                            break;
                        case StoreUserFavoriteProductMappingOrder.FavoriteProduct:
                            query = query.OrderBy(q => q.FavoriteProductId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case StoreUserFavoriteProductMappingOrder.StoreUser:
                            query = query.OrderByDescending(q => q.StoreUserId);
                            break;
                        case StoreUserFavoriteProductMappingOrder.FavoriteProduct:
                            query = query.OrderByDescending(q => q.FavoriteProductId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<StoreUserFavoriteProductMapping>> DynamicSelect(IQueryable<StoreUserFavoriteProductMappingDAO> query, StoreUserFavoriteProductMappingFilter filter)
        {
            List<StoreUserFavoriteProductMapping> StoreUserFavoriteProductMappings = await query.Select(q => new StoreUserFavoriteProductMapping()
            {
                StoreUserId = filter.Selects.Contains(StoreUserFavoriteProductMappingSelect.StoreUser) ? q.StoreUserId : default(long),
                FavoriteProductId = filter.Selects.Contains(StoreUserFavoriteProductMappingSelect.FavoriteProduct) ? q.FavoriteProductId : default(long),
            }).ToListAsync();
            return StoreUserFavoriteProductMappings;
        }

        public async Task<List<StoreUserFavoriteProductMapping>> List(StoreUserFavoriteProductMappingFilter StoreUserFavoriteProductMappingFilter)
        {
            try
            {
                if (StoreUserFavoriteProductMappingFilter == null) return new List<StoreUserFavoriteProductMapping>();
                IQueryable<StoreUserFavoriteProductMappingDAO> StoreUserFavoriteProductMappingDAOs = DataContext.StoreUserFavoriteProductMapping.AsNoTracking();
                StoreUserFavoriteProductMappingDAOs = DynamicFilter(StoreUserFavoriteProductMappingDAOs, StoreUserFavoriteProductMappingFilter);
                StoreUserFavoriteProductMappingDAOs = DynamicOrder(StoreUserFavoriteProductMappingDAOs, StoreUserFavoriteProductMappingFilter);
                List<StoreUserFavoriteProductMapping> StoreUserFavoriteProductMappings = await DynamicSelect(StoreUserFavoriteProductMappingDAOs, StoreUserFavoriteProductMappingFilter);
                return StoreUserFavoriteProductMappings;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> Count(StoreUserFavoriteProductMappingFilter StoreUserFavoriteProductMappingFilter)
        {
            try
            {
                IQueryable<StoreUserFavoriteProductMappingDAO> StoreUserFavoriteProductMappings = DataContext.StoreUserFavoriteProductMapping;
                StoreUserFavoriteProductMappings = DynamicFilter(StoreUserFavoriteProductMappings, StoreUserFavoriteProductMappingFilter);
                return await StoreUserFavoriteProductMappings.CountAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> Delete(long? StoreUserId, long FavoriteProductId)
        {
            await DataContext.StoreUserFavoriteProductMapping
                .Where(x => x.FavoriteProductId == FavoriteProductId)
                .Where(x => StoreUserId.HasValue == false || (StoreUserId.HasValue && x.StoreUserId == StoreUserId.Value))
                .DeleteFromQueryAsync();
            return true;
        }

        public async Task<bool> Update(long StoreUserId, long FavoriteProductId)
        {
            StoreUserFavoriteProductMappingDAO StoreUserFavoriteProductMappingDAO = new StoreUserFavoriteProductMappingDAO
            {
                StoreUserId = StoreUserId,
                FavoriteProductId = FavoriteProductId,
            };
            await DataContext.BulkMergeAsync(new List<StoreUserFavoriteProductMappingDAO> { StoreUserFavoriteProductMappingDAO });
            return true;
        }
    }
}
