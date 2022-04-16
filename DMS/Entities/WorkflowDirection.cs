using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class WorkflowDirection : DataEntity, IEquatable<WorkflowDirection>
    {
        public long Id { get; set; }
        public long WorkflowDefinitionId { get; set; }
        public long FromStepId { get; set; }
        public long ToStepId { get; set; }
        public WorkflowStep FromStep { get; set; }
        public WorkflowStep ToStep { get; set; }
        public string SubjectMailForCreator { get; set; }
        public string SubjectMailForCurrentStep { get; set; }
        public string SubjectMailForNextStep { get; set; }
        public string BodyMailForCreator { get; set; }
        public string BodyMailForCurrentStep { get; set; }
        public string BodyMailForNextStep { get; set; }
        public long StatusId { get; set; }
        public long ModifierId { get; set; }
        public bool Used { get; set; }
        public Guid RowId { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Status Status { get; set; }
        public WorkflowDefinition WorkflowDefinition { get; set; }
        public List<WorkflowParameter> WorkflowParameters { get; set; }
        public List<WorkflowDirectionCondition> WorkflowDirectionConditions { get; set; }


        public bool Equals(WorkflowDirection other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class WorkflowDirectionFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter WorkflowDefinitionId { get; set; }
        public IdFilter FromStepId { get; set; }
        public IdFilter ToStepId { get; set; }
        public IdFilter StatusId { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public List<WorkflowDirectionFilter> OrFilter { get; set; }
        public WorkflowDirectionOrder OrderBy { get; set; }
        public WorkflowDirectionSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum WorkflowDirectionOrder
    {
        Id = 0,
        WorkflowDefinition = 1,
        FromStep = 2,
        ToStep = 3,
        UpdatedAt = 4,
        Status = 5
    }

    [Flags]
    public enum WorkflowDirectionSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        WorkflowDefinition = E._1,
        FromStep = E._2,
        ToStep = E._3,
        UpdatedAt = E._4,
        Status = E._5,
    }
}
