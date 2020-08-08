using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Runly.Testing;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Runly.Examples.Census.Tests
{
	public class CensusPublisherTests
	{
		readonly TestRun<CensusPublisher> testRun;
		readonly InMemoryQueue queue;

		public CensusPublisherTests()
		{
			queue = new InMemoryQueue();

			testRun = new TestHost<CensusPublisher>(new CensusPublisherConfig())
				.ConfigureServices(services =>
				{
					services.AddSingleton<IQueue>(queue);
					services.AddSingleton<IDownloader, TestDownloader>();
				})
				.BuildRunner();
		}

		[Fact]
		public async Task Should_publish_one_message_per_record()
		{
			await testRun.RunAsync();

			testRun.Results.SuccessfulItemCount.Should().Be(3);

			var actual = queue.InnerQueue.Select(s => JsonConvert.DeserializeObject<Place>(s));
			
			actual.Should().BeEquivalentTo(new[]
			{
				new Place() { Name = "Abanda CDP", State = "AL", County = "Chambers County", Type = "Census Designated Place"},
				new Place() { Name = "Abbeville city", State = "AL", County = "Henry County", Type = "Incorporated Place" },
				new Place() { Name = "Adamsville city", State = "AL", County = "Jefferson County", Type = "Incorporated Place" }
			});
		}
	}
}
