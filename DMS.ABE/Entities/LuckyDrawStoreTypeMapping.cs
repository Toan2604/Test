using TrueSight.Common;
using System;
using System.Collections.Generic;
using DMS.ABE.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DMS.ABE.Entities
{
    public class LuckyDrawStoreTypeMapping : DataEntity,  IEquatable<LuckyDrawStoreTypeMapping>
    {
        public long LuckyDrawId { get; set; }
        public long StoreTypeId { get; set; }
        public LuckyDraw LuckyDraw { get; set; }
        public StoreType StoreType { get; set; }
        
        public bool Equals(LuckyDrawStoreTypeMapping other)
        {
            if (other == null) return false;
            if (this.LuckyDrawId != other.LuckyDrawId) return false;
            if (this.StoreTypeId != other.StoreTypeId) return false;
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class LuckyDrawStoreTypeMappingFilter : FilterEntity
    {
        public IdFilter LuckyDrawId { get; set; }
        public IdFilter StoreTypeId { get; set; }
        public List<LuckyDrawStoreTypeMappingFilter> OrFilter { get; set; }
        public LuckyDrawStoreTypeMappingOrder OrderBy {get; set;}
        public LuckyDrawStoreTypeMappingSelect Selects {get; set;}
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum LuckyDrawStoreTypeMappingOrder
    {
        LuckyDraw = 0,
        StoreType = 1,
    }

    [Flags]
    public enum LuckyDrawStoreTypeMappingSelect:long
    {
        ALL = E.ALL,
        LuckyDraw = E._0,
        StoreType = E._1,
    }
}
