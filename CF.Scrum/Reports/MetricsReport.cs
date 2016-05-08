using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CF.Core;
using CF.Scrum.Metrics;
using CF.Scrum.Objects;

namespace CF.Scrum.Reports
{
	[Serializable]
	public class MetricsReport
	{
		public string Title { get; set; }

		private readonly Dictionary<ReportedPerson, IterationMetrics> personalMetrics = new Dictionary<ReportedPerson, IterationMetrics>();

		public IEnumerable<ReportedPerson> Persons
		{
			get { return personalMetrics.Keys.OrderBy(p => p.FullName); }
		}

		public MetricsReport(string title)
		{
			Title = title;
		}

		public void AddPersonalMetrics(Person person, IterationMetrics metrics)
		{
			personalMetrics.Add(new ReportedPerson(person), metrics);
		}

		public void AddAlienMetrics(IterationMetrics metrics)
		{
			personalMetrics.SafeAdd(ReportedPerson.Alien, metrics);
		}

		public bool HasPerson(ReportedPerson person)
		{
			return personalMetrics.ContainsKey(person);
		}

		public IterationMetrics GetPersonalMetrics(ReportedPerson person)
		{
			return personalMetrics[person];
		}

		public IterationMetrics GetTeamMetrics()
		{
			var teamMetrics = personalMetrics.Where(m => !m.Key.IsAlien).Select(m => m.Value).Sum();

			IterationMetrics alienMetrics;
			if (personalMetrics.TryGetValue(ReportedPerson.Alien, out alienMetrics))
			{
				//	Alien donate their Story Points to the team
				teamMetrics.SpocFactors.AddAcceptedStoryPoints(alienMetrics.AcceptedStp);
			}

			return teamMetrics;
		}
	}
}
