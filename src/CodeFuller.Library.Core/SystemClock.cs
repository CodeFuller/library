using System;
using CodeFuller.Library.Core.Facades;

namespace CodeFuller.Library.Core
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
