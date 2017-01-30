using System;

namespace CF.Library.Core.Events
{
	/// <summary>
	/// Contains event data for ExceptionThrown event.
	/// </summary>
	public class ExceptionThrownEventArgs : EventArgs
	{
		/// <summary>
		/// Thrown exception.
		/// </summary>
		public Exception Exception { get; set; }

		/// <summary>
		/// Initializes new instance of ExceptionThrownEventArgs class with given exception.
		/// </summary>
		public ExceptionThrownEventArgs(Exception e)
		{
			Exception = e;
		}
	}
}
