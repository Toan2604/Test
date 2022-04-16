using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class StoreUserFavoriteProductMapping : DataEntity, IEquatable<StoreUserFavoriteProductMapping>
    {
        public long FavoriteProductId { get; set; }
        public long StoreUserId { get; set; }
        public Product FavoriteProduct { get; set; }
        public StoreUser StoreUser { get; set; }

        public bool Equals(StoreUserFavoriteProductMapping other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class StoreUserFavoriteProductMappingFilter : FilterEntity
    {
        public IdFilter FavoriteProductId { get; set; }
        public IdFilter StoreUserId { get; set; }
        public List<StoreUserFavoriteProductMappingFilter> OrFilter { get; set; }
        public StoreUserFavoriteProductMappingOrder OrderBy { get; set; }
        public StoreUserFavoriteProductMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum StoreUserFavoriteProductMappingOrder
    {
        FavoriteProduct = 0,
        StoreUser = 1,
    }

    [Flags]
    public enum StoreUserFavoriteProductMappingSelect : long
    {
        ALL = E.ALL,
        FavoriteProduct = E._0,
        StoreUser = E._1,
    }
}
