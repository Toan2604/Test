using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class WorkflowStep : DataEntity, IEquatable<WorkflowStep>
    {
        public long Id { get; set; }
        public long WorkflowDefinitionId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long RoleId { get; set; }
        public string SubjectMailForReject { get; set; }
        public string BodyMailForReject { get; set; }
        public long ModifierId { get; set; }
        public long StatusId { get; set; }
        public bool Used { get; set; }
        public Role Role { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public WorkflowDefinition WorkflowDefinition { get; set; }
        public List<WorkflowParameter> WorkflowParameters { get; set; }

        public bool Equals(WorkflowStep other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class WorkflowStepFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter WorkflowDefinitionId { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter RoleId { get; set; }
        public IdFilter StatusId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public List<WorkflowStepFilter> OrFilter { get; set; }
        public WorkflowStepOrder OrderBy { get; set; }
        public WorkflowStepSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum WorkflowStepOrder
    {
        Id = 0,
        WorkflowDefinition = 1,
        Code = 2,
        Name = 3,
        Role = 4,
        CreatedAt = 5,
        UpdatedAt = 6,
    }

    [Flags]
    public enum WorkflowStepSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        WorkflowDefinition = E._1,
        Code = E._2,
        Name = E._3,
        Role = E._4,
        Status = E._5,
        CreatedAt = E._6,
        UpdatedAt = E._7,
    }
}
