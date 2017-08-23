namespace CF.Library.Core.Logging
{
	/// <summary>
	/// Interface for the logger that supports different logging levels, e.g. info, warning, error...
	/// </summary>
	public interface IMessageLogger
	{
		/// <summary>
		/// Writes debug message.
		/// </summary>
		void WriteDebug(string message);

		/// <summary>
		/// Writes info message.
		/// </summary>
		void WriteInfo(string message);

		/// <summary>
		/// Writes warning message.
		/// </summary>
		void WriteWarning(string message);

		/// <summary>
		/// Writes error message.
		/// </summary>
		void WriteError(string message);
	}
}
