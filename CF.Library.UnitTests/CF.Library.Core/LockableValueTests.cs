using System;
using CF.Library.Core;
using NUnit.Framework;

namespace CF.Library.UnitTests.CF.Library.Core
{
	[TestFixture]
	public class LockableValueTests
	{
		[Test]
		public void GetValue_ForInitializedValue_ReturnsKeptValue()
		{
			LockableValue<string> value = new LockableValue<string>("Somve value");

			Assert.AreEqual("Somve value", value.Value);
		}

		[Test]
		public void SetValue_OnUnlockedValue_SetsValue()
		{
			LockableValue<string> value = new LockableValue<string>("Somve value");

			value.Value = "Another value";

			Assert.AreEqual("Another value", value.Value);
		}

		[Test]
		public void SetValue_OnLockedValue_ThrowsInvalidOperationException()
		{
			LockableValue<string> value = new LockableValue<string>("Somve value");

			value.Lock();

			Assert.Throws<InvalidOperationException>(() => { value.Value = "Another value"; });
		}
	}
}
