using System;

namespace CF.Library.Wpf
{
	internal interface IPostHibernationGuard
	{
		bool IsInPostHibernationMode { get; }

		void RegisterTick(DateTimeOffset currentTime, TimeSpan userInactivitySpan);
	}
}
