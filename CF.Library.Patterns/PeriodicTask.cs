using System;
using System.Threading.Tasks;
using System.Timers;
using CF.Library.Core.Events;
using CF.Library.Core.Facades;

namespace CF.Library.Patterns
{
	/// <summary>
	/// Implementation of IPeriodicTask based on IProcessStateManager and ITimerFacade.
	/// </summary>
	public class PeriodicTask : IPeriodicTask
	{
		private readonly IProcessStateManager stateManager;
		private readonly ITimerFacade timer;
		private TimeSpan taskInterval;

		/// <summary>
		/// Event that is fired when exception is thrown by task action.
		/// </summary>
		public event EventHandler<ExceptionThrownEventArgs> ExceptionThrown;

		/// <summary>
		/// Interval between task executions.
		/// </summary>
		public TimeSpan Interval
		{
			get { return taskInterval; }
			set
			{
				taskInterval = value;
				timer.Interval = value.TotalMilliseconds;
			}
		}

		/// <summary>
		/// Task action that should be executed periodically.
		/// </summary>
		public Func<Task> TaskAction { get; set; }

		/// <summary>
		/// Constructor.
		/// </summary>
		public PeriodicTask(IProcessStateManager stateManager, ITimerFacade timer)
		{
			if (stateManager == null)
			{
				throw new ArgumentNullException(nameof(stateManager));
			}
			if (timer == null)
			{
				throw new ArgumentNullException(nameof(timer));
			}

			this.stateManager = stateManager;
			this.timer = timer;
			this.timer.Elapsed += Timer_Tick;
			Interval = TimeSpan.FromMilliseconds(timer.Interval);
		}

		/// <summary>
		/// Implementation for IPeriodicTask.Start().
		/// </summary>
		public void Start()
		{
			if (TaskAction == null)
			{
				throw new InvalidOperationException("Periodic task action is not set");
			}

			stateManager.Start();

			//	Setting interval to smallest value for initial instant update.
			//	Interval will be set to requested interval on first tick.
			timer.Interval = 1;
			timer.Start();
		}

		/// <summary>
		/// Implementation for IPeriodicTask.Stop().
		/// </summary>
		public void Stop()
		{
			stateManager.Stop();
			timer.Stop();
		}

		private async void Timer_Tick(object sender, ElapsedEventArgs e)
		{
			try
			{
				// Restoring original task interval.
				timer.Interval = Interval.TotalMilliseconds;

				await TaskAction();
			}
			catch (Exception exception)
			{
				ExceptionThrown?.Invoke(this, new ExceptionThrownEventArgs(exception));
			}
		}
	}
}
