using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class GlobalUserCommentMappingDAO
    {
        public long GlobalUserId { get; set; }
        public long CommentId { get; set; }

        public virtual CommentDAO Comment { get; set; }
        public virtual GlobalUserDAO GlobalUser { get; set; }
    }
}
