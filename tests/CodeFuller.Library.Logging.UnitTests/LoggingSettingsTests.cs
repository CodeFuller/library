using System.Linq;
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
			var configurationBuilder = new ConfigurationBuilder();
			configurationBuilder.AddJsonFile(@"conf\LogSettings1.json", optional: false)
				.AddJsonFile(@"conf\LogSettings2.json", optional: false);

			var configuration = configurationBuilder.Build();

			var loggingSettings = new LoggingSettings();

			// Act
			configuration.Bind("logging", loggingSettings);

			// Assert
			Assert.AreEqual(1, loggingSettings.Targets.Count);

			var targetSettings = loggingSettings.Targets.Single();
			var logPath = targetSettings.Settings.Get<string>("LoGpaTh");
			Assert.AreEqual("path2", logPath);
		}
	}
}
