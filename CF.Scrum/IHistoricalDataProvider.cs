using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CF.Scrum.Objects;

namespace CF.Scrum
{
	public interface IHistoricalDataProvider
	{
		/// <summary>
		/// Returns DateTime when Work Unit was assigned to the iteration
		/// </summary>
		Task<DateTime> GetScopingTimeAsync(Iteration iteration, WorkUnit workUnit);

		Task<decimal[]> GetTaskTimesSnapshotAsync(WorkTask[] tasks, DateTime dt);

		Task<IEnumerable<WorkTask>> GetTasksSnapshotAsync(WorkUnit workUnit, DateTime dt);

		/// <summary>
		/// Updates all units with schedule state at specified time
		/// </summary>
		Task SetSnapshotStateAsync(List<WorkUnit> workUnits, DateTime dt);
	}
}
