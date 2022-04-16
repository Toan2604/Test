using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.price_list
{
    public class PriceList_RequestWorkflowStepMappingDTO : DataDTO
    {
        public Guid RequestId { get; set; }
        public long WorkflowStepId { get; set; }
        public long WorkflowStateId { get; set; }
        public long? AppUserId { get; set; }
        public PriceList_AppUserDTO AppUser { get; set; }
        public List<PriceList_AppUserDTO> NextApprovers { get; set; }
        public PriceList_WorkflowStateDTO WorkflowState { get; set; }
        public PriceList_WorkflowStepDTO WorkflowStep { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public PriceList_RequestWorkflowStepMappingDTO() { }
        public PriceList_RequestWorkflowStepMappingDTO(RequestWorkflowStepMapping RequestWorkflowStepMapping)
        {
            this.RequestId = RequestWorkflowStepMapping.RequestId;
            this.WorkflowStepId = RequestWorkflowStepMapping.WorkflowStepId;
            this.WorkflowStateId = RequestWorkflowStepMapping.WorkflowStateId;
            this.AppUserId = RequestWorkflowStepMapping.AppUserId;
            this.CreatedAt = RequestWorkflowStepMapping.CreatedAt;
            this.UpdatedAt = RequestWorkflowStepMapping.UpdatedAt;
            this.AppUser = RequestWorkflowStepMapping.AppUser == null ? null : new PriceList_AppUserDTO(RequestWorkflowStepMapping.AppUser);
            this.WorkflowState = RequestWorkflowStepMapping.WorkflowState == null ? null : new PriceList_WorkflowStateDTO(RequestWorkflowStepMapping.WorkflowState);
            this.WorkflowStep = RequestWorkflowStepMapping.WorkflowStep == null ? null : new PriceList_WorkflowStepDTO(RequestWorkflowStepMapping.WorkflowStep);
            this.NextApprovers = RequestWorkflowStepMapping.NextApprovers?.Select(x => new PriceList_AppUserDTO(x)).ToList();
        }
    }
}
