using TrueSight.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS.Entities;

namespace DMS.Rpc.discussion
{
    public class Discussion_PostDTO : DataDTO
    {
        public long Id { get; set; }
        public Guid DiscussionId { get; set; }
        public string Content { get; set; }
        public string MobileContent { get; set; }
        public string Url { get; set; }
        public long CreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public Discussion_AppUserDTO Creator { get; set; }
        public List<Discussion_CommentDTO> Comments { get; set; }

        public Discussion_PostDTO() { }
        public Discussion_PostDTO(Post Post)
        {
            this.Id = Post.Id;
            this.DiscussionId = Post.DiscussionId;
            this.Content = Post.Content;
            this.MobileContent = Post.MobileContent;
            this.Url = Post.Url;
            this.CreatorId = Post.CreatorId;
            this.CreatedAt = Post.CreatedAt;
            this.UpdatedAt = Post.UpdatedAt;
            this.DeletedAt = Post.DeletedAt;
            this.Creator = Post.Creator == null ? null : new Discussion_AppUserDTO(Post.Creator);
            this.Comments = Post.Comments?.Select(c => new Discussion_CommentDTO(c)).ToList();
        }
    }

    public class Discussion_PostFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public GuidFilter DiscussionId { get; set; }
    }
}
