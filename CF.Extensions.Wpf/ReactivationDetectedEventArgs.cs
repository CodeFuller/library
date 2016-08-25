using System;

namespace CF.Extensions.Wpf
{
	/// <summary>
	/// Contains event data for ReactivationDetected event
	/// </summary>
	public class ReactivationDetectedEventArgs : EventArgs
	{
		/// <summary>
		/// User inactivity duration
		/// </summary>
		public TimeSpan InactivitySpan { get; set; }

		/// <summary>
		/// Constructor
		/// </summary>
		public ReactivationDetectedEventArgs(TimeSpan inactivitySpan)
		{
			InactivitySpan = inactivitySpan;
		}
	}
}
