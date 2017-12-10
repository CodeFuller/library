using System;
using CF.Library.Core.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace CF.Library.Core.Tests.Configuration
{
	[TestClass]
	public class AppSettingsTests
	{
		[TestInitialize]
		public void SetUp()
		{
			AppSettings.ResetSettingsProvider();
		}

		[TestCleanup]
		public void TearDown()
		{
			AppSettings.ResetSettingsProvider();
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void SettingsProviderGetter_WhenNotSet_ThrowsInvalidOperationException()
		{
			ISettingsProvider provider = AppSettings.SettingsProvider;
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void SettingsProviderSetter_WhenAlreadytSet_ThrowsInvalidOperationException()
		{
			//	Arrange

			AppSettings.SettingsProvider = Substitute.For<ISettingsProvider>();

			//	Act & Assert

			AppSettings.SettingsProvider = Substitute.For<ISettingsProvider>();
		}

		[TestMethod]
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

		[TestMethod]
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
