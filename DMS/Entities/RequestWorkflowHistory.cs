using System;

namespace DMS.Entities
{
    public class RequestWorkflowHistory
    {
        public long Id { get; set; }
        public Guid RequestId { get; set; }
        public long WorkflowStepId { get; set; }
        public long WorkflowStateId { get; set; }
        public long? AppUserId { get; set; }
        public long StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public AppUser AppUser { get; set; }
        public Status Status { get; set; }
        public WorkflowState WorkflowState { get; set; }
        public WorkflowStep WorkflowStep { get; set; }
    }
}
