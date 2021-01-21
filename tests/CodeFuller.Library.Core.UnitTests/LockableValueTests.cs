using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeFuller.Library.Core.UnitTests
{
	[TestClass]
	public class LockableValueTests
	{
		[TestMethod]
		public void GetValue_ForInitializedValue_ReturnsKeptValue()
		{
			LockableValue<string> value = new LockableValue<string>("Somve value");

			Assert.AreEqual("Somve value", value.Value);
		}

		[TestMethod]
		public void SetValue_OnUnlockedValue_SetsValue()
		{
			LockableValue<string> value = new LockableValue<string>("Somve value");

			value.Value = "Another value";

			Assert.AreEqual("Another value", value.Value);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void SetValue_OnLockedValue_ThrowsInvalidOperationException()
		{
			LockableValue<string> value = new LockableValue<string>("Somve value");

			value.Lock();

			value.Value = "Another value";
		}
	}
}
