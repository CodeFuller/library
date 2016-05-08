using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CF.Patterns;

namespace CF.Scrum.Objects
{
	/// <summary>
	/// Items that satisfy 2 conditions:
	///		1. It consists of plain tasks
	///		2. It could be accepted
	/// Basically for this moment only 2 items fall into this category: Plain User Stories (not container US) and Defects
	/// </summary>
	public abstract class WorkUnit : WorkItem
	{
		public enum ScheduleState
		{
			Unknown,
			Idea,
			Defined,
			InProgress,
			Completed,
			Accepted
		}

		public AsyncLazy<Iteration> iteration;

		public AsyncLazy<List<WorkTask>> tasks;

		public ScheduleState State { get; set; }

		public decimal StoryPoints { get; set; }

		public decimal TasksTotalTime { get; set; }
		public decimal TasksRemainingTime { get; set; }

		public bool IsPriced
		{ get { return StoryPoints > 0; } }

		protected WorkUnit(string id)
			: base(id)
		{
		}

		public async Task<Iteration> GetIterationAsync()
		{
			return await iteration;
		}

		public Iteration GetIteration()
		{
			return GetIterationAsync().Result;
		}

		/// <summary>
		/// Instant iteration initializer
		/// </summary>
		public void SetIteration(Iteration iteration)
		{
			this.iteration = new AsyncLazy<Iteration>(() => iteration);
		}

		/// <summary>
		/// Lazy iteration initializer
		/// </summary>
		public void SetIteration(Func<Task<Iteration>> valueFactory)
		{
			iteration = new AsyncLazy<Iteration>(valueFactory);
		}

		/// <summary>
		/// Returns only direct Work Unit tasks, not including tasks for child work units
		/// If User Story has assigned Defects, then Defect tasks will not be ruturned by this method
		/// </summary>
		public async Task<List<WorkTask>> GetDirectTasksAsync()
		{
			return await tasks;
		}

		public List<WorkTask> GetDirectTasks()
		{
			return GetDirectTasksAsync().Result;
		}

		/// <summary>
		/// Returns all Work Unit tasks, including tasks for child work units
		/// If User Story has assigned Defects, then Defect tasks will be ruturned by this method
		/// </summary>
		public abstract Task<IEnumerable<WorkTask>> GetAllTasksAsync();

		public abstract IEnumerable<WorkTask> GetAllTasks();

		/// <summary>
		/// Instant tasks initializer
		/// </summary>
		public void SetTasks(List<WorkTask> tasks)
		{
			this.tasks = new AsyncLazy<List<WorkTask>>(() => tasks);
		}

		/// <summary>
		/// Lazy capacities initializer
		/// </summary>
		public void SetTasks(Func<Task<List<WorkTask>>> valueFactory)
		{
			tasks = new AsyncLazy<List<WorkTask>>(valueFactory);
		}
	}
}
