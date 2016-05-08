using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CF.Patterns;

namespace CF.Scrum.Objects
{
	public class WorkTask : WorkItem
	{
		public enum ScheduleState
		{
			Unknown,
			Defined,
			InProgress,
			Completed,
		}

		private AsyncLazy<Person> owner;

		public ScheduleState State { get; set; }

		public decimal TotalTime { get; set; }
		public decimal RemainingTime { get; set; }
		public decimal SpentTime
		{ get { return TotalTime - RemainingTime; } }

		public WorkTask(string id)
			: base(id)
		{
		}

		public async Task<Person> GetOwnerAsync()
		{
			return await owner;
		}

		public Person GetOwner()
		{
			return GetOwnerAsync().Result;
		}

		/// <summary>
		/// Instant owner initializer
		/// </summary>
		public void SetOwner(Person owner)
		{
			this.owner = new AsyncLazy<Person>(() => owner);
		}

		/// <summary>
		/// Lazy owner initializer
		/// </summary>
		public void SetOwner(Func<Task<Person>> valueFactory)
		{
			owner = new AsyncLazy<Person>(valueFactory);
		}
	}
}
