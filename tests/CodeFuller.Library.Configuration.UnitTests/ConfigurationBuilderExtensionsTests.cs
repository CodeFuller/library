using System;
using System.IO;
using FluentAssertions;
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
			configuration["someSection:setting1"].Should().Be("Value from config1.json");
			configuration["someSection:setting2"].Should().Be("Value from config2.json");
			configuration["someSection:setting3"].Should().Be("Value from external_config.json");
			configuration["someSection:setting4"].Should().Be("Value from environment variable");
			configuration["someSection:setting5"].Should().Be("Value from command line");
		}

		[TestMethod]
		public void LoadSettings_ConfigDirectoryDoesNotExist_ThrowsDirectoryNotFoundException()
		{
			// Arrange

			var configurationBuilder = new ConfigurationBuilder();

			// Act

			var call = () => configurationBuilder.LoadSettings("missing_directory", Array.Empty<string>());

			// Assert

			call.Should().Throw<DirectoryNotFoundException>();
		}

		[TestMethod]
		public void LoadSettings_ConfigFileFromCommandLineDoesNotExist_ThrowsFileNotFoundException()
		{
			// Arrange

			const string configDirectoryPath = "test_conf";

			// Sanity check
			Directory.Exists(configDirectoryPath).Should().BeTrue();

			var configurationBuilder = new ConfigurationBuilder();

			// Act

			configurationBuilder.LoadSettings(configDirectoryPath, ["--config=missing_config.json"]);

			// Assert

			var call = () => configurationBuilder.Build();
			call.Should().Throw<FileNotFoundException>();
		}
	}
}
