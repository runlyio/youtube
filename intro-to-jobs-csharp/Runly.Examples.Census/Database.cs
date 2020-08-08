﻿using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Runly.Examples.Census
{
	public interface IDatabase
	{
		Task SavePlace(Place place);
	}

	public class FakeDatabase : IDatabase
	{
		public static int InstanceCount = 0;

		readonly ILogger<FakeDatabase> logger;
		readonly int instanceId;

		public FakeDatabase(ILogger<FakeDatabase> logger)
		{
			instanceId = Interlocked.Increment(ref InstanceCount);
			this.logger = logger;
		}

		public Task SavePlace(Place place)
		{
			logger.LogDebug("Saving {name} with fake database #{id}", place.Name, instanceId);

			// a real database would save this data
			return Task.Delay(100);
		}
	}
}
