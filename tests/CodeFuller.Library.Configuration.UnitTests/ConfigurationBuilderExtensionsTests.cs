using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeFuller.Library.Configuration.UnitTests
{
	[TestClass]
	public class ConfigurationBuilderExtensionsTests
	{
		[TestMethod]
		public void LoadSettings_LoadsSettingsInCorrectOrder()
		{
			// Arrange

			var commandLineArgs = new[]
			{
				"Some Arg",
				"someSection:setting5=Value from command line",
				"--config=external_config.json",
			};

			Environment.SetEnvironmentVariable("someSection:setting4", "Value from environment variable");
			Environment.SetEnvironmentVariable("someSection:setting5", "Value from environment variable");

			var configurationBuilder = new ConfigurationBuilder();

			// Act

			configurationBuilder.LoadSettings("test_conf", commandLineArgs);

			// Assert

			var configuration = configurationBuilder.Build();
			Assert.AreEqual("Value from config1.json", configuration["someSection:setting1"]);
			Assert.AreEqual("Value from config2.json", configuration["someSection:setting2"]);
			Assert.AreEqual("Value from external_config.json", configuration["someSection:setting3"]);
			Assert.AreEqual("Value from environment variable", configuration["someSection:setting4"]);
			Assert.AreEqual("Value from command line", configuration["someSection:setting5"]);
		}

		[TestMethod]
		public void LoadSettings_ConfigDirectoryDoesNotExist_ThrowsDirectoryNotFoundException()
		{
			// Arrange

			var configurationBuilder = new ConfigurationBuilder();

			// Act

			void Call() => configurationBuilder.LoadSettings("missing_directory", Array.Empty<string>());

			// Assert

			Assert.ThrowsException<DirectoryNotFoundException>(Call);
		}

		[TestMethod]
		public void LoadSettings_ConfigFileFromCommandLineDoesNotExist_ThrowsFileNotFoundException()
		{
			// Arrange

			const string configDirectoryPath = "test_conf";

			// Sanity check
			Assert.IsTrue(Directory.Exists(configDirectoryPath));

			var configurationBuilder = new ConfigurationBuilder();

			// Act

			configurationBuilder.LoadSettings(configDirectoryPath, ["--config=missing_config.json"]);

			// Assert

			Assert.ThrowsException<FileNotFoundException>(() => configurationBuilder.Build());
		}
	}
}
