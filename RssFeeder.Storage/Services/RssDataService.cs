using MongoDB.Driver;
using RssFeeder.Storage.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RssFeeder.Storage.Services
{
    public class RssDataService : BaseService<RssData>
    {
        public RssDataService(INCoVDatabaseSettings settings) : base(settings, "RssData")
        {
        }

        public RssDataService(string connectionString, string databaseName) : base(connectionString, databaseName, "RssData")
        {
        }

        public RssData GetByTitle(string title) =>
            _collection.Find<RssData>(rssData => rssData.Title == title).FirstOrDefault();
    }
}
