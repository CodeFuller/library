using System;

namespace CodeFuller.Library.Logging.Interfaces
{
	internal interface ISystemClock
	{
		DateTime Now { get; }
	}
}
