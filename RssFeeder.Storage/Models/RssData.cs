using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace RssFeeder.Storage.Models
{
    public class RssData : IMongoObject
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string RssFeedName { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Link { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
