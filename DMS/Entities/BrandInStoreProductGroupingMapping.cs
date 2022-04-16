using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class BrandInStoreProductGroupingMapping : DataEntity, IEquatable<BrandInStoreProductGroupingMapping>
    {
        public long BrandInStoreId { get; set; }
        public long ProductGroupingId { get; set; }

        public BrandInStore BrandInStore { get; set; }
        public ProductGrouping ProductGrouping { get; set; }

        public bool Equals(BrandInStoreProductGroupingMapping other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class BrandInStoreProductGroupingMappingFilter : FilterEntity
    {
        public IdFilter BrandInStoreId { get; set; }
        public IdFilter ProductGroupingId { get; set; }
        public List<BrandInStoreProductGroupingMappingFilter> OrFilter { get; set; }
        public BrandInStoreProductGroupingMappingOrder OrderBy { get; set; }
        public BrandInStoreProductGroupingMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum BrandInStoreProductGroupingMappingOrder
    {
        BrandInStore = 0,
        ProductGrouping = 1,
    }

    [Flags]
    public enum BrandInStoreProductGroupingMappingSelect : long
    {
        ALL = E.ALL,
        BrandInStore = E._0,
        ProductGrouping = E._1,
    }
}
