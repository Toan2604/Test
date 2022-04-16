using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class StoreImageMapping : DataEntity, IEquatable<StoreImageMapping>
    {
        public long StoreId { get; set; }
        public long ImageId { get; set; }
        public Image Image { get; set; }
        public Store Store { get; set; }

        public bool Equals(StoreImageMapping other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class StoreImageMappingFilter : FilterEntity
    {
        public IdFilter StoreId { get; set; }
        public IdFilter ImageId { get; set; }
        public List<StoreImageMappingFilter> OrFilter { get; set; }
        public StoreImageMappingOrder OrderBy { get; set; }
        public StoreImageMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum StoreImageMappingOrder
    {
        Store = 0,
        Image = 1,
    }

    [Flags]
    public enum StoreImageMappingSelect : long
    {
        ALL = E.ALL,
        Store = E._0,
        Image = E._1,
    }
}
