using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.dashboards.director
{
    public class DashboardDirector_GeneralApprovalStateDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DashboardDirector_GeneralApprovalStateDTO() { }
        public DashboardDirector_GeneralApprovalStateDTO(GeneralApprovalState GeneralApprovalState)
        {
            this.Id = GeneralApprovalState.Id;
            this.Code = GeneralApprovalState.Code;
            this.Name = GeneralApprovalState.Name;
            this.Errors = GeneralApprovalState.Errors;
        }
    }

    public class DashboardDirector_GeneralApprovalStatefilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public RequestStateOrder OrderBy { get; set; }
    }
}
