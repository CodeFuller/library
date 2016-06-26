using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Extensions.Wpf
{
	/// <summary>
	/// Contains event data for InactivationDetected event
	/// </summary>
	public class InactivationDetectedEventArgs : EventArgs
	{
		/// <summary>
		/// User inactivity duration
		/// </summary>
		public TimeSpan InactivitySpan { get; set; }

		/// <summary>
		/// Constructor
		/// </summary>
		public InactivationDetectedEventArgs(TimeSpan inactivitySpan)
		{
			InactivitySpan = inactivitySpan;
		}
	}
}
