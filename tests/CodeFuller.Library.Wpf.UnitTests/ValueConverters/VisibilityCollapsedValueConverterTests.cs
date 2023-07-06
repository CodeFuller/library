using System.Globalization;
using System.Windows;
using CodeFuller.Library.Wpf.ValueConverters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeFuller.Library.Wpf.UnitTests.ValueConverters
{
	[TestClass]
	public class VisibilityCollapsedValueConverterTests
	{
		[TestMethod]
		public void Convert_IfParameterTypesAreCorrectAndValueIsTrue_ReturnsVisibleValue()
		{
			// Arrange

			var target = new VisibilityCollapsedValueConverter();

			// Act

			var converted = target.Convert(true, typeof(Visibility), null, CultureInfo.InvariantCulture);

			// Assert

			Assert.AreEqual(Visibility.Visible, converted);
		}

		[TestMethod]
		public void Convert_IfParameterTypesAreCorrectAndValueIsFalse_ReturnsCollapsedValue()
		{
			// Arrange

			var target = new VisibilityCollapsedValueConverter();

			// Act

			var converted = target.Convert(false, typeof(Visibility), null, CultureInfo.InvariantCulture);

			// Assert

			Assert.AreEqual(Visibility.Collapsed, converted);
		}

		[TestMethod]
		public void Convert_InInvertedModeIfParameterTypesAreCorrectAndValueIsTrue_ReturnsCollapsedValue()
		{
			// Arrange

			var target = new VisibilityCollapsedValueConverter
			{
				InvertedVisibility = true,
			};

			// Act

			var converted = target.Convert(true, typeof(Visibility), null, CultureInfo.InvariantCulture);

			// Assert

			Assert.AreEqual(Visibility.Collapsed, converted);
		}

		[TestMethod]
		public void Convert_InInvertedModeIfParameterTypesAreCorrectAndValueIsFalse_ReturnsVisibleValue()
		{
			// Arrange

			var target = new VisibilityCollapsedValueConverter
			{
				InvertedVisibility = true,
			};

			// Act

			var converted = target.Convert(false, typeof(Visibility), null, CultureInfo.InvariantCulture);

			// Assert

			Assert.AreEqual(Visibility.Visible, converted);
		}

		[TestMethod]
		public void Convert_IfParameterTypesAreCorrectAndValueIsNull_ReturnsUnsetValue()
		{
			// Arrange

			var target = new VisibilityCollapsedValueConverter();

			// Act

			var converted = target.Convert(null, typeof(Visibility), null, CultureInfo.InvariantCulture);

			// Assert

			Assert.AreEqual(DependencyProperty.UnsetValue, converted);
		}

		[TestMethod]
		public void Convert_IfValueIsNotNullableBoolean_ReturnsUnsetValue()
		{
			// Arrange

			var target = new VisibilityCollapsedValueConverter();

			// Act

			var converted = target.Convert(5, typeof(Visibility), null, CultureInfo.InvariantCulture);

			// Assert

			Assert.AreEqual(DependencyProperty.UnsetValue, converted);
		}

		[TestMethod]
		public void Convert_IfTargetTypeIsNotVisibility_ReturnsUnsetValue()
		{
			// Arrange

			var target = new VisibilityCollapsedValueConverter();

			// Act

			var converted = target.Convert(true, typeof(object), null, CultureInfo.InvariantCulture);

			// Assert

			Assert.AreEqual(DependencyProperty.UnsetValue, converted);
		}
	}
}
