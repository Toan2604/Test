using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_sales_order.report_direct_sales_order_general
{
    public class ReportDirectSalesOrderGeneral_StoreDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string CodeDraft { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public long OrganizationId { get; set; }
        public ReportDirectSalesOrderGeneral_StoreDTO() { }
        public ReportDirectSalesOrderGeneral_StoreDTO(Store Store)
        {
            this.Id = Store.Id;
            this.Code = Store.Code;
            this.CodeDraft = Store.CodeDraft;
            this.Name = Store.Name;
            this.Address = Store.Address;
            this.OrganizationId = Store.OrganizationId;
        }
    }

    public class ReportDirectSalesOrderGeneral_StoreFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }
        public StringFilter CodeDraft { get; set; }

        public StringFilter Name { get; set; }

        public IdFilter OrganizationId { get; set; }

        public IdFilter StatusId { get; set; }
    }
}
