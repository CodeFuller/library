using System;

namespace CodeFuller.Library.Core.Facades
{
	public interface IDateTimeOffsetFacade
	{
		DateTimeOffset Now { get; }

		DateTimeOffset UtcNow { get; }
	}
}
