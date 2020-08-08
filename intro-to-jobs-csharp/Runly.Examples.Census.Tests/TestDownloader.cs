using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Runly.Examples.Census.Tests
{
	public class TestDownloader : IDownloader
	{
		const string file = @"STATE|STATEFP|PLACEFP|PLACENAME|TYPE|FUNCSTAT|COUNTY
AL|01|00100|Abanda CDP|Census Designated Place|S|Chambers County
AL|01|00124|Abbeville city|Incorporated Place|A|Henry County
AL|01|00460|Adamsville city|Incorporated Place|A|Jefferson County";

		public Task<Stream> Download(string url)
		{
			return Task.FromResult<Stream>(new MemoryStream(Encoding.UTF8.GetBytes(file)));
		}
	}
}
