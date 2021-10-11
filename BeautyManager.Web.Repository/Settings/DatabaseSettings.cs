using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeautyManager.Web.Repository.Settings
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string DatabaseType { get; set; }
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
		
    }
}