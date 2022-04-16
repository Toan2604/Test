using DMS.ABE.Common; using TrueSight.Common;
using System;

namespace DMS.ABE.Entities
{
    public class UserNotification : DataEntity
    {
        public long Id { get; set; }
        public long SiteId
        {
            get
            {
                return 2;
            }
        }
        public string TitleWeb { get; set; }
        public string ContentWeb { get; set; }
        public string TitleMobile
        {
            get
            {
                var pageDoc = new HtmlAgilityPack.HtmlDocument();
                pageDoc.LoadHtml(TitleWeb);
                return pageDoc.DocumentNode.InnerText;
            }
        }
        public string ContentMobile
        {
            get
            {
                var pageDoc = new HtmlAgilityPack.HtmlDocument();
                pageDoc.LoadHtml(ContentWeb);
                return pageDoc.DocumentNode.InnerText;
            }
        }
        public long SenderId { get; set; }
        public long RecipientId { get; set; }
        public bool Unread { get; set; }
        public string LinkWebsite { get; set; }
        public string LinkMobile { get; set; }
        public DateTime Time { get; set; }
        public Guid RowId { get; set; }
    }
}
