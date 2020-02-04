using MongoDB.Driver;
using RssFeeder.Storage.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RssFeeder.Storage.Services
{
    public class NewsService : BaseService<News>
    {
        public NewsService(INCoVDatabaseSettings settings) : base(settings, "News")
        {
        }

        public NewsService(string connectionString, string databaseName) : base(connectionString, databaseName, "News")
        {
        }

        public News GetByTitle(string title) =>
            _collection.Find<News>(rssData => rssData.Title == title).FirstOrDefault();


        public News GetByLink(string link) =>
           _collection.Find<News>(rssData => rssData.Link == link).FirstOrDefault();
    }
}
