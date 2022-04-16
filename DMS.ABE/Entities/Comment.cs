using TrueSight.Common;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.ABE.Entities
{
    public class Comment : DataEntity
    {
        public long Id { get; set; }
        public Guid DiscussionId { get; set; }
        public bool IsOwner { get; set; }
        public string Content { get; set; }
        public string MobileContent
        {
            get
            {
                try
                {
                    var pageDoc = new HtmlDocument();
                    pageDoc.LoadHtml(Content);
                    return pageDoc.DocumentNode.InnerText;
                }
                catch
                {
                    return Content;
                }
            }
        }
        public string Url { get; set; }
        public long CreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual GlobalUser Creator { get; set; }
        public List<GlobalUserCommentMapping> GlobalUserCommentMappings { get; set; }
        public List<CommentAttachment> CommentAttachments { get; set; }
    }

    public class CommentFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
    }
}
