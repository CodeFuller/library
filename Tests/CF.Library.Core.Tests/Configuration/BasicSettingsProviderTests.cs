using CF.Library.Core.Configuration;
using CF.Library.Core.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace CF.Library.Core.Tests.Configuration
{
	internal class DerivedSettingsProvider : BasicSettingsProvider
	{
		private readonly string returnedValue;

		public DerivedSettingsProvider(string returnedValue)
		{
			this.returnedValue = returnedValue;
		}

		protected override string GetSettingValue(string key)
		{
			return returnedValue;
		}

		protected override string GetPrivateSettingValue(string key)
		{
			return returnedValue;
		}
	}

	[TestClass]
	public class BasicSettingsProviderTests
	{
		[TestMethod]
		public void GetRequiredValue_WhenValueExists_ReturnsParsedValue()
		{
			//	Arrange

			ISettingValueParser settingValueParserMock = Substitute.For<ISettingValueParser>();

			settingValueParserMock.Parse<string>("SomeSettingValue").Returns("ParsedSettingValue");
			BasicSettingsProvider target = new DerivedSettingsProvider("SomeSettingValue")
			{
				SettingValueParser = settingValueParserMock
			};

			//	Act

			string returnedValue = target.GetRequiredValue<string>("SomeSettingKey");

			//	Assert

			Assert.AreEqual("ParsedSettingValue", returnedValue);
		}

		[TestMethod]
		[ExpectedException(typeof(RequiredSettingIsMissingException))]
		public void GetRequiredValue_WhenValueDoesNotExist_ThrowsRequiredSettingIsMissingException()
		{
			BasicSettingsProvider target = new DerivedSettingsProvider(null)
			{
				SettingValueParser = Substitute.For<ISettingValueParser>()
			};

			target.GetRequiredValue<string>("SomeSettingKey");
		}

		[TestMethod]
		public void GetOptionalValue_WhenValueExists_ReturnsParsedValue()
		{
			//	Arrange

			ISettingValueParser settingValueParserMock = Substitute.For<ISettingValueParser>();

			settingValueParserMock.Parse<string>("SomeSettingValue").Returns("ParsedSettingValue");
			BasicSettingsProvider target = new DerivedSettingsProvider("SomeSettingValue")
			{
				SettingValueParser = settingValueParserMock
			};

			//	Act

			string returnedValue = target.GetOptionalValue<string>("SomeSettingKey");

			//	Assert

			Assert.AreEqual("ParsedSettingValue", returnedValue);
		}

		[TestMethod]
		public void GetOptionalValue_WhenValueDoesNotExist_ReturnsNull()
		{
			//	Arrange

			BasicSettingsProvider target = new DerivedSettingsProvider(null)
			{
				SettingValueParser = Substitute.For<ISettingValueParser>()
			};

			//	Act

			string returnedValue = target.GetOptionalValue<string>("SomeSettingKey");

			//	Assert

			Assert.IsNull(returnedValue);
		}

		[TestMethod]
		public void GetPrivateRequiredValue_WhenValueExists_ReturnsParsedValue()
		{
			//	Arrange

			ISettingValueParser settingValueParserMock = Substitute.For<ISettingValueParser>();

			settingValueParserMock.Parse<string>("SomeSettingValue").Returns("ParsedSettingValue");
			BasicSettingsProvider target = new DerivedSettingsProvider("SomeSettingValue")
			{
				SettingValueParser = settingValueParserMock
			};

			//	Act

			string returnedValue = target.GetPrivateRequiredValue<string>("SomeSettingKey");

			//	Assert

			Assert.AreEqual("ParsedSettingValue", returnedValue);
		}

		[TestMethod]
		[ExpectedException(typeof(RequiredSettingIsMissingException))]
		public void GetPrivateRequiredValue_WhenValueDoesNotExist_ThrowsRequiredSettingIsMissingException()
		{
			BasicSettingsProvider target = new DerivedSettingsProvider(null)
			{
				SettingValueParser = Substitute.For<ISettingValueParser>()
			};

			target.GetPrivateRequiredValue<string>("SomeSettingKey");
		}

		[TestMethod]
		public void GetPrivateOptionalValue_WhenValueExists_ReturnsParsedValue()
		{
			//	Arrange

			ISettingValueParser settingValueParserMock = Substitute.For<ISettingValueParser>();

			settingValueParserMock.Parse<string>("SomeSettingValue").Returns("ParsedSettingValue");
			BasicSettingsProvider target = new DerivedSettingsProvider("SomeSettingValue")
			{
				SettingValueParser = settingValueParserMock
			};

			//	Act

			string returnedValue = target.GetPrivateOptionalValue<string>("SomeSettingKey");

			//	Assert

			Assert.AreEqual("ParsedSettingValue", returnedValue);
		}

		[TestMethod]
		public void GetPrivateOptionalValue_WhenValueDoesNotExist_ReturnsNull()
		{
			//	Arrange

			BasicSettingsProvider target = new DerivedSettingsProvider(null)
			{
				SettingValueParser = Substitute.For<ISettingValueParser>()
			};

			//	Act

			string returnedValue = target.GetPrivateOptionalValue<string>("SomeSettingKey");

			//	Assert

			Assert.IsNull(returnedValue);
		}
	}
}
