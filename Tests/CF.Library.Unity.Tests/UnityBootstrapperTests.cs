using System;
using CF.Library.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Unity;

namespace CF.Library.Unity.Tests
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

	[TestClass]
	public class UnityBootstrapperTests
	{
		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void Run_CalledRepeatedly_ThrowsInvalidOperationException()
		{
			//	Arrange

			UnityBootstrapper<Object> target = new TestUnityBootstrapper(Substitute.For<IUnityContainer>());

			target.Run();

			//	Act & Assert

			target.Run();
		}

		[TestMethod]
		public void Run_IfILoggerIsRegistered_FillsLoggerInLogHolder()
		{
			//	Arrange

			ILogger logger = Substitute.For<ILogger>();
			//	We can't use Substitute for IUnityContainer here because NSubstitute can't stub extension methods.
			IUnityContainer diContainerStub = new UnityContainer();
			diContainerStub.RegisterInstance(logger);

			UnityBootstrapper<Object> target = new TestUnityBootstrapper(diContainerStub);

			//	Act

			target.Run();

			//	Assert

			Assert.AreSame(logger, LogHolder.Logger);
		}

		[TestMethod]
		public void Run_IfIMessageLoggerIsNotRegistered_DoesNotThrow()
		{
			//	Arrange

			//	We can't use Substitute for IUnityContainer here because NSubstitute can't stub extension methods.
			IUnityContainer diContainerStub = new UnityContainer();
			UnityBootstrapper<Object> target = new TestUnityBootstrapper(diContainerStub);

			//	Act & Assert

			target.Run();
		}

		[TestMethod]
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
