using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class ProblemImageMapping : DataEntity, IEquatable<ProblemImageMapping>
    {
        public long ProblemId { get; set; }
        public long ImageId { get; set; }
        public Image Image { get; set; }
        public Problem Problem { get; set; }

        public bool Equals(ProblemImageMapping other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class ProblemImageMappingFilter : FilterEntity
    {
        public IdFilter ProblemId { get; set; }
        public IdFilter ImageId { get; set; }
        public List<ProblemImageMappingFilter> OrFilter { get; set; }
        public ProblemImageMappingOrder OrderBy { get; set; }
        public ProblemImageMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProblemImageMappingOrder
    {
        Problem = 0,
        Image = 1,
    }

    [Flags]
    public enum ProblemImageMappingSelect : long
    {
        ALL = E.ALL,
        Problem = E._0,
        Image = E._1,
    }
}
