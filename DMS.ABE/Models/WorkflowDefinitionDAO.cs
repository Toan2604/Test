using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class WorkflowDefinitionDAO
    {
        public WorkflowDefinitionDAO()
        {
            RequestWorkflowDefinitionMappings = new HashSet<RequestWorkflowDefinitionMappingDAO>();
            WorkflowDirections = new HashSet<WorkflowDirectionDAO>();
            WorkflowSteps = new HashSet<WorkflowStepDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long CreatorId { get; set; }
        public long ModifierId { get; set; }
        public long WorkflowTypeId { get; set; }
        public long OrganizationId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool Used { get; set; }

        public virtual AppUserDAO Creator { get; set; }
        public virtual AppUserDAO Modifier { get; set; }
        public virtual OrganizationDAO Organization { get; set; }
        public virtual StatusDAO Status { get; set; }
        public virtual WorkflowTypeDAO WorkflowType { get; set; }
        public virtual ICollection<RequestWorkflowDefinitionMappingDAO> RequestWorkflowDefinitionMappings { get; set; }
        public virtual ICollection<WorkflowDirectionDAO> WorkflowDirections { get; set; }
        public virtual ICollection<WorkflowStepDAO> WorkflowSteps { get; set; }
    }
}
