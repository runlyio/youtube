using Azure.Storage.Queues;
using System.Collections;
using System.Threading.Tasks;

namespace Runly.Examples.Census
{
	public interface IQueue
	{
		Task CreateIfNotExistsAsync();
		Task SendMessageAsync(string message);
	}

	public class AzureQueue : IQueue
	{
		readonly QueueClient queueClient;

		public AzureQueue(QueueClient queueClient)
		{
			this.queueClient = queueClient;
		}

		public Task CreateIfNotExistsAsync() => queueClient.CreateIfNotExistsAsync();

		public Task SendMessageAsync(string message) => queueClient.SendMessageAsync(message);
	}
}
