using DMS.ABE.Entities;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.ABE.Repositories
{
    public interface IEsProductRepository
    {
        Task<bool> BulkMerge(List<Product> Products);
        Task<int> Count(ProductFilter ProductFilter);
        Task<List<Product>> List(ProductFilter ProductFilter);
        Task<List<Product>> List(List<long> Ids);
        Task<Product> Get(long? Id);
    }
    public class EsProductRepository : IEsProductRepository
    {
        private IElasticClient client;
        public EsProductRepository(IElasticClient _client)
        {
            this.client = _client;
        }

        private BoolQueryDescriptor<Product> DynamicQuery(BoolQueryDescriptor<Product> query, ProductFilter filter)
        {
            const string wildcardSymbol = "*";
            const string singleSpaceSymbol = " ";

            if (filter == null)
                return query;
            query = query.EsWhere(q => q.Id, filter.Id);
            query = query.EsWhere(q => q.Code, filter.Code);
            query = query.EsWhere(q => q.ERPCode, filter.ERPCode);
            query = query.EsWhere(q => q.Name, filter.Name);
            query = query.EsWhere(q => q.Description, filter.Description);
            query = query.EsWhere(q => q.ScanCode, filter.ScanCode);
            query = query.EsWhere(q => q.ProductTypeId, filter.ProductTypeId);
            query = query.EsWhere(q => q.BrandId, filter.BrandId);
            query = query.EsWhere(q => q.UnitOfMeasureId, filter.UnitOfMeasureId);
            query = query.EsWhere(q => q.UnitOfMeasureGroupingId, filter.UnitOfMeasureGroupingId);
            query = query.EsWhere(q => q.SalePrice, filter.SalePrice);
            query = query.EsWhere(q => q.RetailPrice, filter.RetailPrice);
            query = query.EsWhere(q => q.TaxTypeId, filter.TaxTypeId);
            query = query.EsWhere(q => q.StatusId, filter.StatusId);
            query = query.EsWhere(q => q.OtherName, filter.OtherName);
            query = query.EsWhere(q => q.TechnicalName, filter.TechnicalName);
            query = query.EsWhere(q => q.Note, filter.Note);
            query = query.EsWhere(q => q.UsedVariationId, filter.UsedVariationId);
            query = query.EsWhere(q => q.CategoryId, filter.CategoryId);

            if (filter.Search != null)
            {
                filter.Search = filter.Search.TrimStart().TrimEnd();
                List<Func<QueryContainerDescriptor<Product>, QueryContainer>> ShouldQueryList = new List<Func<QueryContainerDescriptor<Product>, QueryContainer>>();

                ShouldQueryList.Add(s => s.Wildcard(w => w
                    .Value(filter.Search + wildcardSymbol)
                    .Field(f => f.SearchName)
                    .Boost(1.5)
                ));
                ShouldQueryList.Add(s => s.Match(m => m
                    .Query(filter.Search) 
                    .Field(f => f.Code)
                    .Boost(2)
                ));
                ShouldQueryList.Add(s => s.Match(m => m
                    .Query(filter.Search)
                    .Field(f => f.SearchName)
                    .Operator(Operator.Or)
                    .Boost(2)
                ));
                ShouldQueryList.Add(s => s.Match(m => m
                    .Query(filter.Search)
                    .Field(f => f.SearchName)
                    .Operator(Operator.And)
                    .Boost(3)
                ));
                ShouldQueryList.Add(s => s.MatchPhrase(m => m
                    .Query(filter.Search)
                    .Field(f => f.Name)
                    .Boost(4)
                ));

                if (filter.Search.Contains(singleSpaceSymbol))
                {
                    List<string> listWordInSearchQuery = filter.Search.Split(singleSpaceSymbol).ToList();
                    foreach (string wordInSearchQuery in listWordInSearchQuery)
                    {
                        ShouldQueryList.Add(s => s.Wildcard(w => w
                            .Value(wildcardSymbol + wordInSearchQuery + wildcardSymbol)
                            .Field(f => f.SearchName)
                        ));
                        ShouldQueryList.Add(s => s.Wildcard(w => w
                            .Value(wildcardSymbol + wordInSearchQuery + wildcardSymbol)
                            .Field(f => f.Name)
                            .Boost(2)
                        ));
                    }
                }
                else
                {
                    ShouldQueryList.Add(s => s.Wildcard(w => w
                        .Value(wildcardSymbol + filter.Search + wildcardSymbol)
                        .Field(f => f.SearchName)
                    ));
                    ShouldQueryList.Add(s => s.Wildcard(w => w
                        .Value(wildcardSymbol + filter.Search + wildcardSymbol)
                        .Field(f => f.Name)
                        .Boost(2)
                    ));
                }

                query.MinimumShouldMatch(1);
                query.Should(ShouldQueryList);
            }
            return query;
        }

        private SortDescriptor<Product> DynamicOrder(ProductFilter ProductFilter)
        {
            SortDescriptor<Product> SortDescriptor = new SortDescriptor<Product>();
            switch (ProductFilter.OrderType)
            {
                case OrderType.ASC:
                    switch (ProductFilter.OrderBy)
                    {
                        case ProductOrder.Id:
                            SortDescriptor = SortDescriptor.Ascending(q => q.Id);
                            break;
                        case ProductOrder.Code:
                            SortDescriptor = SortDescriptor.Ascending(q => q.Code);
                            break;
                        case ProductOrder.Name:
                            SortDescriptor = SortDescriptor.Ascending(q => q.Name);
                            break;
                        case ProductOrder.Description:
                            SortDescriptor = SortDescriptor.Ascending(q => q.Description);
                            break;
                        case ProductOrder.ScanCode:
                            SortDescriptor = SortDescriptor.Ascending(q => q.ScanCode);
                            break;
                        case ProductOrder.ProductType:
                            SortDescriptor = SortDescriptor.Ascending(q => q.ProductType.Name);
                            break;
                        case ProductOrder.Brand:
                            SortDescriptor = SortDescriptor.Ascending(q => q.Brand.Name);
                            break;
                        case ProductOrder.UnitOfMeasure:
                            SortDescriptor = SortDescriptor.Ascending(q => q.UnitOfMeasure.Name);
                            break;
                        case ProductOrder.UnitOfMeasureGrouping:
                            SortDescriptor = SortDescriptor.Ascending(q => q.UnitOfMeasureGrouping.Name);
                            break;
                        case ProductOrder.SalePrice:
                            SortDescriptor = SortDescriptor.Ascending(q => q.SalePrice);
                            break;
                        case ProductOrder.RetailPrice:
                            SortDescriptor = SortDescriptor.Ascending(q => q.RetailPrice);
                            break;
                        case ProductOrder.TaxType:
                            SortDescriptor = SortDescriptor.Ascending(q => q.TaxType.Code);
                            break;
                        case ProductOrder.Status:
                            SortDescriptor = SortDescriptor.Ascending(q => q.StatusId);
                            break;
                        case ProductOrder.OtherName:
                            SortDescriptor = SortDescriptor.Ascending(q => q.OtherName);
                            break;
                        case ProductOrder.TechnicalName:
                            SortDescriptor = SortDescriptor.Ascending(q => q.TechnicalName);
                            break;
                        case ProductOrder.Note:
                            SortDescriptor = SortDescriptor.Ascending(q => q.Note);
                            break;
                        case ProductOrder.UsedVariation:
                            SortDescriptor = SortDescriptor.Ascending(q => q.UsedVariationId);
                            break;
                        default:
                            SortDescriptor = SortDescriptor.Ascending(q => q.UpdatedAt);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (ProductFilter.OrderBy)
                    {
                        case ProductOrder.Id:
                            SortDescriptor = SortDescriptor.Descending(q => q.Id);
                            break;
                        case ProductOrder.Code:
                            SortDescriptor = SortDescriptor.Descending(q => q.Code);
                            break;
                        case ProductOrder.Name:
                            SortDescriptor = SortDescriptor.Descending(q => q.Name);
                            break;
                        case ProductOrder.Description:
                            SortDescriptor = SortDescriptor.Descending(q => q.Description);
                            break;
                        case ProductOrder.ScanCode:
                            SortDescriptor = SortDescriptor.Descending(q => q.ScanCode);
                            break;
                        case ProductOrder.ProductType:
                            SortDescriptor = SortDescriptor.Descending(q => q.ProductType.Name);
                            break;
                        case ProductOrder.Brand:
                            SortDescriptor = SortDescriptor.Descending(q => q.Brand.Name);
                            break;
                        case ProductOrder.UnitOfMeasure:
                            SortDescriptor = SortDescriptor.Descending(q => q.UnitOfMeasure.Name);
                            break;
                        case ProductOrder.UnitOfMeasureGrouping:
                            SortDescriptor = SortDescriptor.Descending(q => q.UnitOfMeasureGrouping.Name);
                            break;
                        case ProductOrder.SalePrice:
                            SortDescriptor = SortDescriptor.Descending(q => q.SalePrice);
                            break;
                        case ProductOrder.RetailPrice:
                            SortDescriptor = SortDescriptor.Descending(q => q.RetailPrice);
                            break;
                        case ProductOrder.TaxType:
                            SortDescriptor = SortDescriptor.Descending(q => q.TaxTypeId);
                            break;
                        case ProductOrder.Status:
                            SortDescriptor = SortDescriptor.Descending(q => q.StatusId);
                            break;
                        case ProductOrder.OtherName:
                            SortDescriptor = SortDescriptor.Descending(q => q.OtherName);
                            break;
                        case ProductOrder.TechnicalName:
                            SortDescriptor = SortDescriptor.Descending(q => q.TechnicalName);
                            break;
                        case ProductOrder.Note:
                            SortDescriptor = SortDescriptor.Descending(q => q.Note);
                            break;
                        case ProductOrder.UsedVariation:
                            SortDescriptor = SortDescriptor.Descending(q => q.UsedVariationId);
                            break;
                        default:
                            SortDescriptor = SortDescriptor.Descending(q => q.UpdatedAt);
                            break;
                    }
                    break;
            }
            return SortDescriptor;
        }

        private SourceFilterDescriptor<Product> DynamicSelect(ProductFilter ProductFilter)
        {
            SourceFilterDescriptor<Product> SourceDescriptor = new SourceFilterDescriptor<Product>();
            List<Nest.Field> Fields = new List<Nest.Field>();
            if (ProductFilter.Selects.Contains(ProductSelect.Id))
                Fields.Add(new Nest.Field(nameof(Product.Id)));
            if (ProductFilter.Selects.Contains(ProductSelect.Code))
                Fields.Add(new Nest.Field(nameof(Product.Code)));
            if (ProductFilter.Selects.Contains(ProductSelect.Name))
                Fields.Add(new Nest.Field(nameof(Product.Name)));
            if (ProductFilter.Selects.Contains(ProductSelect.Description))
                Fields.Add(new Nest.Field(nameof(Product.Description)));
            if (ProductFilter.Selects.Contains(ProductSelect.ScanCode))
                Fields.Add(new Nest.Field(nameof(Product.ScanCode)));
            if (ProductFilter.Selects.Contains(ProductSelect.ERPCode))
                Fields.Add(new Nest.Field(nameof(Product.ERPCode)));
            if (ProductFilter.Selects.Contains(ProductSelect.Category))
                Fields.Add(new Nest.Field(nameof(Product.Category)));
            if (ProductFilter.Selects.Contains(ProductSelect.ProductType))
                Fields.Add(new Nest.Field(nameof(Product.ProductType)));
            if (ProductFilter.Selects.Contains(ProductSelect.Brand))
                Fields.Add(new Nest.Field(nameof(Product.Brand)));
            if (ProductFilter.Selects.Contains(ProductSelect.UnitOfMeasure))
                Fields.Add(new Nest.Field(nameof(Product.UnitOfMeasure)));
            if (ProductFilter.Selects.Contains(ProductSelect.UnitOfMeasureGrouping))
                Fields.Add(new Nest.Field(nameof(Product.UnitOfMeasureGrouping)));
            if (ProductFilter.Selects.Contains(ProductSelect.SalePrice))
                Fields.Add(new Nest.Field(nameof(Product.SalePrice)));
            if (ProductFilter.Selects.Contains(ProductSelect.RetailPrice))
                Fields.Add(new Nest.Field(nameof(Product.RetailPrice)));
            if (ProductFilter.Selects.Contains(ProductSelect.TaxType))
                Fields.Add(new Nest.Field(nameof(Product.TaxType)));
            if (ProductFilter.Selects.Contains(ProductSelect.Status))
                Fields.Add(new Nest.Field(nameof(Product.Status)));
            if (ProductFilter.Selects.Contains(ProductSelect.OtherName))
                Fields.Add(new Nest.Field(nameof(Product.OtherName)));
            if (ProductFilter.Selects.Contains(ProductSelect.TechnicalName))
                Fields.Add(new Nest.Field(nameof(Product.TechnicalName)));
            if (ProductFilter.Selects.Contains(ProductSelect.Note))
                Fields.Add(new Nest.Field(nameof(Product.Note)));
            if (ProductFilter.Selects.Contains(ProductSelect.IsNew))
                Fields.Add(new Nest.Field(nameof(Product.IsNew)));
            if (ProductFilter.Selects.Contains(ProductSelect.UsedVariation))
                Fields.Add(new Nest.Field(nameof(Product.UsedVariation)));
            if (ProductFilter.Selects.Contains(ProductSelect.ProductProductGroupingMapping))
                Fields.Add(new Nest.Field(nameof(Product.ProductProductGroupingMappings)));
            if (ProductFilter.Selects.Contains(ProductSelect.VariationGrouping))
                Fields.Add(new Nest.Field(nameof(VariationGrouping)));

            SourceDescriptor.Includes(q => q.Fields(Fields));
            return SourceDescriptor;
        }

        public async Task<int> Count(ProductFilter ProductFilter)
        {
            BoolQueryDescriptor<Product> query = new BoolQueryDescriptor<Product>();
            query = DynamicQuery(query, ProductFilter);

            var ResponseAll = await client.CountAsync<Product>(s => s
                .Query(q => q.Bool(b => query))
                .Index(nameof(Product).ToEsIndex())
                );
            return (int)ResponseAll.Count;
        }

        public async Task<Product> Get(long? Id)
        {
            var Response = await client.GetAsync<Product>(Id, i => i.Index(nameof(Product).ToEsIndex()));
            Product product = Response.Source;
            return product;
        }


        public async Task<List<Product>> List(ProductFilter ProductFilter)
        {
            if (ProductFilter.Search != null)
            {
                ProductFilter.Search = ProductFilter.Search.ToLower();
            }
            BoolQueryDescriptor<Product> query = new BoolQueryDescriptor<Product>();
            query = DynamicQuery(query, ProductFilter);
            var ResponseAll = await client.SearchAsync<Product>(s => s
                .Query(q => q.Bool(b => query))
                .Index(nameof(Product).ToEsIndex())
                .From(ProductFilter.Skip)
                .Take(ProductFilter.Take)
                .Sort(ss => DynamicOrder(ProductFilter))
                .Source(src => DynamicSelect(ProductFilter))
                );
            List<Product> Products = (List<Product>)ResponseAll.Documents;
            return Products;
        }

        public async Task<List<Product>> List(List<long> Ids)
        {
            var Response = await client.MultiGetAsync(m => m
                .Index(nameof(Product).ToEsIndex())
                .GetMany<Product>(Ids, (g, id) => g.Index(null)));
            var res = Response.GetMany<Product>(Ids).Select(h => h.Source).ToList();
            return res;
        }

        public async Task<bool> BulkMerge(List<Product> Products)
        {
            var res = await client.BulkAsync(b => b
                .IndexMany(Products)
                .Index(nameof(Product).ToEsIndex())
                .Refresh(Elasticsearch.Net.Refresh.WaitFor)
                );
            Console.WriteLine(res);
            return true;
        }
    }
}
