using TrueSight.Common;
using System;
using System.Collections.Generic;
using DMS.ABE.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DMS.ABE.Entities
{
    public class LuckyDrawStoreGroupingMapping : DataEntity,  IEquatable<LuckyDrawStoreGroupingMapping>
    {
        public long LuckyDrawId { get; set; }
        public long StoreGroupingId { get; set; }
        public LuckyDraw LuckyDraw { get; set; }
        public StoreGrouping StoreGrouping { get; set; }
        
        public bool Equals(LuckyDrawStoreGroupingMapping other)
        {
            if (other == null) return false;
            if (this.LuckyDrawId != other.LuckyDrawId) return false;
            if (this.StoreGroupingId != other.StoreGroupingId) return false;
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class LuckyDrawStoreGroupingMappingFilter : FilterEntity
    {
        public IdFilter LuckyDrawId { get; set; }
        public IdFilter StoreGroupingId { get; set; }
        public List<LuckyDrawStoreGroupingMappingFilter> OrFilter { get; set; }
        public LuckyDrawStoreGroupingMappingOrder OrderBy {get; set;}
        public LuckyDrawStoreGroupingMappingSelect Selects {get; set;}
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum LuckyDrawStoreGroupingMappingOrder
    {
        LuckyDraw = 0,
        StoreGrouping = 1,
    }

    [Flags]
    public enum LuckyDrawStoreGroupingMappingSelect:long
    {
        ALL = E.ALL,
        LuckyDraw = E._0,
        StoreGrouping = E._1,
    }
}
