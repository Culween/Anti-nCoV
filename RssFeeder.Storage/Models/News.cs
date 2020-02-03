using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace RssFeeder.Storage.Models
{
    public class News : IMongoObject
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Source { get; set; }
        public string Publisher { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Link { get; set; }
        public DateTime PublishDate { get; set; }
        public NewsStatus Status { get; set; }
    }

    public enum NewsStatus
    {
        /// <summary>
        /// 未确认的
        /// </summary>
        Unconfirmed,
        /// <summary>
        /// 已确认的
        /// </summary>
        Confirmed,
        /// <summary>
        /// 已录入
        /// </summary>
        Recorded,
        /// <summary>
        /// 不需要的
        /// </summary>
        Ignored,
    }
}
