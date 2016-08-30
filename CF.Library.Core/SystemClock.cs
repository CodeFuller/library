using System;
using CF.Library.Core.Facades;

namespace CF.Library.Core
{
	/// <summary>
	/// Implementation of IClock based on DateTime.Now()
	/// </summary>
	public class SystemClock : IClock
	{
		/// <summary>
		/// Returns current date and time
		/// </summary>
		public DateTime Now => DateTime.Now;
	}
}
