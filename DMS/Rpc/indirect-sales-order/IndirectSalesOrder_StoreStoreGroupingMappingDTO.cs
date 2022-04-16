using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.indirect_sales_order
{
    public class IndirectSalesOrder_StoreStoreGroupingMappingDTO : DataDTO
    {
        public long StoreId { get; set; }
        public long StoreGroupingId { get; set; }
        public IndirectSalesOrder_StoreGroupingDTO StoreGrouping { get; set; }
        public IndirectSalesOrder_StoreDTO Store { get; set; }

        public IndirectSalesOrder_StoreStoreGroupingMappingDTO() { }
        public IndirectSalesOrder_StoreStoreGroupingMappingDTO(StoreStoreGroupingMapping StoreStoreGroupingMapping)
        {
            this.StoreId = StoreStoreGroupingMapping.StoreId;
            this.StoreGroupingId = StoreStoreGroupingMapping.StoreGroupingId;
            this.StoreGrouping = StoreStoreGroupingMapping.StoreGrouping == null ? null : new IndirectSalesOrder_StoreGroupingDTO(StoreStoreGroupingMapping.StoreGrouping);
            this.Store = StoreStoreGroupingMapping.Store == null ? null : new IndirectSalesOrder_StoreDTO(StoreStoreGroupingMapping.Store);
        }
    }

    public class IndirectSalesOrder_StoreStoreGroupingMappingFilterDTO : FilterDTO
    {
        public IdFilter StoreId { get; set; }

        public IdFilter StoreGroupingId { get; set; }

        public StoreStoreGroupingMappingOrder OrderBy { get; set; }
    }
}