using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.price_list
{
    public class PriceList_PriceListStoreGroupingMappingDTO : DataDTO
    {
        public long PriceListId { get; set; }
        public long StoreGroupingId { get; set; }
        public PriceList_StoreGroupingDTO StoreGrouping { get; set; }

        public PriceList_PriceListStoreGroupingMappingDTO() { }
        public PriceList_PriceListStoreGroupingMappingDTO(PriceListStoreGroupingMapping PriceListStoreGroupingMapping)
        {
            this.PriceListId = PriceListStoreGroupingMapping.PriceListId;
            this.StoreGroupingId = PriceListStoreGroupingMapping.StoreGroupingId;
            this.StoreGrouping = PriceListStoreGroupingMapping.StoreGrouping == null ? null : new PriceList_StoreGroupingDTO(PriceListStoreGroupingMapping.StoreGrouping);
            this.Errors = PriceListStoreGroupingMapping.Errors;
        }
    }

    public class PriceList_PriceListStoreGroupingMappingFilterDTO : FilterDTO
    {

        public IdFilter PriceListId { get; set; }

        public IdFilter StoreGroupingId { get; set; }

        public PriceListStoreGroupingMappingOrder OrderBy { get; set; }
    }
}