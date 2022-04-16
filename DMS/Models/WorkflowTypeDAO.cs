using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class WorkflowTypeDAO
    {
        public WorkflowTypeDAO()
        {
            WorkflowDefinitions = new HashSet<WorkflowDefinitionDAO>();
            WorkflowParameters = new HashSet<WorkflowParameterDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<WorkflowDefinitionDAO> WorkflowDefinitions { get; set; }
        public virtual ICollection<WorkflowParameterDAO> WorkflowParameters { get; set; }
    }
}
