using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class WorkflowDirectionConditionDAO
    {
        public long Id { get; set; }
        public long WorkflowDirectionId { get; set; }
        public long WorkflowParameterId { get; set; }
        public long WorkflowOperatorId { get; set; }
        public string Value { get; set; }

        public virtual WorkflowDirectionDAO WorkflowDirection { get; set; }
        public virtual WorkflowOperatorDAO WorkflowOperator { get; set; }
        public virtual WorkflowParameterDAO WorkflowParameter { get; set; }
    }
}
