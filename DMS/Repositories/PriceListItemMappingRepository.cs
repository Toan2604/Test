using DMS.Entities;
using DMS.Helpers;
using DMS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Repositories
{
    public interface IPriceListItemMappingRepository
    {
        Task<int> Count(PriceListItemMappingFilter PriceListItemMappingFilter);
        Task<List<PriceListItemMapping>> List(PriceListItemMappingFilter PriceListItemMappingFilter);
    }
    public class PriceListItemMappingRepository : IPriceListItemMappingRepository
    {
        private readonly DataContext DataContext;

        public PriceListItemMappingRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private async Task<IQueryable<PriceListItemMappingDAO>> DynamicFilter(IQueryable<PriceListItemMappingDAO> query, PriceListItemMappingFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.PriceList.DeletedAt == null);

            var PriceListQuery = DataContext.PriceList.AsNoTracking();
            var subquery1 = PriceListQuery.Where(x => x.StartDate == null);
            var subquery2 = PriceListQuery
                .Where(x => x.StartDate, new DateFilter { LessEqual = StaticParams.DateTimeNow })
                .Where(x => x.EndDate == null);
            var subquery3 = PriceListQuery
                .Where(x => x.StartDate, new DateFilter { LessEqual = StaticParams.DateTimeNow })
                .Where(x => x.EndDate, new DateFilter { GreaterEqual = StaticParams.DateTimeNow });
            var joinquery = subquery1.Union(subquery2).Union(subquery3);
            var priceListIds = await joinquery.Select(x => x.Id).ToListWithNoLockAsync();
            query = query.Where(x => x.PriceListId, new IdFilter { In = priceListIds });

            query = query.Where(q => q.PriceListId, filter.PriceListId);
            query = query.Where(q => q.ItemId, filter.ItemId);
            query = query.Where(q => q.Price, filter.Price);
            query = query.Where(q => q.PriceList.OrganizationId, filter.OrganizationId);
            //if (filter.OrganizationId != null)
            //{
            //    if (filter.OrganizationId.Equal != null)
            //    {
            //        OrganizationDAO OrganizationDAO = DataContext.Organization
            //            .Where(o => o.Id == filter.OrganizationId.Equal.Value).FirstOrDefault();
            //        query = query.Where(q => q.PriceList.Organization.Path.StartsWith(OrganizationDAO.Path));
            //    }
            //    if (filter.OrganizationId.NotEqual != null)
            //    {
            //        OrganizationDAO OrganizationDAO = DataContext.Organization
            //            .Where(o => o.Id == filter.OrganizationId.NotEqual.Value).FirstOrDefault();
            //        query = query.Where(q => !q.PriceList.Organization.Path.StartsWith(OrganizationDAO.Path));
            //    }
            //    if (filter.OrganizationId.In != null)
            //    {
            //        List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
            //            .Where(o => o.DeletedAt == null && o.StatusId == 1).ToList();
            //        List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => filter.OrganizationId.In.Contains(o.Id)).ToList();
            //        List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
            //        List<long> Ids = Branches.Select(o => o.Id).ToList();
            //        IdFilter IdFilter = new IdFilter { In = Ids };
            //        query = query.Where(x => x.PriceList.OrganizationId, IdFilter);
            //    }
            //    if (filter.OrganizationId.NotIn != null)
            //    {
            //        List<OrganizationDAO> OrganizationDAOs = DataContext.Organization
            //            .Where(o => o.DeletedAt == null && o.StatusId == 1).ToList();
            //        List<OrganizationDAO> Parents = OrganizationDAOs.Where(o => filter.OrganizationId.NotIn.Contains(o.Id)).ToList();
            //        List<OrganizationDAO> Branches = OrganizationDAOs.Where(o => Parents.Any(p => o.Path.StartsWith(p.Path))).ToList();
            //        List<long> Ids = Branches.Select(o => o.Id).ToList();
            //        IdFilter IdFilter = new IdFilter { NotIn = Ids };
            //        query = query.Where(x => x.PriceList.OrganizationId, IdFilter);
            //    }
            //}
            query = query.Where(q => q.PriceList.StatusId, filter.StatusId);
            query = query.Where(q => q.PriceList.PriceListTypeId, filter.PriceListTypeId);
            query = query.Where(q => q.PriceList.SalesOrderTypeId, filter.SalesOrderTypeId);
            if (filter.StoreGroupingId != null)
            {
                if (filter.StoreGroupingId.Equal.HasValue)
                {
                    StoreGroupingDAO StoreGroupingDAO = DataContext.StoreGrouping.Where(x => x.Id == filter.StoreGroupingId.Equal.Value).FirstOrDefault();
                    if (StoreGroupingDAO != null && StoreGroupingDAO.StatusId == Enums.StatusEnum.ACTIVE.Id)
                    {
                        var PriceListStoreGroupingMappingQuery = DataContext.PriceListStoreGroupingMapping.AsNoTracking();
                        PriceListStoreGroupingMappingQuery = PriceListStoreGroupingMappingQuery.Where(x => x.StoreGroupingId, new IdFilter { Equal = StoreGroupingDAO.Id });
                        var PriceListIds = await PriceListStoreGroupingMappingQuery.Select(x => x.PriceListId).ToListWithNoLockAsync();
                        query = query.Where(x => x.PriceListId, new IdFilter { In = PriceListIds });
                    }
                }
            }
            if (filter.StoreTypeId != null)
            {
                if (filter.StoreTypeId.Equal.HasValue)
                {
                    StoreTypeDAO StoreTypeDAO = DataContext.StoreType.Where(x => x.Id == filter.StoreTypeId.Equal.Value).FirstOrDefault();
                    if (StoreTypeDAO != null && StoreTypeDAO.StatusId == Enums.StatusEnum.ACTIVE.Id)
                    {
                        var PriceListStoreTypeMappingQuery = DataContext.PriceListStoreTypeMapping.AsNoTracking();
                        PriceListStoreTypeMappingQuery = PriceListStoreTypeMappingQuery.Where(x => x.StoreTypeId, new IdFilter { Equal = StoreTypeDAO.Id });
                        var PriceListIds = await PriceListStoreTypeMappingQuery.Select(x => x.PriceListId).ToListWithNoLockAsync();
                        query = query.Where(x => x.PriceListId, new IdFilter { In = PriceListIds });
                    }
                }
            }
            if (filter.StoreId != null)
            {
                if (filter.StoreId.Equal.HasValue)
                {
                    StoreDAO StoreDAO = DataContext.Store.Where(x => x.Id == filter.StoreId.Equal.Value).FirstOrDefault();
                    if (StoreDAO != null && StoreDAO.StatusId == Enums.StatusEnum.ACTIVE.Id)
                    {
                        var PriceListStoreMappingQuery = DataContext.PriceListStoreMapping.AsNoTracking();
                        PriceListStoreMappingQuery = PriceListStoreMappingQuery.Where(x => x.StoreId, new IdFilter { Equal = StoreDAO.Id });
                        var PriceListIds = await PriceListStoreMappingQuery.Select(x => x.PriceListId).ToListWithNoLockAsync();
                        query = query.Where(x => x.PriceListId, new IdFilter { In = PriceListIds });
                    }
                }
            }
            return query;
        }

        public async Task<int> Count(PriceListItemMappingFilter filter)
        {
            IQueryable<PriceListItemMappingDAO> PriceListItemMappings = DataContext.PriceListItemMapping.AsNoTracking();
            PriceListItemMappings = await DynamicFilter(PriceListItemMappings, filter);
            return await PriceListItemMappings.CountWithNoLockAsync();
        }

        public async Task<List<PriceListItemMapping>> List(PriceListItemMappingFilter filter)
        {
            if (filter == null) return new List<PriceListItemMapping>();
            string key = $"{nameof(PriceListItemMappingRepository)}.{nameof(PriceListItemMappingRepository.List)}.{filter.ToHash()}";
            List<PriceListItemMapping> PriceListItemMappings = new List<PriceListItemMapping>();
            IQueryable<PriceListItemMappingDAO> PriceListItemMappingDAOs = DataContext.PriceListItemMapping.AsNoTracking();
            PriceListItemMappingDAOs = await DynamicFilter(PriceListItemMappingDAOs, filter);
            PriceListItemMappingDAOs = PriceListItemMappingDAOs.OrderBy(x => x.Price).Skip(0).Take(int.MaxValue);
            PriceListItemMappings = await PriceListItemMappingDAOs.Select(x => new PriceListItemMapping
            {
                PriceListId = x.PriceListId,
                ItemId = x.ItemId,
                Price = x.Price,
                PriceList = new PriceList
                {
                    Id = x.PriceList.Id,
                    OrganizationId = x.PriceList.OrganizationId
                }
            }).ToListWithNoLockAsync();
            return PriceListItemMappings;
        }
    }

}
