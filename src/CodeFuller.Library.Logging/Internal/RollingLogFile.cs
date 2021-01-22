using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using CodeFuller.Library.Logging.Interfaces;

namespace CodeFuller.Library.Logging.Internal
{
	internal sealed class RollingLogFile : IRollingLogFile, IDisposable
	{
		private const int MaxTriesForDuplicatedFileNames = 10;

		private readonly string logPath;
		private readonly string firstFileNamePattern;
		private readonly string nextFileNamePattern;
		private readonly string fileNameExtension;
		private readonly long rollSize;

		private readonly int pid = Process.GetCurrentProcess().Id;

		public IFileSystemFacade FileSystemFacade { get; set; } = new FileSystemFacade();

		public IClock DateTimeFacade { get; set; } = new SystemClock();

		private IStreamWriterFacade CurrentFileStream { get; set; }

		public StreamWriter StreamWriter
		{
			get
			{
				OpenIfRequired();
				RollIfRequired();
				return CurrentFileStream.StreamWriter;
			}
		}

		public RollingLogFile(string logPath, string firstFileNamePattern, string nextFileNamePattern, string fileNameExtension, long rollSize)
		{
			this.logPath = logPath;
			this.firstFileNamePattern = firstFileNamePattern;
			this.nextFileNamePattern = nextFileNamePattern;
			this.fileNameExtension = fileNameExtension;
			this.rollSize = rollSize;
		}

		public void Write(string data)
		{
			StreamWriter.Write(data);
		}

		private void OpenIfRequired()
		{
			CurrentFileStream ??= OpenNextFile(true);
		}

		private void RollIfRequired()
		{
			if (CurrentFileStream.Length < rollSize)
			{
				return;
			}

			CurrentFileStream.Close();
			CurrentFileStream = OpenNextFile(false);
		}

		private IStreamWriterFacade OpenNextFile(bool isFirstFile)
		{
			return FileSystemFacade.CreateStreamWriter(GetLogFileName(isFirstFile), true, Encoding.UTF8, true);
		}

		private string GetLogFileName(bool isFirstFile)
		{
			if (isFirstFile)
			{
				FileSystemFacade.CreateDirectory(logPath);
			}

			var logFileNamePart = BuildLogFileName(isFirstFile);
			var triedFileNames = new StringBuilder();
			for (var i = 0; i < MaxTriesForDuplicatedFileNames; ++i)
			{
				var currentFileName = BuildFullLogFilePath(logFileNamePart.ToString(), i);
				if (!FileSystemFacade.FileExists(currentFileName))
				{
					return currentFileName;
				}

				triedFileNames.AppendLine(currentFileName);
			}

			throw new InvalidOperationException($"Failed to generate unique log filename after {MaxTriesForDuplicatedFileNames} tries. Following files exist:\n{triedFileNames}");
		}

		private StringBuilder BuildLogFileName(bool isFirstFile)
		{
			var now = DateTimeFacade.Now;

			var logFileName = new StringBuilder(isFirstFile ? firstFileNamePattern : nextFileNamePattern);
			logFileName.Replace("{YYYY}", $"{now.Year:0000}");
			logFileName.Replace("{MM}", $"{now.Month:00}");
			logFileName.Replace("{DD}", $"{now.Day:00}");
			logFileName.Replace("{HH}", $"{now.Hour:00}");
			logFileName.Replace("{mm}", $"{now.Minute:00}");
			logFileName.Replace("{SS}", $"{now.Second:00}");
			logFileName.Replace("{PID}", $"{pid}");

			return logFileName;
		}

		private string BuildFullLogFilePath(string logFileNamePart, int attemptNumber)
		{
			var logFilePath = new StringBuilder(Path.Combine(logPath, logFileNamePart));
			if (attemptNumber > 0)
			{
				logFilePath.Append($".{attemptNumber:000}");
			}

			// We do not use Path.ChangeExtension() because it will remove attempt number suffix (.001).
			logFilePath.Append($".{fileNameExtension}");

			return logFilePath.ToString();
		}

		public void Dispose()
		{
			CurrentFileStream?.Dispose();
			CurrentFileStream = null;
		}
	}
}
