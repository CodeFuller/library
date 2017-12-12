﻿using System;
using System.Collections.Generic;
using CF.Library.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CF.Library.Core.Tests.Extensions
{
	[TestClass]
	public class CollectionExtensionsTests
	{
		[TestMethod]
		public void GetValueOrDefault_WhenValueExistInDictionary_ReturnsExistingValue()
		{
			Dictionary<string, object> dict = new Dictionary<string, object>();
			object existingValue = new object();
			dict.Add("key", existingValue);

			object returnedValue = dict.GetValueOrDefault("key");

			Assert.AreSame(existingValue, returnedValue);
		}

		[TestMethod]
		public void GetValueOrDefault_WhenValueDoesNotExistInDictionary_ReturnsDefaultTypeValue()
		{
			Dictionary<string, object> dict = new Dictionary<string, object>();

			object returnedValue = dict.GetValueOrDefault("key");

			Assert.IsTrue(EqualityComparer<object>.Default.Equals(returnedValue, default(object)));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetValueOrDefault_WhenDictArgumentIsNull_ThrowsArgumentNullException()
		{
			((Dictionary<string, object>) null).GetValueOrDefault("key");
		}

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