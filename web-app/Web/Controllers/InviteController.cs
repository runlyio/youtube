using Runly.Examples.WebApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Runly.Examples.WebApp.Web.Config;
using Runly.Examples.WebApp.Jobs;

namespace Runly.Examples.WebApp.Web.Controllers
{
	public class InviteController : Controller
	{
		readonly RunlyOptions opts;
		readonly IRunClient runs;

		public InviteController(IOptionsSnapshot<RunlyOptions> opts, IRunClient runs)
		{
			this.opts = opts.Value;
			this.runs = runs;
		}

		public IActionResult Index() => View();

		[HttpPost]
		public async Task<IActionResult> InviteUsers(InvitationModel data)
		{
			Guid runId = await SendInvitations(data.EmailList);

			return View("Results", new RunResultsModel
			{
				RunId = runId
			});
		}

		public async Task<Guid> SendInvitations(IEnumerable<string> emailList)
		{
			var run = await runs.Enqueue<InvitationEmailer, InvitationEmailerConfig>(
				opts.Org,
				opts.Env,
				new InvitationEmailerConfig
				{
					EmailList = emailList,
					Execution = new ExecutionConfig
					{
						// let's send 50 emails at a time
						ParallelTaskCount = 50,

						// don't stop the job unless we get over 100 failed items
						ItemFailureCountToStopJob = 100
					}
				}
			);

			return run.Id;
		}
	}
}
