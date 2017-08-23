using System;
using static CF.Library.Core.Extensions.FormattableStringExtensions;

namespace CF.Library.Core.Logging
{
	/// <summary>
	/// Implementation of IMessageLogger that writes messages to console.
	/// </summary>
	public class ConsoleLogger : IMessageLogger
	{
		private readonly bool colorOutput;

		/// <summary>
		/// Constructor.
		/// </summary>
		public ConsoleLogger(bool colorOutput)
		{
			this.colorOutput = colorOutput;
		}

		/// <summary>
		/// Writes debug message.
		/// </summary>
		public void WriteDebug(string message)
		{
			Write("DEBUG", ConsoleColor.Gray, message);
		}

		/// <summary>
		/// Writes info message.
		/// </summary>
		public void WriteInfo(string message)
		{
			Write("INFO", ConsoleColor.White, message);
		}

		/// <summary>
		/// Writes warning message.
		/// </summary>
		public void WriteWarning(string message)
		{
			Write("WARNING", ConsoleColor.Yellow, message);
		}

		/// <summary>
		/// Writes error message.
		/// </summary>
		public void WriteError(string message)
		{
			Write("ERROR", ConsoleColor.Red, message);
		}

		private void Write(string level, ConsoleColor? color, string message)
		{
			bool changedColor = false;
			if (colorOutput && color != null)
			{
				Console.ForegroundColor = color.Value;
				changedColor = true;
			}

			var formattedMessage = Current($"{DateTime.Now:yyyy-MM-dd  HH:mm:ss}    {level + ":",-8}    {message}");
			Console.WriteLine(formattedMessage);

			if (changedColor)
			{
				Console.ResetColor();
			}
		}
	}
}
