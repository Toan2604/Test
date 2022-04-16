using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_sales_order.report_indirect_sales_order_general
{
    public class ReportSalesOrderGeneral_ProvinceDTO : DataDTO
    {

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public long? Priority { get; set; }

        public long StatusId { get; set; }


        public ReportSalesOrderGeneral_ProvinceDTO() { }
        public ReportSalesOrderGeneral_ProvinceDTO(Province Province)
        {

            this.Id = Province.Id;

            this.Code = Province.Code;

            this.Name = Province.Name;

            this.Priority = Province.Priority;

            this.StatusId = Province.StatusId;

        }
    }

    public class ReportSalesOrderGeneral_ProvinceFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Name { get; set; }

        public LongFilter Priority { get; set; }

        public IdFilter StatusId { get; set; }

        public ProvinceOrder OrderBy { get; set; }
    }
}
