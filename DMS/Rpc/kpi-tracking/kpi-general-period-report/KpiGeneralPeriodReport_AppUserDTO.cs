using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.kpi_tracking.kpi_general_period_report
{
    public class KpiGeneralPeriodReport_AppUserDTO : DataDTO
    {
        public long Id { get; set; }
        public long? OrganizationId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }

        public KpiGeneralPeriodReport_AppUserDTO() { }
        public KpiGeneralPeriodReport_AppUserDTO(AppUser AppUser)
        {
            this.Id = AppUser.Id;
            this.OrganizationId = AppUser.OrganizationId;
            this.Username = AppUser.Username;
            this.DisplayName = AppUser.DisplayName;
            this.Errors = AppUser.Errors;
        }
    }

    public class KpiGeneralPeriodReport_AppUserFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public IdFilter OrganizationId { get; set; }
        public StringFilter Username { get; set; }
        public StringFilter DisplayName { get; set; }
        public AppUserOrder OrderBy { get; set; }
    }
}
