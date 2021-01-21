using System;

namespace CodeFuller.Library.Core.Facades
{
	public class DateTimeOffsetFacade : IDateTimeOffsetFacade
	{
		public DateTimeOffset Now => DateTimeOffset.Now;

		public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
	}
}
