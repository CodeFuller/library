using System;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace CF.Library.Wpf.Tests
{
	[TestClass]
	public class PostHibernationGuardTests
	{
		[TestMethod]
		public void RegisterTick_OnFirstTick_DoesNotEnterPostHibernationMode()
		{
			//	Arrange

			var target = new PostHibernationGuard(Substitute.For<ILogger>(), TimeSpan.FromMinutes(3));

			//	Act

			target.RegisterTick(new DateTimeOffset(2018, 3, 5, 18, 0, 0, TimeSpan.Zero), TimeSpan.FromMinutes(1));

			//	Assert

			Assert.IsFalse(target.IsInPostHibernationMode);
		}

		[TestMethod]
		public void RegisterTick_IfThereIsBigSpanFromPreviousTick_EntersPostHibernationMode()
		{
			//	Arrange

			var target = new PostHibernationGuard(Substitute.For<ILogger>(), TimeSpan.FromMinutes(3));

			//	Previous tick.
			target.RegisterTick(new DateTimeOffset(2018, 3, 5, 12, 0, 0, TimeSpan.Zero), TimeSpan.FromMinutes(1));

			//	Act

			target.RegisterTick(new DateTimeOffset(2018, 3, 5, 18, 0, 0, TimeSpan.Zero), TimeSpan.FromMinutes(1));

			//	Assert

			Assert.IsTrue(target.IsInPostHibernationMode);
		}

		[TestMethod]
		public void RegisterTick_InPostHibernationModeWhenUserShownActivityWithinThreshold_ExitsPostHibernationMode()
		{
			//	Arrange

			var target = new PostHibernationGuard(Substitute.For<ILogger>(), TimeSpan.FromMinutes(3));

			target.RegisterTick(new DateTimeOffset(2018, 3, 5, 12, 0, 0, TimeSpan.Zero), TimeSpan.FromMinutes(1));
			target.RegisterTick(new DateTimeOffset(2018, 3, 5, 18, 0, 0, TimeSpan.Zero), TimeSpan.FromMinutes(1));
			//	Sanity check.
			Assert.IsTrue(target.IsInPostHibernationMode);

			//	Act

			target.RegisterTick(new DateTimeOffset(2018, 3, 5, 18, 1, 0, TimeSpan.Zero), TimeSpan.FromSeconds(1));

			//	Assert

			Assert.IsFalse(target.IsInPostHibernationMode);
		}

		[TestMethod]
		public void RegisterTick_InPostHibernationModeWhenUserDidNotShowActivityAndThresholdIsNotReached_KeepsPostHibernationMode()
		{
			//	Arrange

			var target = new PostHibernationGuard(Substitute.For<ILogger>(), TimeSpan.FromMinutes(3));

			target.RegisterTick(new DateTimeOffset(2018, 3, 5, 12, 0, 0, TimeSpan.Zero), TimeSpan.FromMinutes(1));
			target.RegisterTick(new DateTimeOffset(2018, 3, 5, 18, 0, 0, TimeSpan.Zero), TimeSpan.FromMinutes(1));
			//	Sanity check.
			Assert.IsTrue(target.IsInPostHibernationMode);

			//	Act

			target.RegisterTick(new DateTimeOffset(2018, 3, 5, 18, 1, 0, TimeSpan.Zero), TimeSpan.FromHours(6));

			//	Assert

			Assert.IsTrue(target.IsInPostHibernationMode);
		}

		[TestMethod]
		public void RegisterTick_InPostHibernationModeWhenUserDidNotShowActivityAndThresholdIsReached_ExitsPostHibernationMode()
		{
			//	Arrange

			var target = new PostHibernationGuard(Substitute.For<ILogger>(), TimeSpan.FromMinutes(3));

			target.RegisterTick(new DateTimeOffset(2018, 3, 5, 12, 0, 0, TimeSpan.Zero), TimeSpan.FromMinutes(1));
			target.RegisterTick(new DateTimeOffset(2018, 3, 5, 18, 0, 0, TimeSpan.Zero), TimeSpan.FromMinutes(1));
			//	Keeping span from previous tick small.
			target.RegisterTick(new DateTimeOffset(2018, 3, 5, 18, 2, 0, TimeSpan.Zero), TimeSpan.FromHours(6));
			//	Sanity check.
			Assert.IsTrue(target.IsInPostHibernationMode);

			//	Act

			target.RegisterTick(new DateTimeOffset(2018, 3, 5, 18, 4, 0, TimeSpan.Zero), TimeSpan.FromHours(6));

			//	Assert

			Assert.IsFalse(target.IsInPostHibernationMode);
		}
	}
}
