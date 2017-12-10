using System.Text;
using CF.Library.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CF.Library.Core.Tests.Extensions
{
	[TestClass]
	public class StringExtensionsTests
	{
		[TestMethod]
		public void AppendDelimited_ForEmptyDestination_AddsStringWithoutDelimiter()
		{
			var sb = new StringBuilder();

			sb.AppendDelimited("SomeString", "; ");

			Assert.AreEqual("SomeString", sb.ToString());
		}

		[TestMethod]
		public void AppendDelimited_ForNonEmptyDestination_AddsStringWithDelimiter()
		{
			var sb = new StringBuilder();

			sb.AppendDelimited("SomeString1", "; ");
			sb.AppendDelimited("SomeString2", "; ");

			Assert.AreEqual("SomeString1; SomeString2", sb.ToString());
		}
	}
}
