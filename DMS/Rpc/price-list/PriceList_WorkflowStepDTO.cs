using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.price_list
{
    public class PriceList_WorkflowStepDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long RoleId { get; set; }
        public PriceList_RoleDTO Role { get; set; }
        public PriceList_WorkflowStepDTO() { }
        public PriceList_WorkflowStepDTO(WorkflowStep WorkflowStep)
        {
            this.Id = WorkflowStep.Id;
            this.Code = WorkflowStep.Code;
            this.Name = WorkflowStep.Name;
            this.RoleId = WorkflowStep.RoleId;
            this.Role = WorkflowStep.Role == null ? null : new PriceList_RoleDTO(WorkflowStep.Role);
        }
    }
}
