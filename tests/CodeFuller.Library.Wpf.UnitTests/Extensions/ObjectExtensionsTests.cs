using System;
using CodeFuller.Library.Wpf.Extensions;
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

			Assert.AreSame(target, result);
		}

		[TestMethod]
		public void GetViewModel_IfDataContextHasIncorrectType_ThrowsInvalidOperationException()
		{
			// Arrange

			var target = new ViewModel1();

			// Act

			ViewModel2 Call() => target.GetViewModel<ViewModel2>();

			// Assert

			var exception = Assert.ThrowsException<InvalidOperationException>(Call);
			Assert.AreEqual("Unexpected type of DataContext: Expected CodeFuller.Library.Wpf.UnitTests.Extensions.ObjectExtensionsTests+ViewModel2, actual is CodeFuller.Library.Wpf.UnitTests.Extensions.ObjectExtensionsTests+ViewModel1", exception.Message);
		}

		[TestMethod]
		public void GetViewModel_IfDataContextIsNull_ThrowsArgumentNullException()
		{
			// Arrange

			ViewModel1 target = null;

			// Act

			ViewModel1 Call() => target.GetViewModel<ViewModel1>();

			// Assert

			Assert.ThrowsException<ArgumentNullException>(Call);
		}
	}
}
