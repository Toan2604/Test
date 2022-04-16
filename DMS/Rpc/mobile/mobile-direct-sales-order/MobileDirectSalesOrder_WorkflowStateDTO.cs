using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile.direct_sales_order
{
    public class MobileDirectSalesOrder_WorkflowStateDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public MobileDirectSalesOrder_WorkflowStateDTO() { }
        public MobileDirectSalesOrder_WorkflowStateDTO(WorkflowState WorkflowState)
        {
            this.Id = WorkflowState.Id;
            this.Code = WorkflowState.Code;
            this.Name = WorkflowState.Name;
        }
    }
}
