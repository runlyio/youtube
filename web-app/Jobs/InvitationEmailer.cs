using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Runly.Examples.WebApp.Jobs
{
	public class InvitationEmailer : Job<InvitationEmailerConfig, string>
	{
		int count = 0;
		readonly IEmailService emails;

		public InvitationEmailer(InvitationEmailerConfig config, IEmailService emails)
			: base(config)
		{
			this.emails = emails;
		}

		public override IAsyncEnumerable<string> GetItemsAsync() => Config.EmailList.ToAsyncEnumerable();

		public override async Task<Result> ProcessAsync(string email)
		{
			// fake a failure for every 100th item
			int i = Interlocked.Increment(ref count);
			if (i % 100 == 0)
				return Result.Failure("Internet Down");

			// send our fake email
			await emails.SendEmail(email, "You are invited!", "Join us. We have cake.");

			return Result.Success();
		}
	}

	public class InvitationEmailerConfig : Config
	{
		public IEnumerable<string> EmailList { get; set; }
		public string EmailServiceApiKey { get; set; }
	}
}
