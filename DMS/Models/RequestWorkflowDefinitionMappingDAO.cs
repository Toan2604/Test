using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class RequestWorkflowDefinitionMappingDAO
    {
        public Guid RequestId { get; set; }
        public long WorkflowDefinitionId { get; set; }
        public long RequestStateId { get; set; }
        public long CreatorId { get; set; }
        public long Counter { get; set; }

        public virtual RequestStateDAO RequestState { get; set; }
        public virtual WorkflowDefinitionDAO WorkflowDefinition { get; set; }
    }
}
