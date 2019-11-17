using System;

namespace CF.Library.Core.Extensions
{
	/// <summary>
	/// Holder for Object extension methods.
	/// </summary>
	public static class ObjectExtensions
	{
		/// <summary>
		/// Safe ToString() that correctly handles null objects.
		/// </summary>
		public static string Stringify(this Object uri)
		{
			return uri?.ToString() ?? "<null>";
		}
	}
}
