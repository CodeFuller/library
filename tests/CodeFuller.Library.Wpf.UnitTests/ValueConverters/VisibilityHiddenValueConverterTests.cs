using System.Globalization;
using System.Windows;
using CodeFuller.Library.Wpf.ValueConverters;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeFuller.Library.Wpf.UnitTests.ValueConverters
{
	[TestClass]
	public class VisibilityHiddenValueConverterTests
	{
		[TestMethod]
		public void Convert_IfParameterTypesAreCorrectAndValueIsTrue_ReturnsVisibleValue()
		{
			// Arrange

			var target = new VisibilityHiddenValueConverter();

			// Act

			var converted = target.Convert(true, typeof(Visibility), null, CultureInfo.InvariantCulture);

			// Assert

			converted.Should().Be(Visibility.Visible);
		}

		[TestMethod]
		public void Convert_IfParameterTypesAreCorrectAndValueIsFalse_ReturnsHiddenValue()
		{
			// Arrange

			var target = new VisibilityHiddenValueConverter();

			// Act

			var converted = target.Convert(false, typeof(Visibility), null, CultureInfo.InvariantCulture);

			// Assert

			converted.Should().Be(Visibility.Hidden);
		}

		[TestMethod]
		public void Convert_InInvertedModeIfParameterTypesAreCorrectAndValueIsTrue_ReturnsHiddenValue()
		{
			// Arrange

			var target = new VisibilityHiddenValueConverter
			{
				InvertedVisibility = true,
			};

			// Act

			var converted = target.Convert(true, typeof(Visibility), null, CultureInfo.InvariantCulture);

			// Assert

			converted.Should().Be(Visibility.Hidden);
		}

		[TestMethod]
		public void Convert_InInvertedModeIfParameterTypesAreCorrectAndValueIsFalse_ReturnsVisibleValue()
		{
			// Arrange

			var target = new VisibilityHiddenValueConverter
			{
				InvertedVisibility = true,
			};

			// Act

			var converted = target.Convert(false, typeof(Visibility), null, CultureInfo.InvariantCulture);

			// Assert

			converted.Should().Be(Visibility.Visible);
		}

		[TestMethod]
		public void Convert_IfParameterTypesAreCorrectAndValueIsNull_ReturnsUnsetValue()
		{
			// Arrange

			var target = new VisibilityHiddenValueConverter();

			// Act

			var converted = target.Convert(null, typeof(Visibility), null, CultureInfo.InvariantCulture);

			// Assert

			converted.Should().Be(DependencyProperty.UnsetValue);
		}

		[TestMethod]
		public void Convert_IfValueIsNotBoolean_ReturnsUnsetValue()
		{
			// Arrange

			var target = new VisibilityHiddenValueConverter();

			// Act

			var converted = target.Convert(5, typeof(Visibility), null, CultureInfo.InvariantCulture);

			// Assert

			converted.Should().Be(DependencyProperty.UnsetValue);
		}

		[TestMethod]
		public void Convert_IfTargetTypeIsNotVisibility_ReturnsUnsetValue()
		{
			// Arrange

			var target = new VisibilityHiddenValueConverter();

			// Act

			var converted = target.Convert(true, typeof(object), null, CultureInfo.InvariantCulture);

			// Assert

			converted.Should().Be(DependencyProperty.UnsetValue);
		}
	}
}
