using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.mobile.direct_sales_order
{
    public class MobileDirectSalesOrder_RequestWorkflowStepMappingDTO : DataDTO
    {
        public Guid RequestId { get; set; }
        public long WorkflowStepId { get; set; }
        public long WorkflowStateId { get; set; }
        public long? AppUserId { get; set; }
        public MobileDirectSalesOrder_AppUserDTO AppUser { get; set; }
        public List<MobileDirectSalesOrder_AppUserDTO> NextApprovers { get; set; }
        public MobileDirectSalesOrder_WorkflowStateDTO WorkflowState { get; set; }
        public MobileDirectSalesOrder_WorkflowStepDTO WorkflowStep { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public MobileDirectSalesOrder_RequestWorkflowStepMappingDTO() { }
        public MobileDirectSalesOrder_RequestWorkflowStepMappingDTO(RequestWorkflowStepMapping RequestWorkflowStepMapping)
        {
            this.RequestId = RequestWorkflowStepMapping.RequestId;
            this.WorkflowStepId = RequestWorkflowStepMapping.WorkflowStepId;
            this.WorkflowStateId = RequestWorkflowStepMapping.WorkflowStateId;
            this.AppUserId = RequestWorkflowStepMapping.AppUserId;
            this.CreatedAt = RequestWorkflowStepMapping.CreatedAt;
            this.UpdatedAt = RequestWorkflowStepMapping.UpdatedAt;
            this.AppUser = RequestWorkflowStepMapping.AppUser == null ? null : new MobileDirectSalesOrder_AppUserDTO(RequestWorkflowStepMapping.AppUser);
            this.WorkflowState = RequestWorkflowStepMapping.WorkflowState == null ? null : new MobileDirectSalesOrder_WorkflowStateDTO(RequestWorkflowStepMapping.WorkflowState);
            this.WorkflowStep = RequestWorkflowStepMapping.WorkflowStep == null ? null : new MobileDirectSalesOrder_WorkflowStepDTO(RequestWorkflowStepMapping.WorkflowStep);
            this.NextApprovers = RequestWorkflowStepMapping.NextApprovers?.Select(x => new MobileDirectSalesOrder_AppUserDTO(x)).ToList();
        }
    }
}
