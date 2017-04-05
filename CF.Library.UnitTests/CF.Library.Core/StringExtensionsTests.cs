using System.Text;
using CF.Library.Core.Extensions;
using NUnit.Framework;

namespace CF.Library.UnitTests.CF.Library.Core
{
	[TestFixture]
	public class StringExtensionsTests
	{
		[Test]
		public void AppendDelimited_ForEmptyDestination_AddsStringWithoutDelimiter()
		{
			var sb = new StringBuilder();

			sb.AppendDelimited("SomeString", "; ");

			Assert.AreEqual("SomeString", sb.ToString());
		}

		[Test]
		public void AppendDelimited_ForNonEmptyDestination_AddsStringWithDelimiter()
		{
			var sb = new StringBuilder();

			sb.AppendDelimited("SomeString1", "; ");
			sb.AppendDelimited("SomeString2", "; ");

			Assert.AreEqual("SomeString1; SomeString2", sb.ToString());
		}
	}
}
