using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class BrandInStoreHistory : DataEntity, IEquatable<BrandInStore>
    {
        public long Id { get; set; }
        public long BrandInStoreId { get; set; }
        public long StoreId { get; set; }
        public long BrandId { get; set; }
        public long Top { get; set; }
        public long CreatorId { get; set; }
        public DateTime CreatedAt { get; set; }

        public Brand Brand { get; set; }
        public AppUser Creator { get; set; }
        public Store Store { get; set; }

        public bool Equals(BrandInStore other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class BrandInStoreHistoryFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter BrandInStoreId { get; set; }
        public IdFilter StoreId { get; set; }
        public IdFilter BrandId { get; set; }
        public LongFilter Top { get; set; }
        public IdFilter CreatorId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public List<BrandInStoreFilter> OrFilter { get; set; }
        public BrandInStoreHistoryOrder OrderBy { get; set; }
        public BrandInStoreHistorySelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum BrandInStoreHistoryOrder
    {
        Id = 0,
        Store = 1,
        Brand = 2,
        Top = 3,
        Creator = 4,
        CreatedAt = 5,
        BrandInStore = 6,
    }

    [Flags]
    public enum BrandInStoreHistorySelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Store = E._1,
        Brand = E._2,
        Top = E._3,
        Creator = E._4,
        CreatedAt = E._5,
        BrandInStore = E._6,
    }
}
