﻿using System;
using Microsoft.Extensions.Logging;
using static CF.Library.Core.Extensions.FormattableStringExtensions;

namespace CF.Library.Wpf
{
	internal class PostHibernationGuard : IPostHibernationGuard
	{
		private readonly TimeSpan userInactivityThreshold;

		private DateTimeOffset? lastTickTimestamp;
		private DateTimeOffset? unhibernatedTimestamp;

		private readonly ILogger logger;

		public bool IsInPostHibernationMode => unhibernatedTimestamp.HasValue;

		public PostHibernationGuard(ILogger logger, TimeSpan userInactivityThreshold)
		{
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
			this.userInactivityThreshold = userInactivityThreshold;
		}

		public void RegisterTick(DateTimeOffset currentTime, TimeSpan userInactivitySpan)
		{
			if (lastTickTimestamp != null && currentTime - lastTickTimestamp >= userInactivityThreshold)
			{
				logger.LogDebug(Current($"Previous inactivity detection tick happened long ago ({currentTime - lastTickTimestamp:hh\\:mm\\:ss}). Probably workstation was hibernated."));
				unhibernatedTimestamp = currentTime;
			}

			lastTickTimestamp = currentTime;

			if (!IsInPostHibernationMode)
			{
				return;
			}

			if (userInactivitySpan < currentTime - unhibernatedTimestamp)
			{
				logger.LogDebug(Current($"User has shown activity after hibernation, clearing post hibernation mode"));
				unhibernatedTimestamp = null;
			}
			else if (currentTime - unhibernatedTimestamp.Value >= userInactivityThreshold)
			{
				logger.LogDebug(Current($"User is inactive too long after hibernation, clearing AfterHibernation mode"));
				unhibernatedTimestamp = null;
			}
		}
	}
}