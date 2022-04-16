using TrueSight.Common;
using System;
using System.Collections.Generic;
using DMS.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DMS.Entities
{
    public class LuckyDrawStructure : DataEntity,  IEquatable<LuckyDrawStructure>
    {
        public long Id { get; set; }
        public long LuckyDrawId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public long Quantity { get; set; }
        public long UsedQuantity { get; set; }
        public LuckyDraw LuckyDraw { get; set; }
        
        public bool Equals(LuckyDrawStructure other)
        {
            if (other == null) return false;
            if (this.Id != other.Id) return false;
            if (this.LuckyDrawId != other.LuckyDrawId) return false;
            if (this.Name != other.Name) return false;
            if (this.Value != other.Value) return false;
            if (this.Quantity != other.Quantity) return false;
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class LuckyDrawStructureFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter LuckyDrawId { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter Value { get; set; }
        public LongFilter Quantity { get; set; }
        public List<LuckyDrawStructureFilter> OrFilter { get; set; }
        public LuckyDrawStructureOrder OrderBy {get; set;}
        public LuckyDrawStructureSelect Selects {get; set;}
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum LuckyDrawStructureOrder
    {
        Id = 0,
        LuckyDraw = 1,
        Name = 2,
        Value = 3,
        Quantity = 4,
    }

    [Flags]
    public enum LuckyDrawStructureSelect:long
    {
        ALL = E.ALL,
        Id = E._0,
        LuckyDraw = E._1,
        Name = E._2,
        Value = E._3,
        Quantity = E._4,
    }
}
