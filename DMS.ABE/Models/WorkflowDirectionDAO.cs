using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class WorkflowDirectionDAO
    {
        public WorkflowDirectionDAO()
        {
            WorkflowDirectionConditions = new HashSet<WorkflowDirectionConditionDAO>();
        }

        public long Id { get; set; }
        public long WorkflowDefinitionId { get; set; }
        public long FromStepId { get; set; }
        public long ToStepId { get; set; }
        public string SubjectMailForCreator { get; set; }
        public string SubjectMailForCurrentStep { get; set; }
        public string SubjectMailForNextStep { get; set; }
        public string BodyMailForCreator { get; set; }
        public string BodyMailForCurrentStep { get; set; }
        public string BodyMailForNextStep { get; set; }
        public long StatusId { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid RowId { get; set; }

        public virtual WorkflowStepDAO FromStep { get; set; }
        public virtual StatusDAO Status { get; set; }
        public virtual WorkflowStepDAO ToStep { get; set; }
        public virtual WorkflowDefinitionDAO WorkflowDefinition { get; set; }
        public virtual ICollection<WorkflowDirectionConditionDAO> WorkflowDirectionConditions { get; set; }
    }
}
