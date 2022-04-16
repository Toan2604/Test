using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class ProductProductGroupingMapping : DataEntity, IEquatable<ProductProductGroupingMapping>
    {
        public long ProductId { get; set; }
        public long ProductGroupingId { get; set; }
        public Product Product { get; set; }
        public ProductGrouping ProductGrouping { get; set; }

        public bool Equals(ProductProductGroupingMapping other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class ProductProductGroupingMappingFilter : FilterEntity
    {
        public IdFilter ProductId { get; set; }
        public IdFilter ProductGroupingId { get; set; }
        public List<ProductProductGroupingMappingFilter> OrFilter { get; set; }
        public ProductProductGroupingMappingOrder OrderBy { get; set; }
        public ProductProductGroupingMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProductProductGroupingMappingOrder
    {
        Product = 0,
        ProductGrouping = 1,
    }

    [Flags]
    public enum ProductProductGroupingMappingSelect : long
    {
        ALL = E.ALL,
        Product = E._0,
        ProductGrouping = E._1,
    }
}
