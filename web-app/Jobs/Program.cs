﻿using System.Threading.Tasks;

namespace Runly.Examples.WebApp.Jobs
{
	class Program
	{
		static async Task Main(string[] args)
		{
			await JobHost.CreateDefaultBuilder(args)
				.ConfigureServices((host, services) =>
				{
					// Register the deps our job needs using the job config.
					// https://www.runly.io/docs/dependency-injection/#registering-dependencies

					services.AddScoped<IEmailService, InvitationEmailerConfig>((s, cfg) =>
						new FakeEmailService(cfg.EmailServiceApiKey)
					);
				})
				.Build()
				.RunJobAsync();
		}
	}
}
