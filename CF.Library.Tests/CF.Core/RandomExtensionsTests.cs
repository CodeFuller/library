using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using CF.Core;

namespace CF.Library.Tests
{
	class RandomLowerBound : Random
	{
		public override int Next()
		{
			return 0;
		}

		public override int Next(int maxValue)
		{
			return 0;
		}

		public override int Next(int minValue, int maxValue)
		{
			return minValue;
		}

		public override void NextBytes(byte[] buffer)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException(nameof(buffer));
			}

			for (var i = 0; i < buffer.Length; ++i)
			{
				buffer[i] = byte.MinValue;
			}
		}

		public override double NextDouble()
		{
			return 0;
		}

		protected override double Sample()
		{
			return 0;
		}
	}

	class RandomUpperBound : Random
	{
		public override int Next()
		{
			return int.MaxValue;
		}

		public override int Next(int maxValue)
		{
			if (maxValue < 1)
			{
				throw new ArgumentOutOfRangeException(nameof(maxValue));
			}
			return maxValue - 1;
		}

		public override int Next(int minValue, int maxValue)
		{
			if (maxValue < 1)
			{
				throw new ArgumentOutOfRangeException(nameof(maxValue));
			}
			return maxValue - 1;
		}

		public override void NextBytes(byte[] buffer)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException(nameof(buffer));
			}

			for (var i = 0; i < buffer.Length; ++i)
			{
				buffer[i] = byte.MaxValue;
			}
		}

		public override double NextDouble()
		{
			return 1;
		}

		protected override double Sample()
		{
			return 1;
		}
	}

	[TestFixture]
	public class RandomExtensionsTests
	{
		private const int TestRepeatsNumber = 10;

		private Random random;
		private Random lowerRandom;
		private Random upperRandom;

		[OneTimeSetUp]
		public void Init()
		{
			random = new Random();
			lowerRandom = new RandomLowerBound();
			upperRandom = new RandomUpperBound();
		}

		#region NextLong()

		[Test]
		[Repeat(TestRepeatsNumber)]
		public void NextLongWithoutArguments_ForUsualRandomGenerator_ReturnsNonNegativeNumber()
		{
			long rnd = random.NextLong();
			Assert.That(rnd >= 0);
		}
		[Test]
		public void NextLongWithoutArguments_ForLowerBoundRandomGenerator_ReturnsNonNegativeNumber()
		{
			long rnd = lowerRandom.NextLong();
			Assert.That(rnd == 0);
		}
		[Test]
		public void NextLongWithoutArguments_ForUpperBoundRandomGenerator_ReturnsNonNegativeNumber()
		{
			long rnd = upperRandom.NextLong();
			Assert.That(rnd >= 0);
		}

		[TestCase(1)]
		[TestCase(100)]
		[TestCase(long.MaxValue)]
		[Repeat(TestRepeatsNumber)]
		public void NextLongWithOneArgument_ForUsualRandomGenerator_ReturnsNonNegativeNumberLessThanGivenNumber(long maxValue)
		{
			long rnd = random.NextLong(maxValue);
			Assert.That(rnd >= 0);
			Assert.That(rnd < maxValue);
		}
		[TestCase(1)]
		[TestCase(100)]
		[TestCase(long.MaxValue)]
		public void NextLongWithOneArgument_ForLowerBoundRandomGenerator_ReturnsNonNegativeNumberLessThanGivenNumber(long maxValue)
		{
			long rnd = lowerRandom.NextLong(maxValue);
			Assert.That(rnd >= 0);
			Assert.That(rnd < maxValue);
		}
		[TestCase(1)]
		[TestCase(100)]
		[TestCase(long.MaxValue)]
		public void NextLongWithOneArgument_ForUpperBoundRandomGenerator_ReturnsNonNegativeNumberLessThanGivenNumber(long maxValue)
		{
			long rnd = upperRandom.NextLong(maxValue);
			Assert.That(rnd >= 0);
			Assert.That(rnd < maxValue);
		}
		[Test]
		public void NextLongWithOneArgument_ForMaxValueEqualToZero_ReturnsZero()
		{
			long rnd = random.NextLong(0);
			Assert.That(rnd == 0);
		}
		[Test]
		public void NextLongWithOneArgument_ForMaxValueLowerThanZero_ThrowsArgumentOutOfRangeException()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => random.NextLong(-1));
		}

		[TestCase(0, 1)]
		[TestCase(100, 200)]
		[TestCase(-200, -100)]
		[TestCase(long.MinValue, long.MaxValue)]
		[Repeat(TestRepeatsNumber)]
		public void NextLongWithTwoArguments_ForUsualRandomGenerator_ReturnsNumberInGivenRange(long minValue, long maxValue)
		{
			long rnd = random.NextLong(minValue, maxValue);
			Assert.That(rnd >= minValue);
			Assert.That(rnd < maxValue);
		}
		[TestCase(0, 1)]
		[TestCase(100, 200)]
		[TestCase(-200, -100)]
		[TestCase(long.MinValue, long.MaxValue)]
		public void NextLongWithTwoArguments_ForLowerBoundRandomGenerator_ReturnsNumberInGivenRange(long minValue, long maxValue)
		{
			long rnd = lowerRandom.NextLong(minValue, maxValue);
			Assert.That(rnd >= minValue);
			Assert.That(rnd < maxValue);
		}
		[TestCase(0, 1)]
		[TestCase(-200, -100)]
		[TestCase(long.MinValue, long.MaxValue)]
		public void NextLongWithTwoArguments_ForUpperBoundRandomGenerator_ReturnsNumberInGivenRange(long minValue, long maxValue)
		{
			long rnd = upperRandom.NextLong(minValue, maxValue);
			Assert.That(rnd >= minValue);
			Assert.That(rnd < maxValue);
		}
		[TestCase(10)]
		[TestCase(0)]
		[TestCase(-10)]
		public void NextLongWithTwoArguments_ForMinValueEqualToMaxValue_ReturnsMinValue(long value)
		{
			long rnd = random.NextLong(value, value);
			Assert.That(value == rnd);
		}
		[Test]
		public void NextLongWithTwoArguments_ForMinValueGreaterThanMaxValue_ThrowsArgumentOutOfRangeException()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => random.NextLong(11, 10));
		}

		#endregion

		#region NextString()

		[Test]
		[Repeat(TestRepeatsNumber)]
		public void NextStringWithoutArguments_ForUsualRandomGenerator_ReturnsRandomString()
		{
			random.NextString();
			//	No any Assert, just checking that method doesn't throw
		}
		[Test]
		public void NextStringWithoutArguments_ForLowerBoundRandomGenerator_ReturnsEmptyString()
		{
			string rnd = lowerRandom.NextString();
			Assert.IsEmpty(rnd);
		}
		[Test]
		public void NextStringWithoutArguments_ForUpperBoundRandomGenerator_ReturnsNonEmptyString()
		{
			string rnd = upperRandom.NextString();
			Assert.IsNotEmpty(rnd);
		}

		[TestCase(0)]
		[TestCase(10)]
		[Repeat(TestRepeatsNumber)]
		public void NextStringWithOneArgument_ForUsualRandomGenerator_ReturnsRandomStringLongerThanGivenLength(int minLength)
		{
			string rnd = random.NextString(minLength);
			Assert.That(rnd.Length >= minLength);
		}
		[TestCase(0)]
		[TestCase(10)]
		public void NextStringWithOneArgument_ForLowerBoundRandomGenerator_ReturnsRandomStringOfGivenLength(int minLength)
		{
			string rnd = lowerRandom.NextString(minLength);
			Assert.That(rnd.Length == minLength);
		}
		[TestCase(0)]
		[TestCase(10)]
		public void NextStringWithOneArgument_ForUpperBoundRandomGenerator_ReturnsRandomStringLongerThanGivenLength(int minLength)
		{
			string rnd = upperRandom.NextString(minLength);
			Assert.That(rnd.Length >= minLength);
		}
		[Test]
		public void NextStringWithOneArgument_ForNegativeMinLength_ThrowsArgumentOutOfRangeException()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => random.NextString(-1));
		}

		[TestCase(0, 1)]
		[TestCase(0, 10)]
		[TestCase(10, 11)]
		[TestCase(10, 20)]
		[Repeat(TestRepeatsNumber)]
		public void NextStringWithTwoArguments_ForUsualRandomGenerator_ReturnsStringWithLengthInGivenRange(int minLength, int maxLength)
		{
			string rnd = random.NextString(minLength, maxLength);
			Assert.That(rnd.Length >= minLength);
			Assert.That(rnd.Length < maxLength);
		}
		[TestCase(0, 1)]
		[TestCase(0, 10)]
		[TestCase(10, 11)]
		[TestCase(10, 20)]
		public void NextStringWithTwoArguments_ForLowerBoundRandomGenerator_ReturnsStringWithLengthInGivenRange(int minLength, int maxLength)
		{
			string rnd = lowerRandom.NextString(minLength, maxLength);
			Assert.That(rnd.Length == minLength);
		}
		[TestCase(0, 1)]
		[TestCase(0, 10)]
		[TestCase(10, 11)]
		[TestCase(10, 20)]
		public void NextStringWithTwoArguments_ForUpperBoundRandomGenerator_ReturnsStringWithLengthInGivenRange(int minLength, int maxLength)
		{
			string rnd = upperRandom.NextString(minLength, maxLength);
			Assert.That(rnd.Length >= minLength);
			Assert.That(rnd.Length < maxLength);
		}
		[Test]
		public void NextStringWithTwoArguments_ForMinLengthGreaterThanMaxLength_ThrowsArgumentOutOfRangeException()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => random.NextString(20, 10));
		}
		[Test]
		public void NextStringWithTwoArguments_ForNegativeMinLength_ThrowsArgumentOutOfRangeException()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => random.NextString(-20, 10));
		}

		#endregion

		#region NextDate()

		[Test]
		[Repeat(TestRepeatsNumber)]
		public void NextDateTime_ForUsualRandomGenerator_ReturnsValidDateTime()
		{
			DateTime rnd = random.NextDateTime();
			Assert.That(rnd >= DateTime.MinValue);
			Assert.That(rnd <= DateTime.MaxValue);
		}
		[Test]
		public void NextDateTime_ForLowerBoundRandomGenerator_ReturnsValidDateTime()
		{
			DateTime rnd = lowerRandom.NextDateTime();
			Assert.That(rnd == DateTime.MinValue);
		}
		[Test]
		public void NextDateTime_ForUpperBoundRandomGenerator_ReturnsValidDateTime()
		{
			DateTime rnd = upperRandom.NextDateTime();
			Assert.That(rnd >= DateTime.MinValue);
			Assert.That(rnd <= DateTime.MaxValue);
		}

		#endregion

		#region NextUri()

		[Test]
		[Repeat(TestRepeatsNumber)]
		public void NextUri_ForUsualRandomGenerator_ReturnsValidUri()
		{
			random.NextUri();

			//	No any Assert, just checking that method doesn't throw
			Assert.Pass();
		}
		[Test]
		public void NextUri_ForLowerBoundRandomGenerator_ReturnsValidUri()
		{
			lowerRandom.NextUri();

			//	No any Assert, just checking that method doesn't throw
			Assert.Pass();
		}
		[Test]
		public void NextUri_ForUpperBoundRandomGenerator_ReturnsValidUri()
		{
			upperRandom.NextUri();

			//	No any Assert, just checking that method doesn't throw
			Assert.Pass();
		}

		#endregion
	}
}
