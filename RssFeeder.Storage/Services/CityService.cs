using MongoDB.Driver;
using RssFeeder.Storage.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RssFeeder.Storage.Services
{
    public class CityService : BaseService<CityItem>
    {
        public CityService(INCoVDatabaseSettings settings) : base(settings, "City")
        {
        }

        public CityService(string connectionString, string databaseName):base(connectionString, databaseName, "City")
        {
        }

        public CityItem GetByCityName(string name) =>
            _collection.Find<CityItem>(city => city.City == name).FirstOrDefault();

        public CityItem GetLikeCityName(string name) =>
            _collection.Find<CityItem>(city => city.City.Contains(name)).FirstOrDefault();
    }
}
