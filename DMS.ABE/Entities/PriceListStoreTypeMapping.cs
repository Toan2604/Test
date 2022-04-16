using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class PriceListStoreTypeMapping : DataEntity, IEquatable<PriceListStoreTypeMapping>
    {
        public long PriceListId { get; set; }
        public long StoreTypeId { get; set; }
        public PriceList PriceList { get; set; }
        public StoreType StoreType { get; set; }

        public bool Equals(PriceListStoreTypeMapping other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class PriceListStoreTypeMappingFilter : FilterEntity
    {
        public IdFilter PriceListId { get; set; }
        public IdFilter StoreTypeId { get; set; }
        public List<PriceListStoreTypeMappingFilter> OrFilter { get; set; }
        public PriceListStoreTypeMappingOrder OrderBy { get; set; }
        public PriceListStoreTypeMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PriceListStoreTypeMappingOrder
    {
        PriceList = 0,
        StoreType = 1,
    }

    [Flags]
    public enum PriceListStoreTypeMappingSelect : long
    {
        ALL = E.ALL,
        PriceList = E._0,
        StoreType = E._1,
    }
}
