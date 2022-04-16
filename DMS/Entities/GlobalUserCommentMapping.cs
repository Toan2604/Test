using TrueSight.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Entities
{
    public class GlobalUserCommentMapping : DataEntity
    {
        public long GlobalUserId { get; set; }
        public long CommentId { get; set; }
        public GlobalUser GlobalUser { get; set; }
        public Comment Comment { get; set; }
    }
}
