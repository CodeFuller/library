using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace CF.Library.Configuration
{
	public static class ConfigurationBuilderExtensions
	{
		private const string DefaultSettingsFileName = "AppSettings.json";

		public static IConfigurationBuilder LoadSettings(this IConfigurationBuilder configurationBuilder, string[] args)
		{
			return configurationBuilder.LoadSettings(DefaultSettingsFileName, true, args);
		}

		public static IConfigurationBuilder LoadSettings(this IConfigurationBuilder configurationBuilder, string configFileName, string[] args)
		{
			return configurationBuilder.LoadSettings(configFileName, false, args);
		}

		private static IConfigurationBuilder LoadSettings(this IConfigurationBuilder configurationBuilder,
			string configFileName, bool configFileIsOptional, string[] args)
		{
			configurationBuilder.AddJsonFile(configFileName, configFileIsOptional);

			args = AddConfigFilesFromCommandLine(configurationBuilder, args);
			return configurationBuilder.AddEnvironmentVariables()
				.AddCommandLine(args);
		}

		private static string[] AddConfigFilesFromCommandLine(IConfigurationBuilder configurationBuilder, string[] args)
		{
			var configFileKeyRegex = new Regex("^--config=(.+)$");

			var restArgs = new List<string>();
			foreach (var arg in args)
			{
				var match = configFileKeyRegex.Match(arg);
				if (match.Success)
				{
					configurationBuilder.AddJsonFile(match.Groups[1].Value, false);
				}
				else
				{
					restArgs.Add(arg);
				}
			}

			return restArgs.ToArray();
		}
	}
}
