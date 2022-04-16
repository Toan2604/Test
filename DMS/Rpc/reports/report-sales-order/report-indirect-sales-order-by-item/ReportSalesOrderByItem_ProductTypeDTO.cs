using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_sales_order.report_indirect_sales_order_by_item
{
    public class ReportSalesOrderByItem_ProductTypeDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public ReportSalesOrderByItem_ProductTypeDTO() { }
        public ReportSalesOrderByItem_ProductTypeDTO(ProductType ProductType)
        {
            this.Id = ProductType.Id;
            this.Code = ProductType.Code;
            this.Name = ProductType.Name;
            this.Errors = ProductType.Errors;
        }
    }

    public class ReportSalesOrderByItem_ProductTypeFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter StatusId { get; set; }
        public ProductTypeOrder OrderBy { get; set; }
    }
}
