using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RssFeeder.Storage.Models
{
    public class NCoVDatabaseSettings: INCoVDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface INCoVDatabaseSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
