using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Scrum.Reports
{
	public class IterationPlanningResultsReport : BaseReport
	{
		public string IterationName { get; set; }

		public decimal TeamCapacity { get; set; }

		public int UserStoriesNumber { get; set; }

		public decimal UserStoriesPrice { get; set; }

		public decimal UserStoriesPlannedTime { get; set; }

		public decimal UserStoriesAveragePrice
		{
			get { return UserStoriesNumber == 0 ? 0 : UserStoriesPrice/UserStoriesNumber; }
		}

		public int DefectsNumber { get; set; }

		public decimal DefectsPrice { get; set; }

		public decimal DefectsPlannedTime { get; set; }

		public decimal? PlannedVelocity { get; set; }

		public decimal VelocityLoad
		{
			get { return PlannedVelocity == null || PlannedVelocity.Value == 0 ? 0 : ( UserStoriesPrice + DefectsPrice) / PlannedVelocity.Value; }
		}

		public decimal TeamLoad { get; set; }
	}
}
