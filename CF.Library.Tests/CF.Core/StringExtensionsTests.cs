using System.Text;
using CF.Core;
using NUnit.Framework;

namespace CF.Library.Tests.CF.Core
{
	[TestFixture]
	public class StringExtensionsTests
	{
		[Test]
		public void AppendDelimited_ForEmptyDestination_AddsStringWithoutDelimiter()
		{
			var sb = new StringBuilder();

			sb.AppendDelimited("SomeString", "; ");

			Assert.AreEqual(sb.ToString(), "SomeString");
		}

		[Test]
		public void AppendDelimited_ForNonEmptyDestination_AddsStringWithDelimiter()
		{
			var sb = new StringBuilder();

			sb.AppendDelimited("SomeString1", "; ");
			sb.AppendDelimited("SomeString2", "; ");

			Assert.AreEqual(sb.ToString(), "SomeString1; SomeString2");
		}
	}
}
