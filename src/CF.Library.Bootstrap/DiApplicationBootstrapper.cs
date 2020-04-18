using System;
using CF.Library.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CF.Library.Bootstrap
{
	public abstract class DiApplicationBootstrapper<TRoot> : BasicApplicationBootstrapper<TRoot>
	{
		private IServiceProvider serviceProvider;

		public override TRoot Bootstrap(string[] args)
		{
			var configuration = BootstrapConfiguration(args);

			var loggerFactory = BootstrapLogging(configuration);

			var services = new ServiceCollection();

			services.AddSingleton(loggerFactory);
			services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

			services.AddOptions();

			RegisterServices(services, configuration);

			serviceProvider = services.BuildServiceProvider();
			
			return serviceProvider.GetRequiredService<TRoot>();
		}

		public override ILogger<TCategoryName> GetLogger<TCategoryName>()
		{
			return serviceProvider.GetRequiredService<ILogger<TCategoryName>>();
		}

		protected virtual IConfiguration BootstrapConfiguration(string[] commandLineArgs)
		{
			IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();

			BootstrapConfiguration(configurationBuilder, commandLineArgs);

			return configurationBuilder.Build();
		}

		protected virtual void BootstrapConfiguration(IConfigurationBuilder configurationBuilder, string[] commandLineArgs)
		{
			configurationBuilder.LoadSettings("conf", commandLineArgs);
		}

		protected virtual ILoggerFactory BootstrapLogging(IConfiguration configuration)
		{
			ILoggerFactory loggerFactory = new LoggerFactory();

			BootstrapLogging(loggerFactory, configuration);

			return loggerFactory;
		}

		protected virtual void BootstrapLogging(ILoggerFactory loggerFactory, IConfiguration configuration)
		{
			//	No logging by default.
		}

		/// <remarks>
		/// This method is used for Unit Test to check whether specific types (not instantiated by Composition Root) could be resolved.
		/// </remarks>
		protected T Resolve<T>()
		{
			return serviceProvider.GetRequiredService<T>();
		}
	}
}
