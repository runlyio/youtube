using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Runly.Examples.WebApp.Web.Config;

namespace Runly.Examples.WebApp.Web
{
	public class Startup
	{
		readonly IConfiguration cfg;
		readonly IWebHostEnvironment env;

		public Startup(IConfiguration cfg, IWebHostEnvironment env)
		{
			this.cfg = cfg;
			this.env = env;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews();

			// Register the config options classes.
			// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options

			services.Configure<RunlyOptions>(cfg.GetSection("runly"));

			// Configure Runly API services to be available.
			services.AddRunlyApi(s =>
			{
				// pull the API key from our configuration file via the RunlyOptions class that we registered above
				var opts = s.GetRequiredService<IOptions<RunlyOptions>>().Value;
				return opts.SecretKey;
			});
		}

		public void Configure(IApplicationBuilder app)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Invite}/{action=Index}/{id?}");
			});
		}
	}
}
