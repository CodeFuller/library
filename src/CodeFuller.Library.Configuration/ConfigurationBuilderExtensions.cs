using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace CodeFuller.Library.Configuration
{
	/// <summary>
	/// Extension methods for filling <see cref="IConfigurationBuilder"/> with application settings.
	/// </summary>
	public static class ConfigurationBuilderExtensions
	{
		/// <summary>
		/// Adds configuration sources in the following order:
		/// <list type="number">
		/// <item><description>JSON files from provided config directory path (files are enumerated by *.json search pattern).</description></item>
		/// <item><description>JSON files specified as command line arguments (--config=path1 --config=path2 ...).</description></item>
		/// <item><description>Environment variables.</description></item>
		/// <item><description>Command line arguments (excluding --config values).</description></item>
		/// </list>
		/// </summary>
		/// <param name="configurationBuilder">The <see cref="IConfigurationBuilder"/> to add to.</param>
		/// <param name="configDirectoryPath">The path to directory with application configuration files.</param>
		/// <param name="commandLineArgs">Command line arguments.</param>
		/// <returns>The <see cref="IConfigurationBuilder"/> so that additional calls can be chained.</returns>
		public static IConfigurationBuilder LoadSettings(this IConfigurationBuilder configurationBuilder, string configDirectoryPath, string[] commandLineArgs)
		{
			AddJsonConfigs(configurationBuilder, configDirectoryPath);

			var restArgs = AddConfigFilesFromCommandLine(configurationBuilder, commandLineArgs);

			return configurationBuilder
				.AddEnvironmentVariables()
				.AddCommandLine(restArgs);
		}

		private static void AddJsonConfigs(IConfigurationBuilder configurationBuilder, string configDirectoryName)
		{
			var exeDirectoryPath = Path.GetDirectoryName(AppContext.BaseDirectory);

			var confDir = Path.Combine(exeDirectoryPath ?? String.Empty, configDirectoryName);
			var jsonConfigFiles = Directory.EnumerateFiles(confDir, "*.json", SearchOption.TopDirectoryOnly)
				.OrderBy(x => x);

			foreach (var configFile in jsonConfigFiles)
			{
				configurationBuilder.AddJsonFile(configFile, optional: false);
			}
		}

		private static string[] AddConfigFilesFromCommandLine(IConfigurationBuilder configurationBuilder, string[] args)
		{
			var configFileKeyRegex = new Regex("^--config=(.+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

			var remainingArgs = new List<string>();
			foreach (var arg in args)
			{
				var match = configFileKeyRegex.Match(arg);
				if (match.Success)
				{
					configurationBuilder.AddJsonFile(match.Groups[1].Value, optional: false);
				}
				else
				{
					remainingArgs.Add(arg);
				}
			}

			return remainingArgs.ToArray();
		}
	}
}
