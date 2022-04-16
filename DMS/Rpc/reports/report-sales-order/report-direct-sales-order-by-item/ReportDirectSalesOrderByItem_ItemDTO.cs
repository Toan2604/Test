using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_sales_order.report_direct_sales_order_by_item
{
    public class ReportDirectSalesOrderByItem_ItemDTO : DataDTO
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ScanCode { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal? RetailPrice { get; set; }
        public decimal SaleStock { get; set; }
        public long StatusId { get; set; }
        public ReportDirectSalesOrderByItem_ItemDTO() { }
        public ReportDirectSalesOrderByItem_ItemDTO(Item Item)
        {
            this.Id = Item.Id;
            this.ProductId = Item.ProductId;
            this.Code = Item.Code;
            this.Name = Item.Name;
            this.ScanCode = Item.ScanCode;
            this.SalePrice = Item.SalePrice;
            this.RetailPrice = Item.RetailPrice;
            this.SaleStock = Item.SaleStock;
            this.StatusId = Item.StatusId;
            this.Errors = Item.Errors;
        }
    }

    public class ReportDirectSalesOrderByItem_ItemFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public IdFilter ProductTypeId { get; set; }
        public IdFilter ProductGroupingId { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public string Search { get; set; }
        public IdFilter StatusId { get; set; }
        public ProductOrder OrderBy { get; set; }
    }
}
