using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.e_route
{
    public class ERoute_RequestWorkflowStepMappingDTO : DataDTO
    {
        public Guid RequestId { get; set; }
        public long WorkflowStepId { get; set; }
        public long WorkflowStateId { get; set; }
        public long? AppUserId { get; set; }
        public ERoute_AppUserDTO AppUser { get; set; }
        public List<ERoute_AppUserDTO> NextApprovers { get; set; }
        public ERoute_WorkflowStateDTO WorkflowState { get; set; }
        public ERoute_WorkflowStepDTO WorkflowStep { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ERoute_RequestWorkflowStepMappingDTO() { }
        public ERoute_RequestWorkflowStepMappingDTO(RequestWorkflowStepMapping RequestWorkflowStepMapping)
        {
            this.RequestId = RequestWorkflowStepMapping.RequestId;
            this.WorkflowStepId = RequestWorkflowStepMapping.WorkflowStepId;
            this.WorkflowStateId = RequestWorkflowStepMapping.WorkflowStateId;
            this.AppUserId = RequestWorkflowStepMapping.AppUserId;
            this.CreatedAt = RequestWorkflowStepMapping.CreatedAt;
            this.UpdatedAt = RequestWorkflowStepMapping.UpdatedAt;
            this.AppUser = RequestWorkflowStepMapping.AppUser == null ? null : new ERoute_AppUserDTO(RequestWorkflowStepMapping.AppUser);
            this.WorkflowState = RequestWorkflowStepMapping.WorkflowState == null ? null : new ERoute_WorkflowStateDTO(RequestWorkflowStepMapping.WorkflowState);
            this.WorkflowStep = RequestWorkflowStepMapping.WorkflowStep == null ? null : new ERoute_WorkflowStepDTO(RequestWorkflowStepMapping.WorkflowStep);
            this.NextApprovers = RequestWorkflowStepMapping.NextApprovers?.Select(x => new ERoute_AppUserDTO(x)).ToList();
        }
    }
}
