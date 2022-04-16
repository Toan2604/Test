using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class ConversationParticipantDAO
    {
        public long Id { get; set; }
        public long ConversationId { get; set; }
        public long GlobalUserId { get; set; }

        public virtual ConversationDAO Conversation { get; set; }
        public virtual GlobalUserDAO GlobalUser { get; set; }
    }
}
