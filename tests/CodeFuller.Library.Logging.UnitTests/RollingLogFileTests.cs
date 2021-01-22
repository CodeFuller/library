using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using CodeFuller.Library.Core.Facades;
using CodeFuller.Library.Logging.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using static System.FormattableString;

namespace CodeFuller.Library.Logging.UnitTests
{
	[TestClass]
	public class RollingLogFileTests
	{
		private static int Pid => Process.GetCurrentProcess().Id;

		[TestMethod]
		public void Write_WhenCalledFirstTimeIfLogDirectoryDoesNotExist_CreatesLogDirectory()
		{
			//	Arrange

			var fileSystemMock = CreateFileSystemFacade();
			fileSystemMock.DirectoryExists(Arg.Any<string>()).Returns(false);
			var target = new RollingLogFile(@"c:\logs", "TestLog", 128)
			{
				FileSystemFacade = fileSystemMock
			};

			//	Act

			target.Write("Some Message");

			//	Assert

			fileSystemMock.Received(1).CreateDirectory(@"c:\logs");
		}

		[TestMethod]
		public void Write_IfCurrentFileSizeIsLowerThanRollSize_WritesToCurrentFile()
		{
			//	Arrange

			var fileSystemMock = CreateFileSystemFacade();
			var target = new RollingLogFile(@"c:\logs", "TestLog", 4096)
			{
				FileSystemFacade = fileSystemMock
			};

			//	Act

			//	CreateFileSystemFacade(): streamWriter.SetupSequence(x => x.Length).Returns(0).Returns(1024);
			target.Write("Message 1");
			target.Write("Message 2");

			//	Assert

			fileSystemMock.Received(1).CreateStreamWriter(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<Encoding>(), Arg.Any<bool>());
		}

		[TestMethod]
		public void Write_IfCurrentFileSizeIsGreaterThanRollSize_SwitchesToNewFile()
		{
			//	Arrange

			var fileSystemMock = CreateFileSystemFacade();
			var target = new RollingLogFile(@"c:\logs", "TestLog", 128)
			{
				FileSystemFacade = fileSystemMock
			};

			//	Act

			//	CreateFileSystemFacade(): streamWriter.Length.Returns(0, 1024);
			target.Write("Message 1");
			target.Write("Message 2");

			//	Assert

			fileSystemMock.Received(2).CreateStreamWriter(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<Encoding>(), Arg.Any<bool>());
		}

		[TestMethod]
		public void Write_WritesCorrectData()
		{
			//	Arrange

			var fileSystemStub = CreateFileSystemFacade(out Stream writtenStream);
			var target = new RollingLogFile(@"c:\logs", "TestLog", 128)
			{
				FileSystemFacade = fileSystemStub
			};

			//	Act

			target.Write("Hello :)");

			//	Assert

			writtenStream.Position = 0;
			var sr = new StreamReader(writtenStream);
			var writtenData = sr.ReadToEnd();

			Assert.AreEqual("Hello :)", writtenData);
		}

		[TestMethod]
		public void Write_WhenFirstFileIsCreated_UsesCorrectFileName()
		{
			//	Arrange

			var fileSystemMock = CreateFileSystemFacade();

			IClock clockStub = Substitute.For<IClock>();
			clockStub.Now.Returns(ParseDateTime("2016.08.28 14:25:34"));

			var target = new RollingLogFile(@"c:\logs", "TestLog - {YYYY}_{MM}_{DD} - {HH}_{mm}_{SS} - {PID} - START",
				"TestLog - {YYYY}_{MM}_{DD} - {HH}_{mm}_{SS} - {PID}", "txt", 128)
			{
				FileSystemFacade = fileSystemMock,
				DateTimeFacade = clockStub,
			};

			//	Act

			target.Write("Some Message");

			//	Assert

			string expectedLogFilename = Invariant($@"c:\logs\TestLog - 2016_08_28 - 14_25_34 - {Pid} - START.txt");
			fileSystemMock.Received(1).CreateStreamWriter(expectedLogFilename, Arg.Any<bool>(), Arg.Any<Encoding>(), Arg.Any<bool>());
		}

