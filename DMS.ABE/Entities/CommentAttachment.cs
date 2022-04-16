using TrueSight.Common;
using System;
using System.Collections.Generic;
using DMS.ABE.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DMS.ABE.Entities
{
    public class CommentAttachment : DataEntity,  IEquatable<CommentAttachment>
    {
        public long Id { get; set; }
        public long CommentId { get; set; }
        public long AttachmentTypeId { get; set; }
        public string Url { get; set; }
        public string Thumbnail { get; set; }
        public string Size { get; set; }
        public string Name { get; set; }
        public string Checksum { get; set; }
        public string Type { get; set; }
        public AttachmentType AttachmentType { get; set; }
        public Comment Comment { get; set; }
        
        public bool Equals(CommentAttachment other)
        {
            if (other == null) return false;
            if (this.Id != other.Id) return false;
            if (this.CommentId != other.CommentId) return false;
            if (this.AttachmentTypeId != other.AttachmentTypeId) return false;
            if (this.Url != other.Url) return false;
            if (this.Thumbnail != other.Thumbnail) return false;
            if (this.Size != other.Size) return false;
            if (this.Name != other.Name) return false;
            if (this.Checksum != other.Checksum) return false;
            if (this.Type != other.Type) return false;
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class CommentAttachmentFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter CommentId { get; set; }
        public IdFilter AttachmentTypeId { get; set; }
        public StringFilter Url { get; set; }
        public StringFilter Thumbnail { get; set; }
        public StringFilter Size { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter Checksum { get; set; }
        public StringFilter Type { get; set; }
        public List<CommentAttachmentFilter> OrFilter { get; set; }
        public CommentAttachmentOrder OrderBy {get; set;}
        public CommentAttachmentSelect Selects {get; set;}
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum CommentAttachmentOrder
    {
        Id = 0,
        Comment = 1,
        AttachmentType = 2,
        Url = 3,
        Thumbnail = 4,
        Size = 5,
        Name = 6,
        Checksum = 7,
        Type = 8,
    }

    [Flags]
    public enum CommentAttachmentSelect:long
    {
        ALL = E.ALL,
        Id = E._0,
        Comment = E._1,
        AttachmentType = E._2,
        Url = E._3,
        Thumbnail = E._4,
        Size = E._5,
        Name = E._6,
        Checksum = E._7,
        Type = E._8,
    }
}
