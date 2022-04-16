using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class AlbumImageMapping : DataEntity, IEquatable<AlbumImageMapping>
    {
        public long AlbumId { get; set; }
        public long StoreId { get; set; }
        public long ImageId { get; set; }
        public long OrganizationId { get; set; }
        public long? Distance { get; set; }
        public DateTime ShootingAt { get; set; }
        public long? SaleEmployeeId { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Album Album { get; set; }
        public Organization Organization { get; set; }
        public AppUser SaleEmployee { get; set; }
        public Image Image { get; set; }
        public Store Store { get; set; }

        public bool Equals(AlbumImageMapping other)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class AlbumImageMappingFilter : FilterEntity
    {
        public IdFilter AlbumId { get; set; }
        public IdFilter StoreId { get; set; }
        public IdFilter ImageId { get; set; }
        public IdFilter OrganizationId { get; set; }
        public IdFilter SaleEmployeeId { get; set; }
        public List<AlbumImageMappingFilter> OrFilter { get; set; }
        public AlbumImageMappingOrder OrderBy { get; set; }
        public AlbumImageMappingSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum AlbumImageMappingOrder
    {
        Album = 0,
        Store = 1,
        Image = 2,
        SaleEmployee = 3,
        Organization = 4,
    }

    [Flags]
    public enum AlbumImageMappingSelect : long
    {
        ALL = E.ALL,
        Album = E._0,
        Store = E._1,
        Image = E._2,
        SaleEmployee = E._3,
        Organization = E._4,
    }
}
