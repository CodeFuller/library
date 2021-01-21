using System;

namespace CodeFuller.Library.Wpf
{
	internal interface IPostHibernationGuard
	{
		bool IsInPostHibernationMode { get; }

		void RegisterTick(DateTimeOffset currentTime, TimeSpan userInactivitySpan);
	}
}
