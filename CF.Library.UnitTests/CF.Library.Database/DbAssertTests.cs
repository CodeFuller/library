using System;
using System.Data;
using CF.Library.Database;
using CF.Library.Database.Exceptions;
using NSubstitute;
using NUnit.Framework;

namespace CF.Library.UnitTests.CF.Library.Database
{
	[TestFixture]
	public class DbAssertTests
	{
		[Test]
		public void AssertNoMoreData_WhenReaderArgumentIsNull_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => DbAssert.NoMoreData(null));
		}

		[Test]
		public void AssertNoMoreData_CalledForReaderWithoutData_PassesSuccessfully()
		{
			IDataReader reader = Substitute.For<IDataReader>();
			reader.Read().Returns(false);

			Assert.DoesNotThrow(() => DbAssert.NoMoreData(reader));
		}

		[Test]
		public void AssertNoMoreData_CalledForReaderWithData_ThrowsExtraDbDataException()
		{
			IDataReader reader = Substitute.For<IDataReader>();
			reader.Read().Returns(true);

			Assert.Throws<ExtraDbDataException>(() => DbAssert.NoMoreData(reader));
		}

	}
}
