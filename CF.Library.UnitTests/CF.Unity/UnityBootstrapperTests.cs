using System;
using CF.Library.Core;
using CF.Library.Core.Logging;
using CF.Library.Unity;
using Microsoft.Practices.Unity;
using NSubstitute;
using NUnit.Framework;

namespace CF.Library.UnitTests.CF.Unity
{
	class TestUnityBootstrapper : UnityBootstrapper<Object>
	{
		public TestUnityBootstrapper(IUnityContainer diContainer)
		{
			DIContainer = diContainer;
		}

		protected override void RegisterDependencies()
		{
		}
	}

	[TestFixture]
	public class UnityBootstrapperTests
	{
		[Test]
		public void Run_CalledRepeatedly_ThrowsInvalidOperationException()
		{
			//	Arrange

			UnityBootstrapper<Object> target = new TestUnityBootstrapper(Substitute.For<IUnityContainer>());

			//	Act & Assert

			target.Run();

			Assert.Throws<InvalidOperationException>(() => target.Run());
		}

		[Test]
		public void Run_IfIMessageLoggerIsRegistered_SetsApplicationMessageLogger()
		{
			//	Arrange

			IMessageLogger logger = Substitute.For<IMessageLogger>();
			//	We can't use Substitute for IUnityContainer here because NSubstitute can't stub extension methods.
			IUnityContainer diContainerStub = new UnityContainer();
			diContainerStub.RegisterInstance(logger);

			UnityBootstrapper<Object> target = new TestUnityBootstrapper(diContainerStub);

			//	Act

			target.Run();

			//	Assert

			Assert.AreSame(logger, Application.Logger);
		}

		[Test]
		public void Run_IfIMessageLoggerIsNotRegistered_DoesNotThrow()
		{
			//	Arrange

			//	We can't use Substitute for IUnityContainer here because NSubstitute can't stub extension methods.
			IUnityContainer diContainerStub = new UnityContainer();
			UnityBootstrapper<Object> target = new TestUnityBootstrapper(diContainerStub);

			//	Act & Assert

			Assert.DoesNotThrow(() => target.Run());
		}

		[Test]
		public void Dispose_DisposesDIContainer()
		{
			//	Arrange

			IUnityContainer diContainerMock = Substitute.For<IUnityContainer>();
			UnityBootstrapper<Object> target = new TestUnityBootstrapper(diContainerMock);

			//	Act

			target.Dispose();

			//	Assert

			diContainerMock.Received(1).Dispose();
		}
	}
}
