using CF.Library.Core.Configuration;
using CF.Library.Core.Exceptions;
using NSubstitute;
using NUnit.Framework;

namespace CF.Library.UnitTests.CF.Library.Core.Configuration
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

	[TestFixture]
	public class BasicSettingsProviderTests
	{
		[Test]
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

		[Test]
		public void GetRequiredValue_WhenValueDoesNotExist_ThrowsRequiredSettingIsMissingException()
		{
			BasicSettingsProvider target = new DerivedSettingsProvider(null)
			{
				SettingValueParser = Substitute.For<ISettingValueParser>()
			};

			Assert.Throws<RequiredSettingIsMissingException>(() => target.GetRequiredValue<string>("SomeSettingKey"));
		}

		[Test]
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

		[Test]
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

		[Test]
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

		[Test]
		public void GetPrivateRequiredValue_WhenValueDoesNotExist_ThrowsRequiredSettingIsMissingException()
		{
			BasicSettingsProvider target = new DerivedSettingsProvider(null)
			{
				SettingValueParser = Substitute.For<ISettingValueParser>()
			};

			Assert.Throws<RequiredSettingIsMissingException>(() => target.GetPrivateRequiredValue<string>("SomeSettingKey"));
		}

		[Test]
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

		[Test]
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
