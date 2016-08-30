using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using CF.Library.Core;
using CF.Library.Core.Facades;
using NSubstitute;
using NUnit.Framework;
using static System.FormattableString;

namespace CF.Library.UnitTests.CF.Library.Core
{
	[TestFixture]
	public class FileLoggerTests
	{
		private string Pid => Invariant($"{Process.GetCurrentProcess().Id}");

		[Test]
		public void FileLogger_WhenLogDirDoesNotExist_CreatesLogDir()
		{
			//	Arrange

			IFileSystemFacade fileSystemFacadeMock = CreateFileSystemFacade();
			fileSystemFacadeMock.DirectoryExists(Arg.Any<string>()).Returns(false);

			//	Act

			using (new FileLogger(fileSystemFacadeMock, new SystemClock(), @"c:\SomeApp\SomeDir", "TestLog", 128))
			{
			}

			//	Assert

			fileSystemFacadeMock.Received(1).CreateDirectory(@"c:\SomeApp\SomeDir");
		}

		[Test]
		public void FileLogger_WhenLogDirIsNotProvided_UsesDefaultLogDirectory()
		{
			//	Arrange

			IFileSystemFacade fileSystemFacadeMock = CreateFileSystemFacade();
			fileSystemFacadeMock.GetProcessDirectory().Returns(@"c:\SomeApp\SomeDir");
			string logFilename = null;
			fileSystemFacadeMock.CreateStreamWriter(Arg.Do<string>(path => logFilename = path), Arg.Any<bool>(), Arg.Any<Encoding>(), Arg.Any<bool>());
			
			//	Act

			using (new FileLogger(fileSystemFacadeMock, new SystemClock(), "TestLog", 128))
			{
			}

			//	Assert

			string usedLogDir = Path.GetDirectoryName(logFilename);
			Assert.AreEqual(@"c:\SomeApp\SomeDir\logs", usedLogDir);
		}

		[Test]
		public void Write_WhenCurrentFileSizeIsLowerThanRollSize_WritesToCurrentFile()
		{
			//	Arrange

			IFileSystemFacade fileSystemFacadeMock = CreateFileSystemFacade();
			using (FileLogger logger = new FileLogger(fileSystemFacadeMock, new SystemClock(), "TestLog", 4096))
			{
				//	CreateFileSystemFacade(): streamWriter.Length.Returns(0, 1024);

				//	Act

				//	Stubbed file size = 0
				logger.Write("Message 1");
				//	Stubbed file size = 1024
				logger.Write("Message 2");
			}

			//	Assert

			//	Checking that no rolling happened (onle one file created)
			fileSystemFacadeMock.Received(1).CreateStreamWriter(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<Encoding>(), Arg.Any<bool>());
		}

		[Test]
		public void Write_WhenCurrentFileSizeIsGreaterThanRollSize_WritesToNewFile()
		{
			//	Arrange

			IFileSystemFacade fileSystemFacadeMock = CreateFileSystemFacade();
			using (FileLogger logger = new FileLogger(fileSystemFacadeMock, new SystemClock(), "TestLog", 512))
			{
				//	CreateFileSystemFacade(): streamWriter.Length.Returns(0, 1024);

				//	Act

				//	Stubbed file size = 0
				logger.Write("Message 1");
				//	Stubbed file size = 1024
				logger.Write("Message 2");
			}

			//	Assert

			//	Checking that rolling has happened (two files created)
			fileSystemFacadeMock.Received(2).CreateStreamWriter(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<Encoding>(), Arg.Any<bool>());
		}

		[Test]
		public void Write_WhenCalled_WritesCorrectData()
		{
			//	Arrange

			IFileSystemFacade fileSystemFacade = CreateFileSystemFacade();
			//	Getting stream mock created in CreateFileSystemFacade()
			IStreamWriterFacade streamMock = fileSystemFacade.CreateStreamWriter(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<Encoding>(), Arg.Any<bool>());
			using (FileLogger logger = new FileLogger(fileSystemFacade, new SystemClock(), "TestLog", 1024))
			{
				//	Act

				logger.Write("Hello :)");
			}

			//	Assert

			streamMock.Received(1).Write("Hello :)");
		}

