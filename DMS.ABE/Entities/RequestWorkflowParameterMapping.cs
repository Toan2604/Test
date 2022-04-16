using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class RequestWorkflowParameterMapping : DataEntity, IEquatable<RequestWorkflowParameterMapping>
    {
        public long WorkflowParameterId { get; set; }
        public Guid RequestId { get; set; }
        public string Value { get; set; }
        public long WorkflowParameterTypeId { get; set; }
        public long? IdValue
        {
            get
            {
                if (Value == null)
                    return null;
                if (WorkflowParameterTypeId == WorkflowParameterTypeEnum.ID.Id)
                    return long.Parse(Value);
                return null;
            }
        }
        public long? LongValue
        {
            get
            {
                if (Value == null)
                    return null;
                if (WorkflowParameterTypeId == WorkflowParameterTypeEnum.LONG.Id)
                    return long.Parse(Value);
                return null;
            }
        }
        public decimal? DecimalValue
        {
            get
            {
                if (Value == null)
                    return null;
                if (WorkflowParameterTypeId == WorkflowParameterTypeEnum.DECIMAL.Id)
                    return decimal.Parse(Value);
                return null;
            }
        }
        public string StringValue
        {
            get
            {
                if (Value == null)
                    return null;
                if (WorkflowParameterTypeId == WorkflowParameterTypeEnum.STRING.Id)
                    return Value;
                return null;
            }
        }
        public DateTime? DateValue
        {
            get
            {
                if (Value == null)
                    return null;
                if (WorkflowParameterTypeId == WorkflowParameterTypeEnum.DATE.Id)
                    return DateTime.Parse(Value);
                return null;
            }
        }

        public WorkflowParameter WorkflowParameter { get; set; }

        public bool Equals(RequestWorkflowParameterMapping other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class RequestWorkflowParameterMappingFilter : FilterEntity
    {
        public IdFilter WorkflowParameterId { get; set; }
        public IdFilter RequestId { get; set; }
        public StringFilter Value { get; set; }
        public List<RequestWorkflowParameterMappingFilter> OrFilter { get; set; }
        public RequestWorkflowParameterMappingOrder OrderBy { get; set; }
        public RequestWorkflowParameterMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum RequestWorkflowParameterMappingOrder
    {
        WorkflowParameter = 0,
        Value = 2,
    }

    [Flags]
    public enum RequestWorkflowParameterMappingSelect : long
    {
        ALL = E.ALL,
        WorkflowParameter = E._0,
        Value = E._2,
    }
}
