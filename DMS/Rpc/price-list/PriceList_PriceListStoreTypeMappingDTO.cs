using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.price_list
{
    public class PriceList_PriceListStoreTypeMappingDTO : DataDTO
    {
        public long PriceListId { get; set; }
        public long StoreTypeId { get; set; }
        public PriceList_StoreTypeDTO StoreType { get; set; }

        public PriceList_PriceListStoreTypeMappingDTO() { }
        public PriceList_PriceListStoreTypeMappingDTO(PriceListStoreTypeMapping PriceListStoreTypeMapping)
        {
            this.PriceListId = PriceListStoreTypeMapping.PriceListId;
            this.StoreTypeId = PriceListStoreTypeMapping.StoreTypeId;
            this.StoreType = PriceListStoreTypeMapping.StoreType == null ? null : new PriceList_StoreTypeDTO(PriceListStoreTypeMapping.StoreType);
            this.Errors = PriceListStoreTypeMapping.Errors;
        }
    }

    public class PriceList_PriceListStoreTypeMappingFilterDTO : FilterDTO
    {

        public IdFilter PriceListId { get; set; }

        public IdFilter StoreTypeId { get; set; }

        public PriceListStoreTypeMappingOrder OrderBy { get; set; }
    }
}