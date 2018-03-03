using System;
using System.Timers;

namespace CF.Library.Core.Facades
{
	/// <summary>
	/// Facade interface for System.Timers.Timer.
	/// </summary>
	/// <remarks>
	/// System.Timers.Timer was selected over System.Threading.Timer because it supports Interval property and Start() &amp; Stop() methods.
	/// </remarks>
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
		/// Facade property for Timer.Interval.
		/// </summary>
		double Interval { get; set; }

		/// <summary>
		/// Facade property for Timer.AutoReset.
		/// </summary>
		bool AutoReset { get; set; }

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
		/// <summary>
		/// Constructor.
		/// </summary>
		public TimerFacade()
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public TimerFacade(double interval) :
			base(interval)
		{
		}
	}
}