		[TestMethod]
		public void Write_WhenSubsequentFileIsCreated_UsesCorrectFileName()
		{
			//	Arrange

			var fileSystemMock = CreateFileSystemFacade();

			IClock clockStub = Substitute.For<IClock>();
			clockStub.Now.Returns(ParseDateTime("2016.08.28 14:25:34"), ParseDateTime("2016.08.28 14:56:21"));

			var target = new RollingLogFile(@"c:\logs", "TestLog - {YYYY}_{MM}_{DD} - {HH}_{mm}_{SS} - {PID} - START",
				"TestLog - {YYYY}_{MM}_{DD} - {HH}_{mm}_{SS} - {PID}", "txt", 128)
			{
				FileSystemFacade = fileSystemMock,
				DateTimeFacade = clockStub,
			};

			target.Write("First Message");

			//	Act

			target.Write("Second Message");

			//	Assert

			string expectedLogFilename = Invariant($@"c:\logs\TestLog - 2016_08_28 - 14_56_21 - {Pid}.txt");
			fileSystemMock.Received(2).CreateStreamWriter(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<Encoding>(), Arg.Any<bool>());
			fileSystemMock.Received().CreateStreamWriter(expectedLogFilename, Arg.Any<bool>(), Arg.Any<Encoding>(), Arg.Any<bool>());
		}

		[TestMethod]
		public void FileLogger_IfGeneratedFileNameExists_AddsNumericSuffixAndRetries()
		{
			//	Arrange

			var fileSystemMock = CreateFileSystemFacade();
			fileSystemMock.FileExists(Arg.Any<string>()).Returns(true, false);

			var target = new RollingLogFile(@"c:\logs", "TestLog", 5)
			{
				FileSystemFacade = fileSystemMock
			};

			//	Act

			target.Write("Some Message");

			//	Assert

			string expectedLogFilename = Invariant($@"c:\logs\TestLog.001.log");
			fileSystemMock.Received(1).CreateStreamWriter(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<Encoding>(), Arg.Any<bool>());
			fileSystemMock.Received(1).CreateStreamWriter(expectedLogFilename, Arg.Any<bool>(), Arg.Any<Encoding>(), Arg.Any<bool>());
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void FileLogger_IfGeneratedFileNameExistsForAllRetries_ThrowsInvalidOperationException()
		{
			//	Arrange

			var fileSystemStub = CreateFileSystemFacade();
			fileSystemStub.FileExists(Arg.Any<string>()).Returns(true);

			var target = new RollingLogFile(@"c:\logs", "TestLog", 5)
			{
				FileSystemFacade = fileSystemStub
			};

			//	Act & Assert

			target.Write("Some Message");
		}

		[TestMethod]
		public void Dispose_IfLogFileIsOpened_DisposesLogFile()
		{
			//	Arrange

			IFileSystemFacade fileSystemFacade = CreateFileSystemFacade();
			//	Getting stream mock created in CreateFileSystemFacade()
			IStreamWriterFacade streamMock = fileSystemFacade.CreateStreamWriter(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<Encoding>(), Arg.Any<bool>());

			var target = new RollingLogFile(@"c:\logs", "TestLog", 5)
			{
				FileSystemFacade = fileSystemFacade
			};

			target.Write("Some Message");

			//	Act

			target.Dispose();

			//	Assert

			streamMock.Received(1).Dispose();
		}

		private static IFileSystemFacade CreateFileSystemFacade()
		{
			return CreateFileSystemFacade(out _);
		}

		private static IFileSystemFacade CreateFileSystemFacade(out Stream writtenStream)
		{
			writtenStream = new MemoryStream();

			IStreamWriterFacade streamWriter = Substitute.For<IStreamWriterFacade>();
			streamWriter.Length.Returns(0, 1024);
			streamWriter.StreamWriter.Returns(new StreamWriter(writtenStream, Encoding.UTF8)
			{
				AutoFlush = true
			});

			IFileSystemFacade fileSystemFacadeMock = Substitute.For<IFileSystemFacade>();
			fileSystemFacadeMock.CreateStreamWriter(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<Encoding>(), Arg.Any<bool>()).Returns(streamWriter);

			//	Stub check for log directory existence
			fileSystemFacadeMock.DirectoryExists(Arg.Any<string>()).Returns(true);
			//	Stub check for log filename uniqueness
			fileSystemFacadeMock.FileExists(Arg.Any<string>()).Returns(false);

			return fileSystemFacadeMock;
		}

		private DateTime ParseDateTime(string dateTime)
		{
			return DateTime.ParseExact(dateTime, "yyyy.MM.dd HH:mm:ss", CultureInfo.InvariantCulture);
		}
	}
}
