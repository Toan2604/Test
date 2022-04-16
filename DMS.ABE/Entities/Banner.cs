using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class Banner : DataEntity, IEquatable<Banner>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public long? Priority { get; set; }
        public string Content { get; set; }
        public long CreatorId { get; set; }
        public long OrganizationId { get; set; }
        public long? ImageId { get; set; }
        public long StatusId { get; set; }
        public AppUser Creator { get; set; }
        public Organization Organization { get; set; }
        public Image Image { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public bool Equals(Banner other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class BannerFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Title { get; set; }
        public LongFilter Priority { get; set; }
        public StringFilter Content { get; set; }
        public IdFilter CreatorId { get; set; }
        public IdFilter OrganizationId { get; set; }
        public IdFilter ImageId { get; set; }
        public IdFilter StatusId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public List<BannerFilter> OrFilter { get; set; }
        public BannerOrder OrderBy { get; set; }
        public BannerSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum BannerOrder
    {
        Id = 0,
        Code = 1,
        Title = 2,
        Priority = 3,
        Content = 4,
        Creator = 5,
        Image = 6,
        Status = 7,
        Organization = 8,
        CreatedAt = 50,
        UpdatedAt = 51,
    }

    [Flags]
    public enum BannerSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Title = E._2,
        Priority = E._3,
        Content = E._4,
        Creator = E._5,
        Image = E._6,
        Status = E._7,
        Organization = E._8,
    }
}
