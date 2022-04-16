using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class Image : DataEntity, IEquatable<Image>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }
        public Guid RowId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public byte[] Content { get; set; }
        public List<StoreCheckingImageMapping> ImageStoreCheckingMapping { get; set; }
        public bool Equals(Image other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class ImageFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter Url { get; set; }
        public StringFilter ThumbnailUrl { get; set; }
        public IdFilter StoreCheckingId { get; set; }
        public IdFilter AlbumId { get; set; }
        public List<ImageFilter> OrFilter { get; set; }
        public ImageOrder OrderBy { get; set; }
        public ImageSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ImageOrder
    {
        Id = 0,
        Name = 1,
        Url = 2,
        ThumbnailUrl = 3,
    }

    [Flags]
    public enum ImageSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Name = E._1,
        Url = E._2,
        ThumbnailUrl = E._3,
    }
}
