using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class ShowingItemImageMapping : DataEntity, IEquatable<ShowingItemImageMapping>
    {
        public long ShowingItemId { get; set; }
        public long ImageId { get; set; }
        public Image Image { get; set; }
        public ShowingItem ShowingItem { get; set; }

        public bool Equals(ShowingItemImageMapping other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class ShowingItemImageMappingFilter : FilterEntity
    {
        public IdFilter ShowingItemId { get; set; }
        public IdFilter ImageId { get; set; }
        public List<ShowingItemImageMappingFilter> OrFilter { get; set; }
        public ShowingItemImageMappingOrder OrderBy { get; set; }
        public ShowingItemImageMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ShowingItemImageMappingOrder
    {
        ShowingItem = 0,
        Image = 1,
    }

    [Flags]
    public enum ShowingItemImageMappingSelect : long
    {
        ALL = E.ALL,
        ShowingItem = E._0,
        Image = E._1,
    }
}
