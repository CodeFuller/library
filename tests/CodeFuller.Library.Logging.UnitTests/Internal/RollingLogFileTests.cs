using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CodeFuller.Library.Logging.Interfaces;
using CodeFuller.Library.Logging.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace CodeFuller.Library.Logging.UnitTests.Internal
{
	[TestClass]
	public class RollingLogFileTests
	{
		private static int Pid => Process.GetCurrentProcess().Id;

		[TestMethod]
		public void Write_ForFirstCall_CreatesLogDirectory()
		{
			//	Arrange

			var fileSystemMock = CreateFileSystemFacade();
			var target = new RollingLogFile(@"c:\logs", "TestLog", "TestLog", "log", 128)
			{
				FileSystemFacade = fileSystemMock
			};

			//	Act

			target.Write("Some Message");

			//	Assert

			fileSystemMock.Received(1).CreateDirectory(@"c:\logs");
		}

		[TestMethod]
		public void Write_IfCurrentLogIsSmallerThanRollSize_WritesToCurrentFile()
		{
			//	Arrange

			var fileSystemMock = CreateFileSystemFacade(0, 1024);
			var target = new RollingLogFile(@"c:\logs", "TestLog", "TestLog", "log", 4096)
			{
				FileSystemFacade = fileSystemMock
			};

			//	Act

			target.Write("Message 1");
			target.Write("Message 2");

			//	Assert

			fileSystemMock.Received(1).CreateStreamWriter(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<Encoding>(), Arg.Any<bool>());
		}

		[TestMethod]
		public void Write_IfCurrentLogIsBiggerThanRollSize_WritesToNewFile()
		{
			//	Arrange

			var fileSystemMock = CreateFileSystemFacade(0, 1024);
			var target = new RollingLogFile(@"c:\logs", "TestLog", "TestLog", "log", 128)
			{
				FileSystemFacade = fileSystemMock
			};

			//	Act

			target.Write("Message 1");
			target.Write("Message 2");

			//	Assert

			fileSystemMock.Received(2).CreateStreamWriter(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<Encoding>(), Arg.Any<bool>());
		}

		[TestMethod]
		public void Write_WritesDataCorrectly()
		{
			//	Arrange

			var fileSystemStub = CreateFileSystemFacade(out var writtenStream);
			var target = new RollingLogFile(@"c:\logs", "TestLog", "TestLog", "log", 128)
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
		public void Write_ForFirstLogFile_UsesCorrectFileName()
		{
			//	Arrange

			var fileSystemMock = CreateFileSystemFacade();

			var clockStub = Substitute.For<IClock>();
			clockStub.Now.Returns(ParseDateTime("2016.04.03 13:33:27"));

			var target = new RollingLogFile(@"c:\logs", "TestLog - {YYYY}_{MM}_{DD} - {HH}_{mm}_{SS} - {PID} - START", "TestLog - {YYYY}_{MM}_{DD} - {HH}_{mm}_{SS} - {PID}", "txt", 128)
			{
				FileSystemFacade = fileSystemMock,
				DateTimeFacade = clockStub,
			};

			//	Act

			target.Write("Some Message");

			//	Assert

			var expectedLogFilename = $@"c:\logs\TestLog - 2016_04_03 - 13_33_27 - {Pid} - START.txt";
			fileSystemMock.Received(1).CreateStreamWriter(expectedLogFilename, Arg.Any<bool>(), Arg.Any<Encoding>(), Arg.Any<bool>());
		}

		[TestMethod]
		public void Write_ForSubsequentLogFile_UsesCorrectFileName()
		{
			//	Arrange

			var fileSystemMock = CreateFileSystemFacade();

			var clockStub = Substitute.For<IClock>();
			clockStub.Now.Returns(ParseDateTime("2016.04.03 13:33:27"), ParseDateTime("2016.11.28 20:57:18"));

			var target = new RollingLogFile(@"c:\logs", "TestLog - {YYYY}_{MM}_{DD} - {HH}_{mm}_{SS} - {PID} - START", "TestLog - {YYYY}_{MM}_{DD} - {HH}_{mm}_{SS} - {PID}", "txt", 128)
			{
				FileSystemFacade = fileSystemMock,
				DateTimeFacade = clockStub,
			};

			target.Write("First Message");

			//	Act

			target.Write("Second Message");

			//	Assert

			var expectedLogFilename = $@"c:\logs\TestLog - 2016_11_28 - 20_57_18 - {Pid}.txt";
			fileSystemMock.Received(2).CreateStreamWriter(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<Encoding>(), Arg.Any<bool>());
			fileSystemMock.Received().CreateStreamWriter(expectedLogFilename, Arg.Any<bool>(), Arg.Any<Encoding>(), Arg.Any<bool>());
		}

		[TestMethod]
		public void Write_IfFileNameAlreadyExists_RetriesForFileNameWithCorrectSuffix()
		{
			//	Arrange

			var fileSystemMock = CreateFileSystemFacade();
			fileSystemMock.FileExists(Arg.Any<string>()).Returns(true, false);

			var target = new RollingLogFile(@"c:\logs", "TestLog", "TestLog", "log", 5)
			{
				FileSystemFacade = fileSystemMock
			};

			//	Act

			target.Write("Some Message");

			//	Assert

			fileSystemMock.Received(1).CreateStreamWriter(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<Encoding>(), Arg.Any<bool>());
			fileSystemMock.Received(1).CreateStreamWriter(@"c:\logs\TestLog.001.log", Arg.Any<bool>(), Arg.Any<Encoding>(), Arg.Any<bool>());
		}

		[TestMethod]
		public void FileLogger_IfFileNamesWithAllSuffixesExist_ThrowsInvalidOperationException()
		{
			//	Arrange

			var fileSystemStub = CreateFileSystemFacade();
			fileSystemStub.FileExists(Arg.Any<string>()).Returns(true);

			var target = new RollingLogFile(@"c:\logs", "TestLog", "TestLog", "log", 5)
			{
				FileSystemFacade = fileSystemStub
			};

			//	Act

			void Call() => target.Write("Some Message");

			// Assert

			Assert.ThrowsException<InvalidOperationException>(Call);
		}

		[TestMethod]
		public void Dispose_ForOpenedLogFile_DisposesFileStream()
		{
			//	Arrange

			var fileSystemFacade = CreateFileSystemFacade();
			var streamMock = fileSystemFacade.CreateStreamWriter(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<Encoding>(), Arg.Any<bool>());

			var target = new RollingLogFile(@"c:\logs", "TestLog", "TestLog", "log", 5)
			{
				FileSystemFacade = fileSystemFacade
			};

			target.Write("Some Message");

			//	Act

			target.Dispose();

			//	Assert

			streamMock.Received(1).Dispose();
		}

		private static IFileSystemFacade CreateFileSystemFacade(params long[] lengths)
		{
			return CreateFileSystemFacade(out _, lengths);
		}

		private static IFileSystemFacade CreateFileSystemFacade(out Stream writtenStream, params long[] lengths)
		{
			writtenStream = new MemoryStream();

			var streamWriter = Substitute.For<IStreamWriterFacade>();

			if (lengths.Any())
			{
				streamWriter.Length.Returns(lengths[0], lengths.Skip(1).ToArray());
			}
			else
			{
				streamWriter.Length.Returns(0, 1024);
			}

			streamWriter.StreamWriter.Returns(new StreamWriter(writtenStream, Encoding.UTF8)
			{
				AutoFlush = true
			});

			var fileSystemFacadeMock = Substitute.For<IFileSystemFacade>();
			fileSystemFacadeMock.CreateStreamWriter(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<Encoding>(), Arg.Any<bool>()).Returns(streamWriter);

			// Avoiding file name conflict.
			fileSystemFacadeMock.FileExists(Arg.Any<string>()).Returns(false);

			return fileSystemFacadeMock;
		}

		private static DateTime ParseDateTime(string dateTime)
		{
			return DateTime.ParseExact(dateTime, "yyyy.MM.dd HH:mm:ss", CultureInfo.InvariantCulture);
		}
	}
}
