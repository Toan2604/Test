using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class RequestWorkflowStepMappingDAO
    {
        public Guid RequestId { get; set; }
        public long WorkflowStepId { get; set; }
        public long WorkflowStateId { get; set; }
        public long? AppUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual AppUserDAO AppUser { get; set; }
        public virtual WorkflowStateDAO WorkflowState { get; set; }
        public virtual WorkflowStepDAO WorkflowStep { get; set; }
    }
}
