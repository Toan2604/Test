using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile.indirect_sales_order
{
    public class MobileIndirectSalesOrder_StoreStoreGroupingMappingDTO : DataDTO
    {
        public long StoreId { get; set; }
        public long StoreGroupingId { get; set; }
        public MobileIndirectSalesOrder_StoreGroupingDTO StoreGrouping { get; set; }
        public MobileIndirectSalesOrder_StoreDTO Store { get; set; }

        public MobileIndirectSalesOrder_StoreStoreGroupingMappingDTO() { }
        public MobileIndirectSalesOrder_StoreStoreGroupingMappingDTO(StoreStoreGroupingMapping StoreStoreGroupingMapping)
        {
            this.StoreId = StoreStoreGroupingMapping.StoreId;
            this.StoreGroupingId = StoreStoreGroupingMapping.StoreGroupingId;
            this.StoreGrouping = StoreStoreGroupingMapping.StoreGrouping == null ? null : new MobileIndirectSalesOrder_StoreGroupingDTO(StoreStoreGroupingMapping.StoreGrouping);
            this.Store = StoreStoreGroupingMapping.Store == null ? null : new MobileIndirectSalesOrder_StoreDTO(StoreStoreGroupingMapping.Store);
        }
    }

    public class MobileIndirectSalesOrder_StoreStoreGroupingMappingFilterDTO : FilterDTO
    {
        public IdFilter StoreId { get; set; }

        public IdFilter StoreGroupingId { get; set; }

        public StoreStoreGroupingMappingOrder OrderBy { get; set; }
    }
}