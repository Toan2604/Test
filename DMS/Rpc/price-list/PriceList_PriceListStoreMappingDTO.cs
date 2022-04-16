using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.price_list
{
    public class PriceList_PriceListStoreMappingDTO : DataDTO
    {
        public long PriceListId { get; set; }
        public long StoreId { get; set; }
        public PriceList_StoreDTO Store { get; set; }

        public PriceList_PriceListStoreMappingDTO() { }
        public PriceList_PriceListStoreMappingDTO(PriceListStoreMapping PriceListStoreMapping)
        {
            this.PriceListId = PriceListStoreMapping.PriceListId;
            this.StoreId = PriceListStoreMapping.StoreId;
            this.Store = PriceListStoreMapping.Store == null ? null : new PriceList_StoreDTO(PriceListStoreMapping.Store);
            this.Errors = PriceListStoreMapping.Errors;
        }
    }

    public class PriceList_PriceListStoreMappingFilterDTO : FilterDTO
    {

        public IdFilter PriceListId { get; set; }

        public IdFilter StoreId { get; set; }

        public PriceListStoreMappingOrder OrderBy { get; set; }
    }
}