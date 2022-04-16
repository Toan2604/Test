using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class Notification : DataEntity, IEquatable<Notification>
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public long? OrganizationId { get; set; }
        public long NotificationStatusId { get; set; }
        public NotificationStatus NotificationStatus { get; set; }
        public Organization Organization { get; set; }

        public bool Equals(Notification other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class NotificationFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Title { get; set; }
        public StringFilter Content { get; set; }
        public IdFilter OrganizationId { get; set; }
        public IdFilter NotificationStatusId { get; set; }
        public List<NotificationFilter> OrFilter { get; set; }
        public NotificationOrder OrderBy { get; set; }
        public NotificationSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum NotificationOrder
    {
        Id = 0,
        Title = 1,
        Content = 2,
        Organization = 3,
        NotificationStatus = 4,
    }

    [Flags]
    public enum NotificationSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Title = E._1,
        Content = E._2,
        Organization = E._3,
        NotificationStatus = E._4,
    }
}
