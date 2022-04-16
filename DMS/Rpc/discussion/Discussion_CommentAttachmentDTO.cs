using TrueSight.Common;
using DMS.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using DMS.Entities;

namespace DMS.Rpc.discussion
{
    public class Discussion_CommentAttachmentDTO : DataDTO
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
        public Discussion_AttachmentTypeDTO AttachmentType { get; set; }   
        public Discussion_CommentAttachmentDTO() {}
        public Discussion_CommentAttachmentDTO(CommentAttachment CommentAttachment)
        {
            this.Id = CommentAttachment.Id;
            this.CommentId = CommentAttachment.CommentId;
            this.AttachmentTypeId = CommentAttachment.AttachmentTypeId;
            this.Url = CommentAttachment.Url;
            this.Thumbnail = CommentAttachment.Thumbnail;
            this.Size = CommentAttachment.Size;
            this.Name = CommentAttachment.Name;
            this.Checksum = CommentAttachment.Checksum;
            this.Type = CommentAttachment.Type;
            this.AttachmentType = CommentAttachment.AttachmentType == null ? null : new Discussion_AttachmentTypeDTO(CommentAttachment.AttachmentType);
            this.Errors = CommentAttachment.Errors;
        }
    }

    public class Discussion_CommentAttachmentFilterDTO : FilterDTO
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
        
        public CommentAttachmentOrder OrderBy { get; set; }
    }
}