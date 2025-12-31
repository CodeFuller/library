using System;
using CodeFuller.Library.Wpf.Extensions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeFuller.Library.Wpf.UnitTests.Extensions
{
	[TestClass]
	public class ObjectExtensionsTests
	{
		private sealed class ViewModel1
		{
		}

		private sealed class ViewModel2
		{
		}

		[TestMethod]
		public void GetViewModel_IfDataContextHasCorrectType_ReturnsStronglyTypedViewModel()
		{
			// Arrange

			var target = new ViewModel1();

			// Act

			var result = target.GetViewModel<ViewModel1>();

			// Assert

			result.Should().BeSameAs(target);
		}

		[TestMethod]
		public void GetViewModel_IfDataContextHasIncorrectType_ThrowsInvalidOperationException()
		{
			// Arrange

			var target = new ViewModel1();

			// Act

			var call = () => target.GetViewModel<ViewModel2>();

			// Assert

			call.Should().Throw<InvalidOperationException>()
				.And.Message.Should().Be("Unexpected type of DataContext: Expected CodeFuller.Library.Wpf.UnitTests.Extensions.ObjectExtensionsTests+ViewModel2, actual is CodeFuller.Library.Wpf.UnitTests.Extensions.ObjectExtensionsTests+ViewModel1");
		}

		[TestMethod]
		public void GetViewModel_IfDataContextIsNull_ThrowsArgumentNullException()
		{
			// Arrange

			ViewModel1 target = null;

			// Act

			var call = () => target.GetViewModel<ViewModel1>();

			// Assert

			call.Should().Throw<ArgumentNullException>();
		}
	}
}
