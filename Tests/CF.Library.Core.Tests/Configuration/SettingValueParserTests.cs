using System;
using CF.Library.Core.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CF.Library.Core.Tests.Configuration
{
	[TestClass]
	public class SettingValueParserTests
	{
		[TestMethod]
		public void Parse_ForIntValue_ParsesValueCorrectly()
		{
			SettingValueParser target = new SettingValueParser();

			int parsedValue = target.Parse<int>("123");

			Assert.AreEqual(123, parsedValue);
		}

		[TestMethod]
		public void Parse_ForStringValue_ParsesValueCorrectly()
		{
			SettingValueParser target = new SettingValueParser();

			string parsedValue = target.Parse<string>("123");

			Assert.AreEqual("123", parsedValue);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Parse_WhenValueIsNull_ThrowsArgumentNullException()
		{
			SettingValueParser target = new SettingValueParser();

			target.Parse<string>(null);
		}
	}
}
