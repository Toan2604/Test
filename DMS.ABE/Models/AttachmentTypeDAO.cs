using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class AttachmentTypeDAO
    {
        public AttachmentTypeDAO()
        {
            CommentAttachments = new HashSet<CommentAttachmentDAO>();
            ConversationAttachments = new HashSet<ConversationAttachmentDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<CommentAttachmentDAO> CommentAttachments { get; set; }
        public virtual ICollection<ConversationAttachmentDAO> ConversationAttachments { get; set; }
    }
}
