using System;
using System.Collections.Generic;
using System.Text;

namespace RssFeeder.Storage.Models
{
    public interface IMongoObject
    {
        string Id { get; set; }
    }
}
