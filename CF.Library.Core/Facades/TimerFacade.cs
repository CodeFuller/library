using System;
using System.Timers;

namespace CF.Library.Core.Facades
{
	/// <summary>
	/// Facade interface for System.Timers.Timer.
	/// </summary>
	public interface ITimerFacade : IDisposable
	{
		/// <summary>
		/// Facade property for Timer.Enabled.
		/// </summary>
		bool Enabled { get; set; }

		/// <summary>
		/// Facade property for Timer.Elapsed.
		/// </summary>
		event ElapsedEventHandler Elapsed;

		/// <summary>
		/// Facade method for Timer.Start().
		/// </summary>
		void Start();

		/// <summary>
		/// Facade method for Timer.Stop().
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", Justification = "Using the same facade method name as in replaced object.")]
		void Stop();
	}

	/// <summary>
	/// Facade for System.Timers.Timer.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Justification = "False positive. See http://stackoverflow.com/a/8926598/5740031 for details.")]
	public class TimerFacade : Timer, ITimerFacade
	{
	}
}
