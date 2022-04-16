using TrueSight.Common;
using System;
using System.Collections.Generic;
using DMS.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DMS.Entities
{
    public class LuckyDrawStoreMapping : DataEntity,  IEquatable<LuckyDrawStoreMapping>
    {
        public long LuckyDrawId { get; set; }
        public long StoreId { get; set; }
        public LuckyDraw LuckyDraw { get; set; }
        public Store Store { get; set; }
        
        public bool Equals(LuckyDrawStoreMapping other)
        {
            if (other == null) return false;
            if (this.LuckyDrawId != other.LuckyDrawId) return false;
            if (this.StoreId != other.StoreId) return false;
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class LuckyDrawStoreMappingFilter : FilterEntity
    {
        public IdFilter LuckyDrawId { get; set; }
        public IdFilter StoreId { get; set; }
        public List<LuckyDrawStoreMappingFilter> OrFilter { get; set; }
        public LuckyDrawStoreMappingOrder OrderBy {get; set;}
        public LuckyDrawStoreMappingSelect Selects {get; set;}
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum LuckyDrawStoreMappingOrder
    {
        LuckyDraw = 0,
        Store = 1,
    }

    [Flags]
    public enum LuckyDrawStoreMappingSelect:long
    {
        ALL = E.ALL,
        LuckyDraw = E._0,
        Store = E._1,
    }
}
