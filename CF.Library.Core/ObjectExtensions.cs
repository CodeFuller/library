using System;

namespace CF.Library.Core
{
	/// <summary>
	/// Holder for Object extension methods.
	/// </summary>
	public static class ObjectExtensions
	{
		/// <summary>
		/// Safe ToString() that correctly handles null objects.
		/// </summary>
		public static string Stringify(this Uri uri)
		{
			return uri?.ToString() ?? "<null>";
		}
	}
}
