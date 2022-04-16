using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.direct_sales_order
{
    public class DirectSalesOrder_WorkflowStateDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DirectSalesOrder_WorkflowStateDTO() { }
        public DirectSalesOrder_WorkflowStateDTO(WorkflowState WorkflowState)
        {
            this.Id = WorkflowState.Id;
            this.Code = WorkflowState.Code;
            this.Name = WorkflowState.Name;
        }
    }
}
