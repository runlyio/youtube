using Azure.Storage.Queues;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Runly;
using System.Threading.Tasks;

namespace Runly.Examples.Census
{
	class Program
	{
		static async Task Main(string[] args)
		{
			await JobHost.CreateDefaultBuilder(args)
				.ConfigureServices((host, services) =>
				{
					// Quiet the log messages from the HttpClient
					services.AddLogging(builder => builder.SetMinimumLevel(LogLevel.Error));

					// Downloader used by the CensusPublisher
					services.AddHttpClient<IDownloader, HttpDownloader>();

					// Database used by the CensusProcessor
					services.AddScoped<IDatabase, FakeDatabase>();

					// Message queue used by both jobs
					services.AddScoped<IQueue, AzureQueue>();
					services.AddScoped(s =>
					{
						var config = s.GetRequiredService<IQueueConfig>();
						return new QueueClient(config.ConnectionString, config.QueueName);
					});
				})
				.Build()
				.RunJobAsync();
		}
	}
}
