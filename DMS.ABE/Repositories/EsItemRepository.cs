using DMS.ABE.Entities;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.ABE.Repositories
{
    public interface IEsItemRepository
    {
        Task<long> Count(ItemFilter ItemFilter);
        Task<List<Item>> List(ItemFilter ItemFilter);
        Task<List<Item>> List(List<long> Ids);
        Task<Item> Get(long Id);
    }
    public class EsItemRepository : IEsItemRepository
    {
        private IElasticClient client;
        public EsItemRepository(IElasticClient _client)
        {
            this.client = _client;
        }
        private BoolQueryDescriptor<Item> DynamicQuery(BoolQueryDescriptor<Item> query, ItemFilter filter)
        {
            const string wildcardSymbol = "*";
            const string singleSpaceSymbol = " ";
            if (filter == null)
                return query;
            query = query.EsWhere(q => q.Id, filter.Id);
            query = query.EsWhere(q => q.ProductId, filter.ProductId);
            query = query.EsWhere(q => q.Code, filter.Code);
            query = query.EsWhere(q => q.Name, filter.Name);
            query = query.EsWhere(q => q.Product.IsNew, filter.IsNew);
            query = query.EsWhere(q => q.Product.OtherName, filter.OtherName);
            query = query.EsWhere(q => q.ScanCode, filter.ScanCode);
            query = query.EsWhere(q => q.SalePrice, filter.SalePrice);
            query = query.EsWhere(q => q.RetailPrice, filter.RetailPrice);
            query = query.EsWhere(q => q.StatusId, filter.StatusId);
            query = query.EsWhere(q => q.Product.CategoryId, filter.CategoryId);
            query = query.MustNot(q => q.Exists(x => x.Field(f => f.DeletedAt)));

            List<Func<QueryContainerDescriptor<Item>, QueryContainer>> FilterList = new List<Func<QueryContainerDescriptor<Item>, QueryContainer>>();

            if (filter.StatusId != null)
            {
                FilterList.Add(q => q.Term(t => t
                    .Field(f => f.StatusId) 
                    .Value(filter.StatusId.Equal)
                ));
            }

            if (filter.CategoryPath != null)
            {
                List<string> ListPathId = filter.CategoryPath.StartWith.Split(".").ToList();
                foreach(string PathId in ListPathId)
                {
                    FilterList.Add(q => q.Wildcard(t => t
                        .Field(f => f.CategoryPath) 
                        .Value(PathId + wildcardSymbol)
                    ));
                }
            }

            query.Filter(FilterList);

            if (filter.Search != null)
            {
                filter.Search = filter.Search.TrimStart().TrimEnd();
                List<Nest.Field> Fields = new List<Nest.Field>();
                Fields.Add(new Nest.Field(nameof(Item.Name)));

                //query.Must(q => q.MultiMatch(m => m
                //    .Fields(f => f.Fields(Fields))
                //    .Query(filter.Search)
                //    .Fuzziness(Fuzziness.Auto)
                //    .Operator(Operator.Or)
                //));
                query.MinimumShouldMatch(1);
                query.Should(
                    s => s.Match(m => m
                        .Query(filter.Search)
                        .Field(f => f.SearchName)
                        .Operator(Operator.Or)
                        .Boost(2)
                    ),
                    s => s.Match(m => m
                        .Query(filter.Search) 
                        .Field(f => f.Code)
                        .Boost(2)
                    ),
                    s => s.Match(m => m
                        .Query(filter.Search)
                        .Field(f => f.SearchName)
                        .Operator(Operator.And)
                        .Boost(3)
                    ),
                    s => s.MatchPhrase(m => m
                        .Query(filter.Search)
                        .Field(f => f.Name)
                        .Boost(4)
                    ),
                    s => s.Wildcard(w => w
                        .Value("*" + filter.Search + "*")
                        .Field(f => f.SearchName)
                    ),
                    s => s.Wildcard(w => w
                        .Value(filter.Search + "*")
                        .Field(f => f.SearchName)
                        .Boost(1.5)
                    )
                );
            }
            return query;
        }

        private SortDescriptor<Item> DynamicOrder(ItemFilter ItemFilter)
        {
            SortDescriptor<Item> SortDescriptor = new SortDescriptor<Item>();
            switch (ItemFilter.OrderType)
            {
                case OrderType.ASC:
                    switch (ItemFilter.OrderBy)
                    {
                        case ItemOrder.Score:
                            break;
                        case ItemOrder.Id:
                            SortDescriptor = SortDescriptor.Ascending(q => q.Id);
                            break;
                        case ItemOrder.Product:
                            SortDescriptor = SortDescriptor.Ascending(q => q.ProductId);
                            break;
                        case ItemOrder.Code:
                            SortDescriptor = SortDescriptor.Ascending(q => q.Code);
                            break;
                        case ItemOrder.Name:
                            SortDescriptor = SortDescriptor.Ascending(q => q.Name);
                            break;
                        case ItemOrder.ScanCode:
                            SortDescriptor = SortDescriptor.Ascending(q => q.ScanCode);
                            break;
                        case ItemOrder.SalePrice:
                            SortDescriptor = SortDescriptor.Ascending(q => q.SalePrice);
                            break;
                        case ItemOrder.RetailPrice:
                            SortDescriptor = SortDescriptor.Ascending(q => q.RetailPrice);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (ItemFilter.OrderBy)
                    {
                        case ItemOrder.Score:
                            break;
                        case ItemOrder.Id:
                            SortDescriptor = SortDescriptor.Descending(q => q.Id);
                            break;
                        case ItemOrder.Product:
                            SortDescriptor = SortDescriptor.Descending(q => q.ProductId);
                            break;
                        case ItemOrder.Code:
                            SortDescriptor = SortDescriptor.Descending(q => q.Code);
                            break;
                        case ItemOrder.Name:
                            SortDescriptor = SortDescriptor.Descending(q => q.Name);
                            break;
                        case ItemOrder.ScanCode:
                            SortDescriptor = SortDescriptor.Descending(q => q.ScanCode);
                            break;
                        case ItemOrder.SalePrice:
                            SortDescriptor = SortDescriptor.Descending(q => q.SalePrice);
                            break;
                        case ItemOrder.RetailPrice:
                            SortDescriptor = SortDescriptor.Descending(q => q.RetailPrice);
                            break;
                    }
                    break;
            }
            return SortDescriptor;
        }

        private SourceFilterDescriptor<Item> DynamicSelect(ItemFilter ItemFilter)
        {
            SourceFilterDescriptor<Item> SourceDescriptor = new SourceFilterDescriptor<Item>();
            List<Nest.Field> Fields = new List<Nest.Field>();

            if (ItemFilter.Selects.Contains(ItemSelect.Id))
                Fields.Add(new Nest.Field(nameof(Item.Id)));
            if (ItemFilter.Selects.Contains(ItemSelect.ProductId))
                Fields.Add(new Nest.Field(nameof(Item.ProductId)));
            if (ItemFilter.Selects.Contains(ItemSelect.Code))
                Fields.Add(new Nest.Field(nameof(Item.Code)));
            if (ItemFilter.Selects.Contains(ItemSelect.ERPCode))
                Fields.Add(new Nest.Field(nameof(Item.ERPCode)));
            if (ItemFilter.Selects.Contains(ItemSelect.Name))
                Fields.Add(new Nest.Field(nameof(Item.Name)));
            if (ItemFilter.Selects.Contains(ItemSelect.ScanCode))
                Fields.Add(new Nest.Field(nameof(Item.ScanCode)));
            if (ItemFilter.Selects.Contains(ItemSelect.SalePrice))
                Fields.Add(new Nest.Field(nameof(Item.SalePrice)));
            if (ItemFilter.Selects.Contains(ItemSelect.RetailPrice))
                Fields.Add(new Nest.Field(nameof(Item.RetailPrice)));
            if (ItemFilter.Selects.Contains(ItemSelect.Status))
                Fields.Add(new Nest.Field(nameof(Item.Status)));
            if (ItemFilter.Selects.Contains(ItemSelect.Product))
                Fields.Add(new Nest.Field(nameof(Item.Product)));
            SourceDescriptor.Includes(q => q.Fields(Fields));
            return SourceDescriptor;
        }


        public async Task<long> Count(ItemFilter ItemFilter)
        {
            if (ItemFilter.Search != null)
            {
                ItemFilter.Search = ItemFilter.Search.ToLower();
            }
            BoolQueryDescriptor<Item> query = new BoolQueryDescriptor<Item>();
            query = DynamicQuery(query, ItemFilter);
            var ResponseAll = await client.CountAsync<Item>(s => s
                .Query(q => q.Bool(b => query))
                .Index(nameof(Item).ToEsIndex())
                );
            return (int)ResponseAll.Count;
        }

        public async Task<List<Item>> List(ItemFilter ItemFilter)
        {
            if (ItemFilter.Search != null)
            {
                ItemFilter.Search = ItemFilter.Search.ToLower();
            }
            BoolQueryDescriptor<Item> query = new BoolQueryDescriptor<Item>();
            query = DynamicQuery(query, ItemFilter);
            var ResponseAll = await client.SearchAsync<Item>(s => s
                .Query(q => q.Bool(b => query))
                .Index(nameof(Item).ToEsIndex())
                .From(ItemFilter.Skip)
                .Take(ItemFilter.Take)
                .Sort(ss => DynamicOrder(ItemFilter))
                //.Source(src => DynamicSelect(ItemFilter))
               );
            List<Item> Items = (List<Item>)ResponseAll.Documents;
            return Items;
        }

        public async Task<List<Item>> List(List<long> Ids)
        {
            var Response = await client.MultiGetAsync(m => m
                .Index(nameof(Item).ToEsIndex())
                .GetMany<Item>(Ids, (g, id) => g.Index(null)));
            var res = Response.GetMany<Item>(Ids).Select(h => h.Source).ToList();
            return res;
        }

        public async Task<Item> Get(long Id)
        {
            var Response = await client.GetAsync<Item>(Id, i => i.Index(nameof(Item).ToEsIndex()));
            Item item = Response.Source;
            return item;
        }
    }
}
