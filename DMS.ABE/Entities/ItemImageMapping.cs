using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class ItemImageMapping : DataEntity, IEquatable<ItemImageMapping>
    {
        public long ItemId { get; set; }
        public long ImageId { get; set; }
        public Image Image { get; set; }
        public Item Item { get; set; }

        public bool Equals(ItemImageMapping other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class ItemImageMappingFilter : FilterEntity
    {
        public IdFilter ItemId { get; set; }
        public IdFilter ImageId { get; set; }
        public List<ItemImageMappingFilter> OrFilter { get; set; }
        public ItemImageMappingOrder OrderBy { get; set; }
        public ItemImageMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ItemImageMappingOrder
    {
        Item = 0,
        Image = 1,
    }

    [Flags]
    public enum ItemImageMappingSelect : long
    {
        ALL = E.ALL,
        Item = E._0,
        Image = E._1,
    }
}
