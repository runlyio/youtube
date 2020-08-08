namespace Runly.Examples.Census
{
	public interface IQueueConfig
	{
		public string ConnectionString { get; set; } 
		public string QueueName { get; set; }
	}
}
