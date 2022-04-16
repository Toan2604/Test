using DMS.Entities;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.mobile_sync
{
    public class MobileSync_RequestWorkflowStepMappingDTO : DataDTO
    {
        public Guid RequestId { get; set; }
        public long WorkflowStepId { get; set; }
        public long WorkflowStateId { get; set; }
        public long? AppUserId { get; set; }
        public MobileSync_AppUserDTO AppUser { get; set; }
        public List<MobileSync_AppUserDTO> NextApprovers { get; set; }
        public MobileSync_WorkflowStateDTO WorkflowState { get; set; }
        public MobileSync_WorkflowStepDTO WorkflowStep { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public MobileSync_RequestWorkflowStepMappingDTO() { }
        public MobileSync_RequestWorkflowStepMappingDTO(RequestWorkflowStepMapping RequestWorkflowStepMapping)
        {
            this.RequestId = RequestWorkflowStepMapping.RequestId;
            this.WorkflowStepId = RequestWorkflowStepMapping.WorkflowStepId;
            this.WorkflowStateId = RequestWorkflowStepMapping.WorkflowStateId;
            this.AppUserId = RequestWorkflowStepMapping.AppUserId;
            this.CreatedAt = RequestWorkflowStepMapping.CreatedAt;
            this.UpdatedAt = RequestWorkflowStepMapping.UpdatedAt;
            this.AppUser = RequestWorkflowStepMapping.AppUser == null ? null : new MobileSync_AppUserDTO(RequestWorkflowStepMapping.AppUser);
            this.WorkflowState = RequestWorkflowStepMapping.WorkflowState == null ? null : new MobileSync_WorkflowStateDTO(RequestWorkflowStepMapping.WorkflowState);
            this.WorkflowStep = RequestWorkflowStepMapping.WorkflowStep == null ? null : new MobileSync_WorkflowStepDTO(RequestWorkflowStepMapping.WorkflowStep);
        }
    }
}
