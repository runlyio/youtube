namespace Runly.Examples.Census
{
	public class CensusPublisherConfig : Config, IQueueConfig
	{
		/// <summary>
		/// Connection string to the queue.
		/// </summary>
		public string ConnectionString { get; set; } 
		
		/// <summary>
		/// Name of the queue to publish messages to.
		/// </summary>
		public string QueueName { get; set; } = "census-demo";
		
		/// <summary>
		/// URL of the census place file to download.
		/// </summary>
		public string FileUrl { get; set; } = "http://www2.census.gov/geo/docs/reference/codes/files/national_places.txt";
		
		/// <summary>
		/// Optional. A list of states to limit the CensusPublisher to. 
		/// </summary>
		public string[] States { get; set; }
	}
}
