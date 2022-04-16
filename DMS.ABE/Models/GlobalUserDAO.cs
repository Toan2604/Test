using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class GlobalUserDAO
    {
        public GlobalUserDAO()
        {
            Comments = new HashSet<CommentDAO>();
            ConversationMessages = new HashSet<ConversationMessageDAO>();
            ConversationParticipants = new HashSet<ConversationParticipantDAO>();
            ConversationReadHistories = new HashSet<ConversationReadHistoryDAO>();
            Conversations = new HashSet<ConversationDAO>();
            GlobalUserCommentMappings = new HashSet<GlobalUserCommentMappingDAO>();
        }

        public long Id { get; set; }
        public long GlobalUserTypeId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Avatar { get; set; }
        public Guid RowId { get; set; }

        public virtual GlobalUserTypeDAO GlobalUserType { get; set; }
        public virtual ICollection<CommentDAO> Comments { get; set; }
        public virtual ICollection<ConversationMessageDAO> ConversationMessages { get; set; }
        public virtual ICollection<ConversationParticipantDAO> ConversationParticipants { get; set; }
        public virtual ICollection<ConversationReadHistoryDAO> ConversationReadHistories { get; set; }
        public virtual ICollection<ConversationDAO> Conversations { get; set; }
        public virtual ICollection<GlobalUserCommentMappingDAO> GlobalUserCommentMappings { get; set; }
    }
}
