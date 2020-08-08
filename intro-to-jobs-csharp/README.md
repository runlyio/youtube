# Intro to Jobs - C#

This is the code used in the YouTube tutorial [Intro to Runly Jobs in C#](https://youtu.be/42lsOv_CMU4).

## Running Locally

This project builds as a .NET Core console application. It contains two Runly jobs that can be run from the CLI. For a full list of available commands, from the Runly.Examples.Census directory run:

```
dotnet run
```

### Jobs

* [`CensusPublisher`](#censuspublisher)
* [`CensusProcessor`](#censusprocessor)

#### `CensusPublisher`

This job imports national places from a publicly available US Census CSV file and publishes a message to an Azure storage queue for each record. This job demonstrates processing a CSV file in parallel and doing something with the parsed data.

Create an Azure storage queue and copy the connection string, then try running the job from the command line:

```
dotnet run -- CensusPublisher --ConnectionString "<AzureStorageQueueConnectionString>" --QueueName <AzureStorageQueueName>
```

Try increasing the parallel task count to speed it up or filtering states to reduce the messages pushed to the queue:

```
dotnet run -- CensusPublisher --ConnectionString "<AzureStorageQueueConnectionString>" --QueueName <AzureStorageQueueName> --Execution.ParallelTaskCount 50 --States HI WA NC
```

#### `CensusProcessor`

This job processes messages from the CensusPublisher by dequeuing them from an Azure storage queue. This job mimicks a database call, showing how to process messages from a queue and perform an action for each message. 

Try running the job from the command line:

```
dotnet run -- CensusProcessor --ConnectionString "<AzureStorageQueueConnectionString>" --QueueName <AzureStorageQueueName> --Execution.ParallelTaskCount 50
```

## Running on Runly

See the [QuickStart Guide](https://www.runly.io/docs/getting-started/) to package this application up and run the jobs via [Runly](https://www.runly.io/).
