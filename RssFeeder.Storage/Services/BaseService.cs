using MongoDB.Driver;
using RssFeeder.Storage.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RssFeeder.Storage.Services
{
    public class BaseService<T> where T : IMongoObject
    {
        protected readonly IMongoCollection<T> _collection;

        protected BaseService(INCoVDatabaseSettings settings, string collection)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _collection = database.GetCollection<T>(collection);
        }

        protected BaseService(string connectionString, string databaseName, string collection)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            _collection = database.GetCollection<T>(collection);
        }

        public List<T> Get() =>
            _collection.Find(obj => true).ToList();

        public T Get(string id) =>
            _collection.Find<T>(obj => obj.Id == id).FirstOrDefault();

        public T Create(T obj)
        {
            _collection.InsertOne(obj);
            return obj;
        }

        public void Update(string id, T objIn) =>
            _collection.ReplaceOne(obj => obj.Id == id, objIn);

        public void Remove(T objIn) =>
            _collection.DeleteOne(obj => obj.Id == objIn.Id);

        public void Remove(string id) =>
            _collection.DeleteOne(obj => obj.Id == id);
    }
}
