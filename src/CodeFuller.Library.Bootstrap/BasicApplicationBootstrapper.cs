using System;
using CodeFuller.Library.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CodeFuller.Library.Bootstrap
{
	/// <summary>
	/// Basic abstract class for application bootstrap.
	/// </summary>
	/// <typeparam name="TRoot"></typeparam>
	public abstract class BasicApplicationBootstrapper<TRoot> : IDisposable
	{
		private ServiceProvider ServiceProvider { get; set; }

		/// <summary>
		/// Bootstraps application and returns root object.
		/// </summary>
		/// <param name="args">Command line arguments.</param>
		/// <returns>Application root object.</returns>
		public virtual TRoot Bootstrap(string[] args)
		{
			var configuration = BootstrapConfiguration(args);

			// Registering ILoggerFactory via Func overload, so that it is disposed together with ServiceProvider.
			var services = new ServiceCollection()
				.AddSingleton<ILoggerFactory>(_ => BootstrapLogging(configuration))
				.AddSingleton(typeof(ILogger<>), typeof(Logger<>))
				.AddOptions();

			RegisterServices(services, configuration);

			ServiceProvider = services.BuildServiceProvider();

			return ServiceProvider.GetRequiredService<TRoot>();
		}

		/// <summary>
		/// Registers services required for application logic.
		/// </summary>
		/// <param name="services">The instance of <see cref="IServiceCollection"/> to add the service to.</param>
		/// <param name="configuration">Application configuration.</param>
		protected abstract void RegisterServices(IServiceCollection services, IConfiguration configuration);

		internal ILogger<TCategoryName> GetLogger<TCategoryName>()
		{
			return Resolve<ILogger<TCategoryName>>();
		}

		/// <summary>
		/// Bootstraps application configuration.
		/// </summary>
		/// <param name="commandLineArgs">Command line arguments.</param>
		/// <returns>Application configuration.</returns>
		protected virtual IConfiguration BootstrapConfiguration(string[] commandLineArgs)
		{
			var configurationBuilder = new ConfigurationBuilder();

			BootstrapConfiguration(configurationBuilder, commandLineArgs);

			return configurationBuilder.Build();
		}

		/// <summary>
		/// Bootstraps application configuration for existing instance of <see cref="IConfigurationBuilder"/>.
		/// </summary>
		/// <param name="configurationBuilder">The instance of <see cref="IConfigurationBuilder"/> for bootstrap.</param>
		/// <param name="commandLineArgs">Command line arguments.</param>
		protected virtual void BootstrapConfiguration(IConfigurationBuilder configurationBuilder, string[] commandLineArgs)
		{
			configurationBuilder.LoadSettings("conf", commandLineArgs);
		}

		/// <summary>
		/// Bootstraps application logging.
		/// </summary>
		/// <param name="configuration">Application configuration.</param>
		/// <returns>Configured instance of <see cref="ILoggerFactory"/>.</returns>
		protected virtual ILoggerFactory BootstrapLogging(IConfiguration configuration)
		{
			var loggerFactory = new LoggerFactory();

			BootstrapLogging(loggerFactory, configuration);

			return loggerFactory;
		}

		/// <summary>
		/// Bootstraps application logging for existing instance of <see cref="ILoggerFactory"/>.
		/// </summary>
		/// <param name="loggerFactory">The instance of <see cref="ILoggerFactory"/> for bootstrap.</param>
		/// <param name="configuration">Application configuration.</param>
		protected virtual void BootstrapLogging(ILoggerFactory loggerFactory, IConfiguration configuration)
		{
			// No logging by default.
		}

		/// <summary>
		/// This method is used for Unit Test to check whether specific types (not instantiated by Composition Root) could be resolved.
		/// </summary>
		/// <typeparam name="T">The type of service object to get.</typeparam>
		protected T Resolve<T>()
		{
			return ServiceProvider.GetRequiredService<T>();
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				ServiceProvider?.Dispose();
				ServiceProvider = null;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
