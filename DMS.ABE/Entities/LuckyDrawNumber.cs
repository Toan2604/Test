using TrueSight.Common;
using System;
using System.Collections.Generic;
using DMS.ABE.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DMS.ABE.Entities
{
    public class LuckyDrawNumber : DataEntity,  IEquatable<LuckyDrawNumber>
    {
        public long Id { get; set; }
        public long LuckyDrawStructureId { get; set; }
        public bool Used { get; set; }
        public LuckyDrawStructure LuckyDrawStructure { get; set; }
        
        public bool Equals(LuckyDrawNumber other)
        {
            if (other == null) return false;
            if (this.Id != other.Id) return false;
            if (this.LuckyDrawStructureId != other.LuckyDrawStructureId) return false;
            if (this.Used != other.Used) return false;
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class LuckyDrawNumberFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter LuckyDrawStructureId { get; set; }
        public bool? Used { get; set; }
        public List<LuckyDrawNumberFilter> OrFilter { get; set; }
        public LuckyDrawNumberOrder OrderBy {get; set;}
        public LuckyDrawNumberSelect Selects {get; set;}
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum LuckyDrawNumberOrder
    {
        Id = 0,
        LuckyDrawStructure = 1,
        Used = 2,
    }

    [Flags]
    public enum LuckyDrawNumberSelect:long
    {
        ALL = E.ALL,
        Id = E._0,
        LuckyDrawStructure = E._1,
        Used = E._2,
    }
}
