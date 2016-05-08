using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CF.Scrum.Exceptions;
using CF.Scrum.Objects;
using CF.Scrum.Metrics;
using CF.Core;
using CF.Extensions;

namespace CF.Scrum
{
	public static class Extensions
	{
		public static readonly TimeSpan PlanningShakeUpPeriod = TimeSpan.FromDays(2);
		public static readonly TimeSpan IterationAcceptancePeriod = TimeSpan.FromDays(5);

		public static async Task<decimal> UserStoriesPrice(this Iteration iteration)
		{
			return (await iteration.GetUserStoriesAsync()).Sum(us => us.StoryPoints);
		}

		public static async Task<decimal> DefectsPrice(this Iteration iteration)
		{
			return (await iteration.GetDefectsAsync()).Sum(de => de.StoryPoints);
		}

		public static IEnumerable<WorkUnit> Accepted(this IEnumerable<WorkUnit> units)
		{
			return units.Where(u => u.State == WorkUnit.ScheduleState.Accepted);
		}

		/// <summary>
		/// Calculates team Iteration estimation error
		/// </summary>
		public static async Task<EstimationError> CalculateEstimationErrorAsync(this Iteration iteration, IDataProvider provider)
		{
			return await iteration.CalculatePersonalEstimationErrorAsync(provider)
				.Then(result => result.Sum());
		}

		/// <summary>
		/// Calculates personal Iteration estimation errors
		/// </summary>
		public static async Task<IEnumerable<GenericPersonMetric<EstimationError>>> CalculatePersonalEstimationErrorAsync(this Iteration iteration, IDataProvider provider)
		{
			Logger.Info("Calculating estimation errors for {0}", iteration);
			return await iteration.CalculatePersonalEstimationErrorByUnitsAsync(provider)
				.Then(result =>
				{
					return result.Values.SelectMany(r => r)
								.GroupBy(err => err.Person, err => err.Value, (person, personErrors) => new GenericPersonMetric<EstimationError>(person, personErrors.Sum()));
				});
		}

		//	CF 2DO: Remove such grouppings, make Paraller processing on client side

		/// <summary>
		/// Calculates personal Iteration estimation errors for every Work Unit
		/// </summary>
		public static async Task<Dictionary<WorkUnit, List<GenericPersonMetric<EstimationError>>>> CalculatePersonalEstimationErrorByUnitsAsync(this Iteration iteration, IDataProvider provider)
		{
			Logger.Info("Calculating estimation errors, groupped by Work Units, for {0}, ", iteration);
			var workUnits = (await iteration.GetAcceptableWorkUnitsAsync()).ToList();
			Logger.Debug("Analyzing {0} work units", workUnits.Count);

			var workingTasks = workUnits.Select(unit => unit.CalculatePersonalEstimationErrorAsync(provider)).ToArray();

			return await Task.WhenAll(workingTasks)
				.Then(result =>
				{
					var results = new Dictionary<WorkUnit, List<GenericPersonMetric<EstimationError>>>();
					for (var i = 0; i < workUnits.Count; ++i)
					{
						results.FillDefaultValue(workUnits[i]);
						results[workUnits[i]] = workingTasks[i].Result.ToList();
					}
					return results;
				});
		}

		/// <summary>
		/// Calculates team WorkUnit estimation errors
		/// </summary>
		public static async Task<EstimationError> CalculateEstimationErrorAsync(this WorkUnit workUnit, IDataProvider provider)
		{
			return await workUnit.CalculatePersonalEstimationErrorAsync(provider)
				.Then(result => result.Sum());
		}

