
using CommandQuery;
using CommandQuery.AspNetCore;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Recruitment.Api.Helpers;
using Recruitment.Contracts.Handlers;
using Recruitment.Contracts.Models;

namespace Recruitment.Api
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{

			services.AddControllers();

			services.AddHealthChecks().AddCheck<MemoryHealthCheck>("Memory");
			services.AddHealthChecksUI(opt =>
			{
				opt.SetEvaluationTimeInSeconds(15); //time in seconds between check
			}).AddInMemoryStorage();


			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Recruitment.Api", Version = "v1" });
			});

			services.AddCommandControllers(typeof(CalculateHashCommandHandler).Assembly);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Recruitment.Api v1"));
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapHealthChecks("/hc", new HealthCheckOptions
				{
					Predicate = _ => true,
					ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
				});

				endpoints.MapHealthChecksUI(setup =>
				{
					setup.UIPath = "/hc-ui";
					setup.ApiPath = "/hc-json";
				});

				endpoints.MapControllers();
			});

			// Validation
			app.ApplicationServices.GetService<ICommandProcessor>().AssertConfigurationIsValid();
			//app.ApplicationServices.GetService<IQueryProcessor>().AssertConfigurationIsValid();
		}
	}
}
