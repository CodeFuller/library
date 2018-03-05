using System;
using System.Timers;
using CF.Library.Core.Facades;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace CF.Library.Wpf.Tests
{
	[TestClass]
	public class InactivityDetectorTests
	{
		[TestMethod]
		public void Start_StartsInnerTimer()
		{
			//	Arrange

			var timerMock = Substitute.For<ITimerFacade>();
			var target = new InactivityDetector(timerMock, Substitute.For<ILogger<InactivityDetector>>(), TimeSpan.FromMinutes(5));

			//	Act

			target.Start();

			//	Assert

			timerMock.Received(1).Start();
		}

		[TestMethod]
		public void Start_SetsUpTimerCorrectly()
		{
			//	Arrange

			var timer = new TimerFacade();
			var target = new InactivityDetector(timer, Substitute.For<ILogger<InactivityDetector>>(), TimeSpan.FromMinutes(5));

			//	Act

			target.Start();

			//	Assert

			Assert.IsTrue(timer.AutoReset);
			Assert.IsTrue(timer.Interval <= 1000);
		}

		[TestMethod]
		public void Stop_StopsInnerTimer()
		{
			//	Arrange

			var timerMock = Substitute.For<ITimerFacade>();
			var target = new InactivityDetector(timerMock, Substitute.For<ILogger<InactivityDetector>>(), TimeSpan.FromMinutes(5));

			target.Start();

			//	Act

			target.Stop();

			//	Assert

			timerMock.Received(1).Stop();
		}

		[TestMethod]
		public void IsActive_IfStopped_ReturnsFalse()
		{
			//	Arrange

			var timerStub = Substitute.For<ITimerFacade>();
			var target = new InactivityDetector(timerStub, Substitute.For<ILogger<InactivityDetector>>(), TimeSpan.FromMinutes(5));

			//	Act & Assert

			Assert.IsFalse(target.IsActive);
		}

		[TestMethod]
		public void IsActive_IfDeactivated_ReturnsFalse()
		{
			//	Arrange

			bool deactivated = false;

			var timerStub = Substitute.For<ITimerFacade>();
			var systemApiStub = Substitute.For<ISystemApi>();
			systemApiStub.GetUserInactivitySpan().Returns(TimeSpan.FromMinutes(10));

			var postHibernationGuardStub = Substitute.For<IPostHibernationGuard>();
			postHibernationGuardStub.IsInPostHibernationMode.Returns(false);

			var target = new InactivityDetector(timerStub, Substitute.For<ILogger<InactivityDetector>>(), TimeSpan.FromMinutes(5))
			{
				SystemApi = systemApiStub,
				PostHibernationGuard = postHibernationGuardStub,
			};

			target.Start();

			target.Deactivated += (sender, e) => deactivated = true;
			timerStub.Elapsed += Raise.Event<ElapsedEventHandler>(this, (ElapsedEventArgs)null);
			//	Sanity check
			Assert.IsTrue(deactivated);

			//	Act & Assert

			Assert.IsFalse(target.IsActive);
		}

		[TestMethod]
		public void IsActive_IfRunningAndNotDeactivated_ReturnsTrue()
		{
			//	Arrange

			var timerStub = Substitute.For<ITimerFacade>();
			timerStub.Enabled.Returns(true);

			var systemApiStub = Substitute.For<ISystemApi>();
			systemApiStub.GetUserInactivitySpan().Returns(TimeSpan.FromMinutes(10));

			var postHibernationGuardStub = Substitute.For<IPostHibernationGuard>();
			postHibernationGuardStub.IsInPostHibernationMode.Returns(false);

			var target = new InactivityDetector(timerStub, Substitute.For<ILogger<InactivityDetector>>(), TimeSpan.FromMinutes(5))
			{
				SystemApi = systemApiStub,
				PostHibernationGuard = postHibernationGuardStub,
			};

			target.Start();

			//	Act & Assert

			Assert.IsTrue(target.IsActive);
		}

		[TestMethod]
		public void TimerTickHandler_WhenActivatedAndUserIsInactive_SendsDeactivatedEvent()
		{
			//	Arrange

			var timerStub = Substitute.For<ITimerFacade>();
			var systemApiStub = Substitute.For<ISystemApi>();
			systemApiStub.GetUserInactivitySpan().Returns(TimeSpan.FromMinutes(10));

			var postHibernationGuardStub = Substitute.For<IPostHibernationGuard>();
			postHibernationGuardStub.IsInPostHibernationMode.Returns(false);

			var target = new InactivityDetector(timerStub, Substitute.For<ILogger<InactivityDetector>>(), TimeSpan.FromMinutes(5))
			{
				SystemApi = systemApiStub,
				PostHibernationGuard = postHibernationGuardStub,
			};

			TimeSpan? inactivitySpan = null;
			target.Deactivated += (sender, e) => inactivitySpan = e.InactivitySpan;

			target.Start();

			//	Act

			timerStub.Elapsed += Raise.Event<ElapsedEventHandler>(this, (ElapsedEventArgs)null);

			//	Assert

			Assert.IsNotNull(inactivitySpan);
			Assert.AreEqual(TimeSpan.FromMinutes(10), inactivitySpan);
		}

		[TestMethod]
		public void TimerTickHandler_WhenActivatedAndUserIsActive_DoesNotSendDeactivatedEvent()
		{
			//	Arrange

			bool deactivated = false;

			var timerStub = Substitute.For<ITimerFacade>();
			var systemApiStub = Substitute.For<ISystemApi>();
			systemApiStub.GetUserInactivitySpan().Returns(TimeSpan.FromSeconds(10));

			var postHibernationGuardStub = Substitute.For<IPostHibernationGuard>();
			postHibernationGuardStub.IsInPostHibernationMode.Returns(false);

			var target = new InactivityDetector(timerStub, Substitute.For<ILogger<InactivityDetector>>(), TimeSpan.FromMinutes(5))
			{
				SystemApi = systemApiStub,
				PostHibernationGuard = postHibernationGuardStub,
			};

			target.Deactivated += (sender, e) => deactivated = true;

			target.Start();

			//	Act

			timerStub.Elapsed += Raise.Event<ElapsedEventHandler>(this, (ElapsedEventArgs)null);

			//	Assert

			Assert.IsFalse(deactivated);
		}

		[TestMethod]
		public void TimerTickHandler_WhenDeactivatedAndUserIsActive_SendsReactivatedEvent()
		{
			//	Arrange

			var timerStub = Substitute.For<ITimerFacade>();
			var dateTimeStub = Substitute.For<IDateTimeOffsetFacade>();
			var systemApiStub = Substitute.For<ISystemApi>();

			var postHibernationGuardStub = Substitute.For<IPostHibernationGuard>();
			postHibernationGuardStub.IsInPostHibernationMode.Returns(false);

			var target = new InactivityDetector(timerStub, Substitute.For<ILogger<InactivityDetector>>(), TimeSpan.FromMinutes(5))
			{
				DateTimeOffsetFacade = dateTimeStub,
				SystemApi = systemApiStub,
				PostHibernationGuard = postHibernationGuardStub,
			};

			TimeSpan? inactivitySpan = null;
			target.Reactivated += (sender, e) => inactivitySpan = e.InactivitySpan;

			target.Start();

			//	Deactivating detector.
			dateTimeStub.Now.Returns(new DateTimeOffset(2018, 3, 5, 12, 57, 0, TimeSpan.Zero));
			systemApiStub.GetUserInactivitySpan().Returns(TimeSpan.FromMinutes(10));
			timerStub.Elapsed += Raise.Event<ElapsedEventHandler>(this, (ElapsedEventArgs)null);
			//	Sanity check
			Assert.IsFalse(target.IsActive);

			systemApiStub.GetUserInactivitySpan().Returns(TimeSpan.FromSeconds(10));

			//	Act

			dateTimeStub.Now.Returns(new DateTimeOffset(2018, 3, 5, 13, 0, 0, TimeSpan.Zero));
			timerStub.Elapsed += Raise.Event<ElapsedEventHandler>(this, (ElapsedEventArgs)null);

			//	Assert

			Assert.IsNotNull(inactivitySpan);
			Assert.AreEqual(TimeSpan.FromMinutes(3), inactivitySpan);
		}

		[TestMethod]
		public void TimerTickHandler_WhenDeactivatedAndUserIsInactive_DoesNotSendReactivatedEvent()
		{
			//	Arrange

			bool reactivated = false;

			var timerStub = Substitute.For<ITimerFacade>();
			var systemApiStub = Substitute.For<ISystemApi>();
			systemApiStub.GetUserInactivitySpan().Returns(TimeSpan.FromMinutes(10));

			var postHibernationGuardStub = Substitute.For<IPostHibernationGuard>();
			postHibernationGuardStub.IsInPostHibernationMode.Returns(false);

			var target = new InactivityDetector(timerStub, Substitute.For<ILogger<InactivityDetector>>(), TimeSpan.FromMinutes(5))
			{
				SystemApi = systemApiStub,
				PostHibernationGuard = postHibernationGuardStub,
			};

			target.Reactivated += (sender, e) => reactivated = true;

			target.Start();

			//	Deactivating detector.
			timerStub.Elapsed += Raise.Event<ElapsedEventHandler>(this, (ElapsedEventArgs)null);
			//	Sanity check
			Assert.IsFalse(target.IsActive);

			//	Act

			timerStub.Elapsed += Raise.Event<ElapsedEventHandler>(this, (ElapsedEventArgs)null);

			//	Assert

			Assert.IsFalse(reactivated);
		}

		[TestMethod]
		public void TimerTickHandler_WhenActiveInPostHibernationMode_DoesNotSendDeactivatedEvent()
		{
			//	Arrange

			bool deactivated = false;

			var timerStub = Substitute.For<ITimerFacade>();
			var dateTimeStub = Substitute.For<IDateTimeOffsetFacade>();
			var systemApiStub = Substitute.For<ISystemApi>();
			systemApiStub.GetUserInactivitySpan().Returns(TimeSpan.FromMinutes(10));

			var postHibernationGuardStub = Substitute.For<IPostHibernationGuard>();
			postHibernationGuardStub.IsInPostHibernationMode.Returns(true);

			var target = new InactivityDetector(timerStub, Substitute.For<ILogger<InactivityDetector>>(), TimeSpan.FromMinutes(5))
			{
				DateTimeOffsetFacade = dateTimeStub,
				SystemApi = systemApiStub,
				PostHibernationGuard = postHibernationGuardStub,
			};

			target.Deactivated += (sender, e) => deactivated = true;

			target.Start();

			//	Act

			timerStub.Elapsed += Raise.Event<ElapsedEventHandler>(this, (ElapsedEventArgs)null);

			//	Assert

			Assert.IsFalse(deactivated);
		}

		[TestMethod]
		public void TimerTickHandler_RegistersTickInPostHibernationGuard()
		{
			//	Arrange

			bool deactivated = false;

			var timerStub = Substitute.For<ITimerFacade>();
			var dateTimeStub = Substitute.For<IDateTimeOffsetFacade>();
			dateTimeStub.Now.Returns(new DateTimeOffset(2018, 3, 5, 15, 9, 0, TimeSpan.Zero));
			var systemApiStub = Substitute.For<ISystemApi>();
			systemApiStub.GetUserInactivitySpan().Returns(TimeSpan.FromMinutes(10));

			var postHibernationGuardMock = Substitute.For<IPostHibernationGuard>();
			postHibernationGuardMock.IsInPostHibernationMode.Returns(true);

			var target = new InactivityDetector(timerStub, Substitute.For<ILogger<InactivityDetector>>(), TimeSpan.FromMinutes(5))
			{
				DateTimeOffsetFacade = dateTimeStub,
				SystemApi = systemApiStub,
				PostHibernationGuard = postHibernationGuardMock,
			};

			target.Start();

			//	Act

			timerStub.Elapsed += Raise.Event<ElapsedEventHandler>(this, (ElapsedEventArgs)null);

			//	Assert

			postHibernationGuardMock.RegisterTick(new DateTimeOffset(2018, 3, 5, 15, 9, 0, TimeSpan.Zero), TimeSpan.FromMinutes(10));
		}

		[TestMethod]
		public void TimerTickHandler_WhenDeactivatedInPostHibernationMode_DoesNotSendActivatedEvent()
		{
			//	Arrange

			bool reactivated = false;

			var timerStub = Substitute.For<ITimerFacade>();
			var dateTimeStub = Substitute.For<IDateTimeOffsetFacade>();
			var systemApiStub = Substitute.For<ISystemApi>();
			systemApiStub.GetUserInactivitySpan().Returns(TimeSpan.FromMinutes(1));

			var postHibernationGuardStub = Substitute.For<IPostHibernationGuard>();
			postHibernationGuardStub.IsInPostHibernationMode.Returns(true);

			var target = new InactivityDetector(timerStub, Substitute.For<ILogger<InactivityDetector>>(), TimeSpan.FromMinutes(5))
			{
				DateTimeOffsetFacade = dateTimeStub,
				SystemApi = systemApiStub,
				PostHibernationGuard = postHibernationGuardStub,
			};

			target.Reactivated += (sender, e) => reactivated = true;

			target.Start();

			//	Act

			timerStub.Elapsed += Raise.Event<ElapsedEventHandler>(this, (ElapsedEventArgs)null);

			//	Assert

			Assert.IsFalse(reactivated);
		}

		[TestMethod]
		public void Dispose_DisposesInnerTimer()
		{
			//	Arrange

			var timerMock = Substitute.For<ITimerFacade>();
			var target = new InactivityDetector(timerMock, Substitute.For<ILogger<InactivityDetector>>(), TimeSpan.FromMinutes(5));

			//	Act

			target.Dispose();

			//	Assert

			timerMock.Received(1).Dispose();
		}
	}
}
