using System.Collections.Generic;
using System.Linq;
using CodeFuller.Library.Logging.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeFuller.Library.Logging.UnitTests
{
	[TestClass]
	public class LoggingSettingsTests
	{
		[TestMethod]
		public void Bind_IfTargetConfigurationContainsKeysInDifferentCase_BindsSettingsCorrectly()
		{
			// Arrange
			var configurationBuilder = new ConfigurationBuilder()
				.AddInMemoryCollection(new Dictionary<string, string>
				{
					{ "logging:targets:0:settings:logPath", "path1" },
				})
				.AddInMemoryCollection(new Dictionary<string, string>
				{
					{ "logging:targets:0:settings:LogPath", "path2" },
				});

			var configuration = configurationBuilder.Build();

			var loggingSettings = new LoggingSettings();

			// Act

			configuration.Bind("logging", loggingSettings);

			// Assert

			Assert.AreEqual(1, loggingSettings.Targets.Count);

			var targetSettings = loggingSettings.Targets.Single();
			var logPath = targetSettings.Settings.GetOptionalSetting<string>("LoGpaTh");
			Assert.AreEqual("path2", logPath);
		}
	}
}
