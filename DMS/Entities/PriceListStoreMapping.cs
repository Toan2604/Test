using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class PriceListStoreMapping : DataEntity, IEquatable<PriceListStoreMapping>
    {
        public long PriceListId { get; set; }
        public long StoreId { get; set; }
        public PriceList PriceList { get; set; }
        public Store Store { get; set; }

        public bool Equals(PriceListStoreMapping other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class PriceListStoreMappingFilter : FilterEntity
    {
        public IdFilter PriceListId { get; set; }
        public IdFilter StoreId { get; set; }
        public List<PriceListStoreMappingFilter> OrFilter { get; set; }
        public PriceListStoreMappingOrder OrderBy { get; set; }
        public PriceListStoreMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PriceListStoreMappingOrder
    {
        PriceList = 0,
        Store = 1,
    }

    [Flags]
    public enum PriceListStoreMappingSelect : long
    {
        ALL = E.ALL,
        PriceList = E._0,
        Store = E._1,
    }
}
