using CF.Library.Core.Configuration;
using CF.Library.Core.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CF.Library.Core.Tests.Configuration
{
	[TestClass]
	public class DataSizeTests
	{
		[DataRow("0", 0)]
		[DataRow("12345", 12345)]
		[DataRow("2048 B", 2048)]
		[DataRow("12 KB", 12288)]
		[DataRow("64MB", 67108864)]
		[DataRow("2 gb", 2147483648)]
		[DataRow("1 Tb", 1099511627776)]
		[TestMethod]
		public void Constructor_ForValidDataSizeValue_ParsesDataSizeCorrectly(string dataSize, long expectedValue)
		{
			DataSize ds = new DataSize(dataSize);

			long value = ds;

			Assert.AreEqual(expectedValue, value);
		}

		[DataRow("Hello")]
		[DataRow("1.5 KB")]
		[DataRow("7 FB")]
		[DataRow("9223372036854775808")]
		[ExpectedException(typeof(InvalidConfigurationException))]
		[TestMethod]
		public void Constructor_ForInvalidDataSizeValue_ThrowsInvalidConfigurationException(string dataSize)
		{
			var ds = new DataSize(dataSize);
		}
	}
}
