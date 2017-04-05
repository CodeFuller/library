using System;

namespace CF.Library.Patterns
{
	/// <summary>
	/// Base class for passing event data when MultilayerEventManager is used
	/// </summary>
	public class MultilayerEventArgs : EventArgs
	{
		/// <summary>
		/// Indicates whether the event was marked as processed by some layer and should be skipped by all upper layers
		/// </summary>
		public bool Handled { get; set; }
	}
}
