using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Newtonsoft.Json;
using Runly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Runly.Examples.Census
{
	/// <summary>
	/// Persists census place data published to the azure queue specified in the config
	/// </summary>
	public class CensusProcessor : Job<CensusProcessorConfig, QueueMessage, QueueClient, IDatabase>
	{
		readonly QueueClient queue;

		public CensusProcessor(CensusProcessorConfig config, QueueClient queue)
			: base(config)
		{
			this.queue = queue;
		}

		public override async Task InitializeAsync()
		{
			await queue.CreateIfNotExistsAsync();
		}

		public override async IAsyncEnumerable<QueueMessage> GetItemsAsync()
		{
			while (!CancellationToken.IsCancellationRequested)
			{
				foreach (var message in (await queue.ReceiveMessagesAsync(maxMessages: Math.Min(32, Config.Execution.ParallelTaskCount), cancellationToken: CancellationToken)).Value)
				{
					yield return message;
				}
			}
		}

		public override async Task<Result> ProcessAsync(QueueMessage message, QueueClient queue, IDatabase database)
		{
			var place = JsonConvert.DeserializeObject<Place>(message.MessageText);

			await queue.DeleteMessageAsync(message.MessageId, message.PopReceipt);

			await database.SavePlace(place);

			// With a real database, a different category could be used to identify
			// new places that are inserted and existing places that are updated.
			return Result.Success(place.State);
		}
	}
}
