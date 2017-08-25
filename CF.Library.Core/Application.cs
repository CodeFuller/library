using System;
using CF.Library.Core.Logging;

namespace CF.Library.Core
{
	/// <summary>
	/// Holder for cross-cutting concerns.
	/// </summary>
	public static class Application
	{
		private static IMessageLogger logger;

		/// <summary>
		/// Cross-cutting concern for the logging.
		/// </summary>
		public static IMessageLogger Logger
		{
			get
			{
				if (logger == null)
				{
					throw new InvalidOperationException("Logger is not set");
				}

				return logger;
			}

			set { logger = value; }
		}
	}
}
