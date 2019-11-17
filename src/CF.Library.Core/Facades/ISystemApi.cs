using System;

namespace CF.Library.Core.Facades
{
	public interface ISystemApi
	{
		TimeSpan GetUserInactivitySpan();
	}
}
