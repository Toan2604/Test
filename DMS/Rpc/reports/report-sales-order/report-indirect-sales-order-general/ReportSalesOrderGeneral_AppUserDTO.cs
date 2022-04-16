using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_sales_order.report_indirect_sales_order_general
{
    public class ReportSalesOrderGeneral_AppUserDTO : DataDTO
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }

        public ReportSalesOrderGeneral_AppUserDTO() { }
        public ReportSalesOrderGeneral_AppUserDTO(AppUser AppUser)
        {
            this.Id = AppUser.Id;
            this.Username = AppUser.Username;
            this.DisplayName = AppUser.DisplayName;
            this.Errors = AppUser.Errors;
        }
    }

    public class ReportSalesOrderGeneral_AppUserFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public IdFilter OrganizationId { get; set; }
        public StringFilter Username { get; set; }
        public StringFilter DisplayName { get; set; }
        public AppUserOrder OrderBy { get; set; }
    }
}
