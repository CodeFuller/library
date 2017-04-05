using System;

namespace CF.Library.Wpf
{
	/// <summary>
	/// Detects user inactivity (no keyboard presses and no mouse move/clicks during some period of time).
	/// </summary>
	public interface IInactivityDetector
	{
		/// <summary>
		/// Max user inactivity duration after which Deactivated is fired.
		/// </summary>
		TimeSpan InactivityThreshold { get; set; }

		/// <summary>
		/// Event that is fired when user is inactive during InactivityThreshold period.
		/// </summary>
		event EventHandler<InactivationDetectedEventArgs> Deactivated;

		/// <summary>
		/// Event that is fired after user returns to work after some period of inactivity.
		/// </summary>
		event EventHandler<ReactivationDetectedEventArgs> Reactivated;

		/// <summary>
		/// Returns true if detector is in active state.
		/// </summary>
		bool IsActive { get; }

		/// <summary>
		/// Starts inactivity detection.
		/// </summary>
		void Start();

		/// <summary>
		/// Stops inactivity detection.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", Justification = "'Stop' is the best name in current semantics.")]
		void Stop();
	}
}
