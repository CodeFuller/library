using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeFuller.Library.Bootstrap.UnitTests
{
	[TestClass]
	public class BasicApplicationBootstrapperTests
	{
		private sealed class TestApplicationSettings
		{
			public ICollection<string> TestSettings { get; } = new Collection<string>();
		}

		private sealed class TestApplication
		{
			public TestApplicationSettings Settings { get; set; }

			public TestApplication(IOptions<TestApplicationSettings> options)
			{
				Settings = options?.Value ?? throw new ArgumentNullException(nameof(options));
			}
		}

		private sealed class TestApplicationBootstrapper : BasicApplicationBootstrapper<TestApplication>
		{
			private readonly Action<IConfigurationBuilder> configurationSetup;

			public TestApplicationBootstrapper(Action<IConfigurationBuilder> configurationSetup)
			{
				this.configurationSetup = configurationSetup ?? throw new ArgumentNullException(nameof(configurationSetup));
			}

			protected override void BootstrapConfiguration(IConfigurationBuilder configurationBuilder, string[] commandLineArgs)
			{
				base.BootstrapConfiguration(configurationBuilder, commandLineArgs);

				configurationSetup?.Invoke(configurationBuilder);
			}

			protected override void RegisterServices(IServiceCollection services, IConfiguration configuration)
			{
				services.Configure<TestApplicationSettings>(configuration.Bind);

				services.AddSingleton<TestApplication>();
			}
		}

		[TestMethod]
		public void Bootstrap_ForConfigurationWithOverwrittenArrayValues_UsesOnlyArrayValuesFromLastConfigurationProvider()
		{
			// Arrange

			static void SetupConfiguration(IConfigurationBuilder configurationBuilder)
			{
				configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
				{
					{ "testSettings:0", "value 1.1" },
					{ "testSettings:1", "value 1.2" },
					{ "testSettings:2", "value 1.3" },
				});

				configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
				{
					{ "testSettings:0", "value 2.1" },
					{ "testSettings:1", "value 2.2" },
				});

				configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
				{
					{ "otherSetting", "other value" },
				});
			}

			using var target = new TestApplicationBootstrapper(SetupConfiguration);

			// Act

			var testApplication = target.Bootstrap([]);

			// Assert

			var expectedSettings = new[]
			{
				"value 2.1",
				"value 2.2",
			};

			testApplication.Settings.TestSettings.Should().BeEquivalentTo(expectedSettings, x => x.WithStrictOrdering());
		}
	}
}
