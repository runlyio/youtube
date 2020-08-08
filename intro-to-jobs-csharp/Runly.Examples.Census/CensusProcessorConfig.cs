namespace Runly.Examples.Census
{
	public class CensusProcessorConfig : Config, IQueueConfig
	{
		/// <summary>
		/// Connection string to the queue.
		/// </summary>
		public string ConnectionString { get; set; } 

		/// <summary>
		/// Name of the queue to publish messages to.
		/// </summary>
		public string QueueName { get; set; } = "census-demo";
	}
}
