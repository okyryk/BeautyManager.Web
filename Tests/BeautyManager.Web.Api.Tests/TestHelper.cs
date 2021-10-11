using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BeautyManager.Web.Repository.Settings;

namespace BeautyManager.Web.Api.Tests
{
	class TestHelper
	{
		public static DatabaseSettings GetDatabaseSettings()
		{
			return new DatabaseSettings()
			{
				DatabaseType = "MongoDB",
				ConnectionString = @"mongodb://localhost:27017",
				DatabaseName = "BeautyManager"
			};
		}
	}
}
