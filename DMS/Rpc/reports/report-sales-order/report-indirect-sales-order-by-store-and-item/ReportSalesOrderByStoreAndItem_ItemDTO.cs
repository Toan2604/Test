using DMS.Entities;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_sales_order.report_indirect_sales_order_by_store_and_item
{
    public class ReportSalesOrderByStoreAndItem_ItemDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string UnitOfMeasureName { get; set; }
        public decimal SaleStock { get; set; }
        public decimal PromotionStock { get; set; }
        public decimal SalePriceAverage { get; set; }
        public decimal Discount { get; set; }
        public decimal Revenue { get; set; }
        public long SalesOrderCounter => IndirectSalesOrderIds?.Count() ?? 0;
        internal HashSet<long> IndirectSalesOrderIds { get; set; }
        public ReportSalesOrderByStoreAndItem_ItemDTO() { }
        public ReportSalesOrderByStoreAndItem_ItemDTO(Item Item)
        {
            this.Id = Item.Id;
            this.Code = Item.Code;
            this.Name = Item.Name;
        }
    }

    public class ReportSalesOrderByStoreAndItem_ItemFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public IdFilter ProductTypeId { get; set; }
        public IdFilter ProductGroupingId { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter StatusId { get; set; }
        public string Search { get; set; }
        public ItemOrder OrderBy { get; set; }
    }
}
