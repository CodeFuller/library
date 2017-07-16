using System;
using CF.Library.Core.Bootstrap;
using CF.Library.Core.Facades;
using NSubstitute;
using NUnit.Framework;

namespace CF.Library.UnitTests.CF.Library.Core.Bootstrap
{
	[TestFixture]
	public class ServiceApplicationTests
	{
		[Test]
		public void Constructor_WhenBootstrapperArgumentIsNull_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => new ServiceApplication(null));
		}

		[Test]
		public void Run_LaunchesServiceBootstrapper()
		{
			//	Arrange

			IBootstrapper<IServiceFacade> bootstrapperMock = Substitute.For<IBootstrapper<IServiceFacade>>();
			var target = new ServiceApplication(bootstrapperMock);

			//	Act

			target.Run();

			//	Assert

			bootstrapperMock.Received(1).Run();
		}

		[Test]
		public void Run_ForNormalMode_RunsServiceInNormalMode()
		{
			//	Arrange

			IServiceFacade serviceMock = Substitute.For<IServiceFacade>();
			serviceMock.GetCommandLineArgs().Returns(new string[0]);

			IBootstrapper<IServiceFacade> serviceBootstrapper = Substitute.For<IBootstrapper<IServiceFacade>>();
			serviceBootstrapper.Run().Returns(serviceMock);

			var target = new ServiceApplication(serviceBootstrapper);

			//	Act

			target.Run();

			//	Assert

			serviceMock.Received(1).RunService();
		}

		[Test]
		public void Run_ForInteractiveMode_RunsServiceInInteractiveMode()
		{
			//	Arrange

			IServiceFacade serviceMock = Substitute.For<IServiceFacade>();
			serviceMock.ConsoleFacade = Substitute.For<IConsoleFacade>();
			serviceMock.GetCommandLineArgs().Returns(new[] { "--interactive" });

			IBootstrapper<IServiceFacade> serviceBootstrapper = Substitute.For<IBootstrapper<IServiceFacade>>();
			serviceBootstrapper.Run().Returns(serviceMock);

			var target = new ServiceApplication(serviceBootstrapper);

			//	Act

			target.Run();

			//	Assert

			serviceMock.Received(1).RunInteractive();
		}

		[Test]
		public void Run_DisposesServiceBootstrapper()
		{
			//	Arrange

			IBootstrapper<IServiceFacade> bootstrapperMock = Substitute.For<IBootstrapper<IServiceFacade>>();
			var target = new ServiceApplication(bootstrapperMock);

			//	Act

			target.Run();

			//	Assert

			bootstrapperMock.Received(1).Dispose();
		}
	}
}
