using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeautyManager.Web.Api.Models;
using Microsoft.OData.Edm;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;

using BeautyManager.Web.Data;
using BeautyManager.Web.Repository.Settings;

using BeautyManager.Web.Repository;
using BeautyManager.Web.Repository.MongoDB;

namespace BeautyManager.Web.Api
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<DatabaseSettings>(Configuration.GetSection("DatabaseSettings"));

			services.AddSingleton<IDatabaseSettings>(serviceProvider => serviceProvider.GetRequiredService<IOptions<DatabaseSettings>>().Value);

			services.AddScoped(typeof(IRepository<User>), typeof(MongoDBBaseRepository<User>));

			services.AddDistributedMemoryCache();

			services.AddDbContext<BeautyManagerDbContext>(option => option.UseInMemoryDatabase("BeautyManagerContext"));

			IEdmModel model0 =  EdmModelBuilder.GetEdmModel();

			services.AddControllers()
				.AddOData(opt => opt.Count().Filter().Select().OrderBy().SetMaxTop(5)
				.AddRouteComponents("odata",model0));

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "BeautyManager.Web.Api", Version = "v1" });
			});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BeautyManager.Web.Api v1, OData 8.x OpenAPI"));
			}

			app.UseODataRouteDebug();

			//app.UseODataOpenApi();

			app.UseODataQueryRequest();

			app.UseODataBatching();

			//app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
