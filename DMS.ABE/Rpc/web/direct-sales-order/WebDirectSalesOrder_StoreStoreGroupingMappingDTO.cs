using DMS.ABE.Entities;
using TrueSight.Common;
namespace DMS.ABE.Rpc.web.direct_sales_order
{
    public class WebDirectSalesOrder_StoreStoreGroupingMappingDTO : DataDTO
    {
        public long StoreId { get; set; }
        public long StoreGroupingId { get; set; }
        public WebDirectSalesOrder_StoreGroupingDTO StoreGrouping { get; set; }
        public WebDirectSalesOrder_StoreDTO Store { get; set; }
        public WebDirectSalesOrder_StoreStoreGroupingMappingDTO() { }
        public WebDirectSalesOrder_StoreStoreGroupingMappingDTO(StoreStoreGroupingMapping StoreStoreGroupingMapping)
        {
            this.StoreId = StoreStoreGroupingMapping.StoreId;
            this.StoreGroupingId = StoreStoreGroupingMapping.StoreGroupingId;
            this.StoreGrouping = StoreStoreGroupingMapping.StoreGrouping == null ? null : new WebDirectSalesOrder_StoreGroupingDTO(StoreStoreGroupingMapping.StoreGrouping);
            this.Store = StoreStoreGroupingMapping.Store == null ? null : new WebDirectSalesOrder_StoreDTO(StoreStoreGroupingMapping.Store);
        }
    }
    public class WebDirectSalesOrder_StoreStoreGroupingMappingFilterDTO : FilterDTO
    {
        public IdFilter StoreId { get; set; }
        public IdFilter StoreGroupingId { get; set; }
        public StoreStoreGroupingMappingOrder OrderBy { get; set; }
    }
}