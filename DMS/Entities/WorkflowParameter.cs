using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class WorkflowParameter : DataEntity, IEquatable<WorkflowParameter>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long WorkflowTypeId { get; set; }
        public long WorkflowParameterTypeId { get; set; }
        public WorkflowParameterType WorkflowParameterType { get; set; }
        public WorkflowType WorkflowType { get; set; }

        public bool Equals(WorkflowParameter other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class WorkflowParameterFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter WorkflowTypeId { get; set; }
        public IdFilter WorkflowParameterTypeId { get; set; }
        public List<WorkflowParameterFilter> OrFilter { get; set; }
        public WorkflowParameterOrder OrderBy { get; set; }
        public WorkflowParameterSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum WorkflowParameterOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        WorkflowType = 3,
        WorkflowParameterType = 4,
    }

    [Flags]
    public enum WorkflowParameterSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        WorkflowType = E._3,
        WorkflowParameterType = E._4,
    }
}
