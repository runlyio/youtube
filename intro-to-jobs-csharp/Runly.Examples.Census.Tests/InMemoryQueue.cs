using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Runly.Examples.Census.Tests
{
	public class InMemoryQueue : IQueue
	{
		public ConcurrentQueue<string> InnerQueue { get; }

		public InMemoryQueue()
		{
			InnerQueue = new ConcurrentQueue<string>();
		}

		public Task CreateIfNotExistsAsync() => Task.CompletedTask;

		public Task SendMessageAsync(string message)
		{
			InnerQueue.Enqueue(message);

			return Task.CompletedTask;
		}
	}
}
