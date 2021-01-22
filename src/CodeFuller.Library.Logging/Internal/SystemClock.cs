using System;
using CodeFuller.Library.Logging.Interfaces;

namespace CodeFuller.Library.Logging.Internal
{
	internal class SystemClock : IClock
	{
		public DateTime Now => DateTime.Now;
	}
}
