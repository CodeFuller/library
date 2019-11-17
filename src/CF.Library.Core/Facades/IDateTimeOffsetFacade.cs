using System;

namespace CF.Library.Core.Facades
{
	public interface IDateTimeOffsetFacade
	{
		DateTimeOffset Now { get; }

		DateTimeOffset UtcNow { get; }
	}
}
