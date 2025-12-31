using System;
using System.Globalization;
using System.IO;
using System.Text;
using CodeFuller.Library.Logging.Interfaces;
using CodeFuller.Library.Logging.Internal;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CodeFuller.Library.Logging.UnitTests.Internal
{
	[TestClass]
	public class RollingLogFileTests
	{
		private static int Pid => Environment.ProcessId;

		[TestMethod]
		public void Write_ForFirstCall_CreatesLogDirectory()
		{
			// Arrange

			var fileSystemMock = CreateFileSystemMock();
			using var target = new RollingLogFile(@"c:\logs", "TestLog", "TestLog", "log", 128)
			{
				FileSystem = fileSystemMock.Object,
			};

			// Act

			target.Write("Some Message");

			// Assert

			fileSystemMock.Verify(x => x.CreateDirectory(@"c:\logs"), Times.Once);
		}

		[TestMethod]
		public void Write_IfCurrentLogIsSmallerThanRollSize_WritesToCurrentFile()
		{
			// Arrange

			var fileSystemMock = CreateFileSystemMock(0, 1024);
			using var target = new RollingLogFile(@"c:\logs", "TestLog", "TestLog", "log", 4096)
			{
				FileSystem = fileSystemMock.Object,
			};

			// Act

			target.Write("Message 1");
			target.Write("Message 2");

			// Assert

			fileSystemMock.Verify(x => x.CreateStreamWriter(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<Encoding>(), It.IsAny<bool>()), Times.Once);
		}

		[TestMethod]
		public void Write_IfCurrentLogIsBiggerThanRollSize_WritesToNewFile()
		{
			// Arrange

			var fileSystemMock = CreateFileSystemMock(0, 1024);
			using var target = new RollingLogFile(@"c:\logs", "TestLog", "TestLog", "log", 128)
			{
				FileSystem = fileSystemMock.Object,
			};

			// Act

			target.Write("Message 1");
			target.Write("Message 2");

			// Assert

			fileSystemMock.Verify(x => x.CreateStreamWriter(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<Encoding>(), It.IsAny<bool>()), Times.Exactly(2));
		}

		[TestMethod]
		public void Write_WritesDataCorrectly()
		{
			// Arrange

			var fileSystemStub = CreateFileSystemMock(out var writtenStream, out _);
			using var target = new RollingLogFile(@"c:\logs", "TestLog", "TestLog", "log", 128)
			{
				FileSystem = fileSystemStub.Object,
			};

			// Act

			target.Write("Hello :)");

			// Assert

			writtenStream.Position = 0;
			using var streamReader = new StreamReader(writtenStream);
			var writtenData = streamReader.ReadToEnd();

			writtenData.Should().Be("Hello :)");
		}

		[TestMethod]
		public void Write_ForFirstLogFile_UsesCorrectFileName()
		{
			// Arrange

			var fileSystemMock = CreateFileSystemMock();

			var clockStub = new Mock<ISystemClock>();
			clockStub.Setup(x => x.Now).Returns(ParseDateTime("2016.04.03 13:33:27"));

			using var target = new RollingLogFile(@"c:\logs", "TestLog - {YYYY}_{MM}_{DD} - {HH}_{mm}_{SS} - {PID} - START", "TestLog - {YYYY}_{MM}_{DD} - {HH}_{mm}_{SS} - {PID}", "txt", 128)
			{
				FileSystem = fileSystemMock.Object,
				SystemClock = clockStub.Object,
			};

			// Act

			target.Write("Some Message");

			// Assert

			var expectedLogFilename = $@"c:\logs\TestLog - 2016_04_03 - 13_33_27 - {Pid} - START.txt";
			fileSystemMock.Verify(x => x.CreateStreamWriter(expectedLogFilename, It.IsAny<bool>(), It.IsAny<Encoding>(), It.IsAny<bool>()), Times.Once);
		}

		[TestMethod]
		public void Write_ForSubsequentLogFile_UsesCorrectFileName()
		{
			// Arrange

			var fileSystemMock = CreateFileSystemMock();

			var clockStub = new Mock<ISystemClock>();
			clockStub.SetupSequence(x => x.Now)
				.Returns(ParseDateTime("2016.04.03 13:33:27"))
				.Returns(ParseDateTime("2016.11.28 20:57:18"));

			using var target = new RollingLogFile(@"c:\logs", "TestLog - {YYYY}_{MM}_{DD} - {HH}_{mm}_{SS} - {PID} - START", "TestLog - {YYYY}_{MM}_{DD} - {HH}_{mm}_{SS} - {PID}", "txt", 128)
			{
				FileSystem = fileSystemMock.Object,
				SystemClock = clockStub.Object,
			};

			target.Write("First Message");

			// Act

			target.Write("Second Message");

			// Assert

			var expectedLogFilename = $@"c:\logs\TestLog - 2016_11_28 - 20_57_18 - {Pid}.txt";
			fileSystemMock.Verify(x => x.CreateStreamWriter(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<Encoding>(), It.IsAny<bool>()), Times.Exactly(2));
			fileSystemMock.Verify(x => x.CreateStreamWriter(expectedLogFilename, It.IsAny<bool>(), It.IsAny<Encoding>(), It.IsAny<bool>()), Times.Once);
		}

		[TestMethod]
		public void Write_IfFileNameAlreadyExists_RetriesForFileNameWithCorrectSuffix()
		{
			// Arrange

			var fileSystemMock = CreateFileSystemMock();
			fileSystemMock.SetupSequence(x => x.FileExists(It.IsAny<string>()))
				.Returns(true)
				.Returns(false);

			using var target = new RollingLogFile(@"c:\logs", "TestLog", "TestLog", "log", 5)
			{
				FileSystem = fileSystemMock.Object,
			};

			// Act

			target.Write("Some Message");

			// Assert

			fileSystemMock.Verify(x => x.CreateStreamWriter(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<Encoding>(), It.IsAny<bool>()), Times.Once);
			fileSystemMock.Verify(x => x.CreateStreamWriter(@"c:\logs\TestLog.001.log", It.IsAny<bool>(), It.IsAny<Encoding>(), It.IsAny<bool>()), Times.Once);
		}

		[TestMethod]
		public void Write_IfFileNamesWithAllSuffixesExist_ThrowsInvalidOperationException()
		{
			// Arrange

			var fileSystemStub = CreateFileSystemMock();
			fileSystemStub.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);

			using var target = new RollingLogFile(@"c:\logs", "TestLog", "TestLog", "log", 5)
			{
				FileSystem = fileSystemStub.Object,
			};

			// Act

			var call = () => target.Write("Some Message");

			// Assert

			call.Should().Throw<InvalidOperationException>();
		}

		[TestMethod]
		public void Dispose_ForOpenedLogFile_DisposesFileStream()
		{
			// Arrange

			var fileSystemStub = CreateFileSystemMock(out _, out var streamWriterMock);

			var target = new RollingLogFile(@"c:\logs", "TestLog", "TestLog", "log", 5)
			{
				FileSystem = fileSystemStub.Object,
			};

			target.Write("Some Message");

			// Act

			target.Dispose();

			// Assert

			streamWriterMock.Verify(x => x.Dispose(), Times.Once);
		}

		private static Mock<IFileSystem> CreateFileSystemMock(params long[] lengths)
		{
			return CreateFileSystemMock(out _, out _, lengths);
		}

		private static Mock<IFileSystem> CreateFileSystemMock(out Stream writtenStream, out Mock<IStreamWriterWrapper> streamWriterMock, params long[] lengths)
		{
			if (lengths.Length == 0)
			{
				lengths = [0, 1024];
			}

			writtenStream = new MemoryStream();
			streamWriterMock = new Mock<IStreamWriterWrapper>();
			var setupSequence = streamWriterMock.SetupSequence(x => x.Length);
			foreach (var length in lengths)
			{
				setupSequence = setupSequence.Returns(length);
			}

#pragma warning disable CA2000 // Dispose objects before losing scope
			streamWriterMock.Setup(x => x.StreamWriter).Returns(new StreamWriter(writtenStream, Encoding.UTF8) { AutoFlush = true });
#pragma warning restore CA2000 // Dispose objects before losing scope

			var fileSystemStub = new Mock<IFileSystem>();
			fileSystemStub.Setup(x => x.CreateStreamWriter(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<Encoding>(), It.IsAny<bool>())).Returns(streamWriterMock.Object);

			// Avoiding file name conflict.
			fileSystemStub.Setup(x => x.FileExists(It.IsAny<string>())).Returns(false);

			return fileSystemStub;
		}

		private static DateTime ParseDateTime(string dateTime)
		{
			return DateTime.ParseExact(dateTime, "yyyy.MM.dd HH:mm:ss", CultureInfo.InvariantCulture);
		}
	}
}
