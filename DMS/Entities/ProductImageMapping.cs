using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class ProductImageMapping : DataEntity, IEquatable<ProductImageMapping>
    {
        public long ProductId { get; set; }
        public long ImageId { get; set; }
        public Image Image { get; set; }
        public Product Product { get; set; }

        public bool Equals(ProductImageMapping other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class ProductImageMappingFilter : FilterEntity
    {
        public IdFilter ProductId { get; set; }
        public IdFilter ImageId { get; set; }
        public List<ProductImageMappingFilter> OrFilter { get; set; }
        public ProductImageMappingOrder OrderBy { get; set; }
        public ProductImageMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProductImageMappingOrder
    {
        Product = 0,
        Image = 1,
    }

    [Flags]
    public enum ProductImageMappingSelect : long
    {
        ALL = E.ALL,
        Product = E._0,
        Image = E._1,
    }
}
