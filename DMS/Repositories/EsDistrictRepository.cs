using DMS.Entities;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Repositories
{
    public interface IEsDistrictRepository
    {
        Task<bool> BulkMerge(List<District> Districts);
        Task<long> Count(DistrictFilter DistrictFilter);
        Task<List<District>> List(DistrictFilter DistrictFilter);
        Task<List<District>> List(List<long> Ids);
        Task<District> Get(long Id);
    }
    public class EsDistrictRepository : IEsDistrictRepository
    {
        private IElasticClient client;

        public EsDistrictRepository(IElasticClient _client)
        {
            this.client = _client;
        }

        private BoolQueryDescriptor<District> DynamicQuery(BoolQueryDescriptor<District> query, DistrictFilter filter)
        {
            if (filter == null)
                return query;
            query = query.EsWhere(q => q.Id, filter.Id);
            query = query.EsWhere(q => q.Name, filter.Name);
            query = query.EsWhere(q => q.Code, filter.Code);
            query = query.EsWhere(q => q.Priority, filter.Priority);
            query = query.EsWhere(q => q.ProvinceId, filter.ProvinceId);
            query = query.EsWhere(q => q.StatusId, filter.StatusId);
            if (filter.Search != null)
            {
                List<Nest.Field> Fields = new List<Nest.Field>();
                Fields.Add(new Nest.Field(nameof(District.Name)));

                query.Filter(q => q.MultiMatch(m => m
                    .Fields(f => f.Fields(Fields))
                    .Query(filter.Search)
                    .Fuzziness(Fuzziness.Auto)
                ));
            }
            return query;
        }

        private SortDescriptor<District> DynamicOrder(DistrictFilter DistrictFilter)
        {
            SortDescriptor<District> SortDescriptor = new SortDescriptor<District>();
            switch (DistrictFilter.OrderType)
            {
                case OrderType.ASC:
                    switch (DistrictFilter.OrderBy)
                    {
                        case DistrictOrder.Id:
                            SortDescriptor = SortDescriptor.Ascending(q => q.Id);
                            break;
                        case DistrictOrder.Code:
                            SortDescriptor = SortDescriptor.Ascending(q => q.Code);
                            break;
                        case DistrictOrder.Name:
                            SortDescriptor = SortDescriptor.Ascending(q => q.Name);
                            break;
                        case DistrictOrder.Priority:
                            SortDescriptor = SortDescriptor.Ascending(q => q.Priority);
                            break;
                        case DistrictOrder.Province:
                            SortDescriptor = SortDescriptor.Ascending(q => q.Province.Name);
                            break;
                        case DistrictOrder.Status:
                            SortDescriptor = SortDescriptor.Ascending(q => q.StatusId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (DistrictFilter.OrderBy)
                    {
                        case DistrictOrder.Id:
                            SortDescriptor = SortDescriptor.Descending(q => q.Id);
                            break;
                        case DistrictOrder.Code:
                            SortDescriptor = SortDescriptor.Descending(q => q.Code);
                            break;
                        case DistrictOrder.Name:
                            SortDescriptor = SortDescriptor.Descending(q => q.Name);
                            break;
                        case DistrictOrder.Priority:
                            SortDescriptor = SortDescriptor.Descending(q => q.Priority);
                            break;
                        case DistrictOrder.Province:
                            SortDescriptor = SortDescriptor.Descending(q => q.Province.Name);
                            break;
                        case DistrictOrder.Status:
                            SortDescriptor = SortDescriptor.Descending(q => q.StatusId);
                            break;
                    }
                    break;
            }
            return SortDescriptor;
        }

        private SourceFilterDescriptor<District> DynamicSelect(DistrictFilter DistrictFilter)
        {
            SourceFilterDescriptor<District> SourceDescriptor = new SourceFilterDescriptor<District>();
            List<Nest.Field> Fields = new List<Nest.Field>();

            if (DistrictFilter.Selects.Contains(DistrictSelect.Id))
                Fields.Add(new Nest.Field(nameof(District.Id)));
            if (DistrictFilter.Selects.Contains(DistrictSelect.Code))
                Fields.Add(new Nest.Field(nameof(District.Code)));
            if (DistrictFilter.Selects.Contains(DistrictSelect.Name))
                Fields.Add(new Nest.Field(nameof(District.Name)));
            if (DistrictFilter.Selects.Contains(DistrictSelect.Priority))
                Fields.Add(new Nest.Field(nameof(District.Priority)));
            if (DistrictFilter.Selects.Contains(DistrictSelect.Province))
                Fields.Add(new Nest.Field(nameof(District.Province)));
            if (DistrictFilter.Selects.Contains(DistrictSelect.Status))
                Fields.Add(new Nest.Field(nameof(District.Status)));
            SourceDescriptor.Includes(q => q.Fields(Fields));
            return SourceDescriptor;
        }
        public async Task<long> Count(DistrictFilter DistrictFilter)
        {
            BoolQueryDescriptor<District> query = new BoolQueryDescriptor<District>();
            query = DynamicQuery(query, DistrictFilter);
            var ResponseAll = await client.CountAsync<District>(s => s
                .Query(q => q.Bool(b => query)));
            return ResponseAll.Count;
        }

        public async Task<District> Get(long Id)
        {
            var Response = await client.GetAsync<District>(Id, i => i.Index(nameof(District).ToEsIndex()));
            District district = Response.Source;
            return district;
        }

        public async Task<List<District>> List(DistrictFilter DistrictFilter)
        {
            BoolQueryDescriptor<District> query = new BoolQueryDescriptor<District>();
            query = DynamicQuery(query, DistrictFilter);
            var ResponseAll = await client.SearchAsync<District>(s => s
                .Query(q => q.Bool(b => query))
                .Index(nameof(District).ToEsIndex())
                .From(DistrictFilter.Skip)
                .Take(DistrictFilter.Take)
                .Sort(ss => DynamicOrder(DistrictFilter))
                .Source(src => DynamicSelect(DistrictFilter))
                );
            List<District> Districts = (List<District>)ResponseAll.Documents;
            return Districts;
        }


        public async Task<List<District>> List(List<long> Ids)
        {
            var Response = await client.MultiGetAsync(m => m
                .Index(nameof(District).ToEsIndex())
                .GetMany<District>(Ids, (g, id) => g.Index(null)));
            var res = Response.GetMany<District>(Ids).Select(h => h.Source).ToList();
            return res;
        }

        public async Task<bool> BulkMerge(List<District> Districts)
        {
            await client.BulkAsync(b => b
                .IndexMany(Districts)
                .Index(nameof(District).ToEsIndex())
                .Refresh(Elasticsearch.Net.Refresh.WaitFor)
            );
            return true;
        }
    }
}
