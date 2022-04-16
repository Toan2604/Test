using System;
using TrueSight.Common;

namespace DMS.Entities
{
    public class RequestWorkflowDefinitionMapping : DataEntity, IEquatable<RequestWorkflowDefinitionMapping>
    {
        public Guid RequestId { get; set; }
        public long WorkflowDefinitionId { get; set; }
        public long RequestStateId { get; set; }
        public long CreatorId { get; set; }
        public long Counter { get; set; }

        public RequestState RequestState { get; set; }
        public WorkflowDefinition WorkflowDefinition { get; set; }

        public bool Equals(RequestWorkflowDefinitionMapping other)
        {
            return other != null && RequestId == other.RequestId && WorkflowDefinitionId == other.WorkflowDefinitionId;
        }
        public override int GetHashCode()
        {
            return RequestId.GetHashCode() ^ WorkflowDefinitionId.GetHashCode();
        }
    }

    public class RequestWorkflowDefinitionMappingFilter : FilterEntity
    {
        public GuidFilter RequestId { get; set; }
        public IdFilter WorkflowDefinitionId { get; set; }
    }
}
