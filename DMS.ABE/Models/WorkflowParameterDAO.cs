using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class WorkflowParameterDAO
    {
        public WorkflowParameterDAO()
        {
            RequestWorkflowParameterMappings = new HashSet<RequestWorkflowParameterMappingDAO>();
            WorkflowDirectionConditions = new HashSet<WorkflowDirectionConditionDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long WorkflowTypeId { get; set; }
        public long WorkflowParameterTypeId { get; set; }

        public virtual WorkflowParameterTypeDAO WorkflowParameterType { get; set; }
        public virtual WorkflowTypeDAO WorkflowType { get; set; }
        public virtual ICollection<RequestWorkflowParameterMappingDAO> RequestWorkflowParameterMappings { get; set; }
        public virtual ICollection<WorkflowDirectionConditionDAO> WorkflowDirectionConditions { get; set; }
    }
}
