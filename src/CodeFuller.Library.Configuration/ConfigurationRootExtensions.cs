using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace CodeFuller.Library.Configuration
{
	/// <summary>
	/// Extension methods <see cref="IConfigurationRoot"/>.
	/// </summary>
	public static class ConfigurationRootExtensions
	{
		private static readonly Regex ArrayKeyRegex = new($@"^(.+){ConfigurationPath.KeyDelimiter}\d+$", RegexOptions.Compiled);

		/// <summary>
		/// This extension method fixes the problem of overriding array settings.
		/// Config1: "someArray": [ "*.txt", "*.docx" ]
		/// Config2: "someArray": [ "*.jpg" ]
		/// Expected value: [ "*.jpg" ]
		/// Actual value (without a fix): [ "*.jpg", "*.docx" ]
		/// The solution is based on SO answer https://stackoverflow.com/a/47647485/5740031
		/// Note: The solution works only for array of scalar values (e.g. string or number).
		/// It will not work for array of objects.
		/// </summary>
		/// <param name="configurationRoot">The <see cref="IConfigurationRoot"/> for applying the fix.</param>
		/// <returns>The <see cref="IConfigurationRoot"/> so that additional calls can be chained.</returns>
		public static IConfigurationRoot FixOverridenArrays(this IConfigurationRoot configurationRoot)
		{
			var knownArrayKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

			// Enumerating providers in reverse order (the latest one wins).
			foreach (var provider in configurationRoot.Providers.Reverse())
			{
				var currProviderArrayKeys = new HashSet<string>();

				foreach (var key in GetProviderKeys(provider, null).Reverse())
				{
					var match = ArrayKeyRegex.Match(key);
					if (!match.Success)
					{
						continue;
					}

					var arrayKey = match.Groups[1].Value;
					if (knownArrayKeys.Contains(arrayKey))
					{
						ClearProviderKey(provider, key);
					}
					else
					{
						currProviderArrayKeys.Add(arrayKey);
					}
				}

				foreach (var key in currProviderArrayKeys)
				{
					knownArrayKeys.Add(key);
				}
			}

			return configurationRoot;
		}

		private static IEnumerable<string> GetProviderKeys(IConfigurationProvider provider, string parentPath)
		{
			var prefix = parentPath == null
				? string.Empty
				: parentPath + ConfigurationPath.KeyDelimiter;

			var keys = new List<string>();

			var childKeys = provider.GetChildKeys(Enumerable.Empty<string>(), parentPath)
				.Distinct()
				.Select(k => prefix + k).ToList();
			keys.AddRange(childKeys);

			foreach (var key in childKeys)
			{
				keys.AddRange(GetProviderKeys(provider, key));
			}

			return keys;
		}

		private static void ClearProviderKey(IConfigurationProvider provider, string key)
		{
			var providerType = provider.GetType();

			if (provider is not ConfigurationProvider)
			{
				throw new InvalidOperationException($"Fixing overriden configuration arrays is not supported for configuration provider {providerType}");
			}

			var dataPropertyInfo = providerType.GetProperty("Data", BindingFlags.Instance | BindingFlags.NonPublic);
			if (dataPropertyInfo == null)
			{
				throw new InvalidOperationException("Data property is missing in configuration provider");
			}

			var dataPropertyValue = dataPropertyInfo.GetValue(provider);
			if (dataPropertyValue is not IDictionary<string, string> dataDictionary)
			{
				throw new InvalidOperationException($"Data property has unexpected value in configuration provider: {dataPropertyValue?.GetType()}");
			}

			dataDictionary.Remove(key);
		}
	}
}
