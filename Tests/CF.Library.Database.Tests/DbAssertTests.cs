using System;
using System.Data;
using CF.Library.Database.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace CF.Library.Database.Tests
{
	[TestClass]
	public class DbAssertTests
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AssertNoMoreData_WhenReaderArgumentIsNull_ThrowsArgumentNullException()
		{
			DbAssert.NoMoreData(null);
		}

		[TestMethod]
		public void AssertNoMoreData_CalledForReaderWithoutData_DoesNotThrow()
		{
			IDataReader reader = Substitute.For<IDataReader>();
			reader.Read().Returns(false);

			DbAssert.NoMoreData(reader);
		}

		[TestMethod]
		[ExpectedException(typeof(ExtraDbDataException))]
		public void AssertNoMoreData_CalledForReaderWithData_ThrowsExtraDbDataException()
		{
			IDataReader reader = Substitute.For<IDataReader>();
			reader.Read().Returns(true);

			DbAssert.NoMoreData(reader);
		}

	}
}
