using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class StoreStoreGroupingMapping : DataEntity, IEquatable<StoreStoreGroupingMapping>
    {
        public long StoreId { get; set; }
        public long StoreGroupingId { get; set; }
        public Store Store { get; set; }
        public StoreGrouping StoreGrouping { get; set; }

        public bool Equals(StoreStoreGroupingMapping other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class StoreStoreGroupingMappingFilter : FilterEntity
    {
        public IdFilter StoreId { get; set; }
        public IdFilter StoreGroupingId { get; set; }
        public List<StoreStoreGroupingMappingFilter> OrFilter { get; set; }
        public StoreStoreGroupingMappingOrder OrderBy { get; set; }
        public StoreStoreGroupingMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum StoreStoreGroupingMappingOrder
    {
        Store = 0,
        StoreGrouping = 1,
    }

    [Flags]
    public enum StoreStoreGroupingMappingSelect : long
    {
        ALL = E.ALL,
        Store = E._0,
        StoreGrouping = E._1,
    }
}
