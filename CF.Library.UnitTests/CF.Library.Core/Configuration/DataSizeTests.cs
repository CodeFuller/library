using CF.Library.Core.Configuration;
using CF.Library.Core.Exceptions;
using NUnit.Framework;

namespace CF.Library.UnitTests.CF.Library.Core.Configuration
{
	[TestFixture]
	public class DataSizeTests
	{
		[TestCase("0", 0)]
		[TestCase("12345", 12345)]
		[TestCase("2048 B", 2048)]
		[TestCase("12 KB", 12288)]
		[TestCase("64MB", 67108864)]
		[TestCase("2 gb", 2147483648)]
		[TestCase("1 Tb", 1099511627776)]
		public void Constructor_ForValidDataSizeValue_ParsesDataSizeCorrectly(string dataSize, long expectedValue)
		{
			DataSize ds = new DataSize(dataSize);

			long value = ds;

			Assert.AreEqual(expectedValue, value);
		}

		[TestCase("Hello")]
		[TestCase("1.5 KB")]
		[TestCase("7 FB")]
		[TestCase("9223372036854775808")]
		public void Constructor_ForInvalidDataSizeValue_ThrowsInvalidConfigurationException(string dataSize)
		{
			Assert.Throws<InvalidConfigurationException>(() => new DataSize(dataSize));
		}
	}
}
