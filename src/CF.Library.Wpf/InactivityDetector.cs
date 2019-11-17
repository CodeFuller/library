using System;
using CF.Library.Core.Facades;
using Microsoft.Extensions.Logging;
using static CF.Library.Core.Extensions.FormattableStringExtensions;

namespace CF.Library.Wpf
{
	/// <summary>
	/// Implementation for IInactivityDetector.
	/// </summary>
	public class InactivityDetector : IInactivityDetector, IDisposable
	{
		private bool deactivated;
		private DateTimeOffset? inactivationTimestamp;

		private readonly TimeSpan inactivityThreshold;

		private readonly ITimerFacade timer;
		private readonly ILogger<InactivityDetector> logger;

		/// <summary>
		/// Property Injection for IDateTimeOffsetFacade.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Setter could be used by Unit tests from assembly for which internals are visible.")]
		internal IDateTimeOffsetFacade DateTimeOffsetFacade { get; set; } = new DateTimeOffsetFacade();

		/// <summary>
		/// Property Injection for ISystemApi.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Setter could be used by Unit tests from assembly for which internals are visible.")]
		internal ISystemApi SystemApi { get; set; } = new SystemApi();

		internal IPostHibernationGuard PostHibernationGuard { get; set; }

		/// <summary>
		/// Event that is fired when user is inactive during InactivityThreshold period.
		/// </summary>
		public event EventHandler<InactivationDetectedEventArgs> Deactivated;

		/// <summary>
		/// Event that is fired after user returns to work after some period of inactivity.
		/// </summary>
		public event EventHandler<ReactivationDetectedEventArgs> Reactivated;

		/// <summary>
		/// Returns true if detector is in active state.
		/// </summary>
		public bool IsActive => timer.Enabled && !deactivated;

		/// <summary>
		/// Constructor
		/// </summary>
		public InactivityDetector(ITimerFacade timer, ILogger<InactivityDetector> logger, TimeSpan inactivityThreshold)
		{
			this.timer = timer ?? throw new ArgumentNullException(nameof(timer));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
			this.inactivityThreshold = inactivityThreshold;
			this.timer.Elapsed += (sender, e) => OnTick();
			this.timer.Interval = 1000;

			PostHibernationGuard = new PostHibernationGuard(logger, inactivityThreshold);
		}

		/// <summary>
		/// Starts inactivity detection
		/// </summary>
		public void Start()
		{
			deactivated = false;
			timer.Start();
		}

		/// <summary>
		/// Stops inactivity detection
		/// </summary>
		public void Stop()
		{
			timer.Stop();
		}

		private void OnTick()
		{
			var currentTime = DateTimeOffsetFacade.Now;
			var inactivitySpan = SystemApi.GetUserInactivitySpan();

			PostHibernationGuard.RegisterTick(currentTime, inactivitySpan);
			if (PostHibernationGuard.IsInPostHibernationMode)
			{
				logger.LogDebug(Current($"User is still inactive after hibernation, keeping AfterHibernation mode"));
				return;
			}

			if (!deactivated)
			{
				if (inactivitySpan >= inactivityThreshold)
				{
					deactivated = true;
					inactivationTimestamp = currentTime;
					OnInactivition(inactivitySpan);
				}
			}
			else
			{
				if (inactivitySpan < inactivityThreshold)
				{
					deactivated = false;
					var periodOfInactivity = currentTime - inactivationTimestamp ?? TimeSpan.Zero;
					inactivationTimestamp = null;
					OnReactivation(periodOfInactivity);
				}
			}
		}

		/// <summary>
		/// Is called when user is inactive during Threshold period
		/// </summary>
		protected virtual void OnInactivition(TimeSpan inactivitySpan)
		{
			Deactivated?.Invoke(this, new InactivationDetectedEventArgs(inactivitySpan));
		}

		/// <summary>
		/// Is called when user returns to work after some period of inactivity
		/// </summary>
		protected virtual void OnReactivation(TimeSpan inactivitySpan)
		{
			Reactivated?.Invoke(this, new ReactivationDetectedEventArgs(inactivitySpan));
		}

		/// <summary>
		/// Implementation for IDisposable.Dispose()
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases object resources
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", Justification = "False positive. See http://stackoverflow.com/q/34583417/5740031 for details.")]
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				timer?.Dispose();
			}
		}
	}
}
