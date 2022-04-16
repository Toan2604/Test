using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class PriceListStoreGroupingMapping : DataEntity, IEquatable<PriceListStoreGroupingMapping>
    {
        public long PriceListId { get; set; }
        public long StoreGroupingId { get; set; }
        public PriceList PriceList { get; set; }
        public StoreGrouping StoreGrouping { get; set; }

        public bool Equals(PriceListStoreGroupingMapping other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class PriceListStoreGroupingMappingFilter : FilterEntity
    {
        public IdFilter PriceListId { get; set; }
        public IdFilter StoreGroupingId { get; set; }
        public List<PriceListStoreGroupingMappingFilter> OrFilter { get; set; }
        public PriceListStoreGroupingMappingOrder OrderBy { get; set; }
        public PriceListStoreGroupingMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PriceListStoreGroupingMappingOrder
    {
        PriceList = 0,
        StoreGrouping = 1,
    }

    [Flags]
    public enum PriceListStoreGroupingMappingSelect : long
    {
        ALL = E.ALL,
        PriceList = E._0,
        StoreGrouping = E._1,
    }
}
