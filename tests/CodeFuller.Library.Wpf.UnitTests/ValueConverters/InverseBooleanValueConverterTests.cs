using System.Globalization;
using System.Windows;
using CodeFuller.Library.Wpf.ValueConverters;
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

            Assert.AreEqual(false, converted);
        }

        [TestMethod]
        public void Convert_IfParameterTypesAreCorrectAndValueIsFalse_ReturnsTrue()
        {
            // Arrange

            var target = new InverseBooleanValueConverter();

            // Act

            var converted = target.Convert(false, typeof(bool), null, CultureInfo.InvariantCulture);

            // Assert

            Assert.AreEqual(true, converted);
        }

        [TestMethod]
        public void Convert_IfParameterTypesAreCorrectAndValueIsNull_ReturnsUnsetValue()
        {
            // Arrange

            var target = new InverseBooleanValueConverter();

            // Act

            var converted = target.Convert(null, typeof(bool), null, CultureInfo.InvariantCulture);

            // Assert

            Assert.AreEqual(DependencyProperty.UnsetValue, converted);
        }

        [TestMethod]
        public void Convert_IfValueIsNotBoolean_ReturnsUnsetValue()
        {
            // Arrange

            var target = new InverseBooleanValueConverter();

            // Act

            var converted = target.Convert(5, typeof(bool), null, CultureInfo.InvariantCulture);

            // Assert

            Assert.AreEqual(DependencyProperty.UnsetValue, converted);
        }

        [TestMethod]
        public void Convert_IfTargetTypeIsNotBoolean_ReturnsUnsetValue()
        {
            // Arrange

            var target = new InverseBooleanValueConverter();

            // Act

            var converted = target.Convert(true, typeof(object), null, CultureInfo.InvariantCulture);

            // Assert

            Assert.AreEqual(DependencyProperty.UnsetValue, converted);
        }
    }
}
