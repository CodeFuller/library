using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CF.Core;
using CF.Core.Exceptions;
using CF.Extensions;
using CF.Patterns;

namespace CF.Scrum.Objects
{
	public class Iteration : BaseObject
	{
		private AsyncLazy<Dictionary<Person, decimal>> capacities;

		private AsyncLazy<List<UserStory>> userStories;
		private AsyncLazy<List<Defect>> defects;

		public string Name { get; set; }

		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }

		public bool HitsTimeInterval(DateTime start, DateTime end)
		{
			//	http://stackoverflow.com/questions/13513932/algorithm-to-detect-overlapping-periods
			return StartTime <= end && start <= EndTime;
		}

		public decimal? PlannedVelocity { get; set; }

		public Iteration(string id) : base(id)
		{
		}

		public override string ToString()
		{
			return Name;
		}

		public async Task<decimal> CapacityAsync()
		{
			return (await GetCapacitiesAsync()).Sum(x => x.Value);
		}

		public decimal Capacity()
		{
			return CapacityAsync().Result;
		}

		public decimal Capacity(Person person)
		{
			return GetCapacitiesAsync().Result[person];
		}

		public async Task<IEnumerable<Person>> Workers()
		{
			return (await GetCapacitiesAsync()).Keys;
		}

		public bool IsTeamMember(Person worker)
		{
			return Workers().Result.Contains(worker);
		}

		/// <summary>
		/// Instant capacities initializer
		/// </summary>
		public void SetCapacities(Dictionary<Person, decimal> capacities)
		{
			this.capacities = new AsyncLazy<Dictionary<Person, decimal>>(() => capacities);
		}

		/// <summary>
		/// Lazy capacities initializer
		/// </summary>
		public void SetCapacities(Func<Task<Dictionary<Person, decimal>>> valueFactory)
		{
			capacities = new AsyncLazy<Dictionary<Person, decimal>>(valueFactory);
		}

		public async Task<Dictionary<Person, decimal>> GetCapacitiesAsync()
		{
			return await capacities;
		}

		public Dictionary<Person, decimal> GetCapacities()
		{
			return GetCapacitiesAsync().Result;
		}

		public async Task<List<UserStory>> GetUserStoriesAsync()
		{
			return await userStories;
		}

		public List<UserStory> GetUserStories()
		{
			return GetUserStoriesAsync().Result;
		}

		/// <summary>
		/// Instant user stories initializer
		/// </summary>
		public void SetUserStories(List<UserStory> userStories)
		{
			this.userStories = new AsyncLazy<List<UserStory>>(() => userStories);
		}

		/// <summary>
		/// Lazy user stories initializer
		/// </summary>
		public void SetUserStories(Func<Task<List<UserStory>>> valueFactory)
		{
			userStories = new AsyncLazy<List<UserStory>>(valueFactory);
		}

		public async Task<List<Defect>> GetDefectsAsync()
		{
			return await defects;
		}

		public List<Defect> GetDefects()
		{
			return GetDefectsAsync().Result;
		}

		/// <summary>
		/// Instant defects initializer
		/// </summary>
		public void SetDefects(List<Defect> defects)
		{
			this.defects = new AsyncLazy<List<Defect>>(() => defects);
		}

		/// <summary>
		/// Lazy user stories initializer
		/// </summary>
		public void SetDefects(Func<Task<List<Defect>>> valueFactory)
		{
			defects = new AsyncLazy<List<Defect>>(valueFactory);
		}

		/// <returns>
		/// User Stories and separate defects. Defects assigned to other User Stories from the iteration are skipped
		/// </returns>
		public async Task<List<WorkUnit>> GetAcceptableWorkUnitsAsync()
		{
			await Task.WhenAll(GetUserStoriesAsync(), GetDefectsAsync());

			//	Bunch load of user storis defects
			//	This is only required for paranoic check inside below loop
			await Task.WhenAll(GetUserStories().Select(us => us.GetDefectsAsync()));

			var results = new List<WorkUnit>(GetUserStories().ToList());

			foreach (var defect in GetDefects())
			{
				bool belongsToUserStory = (await GetUserStoriesAsync()).Any(us => us.GetDefects().Contains(defect));

				if (!belongsToUserStory)
				{
					results.Add(defect);
				}
				else if (defect.IsPriced)
				{
					//	Paranoic check that this priced defect isn't assigned to some other User Story from the iteration
					Logger.Warning("[DataError] Defect {0} is priced and is assigned to other User Story from current iteration", defect);
					//	CF 2DO: Fill data inconsistency warning
				}
			}

			return results;
		}

		/// <returns>
		/// User Stories and standalone defects. Defects assigned to other User Stories from the iteration are skipped
		/// </returns>
		public List<WorkUnit> GetAcceptableWorkUnits()
		{
			return GetAcceptableWorkUnitsAsync().Result;
		}

		/// <returns>
		/// User Stories and all defects, including the ones assigned to other User Stories
		/// </returns>
		/// <remarks>
		/// Be cautious not to call GetAllTaks() for units returned by this method, some tasks will be processed twice in this case
		/// </remarks>
		public async Task<IEnumerable<WorkUnit>> GetAllWorkUnitsAsync()
		{
			await Task.WhenAll(GetUserStoriesAsync(), GetDefectsAsync());

			return GetUserStories().Concat<WorkUnit>(GetDefects());
		}

		public IEnumerable<WorkUnit> GetAllWorkUnits()
		{
			return GetAllWorkUnitsAsync().Result;
		}

		public async Task<IEnumerable<WorkTask>> GetTasksAsync()
		{
			//	Loading all units
			var workUnits = await GetAllWorkUnitsAsync();
			
			//	Loading all tasks
			await Task.WhenAll(workUnits.Select(unit => unit.GetDirectTasksAsync()));

			return workUnits.SelectMany(unit => unit.GetDirectTasks());
		}

		/// <summary>
		/// The difference fromm GetTasksAsync() is that aliens tasks are filtered out
		/// </summary>
		public async Task<IEnumerable<WorkTask>> GetTeamTasksAsync()
		{
			var capacities = await GetCapacitiesAsync();
			return (await GetTasksAsync()).Where(task => capacities.ContainsKey(task.GetOwner()));
		}
	}
}
