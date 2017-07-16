using System;
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

			IUnityContainer diContainerStub = Substitute.For<IUnityContainer>();
			UnityBootstrapper<Object> target = new TestUnityBootstrapper(diContainerStub);

			//	Act & Assert

			target.Run();

			Assert.Throws<InvalidOperationException>(() => target.Run());
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
