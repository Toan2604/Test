using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class AppUserStoreMapping : DataEntity, IEquatable<AppUserStoreMapping>
    {
        public long AppUserId { get; set; }
        public long StoreId { get; set; }
        public AppUser AppUser { get; set; }
        public Store Store { get; set; }

        public bool Equals(AppUserStoreMapping other)
        {
            if (other == null) return false;
            if (this.AppUserId != other.AppUserId) return false;
            if (this.StoreId != other.StoreId) return false;
            return true;
        }
        public override int GetHashCode()
        {
            return AppUserId.GetHashCode() ^ StoreId.GetHashCode();
        }
    }

    public class AppUserStoreMappingFilter : FilterEntity
    {
        public IdFilter AppUserId { get; set; }
        public IdFilter StoreId { get; set; }
        public List<AppUserStoreMappingFilter> OrFilter { get; set; }
        public AppUserStoreMappingOrder OrderBy { get; set; }
        public AppUserStoreMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum AppUserStoreMappingOrder
    {
        AppUser = 0,
        Store = 1,
    }

    [Flags]
    public enum AppUserStoreMappingSelect : long
    {
        ALL = E.ALL,
        AppUser = E._0,
        Store = E._1,
    }
}
