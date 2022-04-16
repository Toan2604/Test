using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile.direct_sales_order
{
    public class MobileDirectSalesOrder_WorkflowStepDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long RoleId { get; set; }
        public MobileDirectSalesOrder_RoleDTO Role { get; set; }
        public MobileDirectSalesOrder_WorkflowStepDTO() { }
        public MobileDirectSalesOrder_WorkflowStepDTO(WorkflowStep WorkflowStep)
        {
            this.Id = WorkflowStep.Id;
            this.Code = WorkflowStep.Code;
            this.Name = WorkflowStep.Name;
            this.RoleId = WorkflowStep.RoleId;
            this.Role = WorkflowStep.Role == null ? null : new MobileDirectSalesOrder_RoleDTO(WorkflowStep.Role);
        }
    }
}
