using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CF.Library.Logging.Tests
{
	[TestClass]
	public class LogHolderTests
	{
		[TestMethod]
		public void Logger_WhenNoLoggerIsSet_ReturnsNullLogger()
		{
			Assert.AreSame(NullLogger.Instance, LogHolder.Logger);
		}
	}
}
