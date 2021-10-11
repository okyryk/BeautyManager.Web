using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

using BeautyManager.Web.Data;

namespace BeautyManager.Web.Api.Models
{
	public static class EdmModelBuilder
	{
		public static IEdmModel GetEdmModel()
		{
			var odataBuilder = new ODataConventionModelBuilder();
			odataBuilder.EntitySet<User>("Users");
			odataBuilder.EntityType<User>().HasKey(s => s.UserId);
			return odataBuilder.GetEdmModel();
		}
	}

}

