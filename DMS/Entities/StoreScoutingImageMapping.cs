using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class StoreScoutingImageMapping : DataEntity, IEquatable<StoreScoutingImageMapping>
    {
        public long StoreScoutingId { get; set; }
        public long ImageId { get; set; }
        public Image Image { get; set; }
        public StoreScouting StoreScouting { get; set; }

        public bool Equals(StoreScoutingImageMapping other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class StoreScoutingImageMappingFilter : FilterEntity
    {
        public IdFilter StoreScoutingId { get; set; }
        public IdFilter ImageId { get; set; }
        public List<StoreImageMappingFilter> OrFilter { get; set; }
        public StoreImageMappingOrder OrderBy { get; set; }
        public StoreImageMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum StoreScoutingImageMappingOrder
    {
        StoreScouting = 0,
        Image = 1,
    }

    [Flags]
    public enum StoreScoutingImageMappingSelect : long
    {
        ALL = E.ALL,
        StoreScouting = E._0,
        Image = E._1,
    }
}
