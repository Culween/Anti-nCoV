using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace RssFeeder.Storage.Models
{
    public class RssFeed : IMongoObject
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
    }
}
