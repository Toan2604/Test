using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_sales_order.report_indirect_sales_order_general
{
    public class ReportSalesOrderGeneral_StoreStatusDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }


        public ReportSalesOrderGeneral_StoreStatusDTO() { }
        public ReportSalesOrderGeneral_StoreStatusDTO(StoreStatus StoreStatus)
        {

            this.Id = StoreStatus.Id;

            this.Code = StoreStatus.Code;

            this.Name = StoreStatus.Name;

        }
    }

    public class ReportSalesOrderGeneral_StoreStatusFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public StatusOrder OrderBy { get; set; }
    }
}