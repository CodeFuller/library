using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CF.Patterns;

namespace CF.Scrum.Objects
{
	public class UserStory : WorkUnit
	{
		private AsyncLazy<List<Defect>> defects;

		public UserStory(string id)
			: base(id)
		{
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

		/// <summary>
		/// Returns all User Story tasks, including tasks for assigned defects
		/// </summary>
		public override async Task<IEnumerable<WorkTask>> GetAllTasksAsync()
		{
			await Task.WhenAll(GetDirectTasksAsync(), GetDefectsAsync());

			return GetDirectTasks().Concat(GetDefects().SelectMany(defect => defect.GetDirectTasks()));
		}

		public override IEnumerable<WorkTask> GetAllTasks()
		{
			return GetAllTasksAsync().Result;
		}

		public decimal GetPersonPlannedTime(Person person)
		{
			
			throw new NotImplementedException();
		}
	}
}
