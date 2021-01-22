using System;

namespace CodeFuller.Library.Logging.Interfaces
{
	internal interface IClock
	{
		DateTime Now { get; }
	}
}
