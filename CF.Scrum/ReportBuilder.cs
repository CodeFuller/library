using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CF.Scrum.Objects;
using CF.Scrum.Reports;

namespace CF.Scrum
{
	public static class ReportBuilder
	{
		public static async Task<IterationPlanningResultsReport> BuildIterationPlanningResultsReport(Iteration iteration)
		{
			return new IterationPlanningResultsReport()
			{
				IterationName = iteration.Name,

				TeamCapacity = iteration.Capacity(),

				UserStoriesNumber = iteration.GetUserStories().Count,

				UserStoriesPrice = await iteration.UserStoriesPrice(),

				UserStoriesPlannedTime = iteration.GetUserStories().Sum(us => us.TasksTotalTime),

				DefectsNumber = iteration.GetDefects().Count,

				DefectsPrice = await iteration.DefectsPrice(),

				DefectsPlannedTime = iteration.GetDefects().Sum(de => de.TasksTotalTime),

				PlannedVelocity = iteration.PlannedVelocity,

				TeamLoad = iteration.Capacity() == 0 ? 0 : iteration.GetTeamTasksAsync().Result.Sum(task => task.TotalTime) / iteration.Capacity()
			};
		}
	}
}
