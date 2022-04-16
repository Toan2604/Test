using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class WorkflowParameterTypeDAO
    {
        public WorkflowParameterTypeDAO()
        {
            WorkflowOperators = new HashSet<WorkflowOperatorDAO>();
            WorkflowParameters = new HashSet<WorkflowParameterDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<WorkflowOperatorDAO> WorkflowOperators { get; set; }
        public virtual ICollection<WorkflowParameterDAO> WorkflowParameters { get; set; }
    }
}
