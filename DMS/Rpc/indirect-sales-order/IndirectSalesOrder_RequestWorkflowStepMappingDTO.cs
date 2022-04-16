using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.indirect_sales_order
{
    public class IndirectSalesOrder_RequestWorkflowStepMappingDTO : DataDTO
    {
        public Guid RequestId { get; set; }
        public long WorkflowStepId { get; set; }
        public long WorkflowStateId { get; set; }
        public long? AppUserId { get; set; }
        public IndirectSalesOrder_AppUserDTO AppUser { get; set; }
        public List<IndirectSalesOrder_AppUserDTO> NextApprovers { get; set; }
        public IndirectSalesOrder_WorkflowStateDTO WorkflowState { get; set; }
        public IndirectSalesOrder_WorkflowStepDTO WorkflowStep { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public IndirectSalesOrder_RequestWorkflowStepMappingDTO() { }
        public IndirectSalesOrder_RequestWorkflowStepMappingDTO(RequestWorkflowStepMapping RequestWorkflowStepMapping)
        {
            this.RequestId = RequestWorkflowStepMapping.RequestId;
            this.WorkflowStepId = RequestWorkflowStepMapping.WorkflowStepId;
            this.WorkflowStateId = RequestWorkflowStepMapping.WorkflowStateId;
            this.AppUserId = RequestWorkflowStepMapping.AppUserId;
            this.CreatedAt = RequestWorkflowStepMapping.CreatedAt;
            this.UpdatedAt = RequestWorkflowStepMapping.UpdatedAt;
            this.AppUser = RequestWorkflowStepMapping.AppUser == null ? null : new IndirectSalesOrder_AppUserDTO(RequestWorkflowStepMapping.AppUser);
            this.WorkflowState = RequestWorkflowStepMapping.WorkflowState == null ? null : new IndirectSalesOrder_WorkflowStateDTO(RequestWorkflowStepMapping.WorkflowState);
            this.WorkflowStep = RequestWorkflowStepMapping.WorkflowStep == null ? null : new IndirectSalesOrder_WorkflowStepDTO(RequestWorkflowStepMapping.WorkflowStep);
            this.NextApprovers = RequestWorkflowStepMapping.NextApprovers?.Select(x => new IndirectSalesOrder_AppUserDTO(x)).ToList();
        }
    }
}
