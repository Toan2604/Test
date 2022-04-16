using DMS.Entities;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Repositories
{
    public interface IEsStoreRepository
    {
        Task<bool> BulkMerge(List<Store> Stores);
        Task<long> Count(StoreFilter StoreFilter);
        Task<List<Store>> List(StoreFilter StoreFilter);
        Task<List<Store>> List(List<long> Ids);
        Task<Store> Get(long Id);
    }
    public class EsStoreRepository : IEsStoreRepository
    {
        private IElasticClient client;
        public EsStoreRepository(IElasticClient _client)
        {
            this.client = _client;
        }

        private BoolQueryDescriptor<Store> DynamicQuery(BoolQueryDescriptor<Store> query, StoreFilter filter)
        {
            if (filter == null)
                return query;
            query = query.EsWhere(x => x.Id, filter.Id);
            query = query.EsWhere(x => x.Code, filter.Code);
            query = query.EsWhere(x => x.CodeDraft, filter.CodeDraft);
            query = query.EsWhere(x => x.Name, filter.Name);
            query = query.EsWhere(x => x.UnsignName, filter.UnsignName);
            query = query.EsWhere(x => x.ParentStoreId, filter.ParentStoreId);
            query = query.EsWhere(x => x.OrganizationId, filter.OrganizationId);
            query = query.EsWhere(x => x.StoreTypeId, filter.StoreTypeId);
            query = query.EsWhere(x => x.ProvinceId, filter.ProvinceId);
            query = query.EsWhere(x => x.DistrictId, filter.DistrictId);
            query = query.EsWhere(x => x.WardId, filter.WardId);
            query = query.EsWhere(x => x.Address, filter.Address);
            query = query.EsWhere(x => x.UnsignAddress, filter.UnsignAddress);
            query = query.EsWhere(x => x.DeliveryAddress, filter.DeliveryAddress);
            query = query.EsWhere(x => x.Latitude, filter.Latitude);
            query = query.EsWhere(x => x.Longitude, filter.Longitude);
            query = query.EsWhere(x => x.DeliveryLatitude, filter.DeliveryLatitude);
            query = query.EsWhere(x => x.DeliveryLongitude, filter.DeliveryLongitude);
            query = query.EsWhere(x => x.OwnerName, filter.OwnerName);
            query = query.EsWhere(x => x.OwnerPhone, filter.OwnerPhone);
            query = query.EsWhere(x => x.OwnerEmail, filter.OwnerEmail);
            query = query.EsWhere(x => x.AppUserId, filter.AppUserId);
            query = query.EsWhere(x => x.StatusId, filter.StatusId);
            query = query.EsWhere(x => x.StoreStatusId, filter.StoreStatusId);
            if (filter.Search != null)
            {
                List<Nest.Field> Fields = new List<Nest.Field>();
                Fields.Add(new Nest.Field(nameof(Store.Name)));

                query.Filter(q => q.MultiMatch(m => m
                    .Fields(f => f.Fields(Fields))
                    .Query(filter.Search)
                    .Fuzziness(Fuzziness.Auto)
                ));
            }

            return query;
        }

        private SortDescriptor<Store> DynamicOrder(StoreFilter StoreFilter)
        {
            SortDescriptor<Store> SortDescriptor = new SortDescriptor<Store>();
            switch (StoreFilter.OrderType)
            {
                case OrderType.ASC:
                    switch (StoreFilter.OrderBy)
                    {
                        case StoreOrder.Id:
                            SortDescriptor = SortDescriptor.Ascending(q => q.Id);
                            break;
                        case StoreOrder.Code:
                            SortDescriptor = SortDescriptor.Ascending(q => q.Code);
                            break;
                        case StoreOrder.CodeDraft:
                            SortDescriptor = SortDescriptor.Ascending(q => q.CodeDraft);
                            break;
                        case StoreOrder.Name:
                            SortDescriptor = SortDescriptor.Ascending(q => q.Name);
                            break;
                        case StoreOrder.ParentStore:
                            SortDescriptor = SortDescriptor.Ascending(q => q.ParentStoreId);
                            break;
                        case StoreOrder.Organization:
                            SortDescriptor = SortDescriptor.Ascending(q => q.OrganizationId);
                            break;
                        case StoreOrder.StoreType:
                            SortDescriptor = SortDescriptor.Ascending(q => q.StoreTypeId);
                            break;
                        case StoreOrder.Telephone:
                            SortDescriptor = SortDescriptor.Ascending(q => q.Telephone);
                            break;
                        case StoreOrder.Province:
                            SortDescriptor = SortDescriptor.Ascending(q => q.ProvinceId);
                            break;
                        case StoreOrder.District:
                            SortDescriptor = SortDescriptor.Ascending(q => q.DistrictId);
                            break;
                        case StoreOrder.Ward:
                            SortDescriptor = SortDescriptor.Ascending(q => q.WardId);
                            break;
                        case StoreOrder.Address:
                            SortDescriptor = SortDescriptor.Ascending(q => q.Address);
                            break;
                        case StoreOrder.DeliveryAddress:
                            SortDescriptor = SortDescriptor.Ascending(q => q.DeliveryAddress);
                            break;
                        case StoreOrder.Latitude:
                            SortDescriptor = SortDescriptor.Ascending(q => q.Latitude);
                            break;
                        case StoreOrder.Longitude:
                            SortDescriptor = SortDescriptor.Ascending(q => q.Longitude);
                            break;
                        case StoreOrder.DeliveryLatitude:
                            SortDescriptor = SortDescriptor.Ascending(q => q.DeliveryLatitude);
                            break;
                        case StoreOrder.DeliveryLongitude:
                            SortDescriptor = SortDescriptor.Ascending(q => q.DeliveryLongitude);
                            break;
                        case StoreOrder.OwnerName:
                            SortDescriptor = SortDescriptor.Ascending(q => q.OwnerName);
                            break;
                        case StoreOrder.OwnerPhone:
                            SortDescriptor = SortDescriptor.Ascending(q => q.OwnerPhone);
                            break;
                        case StoreOrder.OwnerEmail:
                            SortDescriptor = SortDescriptor.Ascending(q => q.OwnerEmail);
                            break;
                        case StoreOrder.Status:
                            SortDescriptor = SortDescriptor.Ascending(q => q.StatusId);
                            break;
                        case StoreOrder.AppUser:
                            SortDescriptor = SortDescriptor.Ascending(q => q.AppUserId);
                            break;
                        case StoreOrder.StoreStatus:
                            SortDescriptor = SortDescriptor.Ascending(q => q.StoreStatusId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (StoreFilter.OrderBy)
                    {
                        case StoreOrder.Id:
                            SortDescriptor = SortDescriptor.Descending(q => q.Id);
                            break;
                        case StoreOrder.Code:
                            SortDescriptor = SortDescriptor.Descending(q => q.Code);
                            break;
                        case StoreOrder.CodeDraft:
                            SortDescriptor = SortDescriptor.Descending(q => q.CodeDraft);
                            break;
                        case StoreOrder.Name:
                            SortDescriptor = SortDescriptor.Descending(q => q.Name);
                            break;
                        case StoreOrder.ParentStore:
                            SortDescriptor = SortDescriptor.Descending(q => q.ParentStoreId);
                            break;
                        case StoreOrder.Organization:
                            SortDescriptor = SortDescriptor.Descending(q => q.OrganizationId);
                            break;
                        case StoreOrder.StoreType:
                            SortDescriptor = SortDescriptor.Descending(q => q.StoreTypeId);
                            break;
                        case StoreOrder.Telephone:
                            SortDescriptor = SortDescriptor.Descending(q => q.Telephone);
                            break;
                        case StoreOrder.Province:
                            SortDescriptor = SortDescriptor.Descending(q => q.ProvinceId);
                            break;
                        case StoreOrder.District:
                            SortDescriptor = SortDescriptor.Descending(q => q.DistrictId);
                            break;
                        case StoreOrder.Ward:
                            SortDescriptor = SortDescriptor.Descending(q => q.WardId);
                            break;
                        case StoreOrder.Address:
                            SortDescriptor = SortDescriptor.Descending(q => q.Address);
                            break;
                        case StoreOrder.DeliveryAddress:
                            SortDescriptor = SortDescriptor.Descending(q => q.DeliveryAddress);
                            break;
                        case StoreOrder.Latitude:
                            SortDescriptor = SortDescriptor.Descending(q => q.Latitude);
                            break;
                        case StoreOrder.Longitude:
                            SortDescriptor = SortDescriptor.Descending(q => q.Longitude);
                            break;
                        case StoreOrder.DeliveryLatitude:
                            SortDescriptor = SortDescriptor.Descending(q => q.DeliveryLatitude);
                            break;
                        case StoreOrder.DeliveryLongitude:
                            SortDescriptor = SortDescriptor.Descending(q => q.DeliveryLongitude);
                            break;
                        case StoreOrder.OwnerName:
                            SortDescriptor = SortDescriptor.Descending(q => q.OwnerName);
                            break;
                        case StoreOrder.OwnerPhone:
                            SortDescriptor = SortDescriptor.Descending(q => q.OwnerPhone);
                            break;
                        case StoreOrder.OwnerEmail:
                            SortDescriptor = SortDescriptor.Descending(q => q.OwnerEmail);
                            break;
                        case StoreOrder.Status:
                            SortDescriptor = SortDescriptor.Descending(q => q.StatusId);
                            break;
                        case StoreOrder.AppUser:
                            SortDescriptor = SortDescriptor.Descending(q => q.AppUserId);
                            break;
                        case StoreOrder.StoreStatus:
                            SortDescriptor = SortDescriptor.Descending(q => q.StoreStatusId);
                            break;
                    }
                    break;
            }
            return SortDescriptor;
        }

        private SourceFilterDescriptor<Store> DynamicSelect(StoreFilter StoreFilter)
        {
            SourceFilterDescriptor<Store> SourceDescriptor = new SourceFilterDescriptor<Store>();
            List<Nest.Field> Fields = new List<Nest.Field>();

            if (StoreFilter.Selects.Contains(StoreSelect.Id))
                Fields.Add(new Nest.Field(nameof(Store.Id)));
            if (StoreFilter.Selects.Contains(StoreSelect.Code))
                Fields.Add(new Nest.Field(nameof(Store.Code)));
            if (StoreFilter.Selects.Contains(StoreSelect.CodeDraft))
                Fields.Add(new Nest.Field(nameof(Store.CodeDraft)));
            if (StoreFilter.Selects.Contains(StoreSelect.Name))
                Fields.Add(new Nest.Field(nameof(Store.Name)));
            if (StoreFilter.Selects.Contains(StoreSelect.UnsignName))
                Fields.Add(new Nest.Field(nameof(Store.UnsignName)));
            if (StoreFilter.Selects.Contains(StoreSelect.ParentStore))
                Fields.Add(new Nest.Field(nameof(Store.ParentStore)));
            if (StoreFilter.Selects.Contains(StoreSelect.Organization))
                Fields.Add(new Nest.Field(nameof(Store.Organization)));
            if (StoreFilter.Selects.Contains(StoreSelect.StoreType))
                Fields.Add(new Nest.Field(nameof(Store.StoreType)));
            if (StoreFilter.Selects.Contains(StoreSelect.Telephone))
                Fields.Add(new Nest.Field(nameof(Store.Telephone)));
            if (StoreFilter.Selects.Contains(StoreSelect.Province))
                Fields.Add(new Nest.Field(nameof(Store.Province)));
            if (StoreFilter.Selects.Contains(StoreSelect.District))
                Fields.Add(new Nest.Field(nameof(Store.District)));
            if (StoreFilter.Selects.Contains(StoreSelect.Ward))
                Fields.Add(new Nest.Field(nameof(Store.Ward)));
            if (StoreFilter.Selects.Contains(StoreSelect.Address))
                Fields.Add(new Nest.Field(nameof(Store.Address)));
            if (StoreFilter.Selects.Contains(StoreSelect.UnsignAddress))
                Fields.Add(new Nest.Field(nameof(Store.UnsignAddress)));
            if (StoreFilter.Selects.Contains(StoreSelect.DeliveryAddress))
                Fields.Add(new Nest.Field(nameof(Store.DeliveryAddress)));
            if (StoreFilter.Selects.Contains(StoreSelect.Latitude))
                Fields.Add(new Nest.Field(nameof(Store.Latitude)));
            if (StoreFilter.Selects.Contains(StoreSelect.Longitude))
                Fields.Add(new Nest.Field(nameof(Store.Longitude)));
            if (StoreFilter.Selects.Contains(StoreSelect.DeliveryLatitude))
                Fields.Add(new Nest.Field(nameof(Store.DeliveryLatitude)));
            if (StoreFilter.Selects.Contains(StoreSelect.DeliveryLongitude))
                Fields.Add(new Nest.Field(nameof(Store.DeliveryLongitude)));
            if (StoreFilter.Selects.Contains(StoreSelect.OwnerName))
                Fields.Add(new Nest.Field(nameof(Store.OwnerName)));
            if (StoreFilter.Selects.Contains(StoreSelect.OwnerPhone))
                Fields.Add(new Nest.Field(nameof(Store.OwnerPhone)));
            if (StoreFilter.Selects.Contains(StoreSelect.OwnerEmail))
                Fields.Add(new Nest.Field(nameof(Store.OwnerEmail)));
            if (StoreFilter.Selects.Contains(StoreSelect.TaxCode))
                Fields.Add(new Nest.Field(nameof(Store.TaxCode)));
            if (StoreFilter.Selects.Contains(StoreSelect.LegalEntity))
                Fields.Add(new Nest.Field(nameof(Store.LegalEntity)));
            if (StoreFilter.Selects.Contains(StoreSelect.AppUser))
                Fields.Add(new Nest.Field(nameof(Store.AppUser)));
            if (StoreFilter.Selects.Contains(StoreSelect.Status))
                Fields.Add(new Nest.Field(nameof(Store.Status)));
            if (StoreFilter.Selects.Contains(StoreSelect.StoreStatus))
                Fields.Add(new Nest.Field(nameof(StoreStatus)));
            if (StoreFilter.Selects.Contains(StoreSelect.StoreGrouping))
                Fields.Add(new Nest.Field(nameof(StoreGrouping)));

            SourceDescriptor.Includes(q => q.Fields(Fields));
            return SourceDescriptor;
        }
        public async Task<long> Count(StoreFilter StoreFilter)
        {
            BoolQueryDescriptor<Store> query = new BoolQueryDescriptor<Store>();
            query = DynamicQuery(query, StoreFilter);
            var ResponseAll = await client.CountAsync<Store>(s => s
                .Query(q => q.Bool(b => query))
                .Index(nameof(Store).ToEsIndex())
                );
            return ResponseAll.Count;
        }

        public async Task<Store> Get(long Id)
        {
            var Response = await client.GetAsync<Store>(Id, i => i.Index(nameof(Store).ToEsIndex()));
            Store store = Response.Source;
            return store;
        }

        public async Task<List<Store>> List(StoreFilter StoreFilter)
        {
            BoolQueryDescriptor<Store> query = new BoolQueryDescriptor<Store>();
            query = DynamicQuery(query, StoreFilter);
            var ResponseAll = await client.SearchAsync<Store>(s => s
                .Query(q => q.Bool(b => query))
                .Index(nameof(Store).ToEsIndex())
                .From(StoreFilter.Skip)
                .Take(StoreFilter.Take)
                .Sort(ss => DynamicOrder(StoreFilter))
                .Source(src => DynamicSelect(StoreFilter))
                );
            List<Store> Stores = (List<Store>)ResponseAll.Documents;
            return Stores;
        }
        public async Task<List<Store>> List(List<long> Ids)
        {
            var Response = await client.MultiGetAsync(m => m
                .Index(nameof(Store).ToEsIndex())
                .GetMany<Store>(Ids, (g, id) => g.Index(null)));
            var res = Response.GetMany<Store>(Ids).Select(h => h.Source).ToList();
            return res;
        }

        public async Task<bool> BulkMerge(List<Store> Stores)
        {
            await client.BulkAsync(b => b
                .IndexMany(Stores)
                .Index(nameof(Store).ToEsIndex())
                .Refresh(Elasticsearch.Net.Refresh.WaitFor)
                );
            return true;
        }
    }

}
