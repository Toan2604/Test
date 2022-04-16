using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class CommentAttachmentDAO
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

        public virtual AttachmentTypeDAO AttachmentType { get; set; }
        public virtual CommentDAO Comment { get; set; }
    }
}
