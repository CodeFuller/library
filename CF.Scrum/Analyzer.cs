using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CF.Core;
using CF.Scrum.Exceptions;
using CF.Scrum.Objects;
using CF.Scrum.Metrics;
using CF.Extensions;
using CF.Scrum.Reports;

namespace CF.Scrum
{
	public static class Analyzer
	{
		public static readonly decimal IdealDayDuration = (decimal)8.0;

		public static int MetricsPrecision = 4;

		static decimal ConvertTimeToIdealDays(decimal time)
		{
			return time / IdealDayDuration;
		}

		/// <remarks>
		/// Result includes also aliens
		/// </remarks>
		public static async Task<IEnumerable<GenericPersonMetric<SpocFactors>>> CalculatePersonalSpocFactors(this Iteration iteration, IDataProvider provider)
		{
			//	Current SPOC factors:
			//	1.	Estimation errors
			//	2.	Not accepted work
			//	3.	Not reported time
			//	4.	Not priced work

			Logger.Info("Analyzing iteration {0}...", iteration);

			var workUnits = await iteration.GetAcceptableWorkUnitsAsync();
			//	Populating work units status at iteration end
			await provider.GetHistoricalDataProvider().SetSnapshotStateAsync(workUnits, iteration.GetAcceptanceEndTime());

			var factors = new Dictionary<Person, SpocFactors>();

			//	1.	Estimation errors
			var estimationErrors = await iteration.CalculatePersonalEstimationErrorByUnitsAsync(provider);
			foreach (var estimationError in estimationErrors.Values.SelectMany(list => list))
			{
				factors.FillDefaultValue(estimationError.Person);
				factors[estimationError.Person].AddEstimationError(estimationError.Value);
			}

			//	2. Not accepted work
			foreach (var workUnit in workUnits.Where(unit => unit.IsPriced && unit.State != WorkUnit.ScheduleState.Accepted))
			{
				Logger.Debug("Not accepted unit: {0}", workUnit);
				foreach (var task in await workUnit.GetAllTasksAsync())
				{
					factors.FillDefaultValue(task.GetOwner());
					factors[task.GetOwner()].AddNotAcceptedWork(task.SpentTime);
				}
			}

			//	3.	Not reported time
			var spentTimes = new Dictionary<Person, decimal>();
			foreach (var task in await iteration.GetTasksAsync())
			{
				if (!spentTimes.ContainsKey(task.GetOwner()))
				{
					spentTimes.Add(task.GetOwner(), 0);
				}
				spentTimes[task.GetOwner()] += task.SpentTime;
			}
			foreach (var capacity in await iteration.GetCapacitiesAsync())
			{
				decimal spentTime;
				spentTimes.TryGetValue(capacity.Key, out spentTime);
				factors.FillDefaultValue(capacity.Key);
				factors[capacity.Key].AddNotReportedTime(capacity.Value - spentTime);
			}

			//	4.	Not priced work
			foreach (var workUnit in workUnits.Where(unit => !unit.IsPriced))
			{
				if (workUnit.State != WorkUnit.ScheduleState.Accepted)
				{
					Logger.Warning(String.Format("Unaccepted priceless unit: {0}", workUnit));
				}

				foreach (var task in await workUnit.GetAllTasksAsync())
				{
					factors.FillDefaultValue(task.GetOwner());
					factors[task.GetOwner()].AddNotPricedWork(task.SpentTime);
				}
			}

			//	Filling capacity
			foreach (var capacity in await iteration.GetCapacitiesAsync())
			{
				factors.FillDefaultValue(capacity.Key);
				factors[capacity.Key].AddCapacity(capacity.Value);
			}

			//	Filling accepted points
			foreach (var workUnit in workUnits.Accepted())
			{
				foreach (var pair in workUnit.CalculatePersonalAcceptedPoints(estimationErrors[workUnit]))
				{
					factors.FillDefaultValue(pair.Key);
					factors[pair.Key].AddAcceptedStoryPoints(pair.Value);
				}
			}

			return factors.Select(pair => new GenericPersonMetric<SpocFactors>(pair.Key, pair.Value));
		}

		public static SpocFactors CalculateTeamSpocFactors(this Iteration iteration, IEnumerable<GenericPersonMetric<SpocFactors>> personalSpocFactors)
		{
			return personalSpocFactors.Where(x => iteration.IsTeamMember(x.Person)).Select(x => x.Value).Sum();
		}

		public static async Task<List<MetricsReport>> AnalyzeTeam(IDataProvider provider, string teamName, DateTime start, DateTime end)
		{
			Logger.Info("Analyzing {0} for period {1} - {2}...", teamName, start, end);

			var team = await provider.GetTeamAsync(teamName);

			var iterations = await provider.GetTeamIterationsAsync(team);

			var iterationMetrics = new List<MetricsReport>();
			foreach (var iteration in iterations.Where(it => it.HitsTimeInterval(start, end)).OrderBy(it => it.StartTime))
			{
				Logger.Info("Analyzing {0}...", iteration);

				var currIterationMetrics = new MetricsReport(iteration.Name);

				var iterationResults = await iteration.CalculatePersonalSpocFactors(provider);
				foreach (var personalResult in iterationResults)
				{
					var metrics = new IterationMetrics(personalResult.Value);
					if (iteration.IsTeamMember(personalResult.Person))
					{
						currIterationMetrics.AddPersonalMetrics(personalResult.Person, metrics);
					}
					else
					{
						currIterationMetrics.AddAlienMetrics(metrics);
					}
				}

				iterationMetrics.Add(currIterationMetrics);
			}

			Logger.Info("Finished team analysis");

			return iterationMetrics;
		}
	}
}
