using MongoDB.Driver;
using RssFeeder.Storage.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RssFeeder.Storage.Services
{
    public class RssFeedService : BaseService<RssFeed>
    {
        public RssFeedService(INCoVDatabaseSettings settings) : base(settings, "RssFeed")
        {
        }

        public RssFeedService(string connectionString, string databaseName) : base(connectionString, databaseName, "RssFeed")
        {
        }

        public RssFeed GetByName(string name) =>
            _collection.Find<RssFeed>(rssFeed => rssFeed.Name == name).FirstOrDefault();
    }
}
