using MongoDB.Driver;
using RssFeeder.Storage.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RssFeeder.Storage.Services
{
    public class WeiboUserService : BaseService<WeiboUser>
    {
        public WeiboUserService(INCoVDatabaseSettings settings) : base(settings, "WeiboUser")
        {
        }

        public WeiboUserService(string connectionString, string databaseName) : base(connectionString, databaseName, "WeiboUser")
        {
        }

        public WeiboUser GetByName(string name) =>
           _collection.Find<WeiboUser>(u => u.Name == name).FirstOrDefault();
    }
}
