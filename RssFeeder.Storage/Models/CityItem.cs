using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace RssFeeder.Storage.Models
{
    public class CityItem : IMongoObject
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Nation { get; set; }
        public string Province { get; set; }
        public string Code { get; set; }
        public string City { get; set; }
        public List<CountryItem> Countries { get; set; }
    }

    public class CountryItem
    {
        public string Code { get; set; }

        public string Country { get; set; }
    }
}
