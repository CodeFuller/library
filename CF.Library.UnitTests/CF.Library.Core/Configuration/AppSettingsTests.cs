using System;
using CF.Library.Core.Configuration;
using NSubstitute;
using NUnit.Framework;

namespace CF.Library.UnitTests.CF.Library.Core.Configuration
{
	[TestFixture]
	public class AppSettingsTests
	{
		[SetUp]
		public void SetUp()
		{
			AppSettings.ResetSettingsProvider();
		}

		[TearDown]
		public void TearDown()
		{
			AppSettings.ResetSettingsProvider();
		}

		[Test]
		public void SettingsProviderGetter_WhenNotSet_ThrowsInvalidOperationException()
		{
			ISettingsProvider provider;
			Assert.Throws<InvalidOperationException>(() => provider = AppSettings.SettingsProvider);
		}

		[Test]
		public void SettingsProviderSetter_WhenAlreadytSet_ThrowsInvalidOperationException()
		{
			AppSettings.SettingsProvider = Substitute.For<ISettingsProvider>();

			Assert.Throws<InvalidOperationException>(() => AppSettings.SettingsProvider = Substitute.For<ISettingsProvider>());
		}

		[Test]
		public void GetRequiredValue_ReturnsRequiredValueFromProvider()
		{
			//	Arrange

			ISettingsProvider settingsProviderMock = Substitute.For<ISettingsProvider>();
			settingsProviderMock.GetRequiredValue<string>("SomeSettingKey").Returns("SomeSettingValue");
			AppSettings.SettingsProvider = settingsProviderMock;

			//	Act

			var returnedValue = AppSettings.GetRequiredValue<string>("SomeSettingKey");

			//	Assert

			Assert.AreEqual("SomeSettingValue", returnedValue);
		}

		[Test]
		public void GetOptionalValue_ReturnsOptionalValueFromProvider()
		{
			//	Arrange

			ISettingsProvider settingsProviderMock = Substitute.For<ISettingsProvider>();
			settingsProviderMock.GetOptionalValue<string>("SomeSettingKey").Returns("SomeSettingValue");
			AppSettings.SettingsProvider = settingsProviderMock;

			//	Act

			var returnedValue = AppSettings.GetOptionalValue<string>("SomeSettingKey");

			//	Assert

			Assert.AreEqual("SomeSettingValue", returnedValue);
		}
	}
}
