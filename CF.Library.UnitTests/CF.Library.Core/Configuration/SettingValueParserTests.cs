using System;
using CF.Library.Core.Configuration;
using NUnit.Framework;

namespace CF.Library.UnitTests.CF.Library.Core.Configuration
{
	[TestFixture]
	public class SettingValueParserTests
	{
		[Test]
		public void Parse_ForIntValue_ParsesValueCorrectly()
		{
			SettingValueParser target = new SettingValueParser();

			int parsedValue = target.Parse<int>("123");

			Assert.AreEqual(123, parsedValue);
		}

		[Test]
		public void Parse_ForStringValue_ParsesValueCorrectly()
		{
			SettingValueParser target = new SettingValueParser();

			string parsedValue = target.Parse<string>("123");

			Assert.AreEqual("123", parsedValue);
		}

		[Test]
		public void Parse_WhenValueIsNull_ThrowsArgumentNullException()
		{
			SettingValueParser target = new SettingValueParser();

			Assert.Throws<ArgumentNullException>(() => target.Parse<string>(null));
		}
	}
}
