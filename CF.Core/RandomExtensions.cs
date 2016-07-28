using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Core
{
	/// <summary>
	/// Holder for Random class extension methods.
	/// </summary>
	public static class RandomExtensions
	{
		private const string RandomStringCharacterSet = @"$%#@!*abcdefghijklmnopqrstuvwxyz1234567890?;:ABCDEFGHIJKLMNOPQRSTUVWXYZ^&/\";
		private const string RandomUriDomainPartCharacterSet = @"abcdefghijklmnopqrstuvwxyz1234567890";
		private const int DefaultMaxRandomStringLength = 256;
		private const int MaxGeneratedDomainPartLength = 32;

		/// <summary>
		/// Returns a non-negative random long number.
		/// </summary>
		public static long NextLong(this Random rnd)
		{
			return rnd.NextLong(long.MaxValue);
		}

		/// <summary>
		/// Returns a non-negative random long number that is less than the specified maximum.
		/// </summary>
		public static long NextLong(this Random rnd, long maxValue)
		{
			return rnd.NextLong(0, maxValue);
		}

		/// <summary>
		/// Returns a non-negative random long number within a specified range.
		/// </summary>
		public static long NextLong(this Random rnd, long minValue, long maxValue)
		{
			if (minValue > maxValue)
			{
				throw new ArgumentOutOfRangeException();
			}
			else if (minValue == maxValue)
			{
				return minValue;
			}

			byte[] buf = new byte[8];
			rnd.NextBytes(buf);

			return Math.Abs(BitConverter.ToInt64(buf, 0)) % (maxValue - minValue) + minValue;
		}

		/// <summary>
		/// Returns random string of length between 0 and 256 characters.
		/// </summary>
		public static string NextString(this Random rnd)
		{
			return rnd.NextString(0);
		}

		/// <summary>
		/// Returns random string of length that is not less than the specified minimum.
		/// </summary>
		public static string NextString(this Random rnd, int minLength)
		{
			return rnd.NextString(minLength, DefaultMaxRandomStringLength);
		}

		/// <summary>
		/// Returns random string of length within a specified range.
		/// </summary>
		public static string NextString(this Random rnd, int minLength, int maxLength)
		{
			return rnd.NextString(RandomStringCharacterSet, minLength, maxLength);
		}

		/// <summary>
		/// Returns random string in a given characters set of length within a specified range.
		/// </summary>
		public static string NextString(this Random rnd, string charSet, int minLength, int maxLength)
		{
			if (minLength < 0 || maxLength < 0 || minLength > maxLength)
			{
				throw new ArgumentOutOfRangeException();
			}

			return new string(Enumerable.Repeat(charSet, rnd.Next(minLength, maxLength)).
				Select(s => s[rnd.Next(s.Length)]).ToArray());
		}

		/// <summary>
		/// Returns random DateTime object.
		/// </summary>
		public static DateTime NextDateTime(this Random rnd)
		{
			return new DateTime(rnd.NextLong(DateTime.MinValue.Ticks, DateTime.MaxValue.Ticks));
		}

		/// <summary>
		/// Returns random Uri object.
		/// </summary>
		public static Uri NextUri(this Random rnd)
		{
			return new Uri($"http://{GenerateDomainPart(rnd)}.{GenerateDomainPart(rnd)}.{GenerateDomainPart(rnd)}/{rnd.NextString()}");
		}

		private static string GenerateDomainPart(Random rnd)
		{
			return rnd.NextString(RandomUriDomainPartCharacterSet, 1, MaxGeneratedDomainPartLength);
		}
	}
}
