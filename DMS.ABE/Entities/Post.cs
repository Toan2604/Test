using TrueSight.Common;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.ABE.Entities
{
    public class Post : DataEntity
    {
        public long Id { get; set; }
        public Guid DiscussionId { get; set; }
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

        public AppUser Creator { get; set; }
        public List<Comment> Comments { get; set; }
        public List<AppUserPostMapping> AppUserPostMappings { get; set; }
    }

    public class PostFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
    }
}
