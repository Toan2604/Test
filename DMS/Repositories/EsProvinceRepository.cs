using DMS.Entities;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Repositories
{
    public interface IEsProvinceRepository
    {
        Task<bool> BulkMerge(List<Province> Provinces);
        Task<long> Count(ProvinceFilter ProvinceFilter);
        Task<List<Province>> List(ProvinceFilter ProvinceFilter);
        Task<List<Province>> List(List<long> Ids);
        Task<Province> Get(long Id);
    }
    public class EsProvinceRepository : IEsProvinceRepository
    {
        private IElasticClient client;
        public EsProvinceRepository(IElasticClient _client)
        {
            this.client = _client;
        }

        private BoolQueryDescriptor<Province> DynamicQuery(BoolQueryDescriptor<Province> query, ProvinceFilter filter)
        {
            if (filter == null)
                return query;
            query = query.EsWhere(q => q.Id, filter.Id);
            query = query.EsWhere(q => q.Code, filter.Code);
            query = query.EsWhere(q => q.Name, filter.Name);
            query = query.EsWhere(q => q.Priority, filter.Priority);
            query = query.EsWhere(q => q.StatusId, filter.StatusId);
            if (filter.Search != null)
            {
                List<Nest.Field> Fields = new List<Nest.Field>();
                Fields.Add(new Nest.Field(nameof(Province.Name)));

                query.Filter(q => q.MultiMatch(m => m
                    .Fields(f => f.Fields(Fields))
                    .Query(filter.Search)
                    .Fuzziness(Fuzziness.Auto)
                ));
            }
            return query;
        }

        private SortDescriptor<Province> DynamicOrder(ProvinceFilter ProvinceFilter)
        {
            SortDescriptor<Province> SortDescriptor = new SortDescriptor<Province>();
            switch (ProvinceFilter.OrderType)
            {
                case OrderType.ASC:
                    switch (ProvinceFilter.OrderBy)
                    {
                        case ProvinceOrder.Id:
                            SortDescriptor = SortDescriptor.Ascending(q => q.Id);
                            break;
                        case ProvinceOrder.Code:
                            SortDescriptor = SortDescriptor.Ascending(q => q.Code);
                            break;
                        case ProvinceOrder.Name:
                            SortDescriptor = SortDescriptor.Ascending(q => q.Name);
                            break;
                        case ProvinceOrder.Priority:
                            SortDescriptor = SortDescriptor.Ascending(q => q.Priority);
                            break;
                        case ProvinceOrder.Status:
                            SortDescriptor = SortDescriptor.Ascending(q => q.StatusId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (ProvinceFilter.OrderBy)
                    {
                        case ProvinceOrder.Id:
                            SortDescriptor = SortDescriptor.Descending(q => q.Id);
                            break;
                        case ProvinceOrder.Code:
                            SortDescriptor = SortDescriptor.Descending(q => q.Code);
                            break;
                        case ProvinceOrder.Name:
                            SortDescriptor = SortDescriptor.Descending(q => q.Name);
                            break;
                        case ProvinceOrder.Priority:
                            SortDescriptor = SortDescriptor.Descending(q => q.Priority);
                            break;
                        case ProvinceOrder.Status:
                            SortDescriptor = SortDescriptor.Descending(q => q.StatusId);
                            break;
                    }
                    break;
            }
            return SortDescriptor;
        }

        private SourceFilterDescriptor<Province> DynamicSelect(ProvinceFilter ProvinceFilter)
        {
            SourceFilterDescriptor<Province> SourceDescriptor = new SourceFilterDescriptor<Province>();
            List<Nest.Field> Fields = new List<Nest.Field>();

            if (ProvinceFilter.Selects.Contains(ProvinceSelect.Id))
                Fields.Add(new Nest.Field(nameof(Province.Id)));
            if (ProvinceFilter.Selects.Contains(ProvinceSelect.Code))
                Fields.Add(new Nest.Field(nameof(Province.Code)));
            if (ProvinceFilter.Selects.Contains(ProvinceSelect.Name))
                Fields.Add(new Nest.Field(nameof(Province.Name)));
            if (ProvinceFilter.Selects.Contains(ProvinceSelect.Priority))
                Fields.Add(new Nest.Field(nameof(Province.Priority)));
            if (ProvinceFilter.Selects.Contains(ProvinceSelect.Status))
                Fields.Add(new Nest.Field(nameof(Province.Status)));
            if (ProvinceFilter.Selects.Contains(ProvinceSelect.Districts))
                Fields.Add(new Nest.Field(nameof(Province.Districts)));

            SourceDescriptor.Includes(q => q.Fields(Fields));
            return SourceDescriptor;
        }
        public async Task<long> Count(ProvinceFilter ProvinceFilter)
        {
            BoolQueryDescriptor<Province> query = new BoolQueryDescriptor<Province>();
            query = DynamicQuery(query, ProvinceFilter);
            var ResponseAll = await client.CountAsync<Province>(s => s
                .Query(q => q.Bool(b => query))
                .Index(nameof(Province).ToEsIndex())
                );
            return ResponseAll.Count;
        }

        public async Task<Province> Get(long Id)
        {
            var Response = await client.GetAsync<Province>(Id, i => i.Index(nameof(Province).ToEsIndex()));
            Province province = Response.Source;
            return province;
        }


        public async Task<List<Province>> List(ProvinceFilter ProvinceFilter)
        {
            BoolQueryDescriptor<Province> query = new BoolQueryDescriptor<Province>();
            query = DynamicQuery(query, ProvinceFilter);
            var ResponseAll = await client.SearchAsync<Province>(s => s
                .Query(q => q.Bool(b => query))
                .Index(nameof(Province).ToEsIndex())
                .From(ProvinceFilter.Skip)
                .Take(ProvinceFilter.Take)
                .Sort(ss => DynamicOrder(ProvinceFilter))
                .Source(src => DynamicSelect(ProvinceFilter))
                );
            List<Province> Provinces = (List<Province>)ResponseAll.Documents;
            return Provinces;
        }

        public async Task<List<Province>> List(List<long> Ids)
        {
            var Response = await client.MultiGetAsync(m => m
                .Index(nameof(Province).ToEsIndex())
                .GetMany<Province>(Ids, (g, id) => g.Index(null)));
            var res = Response.GetMany<Province>(Ids).Select(h => h.Source).ToList();
            return res;
        }

        public async Task<bool> BulkMerge(List<Province> Provinces)
        {
            await client.BulkAsync(b => b
                .IndexMany(Provinces)
                .Index(nameof(Province).ToEsIndex())
                .Refresh(Elasticsearch.Net.Refresh.WaitFor)
                );
            return true;
        }
    }
}
