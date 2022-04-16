using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class WorkflowStateDAO
    {
        public WorkflowStateDAO()
        {
            RequestWorkflowHistories = new HashSet<RequestWorkflowHistoryDAO>();
            RequestWorkflowStepMappings = new HashSet<RequestWorkflowStepMappingDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<RequestWorkflowHistoryDAO> RequestWorkflowHistories { get; set; }
        public virtual ICollection<RequestWorkflowStepMappingDAO> RequestWorkflowStepMappings { get; set; }
    }
}