		[Test]
		public void FileLogger_ForFirstLogFile_UsesAppropriateFileName()
		{
			//	Arrange

			IFileSystemFacade fileSystemFacadeMock = CreateFileSystemFacade();
			IClock clock = Substitute.For<IClock>();
			clock.Now.Returns(ParseDateTime("2016.08.28 14:25:34"));

			//	Act

			using (new FileLogger(fileSystemFacadeMock, clock, @"c:\SomeApp\SomeDir", "TestLog", 128))
			{
			}

			//	Assert

			string expectedLogFilename = Invariant($@"c:\SomeApp\SomeDir\TestLog - 2016_08_28 - 14_25_34 - {Pid} - START.log");
			fileSystemFacadeMock.Received(1).CreateStreamWriter(expectedLogFilename, Arg.Any<bool>(), Arg.Any<Encoding>(), Arg.Any<bool>());
		}

		[Test]
		public void FileLogger_ForSubsequentLogFile_UsesAppropriateFileName()
		{
			//	Arrange

			IFileSystemFacade fileSystemFacadeMock = CreateFileSystemFacade();
			IClock clock = Substitute.For<IClock>();
			clock.Now.Returns(ParseDateTime("2016.08.28 14:25:34"), ParseDateTime("2016.08.28 14:56:21"));

			using (FileLogger logger = new FileLogger(fileSystemFacadeMock, clock, @"c:\SomeApp\SomeDir", "TestLog", 1))
			{
				logger.Write("Hello!");

				//	Act

				//	For second write file rolling will happen
				logger.Write("Good bye!");
			}

			//	Assert

			string expectedLogFilename = Invariant($@"c:\SomeApp\SomeDir\TestLog - 2016_08_28 - 14_56_21 - {Pid}.log");
			fileSystemFacadeMock.Received(2).CreateStreamWriter(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<Encoding>(), Arg.Any<bool>());
			fileSystemFacadeMock.Received().CreateStreamWriter(expectedLogFilename, Arg.Any<bool>(), Arg.Any<Encoding>(), Arg.Any<bool>());
		}

		[Test]
		public void FileLogger_WhenGeneratedFileNameExists_AddsNumericSuffixAndRetries()
		{
			//	Arrange

			IFileSystemFacade fileSystemFacadeMock = CreateFileSystemFacade();
			fileSystemFacadeMock.FileExists(Arg.Any<string>()).Returns(true, false);
			IClock clock = Substitute.For<IClock>();
			clock.Now.Returns(ParseDateTime("2016.08.28 14:25:34"));

			//	Act

			using (FileLogger logger = new FileLogger(fileSystemFacadeMock, clock, @"c:\SomeApp\SomeDir", "TestLog", 1))
			{
			}

			//	Assert

			string expectedLogFilename = Invariant($@"c:\SomeApp\SomeDir\TestLog - 2016_08_28 - 14_25_34 - {Pid} - START.001.log");
			fileSystemFacadeMock.Received(1).CreateStreamWriter(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<Encoding>(), Arg.Any<bool>());
			fileSystemFacadeMock.Received(1).CreateStreamWriter(expectedLogFilename, Arg.Any<bool>(), Arg.Any<Encoding>(), Arg.Any<bool>());
		}

		[Test]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope", Justification = "Test for constructor exception, object is not created")]
		public void FileLogger_WhenGeneratedFileNameExistsForAllRetries_ThrowsIOException()
		{
			IFileSystemFacade fileSystemFacade = CreateFileSystemFacade();
			fileSystemFacade.FileExists(Arg.Any<string>()).Returns(true);

			Assert.Throws<IOException>(() => new FileLogger(fileSystemFacade, new SystemClock(), @"c:\SomeApp\SomeDir", "TestLog", 1));
		}

		[Test]
		public void Dispose_WhenCalledForInitializedObject_DisposesLogFile()
		{
			IFileSystemFacade fileSystemFacade = CreateFileSystemFacade();
			//	Getting stream mock created in CreateFileSystemFacade()
			IStreamWriterFacade streamMock = fileSystemFacade.CreateStreamWriter(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<Encoding>(), Arg.Any<bool>());

			using (new FileLogger(fileSystemFacade, new SystemClock(), "TestLog", 1024))
			{
			}

			streamMock.Received(1).Dispose();
		}

		private static IFileSystemFacade CreateFileSystemFacade()
		{
			IStreamWriterFacade streamWriter = Substitute.For<IStreamWriterFacade>();
			streamWriter.Length.Returns(0, 1024);

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
