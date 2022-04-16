using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_sales_order.report_indirect_sales_order_by_employee_and_item
{
    public class ReportSalesOrderByEmployeeAndItem_ProductGroupingDTO : DataDTO
    {
        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public long? ParentId { get; set; }

        public string Path { get; set; }

        public string Description { get; set; }


        public ReportSalesOrderByEmployeeAndItem_ProductGroupingDTO() { }
        public ReportSalesOrderByEmployeeAndItem_ProductGroupingDTO(ProductGrouping ProductGrouping)
        {

            this.Id = ProductGrouping.Id;

            this.Code = ProductGrouping.Code;

            this.Name = ProductGrouping.Name;

            this.ParentId = ProductGrouping.ParentId;

            this.Path = ProductGrouping.Path;

            this.Description = ProductGrouping.Description;

        }
    }

    public class ReportSalesOrderByEmployeeAndItem_ProductGroupingFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public IdFilter ParentId { get; set; }

        public StringFilter Path { get; set; }

        public StringFilter Description { get; set; }

        public ProductGroupingOrder OrderBy { get; set; }
    }
}
