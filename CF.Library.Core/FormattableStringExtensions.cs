using System;
using System.Globalization;

namespace CF.Library.Core
{
	/// <summary>
	/// Holder for FormattableString class extension methods.
	/// </summary>
	public static class FormattableStringExtensions
	{
		/// <summary>
		/// Returns a result string in which arguments are formatted by using the conventions of CurtureInfo.CurrentCulture.
		/// </summary>
		/// <remarks>
		/// Copy/Paste from http://blog.jessehouwing.nl/2015/09/c-6-interpolated-strings-and-code.html
		/// </remarks>
		public static string Current(FormattableString formattable)
		{
			if (formattable == null)
			{
				throw new ArgumentNullException(nameof(formattable));
			}

			return formattable.ToString(CultureInfo.CurrentCulture);
		}
	}
}
