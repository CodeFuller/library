using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace CodeFuller.Library.Configuration
{
	public static class ConfigurationBuilderExtensions
	{
		public static IConfigurationBuilder LoadSettings(this IConfigurationBuilder configurationBuilder, string configDirectoryName, string[] args)
		{
			AddJsonConfigs(configurationBuilder, configDirectoryName);

			var restArgs = AddConfigFilesFromCommandLine(configurationBuilder, args);

			return configurationBuilder
				.AddEnvironmentVariables()
				.AddCommandLine(restArgs);
		}

		private static void AddJsonConfigs(IConfigurationBuilder configurationBuilder, string configDirectoryName)
		{
			var exeDirectoryPath = Path.GetDirectoryName(AppContext.BaseDirectory);

			var confDir = Path.Combine(exeDirectoryPath ?? String.Empty, configDirectoryName);
			var jsonConfigFiles = Directory.EnumerateFiles(confDir, "*.json*", SearchOption.TopDirectoryOnly)
				.OrderBy(x => x);

			foreach (var configFile in jsonConfigFiles)
			{
				configurationBuilder.AddJsonFile(configFile, false);
			}
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
