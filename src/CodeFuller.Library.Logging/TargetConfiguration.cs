using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace CodeFuller.Library.Logging
{
	public class TargetConfiguration
	{
		public string Type { get; set; }

		public LogLevel? LogLevel { get; set; }

		public IDictionary<string, string> Settings { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
	}
}
