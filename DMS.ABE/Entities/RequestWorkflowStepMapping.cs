using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class RequestWorkflowStepMapping : DataEntity, IEquatable<RequestWorkflowStepMapping>
    {
        public Guid RequestId { get; set; }
        public long WorkflowStepId { get; set; }
        public long WorkflowStateId { get; set; }
        public long? AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public WorkflowState WorkflowState { get; set; }
        public RequestState RequestState { get; set; }
        public WorkflowStep WorkflowStep { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<AppUser> NextApprovers { get; set; }

        public bool Equals(RequestWorkflowStepMapping other)
        {
            return other != null && RequestId == other.RequestId && WorkflowStepId == other.WorkflowStepId;
        }
        public override int GetHashCode()
        {
            return RequestId.GetHashCode() ^ WorkflowStepId.GetHashCode();
        }
    }

    public class RequestWorkflowStepMappingFilter : FilterEntity
    {
        public GuidFilter RequestId { get; set; }
        public IdFilter WorkflowStepId { get; set; }
        public IdFilter WorkflowStateId { get; set; }
        public IdFilter AppUserId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public List<RequestWorkflowStepMappingFilter> OrFilter { get; set; }
        public RequestWorkflowOrder OrderBy { get; set; }
        public RequestWorkflowSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum RequestWorkflowOrder
    {
        WorkflowStep = 2,
        WorkflowState = 3,
        RequestId = 4,
        AppUser = 5,
        CreatedAt = 50,
        UpdatedAt = 51,
    }

    [Flags]
    public enum RequestWorkflowSelect : long
    {
        ALL = E.ALL,
        WorkflowStep = E._2,
        WorkflowState = E._3,
        RequestId = E._4,
        AppUser = E._5,
    }
}
