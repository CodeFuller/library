using System;
using System.Text;

namespace CF.Library.Core.Extensions
{
	/// <summary>
	/// Holder for String and StringBuilder classes extension methods.
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// Appends strings to StringBuilder separating them with comma (and space)
		/// </summary>
		public static void AppendCommaSeparated(this StringBuilder sb, string value)
		{
			sb.AppendDelimited(value,  ", ");
		}

		/// <summary>
		/// Appends strings to StringBuilder separating them with specified delimiter.
		/// </summary>
		public static void AppendDelimited(this StringBuilder sb, string value, string delimiter)
		{
			if (sb == null)
			{
				throw new ArgumentNullException(nameof(sb));
			}

			if (sb.Length > 0)
			{
				sb.Append(delimiter);
			}
			sb.Append(value);
		}
	}
}
