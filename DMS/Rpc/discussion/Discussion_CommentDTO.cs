using TrueSight.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS.Entities;

namespace DMS.Rpc.discussion
{
    public class Discussion_CommentDTO : DataDTO
    {
        public long Id { get; set; }
        public bool IsOwner { get; set; }
        public Guid DiscussionId { get; set; }
        public string Content { get; set; }
        public string MobileContent { get; set; }
        public string Url { get; set; }
        public long CreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public Discussion_GlobalUserDTO Creator { get; set; }
        public List<Discussion_CommentAttachmentDTO> CommentAttachments { get; set; }

        public Discussion_CommentDTO() { }
        public Discussion_CommentDTO(Comment Comment)
        {
            this.Id = Comment.Id;
            this.IsOwner = Comment.IsOwner;
            this.DiscussionId = Comment.DiscussionId;
            this.Content = Comment.Content;
            this.MobileContent = Comment.MobileContent;
            this.Url = Comment.Url;
            this.CreatorId = Comment.CreatorId;
            this.CreatedAt = Comment.CreatedAt;
            this.UpdatedAt = Comment.UpdatedAt;
            this.DeletedAt = Comment.DeletedAt;
            this.CommentAttachments = Comment.CommentAttachments == null ? null : Comment.CommentAttachments.Select(x => new Discussion_CommentAttachmentDTO(x)).ToList();
            this.Creator = Comment.Creator == null ? null : new Discussion_GlobalUserDTO(Comment.Creator);
        }
    }

    public class Discussion_CommentFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public GuidFilter DiscussionId { get; set; }
        public IdFilter CreatorId { get; set; }
        public StringFilter Content { get; set; }
        public StringFilter Url { get; set; }
    }
}
