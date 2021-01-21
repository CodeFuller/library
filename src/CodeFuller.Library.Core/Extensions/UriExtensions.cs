using System;
using System.Linq;
using System.Text;

namespace CodeFuller.Library.Core.Extensions
{
	/// <summary>
	/// Holder for Uri extension methods
	/// </summary>
	public static class UriExtensions
	{
		/// <summary>
		/// Charachter used as URI separator.
		/// </summary>
		public static char UriSeparatorChar => '/';

		/// <summary>
		/// String used as URI separator.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
		public static string UriSeparatorString => new string(UriSeparatorChar, 1);

		/// <summary>
		/// Returns value of UriKind for the Uri.
		/// </summary>
		public static UriKind GetUriKind(this Uri uri)
		{
			if (uri == null)
			{
				throw new ArgumentNullException(nameof(uri));
			}

			return uri.IsAbsoluteUri ? UriKind.Absolute : UriKind.Relative;
		}

		/// <summary>
		/// Analogue of Uri.Segments that supports relative URIs.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
		public static string[] SegmentsEx(this Uri uri)
		{
			if (uri == null)
			{
				throw new ArgumentNullException(nameof(uri));
			}

			var uriString = uri.ToString();
			if (uriString == UriSeparatorString)
			{
				return Array.Empty<string>();
			}

			return uriString.Split(UriSeparatorChar);
		}

		/// <summary>
		/// Removes last segment from the Uri.
		/// Works both for absolute and relative URIs.
		/// </summary>
		/// <example>
		/// Returns '/part1/part2' for '/part1/part2/part3'.
		/// Returns '/part1/part2/' for '/part1/part2/part3/'.
		/// </example>
		public static Uri RemoveLastSegment(this Uri uri)
		{
			var segments = uri.SegmentsEx().ToList();

			var removedSegment = uri.ToString().LastOrDefault() == UriSeparatorChar ? segments.Count - 2 : segments.Count - 1;
			if (removedSegment < segments.Count)
			{
				segments.RemoveAt(removedSegment);
			}

			return new Uri(String.Join(UriSeparatorString, segments), uri.GetUriKind());
		}

		/// <summary>
		/// Analogue of constructor Uri(Uri baseUri, Uri relativeUri) that supports relative URIs.
		/// </summary>
		/// <remarks>
		/// This method combines URIs as is without any slash separator modifications.
		/// </remarks>
		public static Uri Combine(this Uri baseUri, Uri relativeUri)
		{
			if (baseUri == null)
			{
				throw new ArgumentNullException(nameof(baseUri));
			}
			if (relativeUri == null)
			{
				throw new ArgumentNullException(nameof(relativeUri));
			}

			return baseUri.Combine(relativeUri.ToString());
		}

		/// <summary>
		/// Analogue of constructor Uri(Uri baseUri, string relativeUri) that supports relative URIs.
		/// </summary>
		/// <remarks>
		/// This method combines URIs as is without any slash separator modifications.
		/// </remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1057:StringUriOverloadsCallSystemUriOverloads")]
		public static Uri Combine(this Uri baseUri, string relativeUri)
		{
			if (baseUri == null)
			{
				throw new ArgumentNullException(nameof(baseUri));
			}
			if (relativeUri == null)
			{
				throw new ArgumentNullException(nameof(relativeUri));
			}

			bool addSlash = baseUri.ToString().LastOrDefault() != UriSeparatorChar &&
			                relativeUri.FirstOrDefault() != UriSeparatorChar;

			StringBuilder sb = new StringBuilder(baseUri.ToString());
			if (addSlash)
			{
				sb.Append(UriSeparatorChar);
			}
			sb.Append(relativeUri);

			return new Uri(sb.ToString(), baseUri.GetUriKind());
		}
	}
}