		/// <summary>
		/// Calculates personal WorkUnit estimation errors
		/// </summary>
		public static async Task<IEnumerable<GenericPersonMetric<EstimationError>>> CalculatePersonalEstimationErrorAsync(this WorkUnit workUnit, IDataProvider provider)
		{
			/*
			 *	Algorithm of personal errors calculation:
			 *
			 *	Get DateTime when userStory was assigned to iteration
			 *	Select DateTime of Planning end - max(IterationStart, UserStoryAssignmentTime) + delta
			 *	For each task from UserStory (usual tasks, US defects tasks):
			 *		Error(Owner) += (SpentTime - PlannedTime);	//	PlannedTime := 0 if task has not existed
			 *	
			 *	For each task existed when planning finished:
			 *		Skip if task was already processed (has not been deleted)
			 *		Error(Owner) -= PlannedTime;
			 */

			Logger.Info("Calculating estimation errors for {0}...", workUnit);

			var historicalProvider = provider.GetHistoricalDataProvider();

			//	Get DateTime when userStory was assigned to iteration
			var assigned = await historicalProvider.GetScopingTimeAsync(await workUnit.GetIterationAsync(), workUnit);
			//	Select DateTime of Planning end - max(IterationStart, UserStoryAssignmentTime) + delta
			var planningEnd = workUnit.GetPlanningEndTime(assigned);

			Logger.Info("Assigned to iteration on {0}. Planning finished on {1}.", assigned, planningEnd);

			Dictionary<Person, EstimationError> personalErrors = new Dictionary<Person, EstimationError>();
			var processedTasks = new HashSet<WorkTask>();

			var tasks = workUnit.GetAllTasks().ToArray();
			Logger.Debug("Analyzing current {0} tasks...", tasks.Length);

			if (tasks.Length > 0)
			{
				//	Bunch request of task times for the planning finished data
				var taskPlannedTimes = await historicalProvider.GetTaskTimesSnapshotAsync(tasks, planningEnd);

				//	For each task from UserStory (usual tasks, US defects tasks):
				for (var i = 0; i < tasks.Length; ++i)
				{
					var task = tasks[i];
					var taskOwner = task.GetOwner();
					if (!personalErrors.ContainsKey(taskOwner))
					{
						personalErrors[taskOwner] = new EstimationError();
					}

					//	Error(Owner) += (SpentTime - PlannedTime);	//	PlannedTime := 0 if task has not existed
					personalErrors[taskOwner] = personalErrors[taskOwner] + new EstimationError(taskPlannedTimes[i], task.TotalTime);

					processedTasks.Add(task);
				}
			}

			Logger.Debug("Analyzing deleted tasks...");

			//	For each task existed when planning finished:
			foreach (var task in await historicalProvider.GetTasksSnapshotAsync(workUnit, planningEnd))
			{
				//	Skip if task was already processed (has not been deleted)
				if (processedTasks.Contains(task))
				{
					continue;
				}

				var taskOwner = task.GetOwner();
				if (!personalErrors.ContainsKey(taskOwner))
				{
					personalErrors[taskOwner] = new EstimationError();
				}

				//	Error(Owner) -= PlannedTime;
				personalErrors[taskOwner] = personalErrors[taskOwner] + new EstimationError(task.TotalTime, 0);
			}

			Logger.Debug("Analyzed estimations for {0} users", personalErrors.Count);

			return personalErrors.Select(pair => new GenericPersonMetric<EstimationError>(pair.Key, pair.Value));
		}

		public static Dictionary<Person, decimal> CalculatePersonalAcceptedPoints(this WorkUnit workUnit, List<GenericPersonMetric<EstimationError>> personalErrors)
		{
			var result = new Dictionary<Person, decimal>();
			if (!workUnit.IsPriced)
			{
				return result;
			}

			var personalInput = personalErrors.ToDictionary(p => p.Person, p => Math.Min(p.Value.EstimatedTime, p.Value.SpentTime));
			decimal totalTime = personalInput.Values.Sum();

			if (totalTime > 0)
			{
				foreach (var entry in personalInput)
				{
					var currInput = workUnit.StoryPoints * personalInput[entry.Key] / totalTime;
					Logger.Debug("{0} provided {1:0.00} of {2} stp for {3}", entry.Key, currInput, workUnit.StoryPoints, workUnit);
					result.FillDefaultValue(entry.Key);
					result[entry.Key] += currInput;
				}

				Logger.Debug("Team got {0:0.00} of {1:0.00} for {2}", result.Values.Sum(), workUnit.StoryPoints, workUnit);
			}

			return result;
		}

		public static DateTime GetPlanningEndTime(this WorkUnit workUnit, DateTime userStoryAssignmentTime)
		{
			return DateTimeExtensions.Max(workUnit.GetIteration().StartTime, userStoryAssignmentTime) + PlanningShakeUpPeriod;
		}

		public static DateTime GetAcceptanceEndTime(this Iteration iteration)
		{
			return iteration.EndTime + IterationAcceptancePeriod;
		}
	}
}
