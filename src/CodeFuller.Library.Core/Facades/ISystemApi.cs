using System;

namespace CodeFuller.Library.Core.Facades
{
	public interface ISystemApi
	{
		TimeSpan GetUserInactivitySpan();
	}
}
