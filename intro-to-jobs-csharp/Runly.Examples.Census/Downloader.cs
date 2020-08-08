using Runly;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Runly.Examples.Census
{
	public interface IDownloader
	{
		Task<Stream> Download(string url);
	}

	public class HttpDownloader : IDownloader
	{
		readonly HttpClient client;

		public HttpDownloader(HttpClient client)
		{
			this.client = client;
		}

		public async Task<Stream> Download(string url)
		{
			var response = await client.GetAsync(url);
			await response.EnsureSuccess();

			return await response.Content.ReadAsStreamAsync();
		}
	}
}
