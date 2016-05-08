using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CF.Patterns;

namespace CF.Scrum.Objects
{
	public class Defect : WorkUnit
	{
		public Defect(string id)
			: base(id)
		{
		}

		public override async Task<IEnumerable<WorkTask>> GetAllTasksAsync()
		{
			return await GetDirectTasksAsync();
		}

		public override IEnumerable<WorkTask> GetAllTasks()
		{
			return GetAllTasksAsync().Result;
		}
	}
}
