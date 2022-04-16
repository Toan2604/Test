using TrueSight.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Rpc.discussion
{
    public class Discussion_MentionedDTO : DataDTO
    {
        public string Avatar { get; set; }
        public string GlobalUserName { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class Discussion_MentionedFilterDTO : FilterDTO
    {
        public long? GlobalUserId { get; set; }
    }
}
