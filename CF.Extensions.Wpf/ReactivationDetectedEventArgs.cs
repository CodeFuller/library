using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Extensions.Wpf
{
	public class ReactivationDetectedEventArgs : EventArgs
	{
		public TimeSpan InactivitySpan { get; set; }

		public ReactivationDetectedEventArgs(TimeSpan inactivitySpan)
		{
			InactivitySpan = inactivitySpan;
		}
	}
}
