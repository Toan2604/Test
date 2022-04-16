using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.direct_sales_order
{
    public class DirectSalesOrder_RequestWorkflowStepMappingDTO : DataDTO
    {
        public Guid RequestId { get; set; }
        public long WorkflowStepId { get; set; }
        public long WorkflowStateId { get; set; }
        public long? AppUserId { get; set; }
        public DirectSalesOrder_AppUserDTO AppUser { get; set; }
        public List<DirectSalesOrder_AppUserDTO> NextApprovers { get; set; }
        public DirectSalesOrder_WorkflowStateDTO WorkflowState { get; set; }
        public DirectSalesOrder_WorkflowStepDTO WorkflowStep { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public DirectSalesOrder_RequestWorkflowStepMappingDTO() { }
        public DirectSalesOrder_RequestWorkflowStepMappingDTO(RequestWorkflowStepMapping RequestWorkflowStepMapping)
        {
            this.RequestId = RequestWorkflowStepMapping.RequestId;
            this.WorkflowStepId = RequestWorkflowStepMapping.WorkflowStepId;
            this.WorkflowStateId = RequestWorkflowStepMapping.WorkflowStateId;
            this.AppUserId = RequestWorkflowStepMapping.AppUserId;
            this.CreatedAt = RequestWorkflowStepMapping.CreatedAt;
            this.UpdatedAt = RequestWorkflowStepMapping.UpdatedAt;
            this.AppUser = RequestWorkflowStepMapping.AppUser == null ? null : new DirectSalesOrder_AppUserDTO(RequestWorkflowStepMapping.AppUser);
            this.WorkflowState = RequestWorkflowStepMapping.WorkflowState == null ? null : new DirectSalesOrder_WorkflowStateDTO(RequestWorkflowStepMapping.WorkflowState);
            this.WorkflowStep = RequestWorkflowStepMapping.WorkflowStep == null ? null : new DirectSalesOrder_WorkflowStepDTO(RequestWorkflowStepMapping.WorkflowStep);
            this.NextApprovers = RequestWorkflowStepMapping.NextApprovers?.Select(x => new DirectSalesOrder_AppUserDTO(x)).ToList();
        }
    }
}
