using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile.indirect_sales_order
{
    public class MobileIndirectSalesOrder_WorkflowStepDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long RoleId { get; set; }
        public MobileIndirectSalesOrder_RoleDTO Role { get; set; }
        public MobileIndirectSalesOrder_WorkflowStepDTO() { }
        public MobileIndirectSalesOrder_WorkflowStepDTO(WorkflowStep WorkflowStep)
        {
            this.Id = WorkflowStep.Id;
            this.Code = WorkflowStep.Code;
            this.Name = WorkflowStep.Name;
            this.RoleId = WorkflowStep.RoleId;
            this.Role = WorkflowStep.Role == null ? null : new MobileIndirectSalesOrder_RoleDTO(WorkflowStep.Role);
        }
    }
}
