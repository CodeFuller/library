using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Library.Patterns
{
	/// <summary>
	/// Interface for task executed periodically with support of start/stop/pause operations.
	/// </summary>
	public interface IPeriodicTask
	{
		/// <summary>
		/// Interval between task executions.
		/// </summary>
		TimeSpan Interval { get; set; }

		/// <summary>
		/// Task action that should be executed periodically.
		/// </summary>
		Func<Task> TaskAction { get; set; }

		/// <summary>
		/// Starts periodic task execution.
		/// </summary>
		void Start();

		/// <summary>
		/// Stops periodic task execution.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", Justification = "Stop name is the best in current semantics.")]
		void Stop();
	}
}
