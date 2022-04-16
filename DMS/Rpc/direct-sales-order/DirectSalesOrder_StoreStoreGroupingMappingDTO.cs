using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.direct_sales_order
{
    public class DirectSalesOrder_StoreStoreGroupingMappingDTO : DataDTO
    {
        public long StoreId { get; set; }
        public long StoreGroupingId { get; set; }
        public DirectSalesOrder_StoreGroupingDTO StoreGrouping { get; set; }
        public DirectSalesOrder_StoreDTO Store { get; set; }

        public DirectSalesOrder_StoreStoreGroupingMappingDTO() { }
        public DirectSalesOrder_StoreStoreGroupingMappingDTO(StoreStoreGroupingMapping StoreStoreGroupingMapping)
        {
            this.StoreId = StoreStoreGroupingMapping.StoreId;
            this.StoreGroupingId = StoreStoreGroupingMapping.StoreGroupingId;
            this.StoreGrouping = StoreStoreGroupingMapping.StoreGrouping == null ? null : new DirectSalesOrder_StoreGroupingDTO(StoreStoreGroupingMapping.StoreGrouping);
            this.Store = StoreStoreGroupingMapping.Store == null ? null : new DirectSalesOrder_StoreDTO(StoreStoreGroupingMapping.Store);
        }
    }

    public class DirectSalesOrder_StoreStoreGroupingMappingFilterDTO : FilterDTO
    {
        public IdFilter StoreId { get; set; }

        public IdFilter StoreGroupingId { get; set; }

        public StoreStoreGroupingMappingOrder OrderBy { get; set; }
    }
}