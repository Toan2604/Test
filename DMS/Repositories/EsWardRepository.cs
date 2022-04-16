using DMS.Entities;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Repositories
{
    public interface IEsWardRepository
    {
        Task<bool> BulkMerge(List<Ward> Wards);
        Task<long> Count(WardFilter WardFilter);
        Task<List<Ward>> List(WardFilter WardFilter);
        Task<List<Ward>> List(List<long> Ids);
        Task<Ward> Get(long Id);
    }
    public class EsWardRepository : IEsWardRepository
    {
        private IElasticClient client;
        public EsWardRepository(IElasticClient _client)
        {
            this.client = _client;
        }


        private BoolQueryDescriptor<Ward> DynamicQuery(BoolQueryDescriptor<Ward> query, WardFilter filter)
        {
            if (filter == null)
                return query;
            query = query.EsWhere(q => q.Id, filter.Id);
            query = query.EsWhere(q => q.Code, filter.Code);
            query = query.EsWhere(q => q.Name, filter.Name);
            query = query.EsWhere(q => q.Priority, filter.Priority);
            query = query.EsWhere(q => q.DistrictId, filter.DistrictId);
            query = query.EsWhere(q => q.StatusId, filter.StatusId);

            if (filter.Search != null)
            {
                List<Nest.Field> Fields = new List<Nest.Field>();
                Fields.Add(new Nest.Field(nameof(Ward.Name)));

                query.Filter(q => q.MultiMatch(m => m
                    .Fields(f => f.Fields(Fields))
                    .Query(filter.Search)
                    .Fuzziness(Fuzziness.Auto)
                ));
            }

            return query;
        }

        private SortDescriptor<Ward> DynamicOrder(WardFilter WardFilter)
        {
            SortDescriptor<Ward> SortDescriptor = new SortDescriptor<Ward>();
            switch (WardFilter.OrderType)
            {
                case OrderType.ASC:
                    switch (WardFilter.OrderBy)
                    {
                        case WardOrder.Id:
                            SortDescriptor = SortDescriptor.Ascending(q => q.Id);
                            break;
                        case WardOrder.Code:
                            SortDescriptor = SortDescriptor.Ascending(q => q.Code);
                            break;
                        case WardOrder.Name:
                            SortDescriptor = SortDescriptor.Ascending(q => q.Name);
                            break;
                        case WardOrder.Priority:
                            SortDescriptor = SortDescriptor.Ascending(q => q.Priority);
                            break;
                        case WardOrder.District:
                            SortDescriptor = SortDescriptor.Ascending(q => q.District.Name);
                            break;
                        case WardOrder.Status:
                            SortDescriptor = SortDescriptor.Ascending(q => q.StatusId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (WardFilter.OrderBy)
                    {
                        case WardOrder.Id:
                            SortDescriptor = SortDescriptor.Descending(q => q.Id);
                            break;
                        case WardOrder.Code:
                            SortDescriptor = SortDescriptor.Descending(q => q.Code);
                            break;
                        case WardOrder.Name:
                            SortDescriptor = SortDescriptor.Descending(q => q.Name);
                            break;
                        case WardOrder.Priority:
                            SortDescriptor = SortDescriptor.Descending(q => q.Priority);
                            break;
                        case WardOrder.District:
                            SortDescriptor = SortDescriptor.Descending(q => q.District.Name);
                            break;
                        case WardOrder.Status:
                            SortDescriptor = SortDescriptor.Descending(q => q.StatusId);
                            break;
                    }
                    break;
            }
            return SortDescriptor;
        }

        private SourceFilterDescriptor<Ward> DynamicSelect(WardFilter WardFilter)
        {
            SourceFilterDescriptor<Ward> SourceDescriptor = new SourceFilterDescriptor<Ward>();
            List<Nest.Field> Fields = new List<Nest.Field>();

            if (WardFilter.Selects.Contains(WardSelect.Id))
                Fields.Add(new Nest.Field(nameof(Ward.Id)));
            if (WardFilter.Selects.Contains(WardSelect.Code))
                Fields.Add(new Nest.Field(nameof(Ward.Code)));
            if (WardFilter.Selects.Contains(WardSelect.Name))
                Fields.Add(new Nest.Field(nameof(Ward.Name)));
            if (WardFilter.Selects.Contains(WardSelect.Priority))
                Fields.Add(new Nest.Field(nameof(Ward.Priority)));
            if (WardFilter.Selects.Contains(WardSelect.District))
                Fields.Add(new Nest.Field(nameof(Ward.District)));
            if (WardFilter.Selects.Contains(WardSelect.Status))
                Fields.Add(new Nest.Field(nameof(Ward.Status)));

            SourceDescriptor.Includes(q => q.Fields(Fields));
            return SourceDescriptor;
        }

        public async Task<long> Count(WardFilter WardFilter)
        {
            BoolQueryDescriptor<Ward> query = new BoolQueryDescriptor<Ward>();
            query = DynamicQuery(query, WardFilter);
            var ResponseAll = await client.CountAsync<Ward>(s => s
                .Query(q => q.Bool(b => query))
                .Index(nameof(Ward).ToEsIndex())
                );
            return ResponseAll.Count;
        }

        public async Task<Ward> Get(long Id)
        {
            var Response = await client.GetAsync<Ward>(Id, i => i.Index(nameof(Ward).ToEsIndex()));
            Ward ward = Response.Source;
            return ward;
        }


        public async Task<List<Ward>> List(WardFilter WardFilter)
        {
            BoolQueryDescriptor<Ward> query = new BoolQueryDescriptor<Ward>();
            query = DynamicQuery(query, WardFilter);
            var ResponseAll = await client.SearchAsync<Ward>(s => s
                .Query(q => q.Bool(b => query))
                .Index(nameof(Ward).ToEsIndex())
                .From(WardFilter.Skip)
                .Take(WardFilter.Take)
                .Sort(ss => DynamicOrder(WardFilter))
                .Source(src => DynamicSelect(WardFilter))
                );
            List<Ward> Wards = (List<Ward>)ResponseAll.Documents;
            return Wards;
        }

        public async Task<List<Ward>> List(List<long> Ids)
        {
            var Response = await client.MultiGetAsync(m => m
                .Index(nameof(Ward).ToEsIndex())
                .GetMany<Ward>(Ids, (g, id) => g.Index(null)));
            var res = Response.GetMany<Ward>(Ids).Select(h => h.Source).ToList();
            return res;
        }

        public async Task<bool> BulkMerge(List<Ward> Wards)
        {
            await client.BulkAsync(b => b
                .IndexMany(Wards)
                .Index(nameof(Ward).ToEsIndex())
                .Refresh(Elasticsearch.Net.Refresh.WaitFor)
                );
            return true;
        }
    }
}
