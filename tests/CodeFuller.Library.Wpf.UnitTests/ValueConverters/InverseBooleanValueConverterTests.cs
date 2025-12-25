using System.Globalization;
using System.Windows;
using CodeFuller.Library.Wpf.ValueConverters;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeFuller.Library.Wpf.UnitTests.ValueConverters
{
	[TestClass]
	public class InverseBooleanValueConverterTests
	{
		[TestMethod]
		public void Convert_IfParameterTypesAreCorrectAndValueIsTrue_ReturnsFalse()
		{
			// Arrange

			var target = new InverseBooleanValueConverter();

			// Act

			var converted = target.Convert(true, typeof(bool), null, CultureInfo.InvariantCulture);

			// Assert

			converted.Should().Be(false);
		}

		[TestMethod]
		public void Convert_IfParameterTypesAreCorrectAndValueIsFalse_ReturnsTrue()
		{
			// Arrange

			var target = new InverseBooleanValueConverter();

			// Act

			var converted = target.Convert(false, typeof(bool), null, CultureInfo.InvariantCulture);

			// Assert

			converted.Should().Be(true);
		}

		[TestMethod]
		public void Convert_IfParameterTypesAreCorrectAndValueIsNull_ReturnsUnsetValue()
		{
			// Arrange

			var target = new InverseBooleanValueConverter();

			// Act

			var converted = target.Convert(null, typeof(bool), null, CultureInfo.InvariantCulture);

			// Assert

			converted.Should().Be(DependencyProperty.UnsetValue);
		}

		[TestMethod]
		public void Convert_IfValueIsNotBoolean_ReturnsUnsetValue()
		{
			// Arrange

			var target = new InverseBooleanValueConverter();

			// Act

			var converted = target.Convert(5, typeof(bool), null, CultureInfo.InvariantCulture);

			// Assert

			converted.Should().Be(DependencyProperty.UnsetValue);
		}

		[TestMethod]
		public void Convert_IfTargetTypeIsNotBoolean_ReturnsUnsetValue()
		{
			// Arrange

			var target = new InverseBooleanValueConverter();

			// Act

			var converted = target.Convert(true, typeof(object), null, CultureInfo.InvariantCulture);

			// Assert

			converted.Should().Be(DependencyProperty.UnsetValue);
		}
	}
}
