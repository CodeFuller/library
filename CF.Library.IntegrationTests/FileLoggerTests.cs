using System.IO;
using System.Linq;
using CF.Library.Core;
using CF.Library.Core.Facades;
using CF.Library.Testing;
using NUnit.Framework;

namespace CF.Library.IntegrationTests
{
	[TestFixture]
	[Category("Integration")]
	public class FileLoggerTests
	{
		[Test]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope", Justification = "Object is disposed by called method")]
		public void Write_ForValidFileLogger_WritesDataToLogFile()
		{
			//	Arrange

			using (TempDirectory tempDir = new TempDirectory())
			{
				FileLogger logger = new FileLogger(tempDir.Path, nameof(FileLoggerTests), 128);

				//	Act & Assert

				PerformFileLoggerIntegrationTest(logger, tempDir.Path);
			}
		}

		[Test]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope", Justification = "Object is disposed by called method")]
		public void FileLogger_WhenLogDirIsNotProvided_UsesDefaultLogDirectory()
		{
			//	Arrange

			FileLogger logger = new FileLogger(nameof(FileLoggerTests), 128);

			//	Act & Assert

			string logDir = Path.Combine((new FileSystemFacade()).GetProcessDirectory(), "logs");
			PerformFileLoggerIntegrationTest(logger, logDir);

			//	Cleanup

			Directory.Delete(logDir, true);
		}

		private void PerformFileLoggerIntegrationTest(FileLogger logger, string logDir)
		{
			using (logger)
			{
				//	Act

				logger.Write("Hello!");
			}

			//	Assert

			string[] logFiles = Directory.GetFiles(logDir);
			Assert.AreEqual(1, logFiles.Length);
			string writtenData = File.ReadAllText(logFiles.Single());
			Assert.AreEqual("Hello!", writtenData);
		}
	}
}
