using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CF.Library.Core.Exceptions;
using static System.FormattableString;

namespace CF.Library.Core.Configuration
{
	/// <summary>
	/// Class for parsing data size values like 256KB, 2GB, etc.
	/// </summary>
	public class DataSize
	{
		private static readonly Regex ValueRegex = new Regex(@"^(\d+)\s*(.*)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private static readonly Dictionary<string, long> SizeUnits = new Dictionary<string, long>
		{
			{ "",	1 },
			{ "B",	1 },
			{ "KB",	1 * 1024L },
			{ "MB",	1 * 1024L * 1024L },
			{ "GB", 1 * 1024L * 1024L * 1024L },
			{ "TB", 1 * 1024L * 1024L * 1024L * 1024L },
		};

		private readonly long size;

		/// <summary>
		/// Constructs instance from data size string.
		/// </summary>
		public DataSize(string sizeValue)
		{
			size = ParseSize(sizeValue);
		}

		/// <summary>
		/// Implicit conversion operator from DataSize to long.
		/// </summary>
		public static implicit operator long(DataSize ds)
		{
			return ds?.ToValue() ?? 0;
		}

		/// <summary>
		/// Converts data size value to long.
		/// </summary>
		public long ToValue()
		{
			return size;
		}

		private static long ParseSize(string sizeValue)
		{
			var match = ValueRegex.Match(sizeValue);
			if (!match.Success)
			{
				throw new InvalidConfigurationException(Invariant($"'{sizeValue}' is not a valid data size value"));
			}

			var unitsNumberValue = match.Groups[1].Value;
			long unitsNumber;
			if (!Int64.TryParse(unitsNumberValue, out unitsNumber))
			{
				throw new InvalidConfigurationException(Invariant($"Invalid data size number '{unitsNumberValue}'"));
			}

			var unitName = match.Groups[2].Value.ToUpperInvariant();
			long unitSize;
			if (!SizeUnits.TryGetValue(unitName, out unitSize))
			{
				throw new InvalidConfigurationException(Invariant($"Unknown data size unit '{unitName}'"));
			}

			return unitsNumber * unitSize;
		}
	}
}
