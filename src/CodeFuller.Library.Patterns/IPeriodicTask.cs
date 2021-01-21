using System;
using System.Threading.Tasks;
using CodeFuller.Library.Core.Events;

namespace CodeFuller.Library.Patterns
{
	/// <summary>
	/// Interface for task executed periodically with support of start/stop/pause operations.
	/// </summary>
	public interface IPeriodicTask
	{
		/// <summary>
		/// Event that is fired when exception is thrown by task action.
		/// </summary>
		event EventHandler<ExceptionThrownEventArgs> ExceptionThrown;

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
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", Justification = "'Stop' is the best name in current semantics.")]
		void Stop();
	}
}
