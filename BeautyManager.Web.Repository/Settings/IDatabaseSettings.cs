using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace BeautyManager.Web.Repository.Settings
{
    public interface IDatabaseSettings
    {
        /// <summary>
        /// MongoDB, ...
        /// </summary>
        string DatabaseType { get; set; }
        string DatabaseName { get; set; }
        string ConnectionString { get; set; }
    }
}