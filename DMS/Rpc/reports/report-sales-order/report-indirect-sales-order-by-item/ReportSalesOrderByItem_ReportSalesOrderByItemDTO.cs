using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_sales_order.report_indirect_sales_order_by_item
{
    public class ReportSalesOrderByItem_ReportSalesOrderByItemDTO : DataDTO
    {
        public long OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public List<ReportSalesOrderByItem_ItemDetailDTO> ItemDetails { get; set; }
    }

    public class ReportSalesOrderByItem_ItemDetailDTO : DataDTO
    {
        public long STT { get; set; }
        public long ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string UnitOfMeasureName { get; set; }
        public decimal SaleStock { get; set; }
        public decimal PromotionStock { get; set; }
        public decimal Discount { get; set; }
        public decimal Revenue { get; set; }
        public int SalesOrderCounter => IndirectSalesOrderIds.Count();
        public int BuyerStoreCounter => BuyerStoreIds.Count();
        internal HashSet<long> IndirectSalesOrderIds { get; set; }
        internal HashSet<long> BuyerStoreIds { get; set; }
    }

    public class ReportSalesOrderByItem_ReportSalesOrderByItemFilterDTO : FilterDTO
    {
        public IdFilter OrganizationId { get; set; }
        public IdFilter ItemId { get; set; }
        public IdFilter ProductGroupingId { get; set; }
        public IdFilter ProductTypeId { get; set; }
        public DateFilter Date { get; set; }
        public IdFilter ProvinceId { get; set; }
        public IdFilter DistrictId { get; set; }
        internal bool HasValue => (ItemId != null && ItemId.HasValue) ||
            (ProductGroupingId != null && ProductGroupingId.HasValue) ||
            (ProductTypeId != null && ProductTypeId.HasValue) ||
            (Date != null && Date.HasValue) ||
            (ProvinceId != null && ProvinceId.HasValue) ||
            (DistrictId != null && DistrictId.HasValue);
    }
}
