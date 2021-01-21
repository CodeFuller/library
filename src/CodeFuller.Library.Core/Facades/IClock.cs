using System;

namespace CodeFuller.Library.Core.Facades
{
	/// <summary>
	/// Abstraction that provides current date and time.
	/// </summary>
	public interface IClock
	{
		/// <summary>
		/// Returns current date and time
		/// </summary>
		DateTime Now { get; }
	}
}
