using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class CommentDAO
    {
        public CommentDAO()
        {
            CommentAttachments = new HashSet<CommentAttachmentDAO>();
            GlobalUserCommentMappings = new HashSet<GlobalUserCommentMappingDAO>();
        }

        public long Id { get; set; }
        public Guid DiscussionId { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }
        public long CreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual GlobalUserDAO Creator { get; set; }
        public virtual ICollection<CommentAttachmentDAO> CommentAttachments { get; set; }
        public virtual ICollection<GlobalUserCommentMappingDAO> GlobalUserCommentMappings { get; set; }
    }
}
