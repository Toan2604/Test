using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Entities
{
    public class GeneralApprovalState : DataEntity, IEquatable<GeneralApprovalState>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public bool Equals(GeneralApprovalState other)
        {
            return other != null && Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class GeneralApprovalStateFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public List<GeneralApprovalStateFilter> OrFilter { get; set; }
        public GeneralApprovalStateOrder OrderBy { get; set; }
        public GeneralApprovalStateSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum GeneralApprovalStateOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
    }

    [Flags]
    public enum GeneralApprovalStateSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
    }
}
