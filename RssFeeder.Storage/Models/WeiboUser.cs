using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace RssFeeder.Storage.Models
{
    public class WeiboUser : IMongoObject
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public string Province { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public string Followers { get; set; }

        public string PublishCount { get; set; }

        public string Summary { get; set; }

        public string Tags { get; set; }

        public DateTime UpdateTime { get; set; }

        public string UserId { get; set; }

        public string UserIdUrl { get; set; }

        public string RssUrl { get; set; }
    }
}
