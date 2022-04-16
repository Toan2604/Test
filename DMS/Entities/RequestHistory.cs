using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    [BsonIgnoreExtraElements]
    public class OldRequestHistory<T> : DataEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public long RequestId { get; set; }
        public string ActionName { get; set; }
        public DateTime SavedAt { get; set; }
        public T OldContent { get; set; }
        public T Content { get; set; }
        public Dictionary<string, object> DisplayFields { get; set; }
        public long? AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class RequestHistory<T> : DataEntity
    {
        [BsonId]
        public Guid Id { get; set; }
        public long RequestId { get; set; }
        public string ActionName { get; set; }
        public DateTime SavedAt { get; set; }
        public T OldContent { get; set; }
        public T Content { get; set; }
        public Dictionary<string, object> DisplayFields { get; set; }
        public long? AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }

    public class RequestHistoryFilter : FilterEntity
    {
        public long RequestId { get; set; }
    }
}
