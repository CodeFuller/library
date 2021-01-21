using System;
using System.Collections.Generic;
using CodeFuller.Library.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeFuller.Library.Core.UnitTests.Extensions
{
	[TestClass]
	public class DictionaryExtensionsTests
	{
		[TestMethod]
		public void ProvideValue_WhenValueExistInDictionary_DoesNotAffectDictionary()
		{
			Dictionary<string, object> dict = new Dictionary<string, object>();
			object existingValue = new object();
			dict.Add("key", existingValue);

			dict.ProvideValue("key");

			Assert.AreEqual(1, dict.Count);
			Assert.AreSame(existingValue, dict["key"]);
		}

		[TestMethod]
		public void ProvideValue_WhenValueExistInDictionary_ReturnsExistingValue()
		{
			Dictionary<string, object> dict = new Dictionary<string, object>();
			object existingValue = new object();
			dict.Add("key", existingValue);

			object returnedValue = dict.ProvideValue("key");

			Assert.AreSame(returnedValue, existingValue);
		}

		[TestMethod]
		public void ProvideValue_WhenValueDoesNotExistInDictionary_AddsValueToDictionary()
		{
			Dictionary<string, object> dict = new Dictionary<string, object>();

			dict.ProvideValue("key");

			Assert.AreEqual(1, dict.Count);
			Assert.IsTrue(dict.ContainsKey("key"));
		}

		[TestMethod]
		public void ProvideValue_WhenValueDoesNotExistInDictionary_ReturnsAddedValue()
		{
			Dictionary<string, object> dict = new Dictionary<string, object>();

			object returnedValue = dict.ProvideValue("key");

			Assert.AreSame(dict["key"], returnedValue);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ProvideValue_WhenDictArgumentIsNull_ThrowsArgumentNullException()
		{
			((Dictionary<string, object>) null).ProvideValue("key");
		}
	}
}
