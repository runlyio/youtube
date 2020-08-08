using CsvHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runly.Examples.Census
{
	/// <summary>
	/// Gets census data and publishes it to a message queue.
	/// </summary>
	/// <remarks>
	/// Uses the OSS library CsvHelper. https://joshclose.github.io/CsvHelper/
	/// </remarks>
	public class CensusPublisher : Job<CensusPublisherConfig, Place, IQueue>
	{
		readonly IDownloader downloader;
		readonly IQueue queue;
		CsvReader csv;

		public CensusPublisher(CensusPublisherConfig config, IDownloader downloader, IQueue queue)
			: base(config)
		{
			this.downloader = downloader;
			this.queue = queue;
		}

		public override async Task InitializeAsync()
		{
			await queue.CreateIfNotExistsAsync();

			var data = await downloader.Download(Config.FileUrl);

			csv = new CsvReader(new StreamReader(data, Encoding.UTF8), CultureInfo.InvariantCulture);
			csv.Configuration.Delimiter = "|";
		}

		public override IAsyncEnumerable<Place> GetItemsAsync() => csv.GetRecordsAsync<Place>();

		public override async Task<Result> ProcessAsync(Place place, IQueue queue)
		{
			// Fix errors in the file
			place.Name = place.Name.Replace('�', 'n');
			place.County = place.County.Replace('�', 'n');

			// If the config has a state filter, use it!
			if (Config.States == null || Config.States.Length == 0 || Config.States.Contains(place.State, StringComparer.InvariantCultureIgnoreCase))
			{
				var message = JsonConvert.SerializeObject(place);

				await queue.SendMessageAsync(message);

				return Result.Success(place.State);
			}
			else
			{
				return Result.Success("Skipped");
			}
		}

		public override Task<object> FinalizeAsync(Disposition disposition)
		{
			csv?.Dispose();

			return base.FinalizeAsync(disposition);
		}
	}
}
