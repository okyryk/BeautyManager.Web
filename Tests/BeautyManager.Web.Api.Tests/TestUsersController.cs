using NUnit.Framework;
using System.Threading.Tasks;

using BeautyManager.Web.Api.Controllers;
using BeautyManager.Web.Repository.MongoDB;
using BeautyManager.Web.Data;




namespace BeautyManager.Web.Api.Tests
{

	public class TestUsersController
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public async Task Test_Put()
		{
			MongoDBBaseRepository<User> _repository = new MongoDBBaseRepository<User>(TestHelper.GetDatabaseSettings());
			UsersController controller = new UsersController(_repository);
			var result = await controller.Post(new Data.User() { FirstName = "FirstName", LastName = "LastName" });
			Assert.IsNotNull(result);
		}


	}
}