using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using CF.Library.Core;
using CF.Library.Core.Facades;

namespace CF.Library.Logging
{
	internal class RollingLogFile : IRollingLogFile
	{
		private const int MaxTriesForDuplicatedFileNames = 10;

		private IStreamWriterFacade currFile;

		private readonly string logPath;
		private readonly string firstFileNamePattern;
		private readonly string nextFileNamePattern;
		private readonly string fileNameExtension;
		private readonly long rollSize;

		private readonly int pid = Process.GetCurrentProcess().Id;

		/// <summary>
		/// Property injection for IFileSystemFacade.
		/// </summary>
		public IFileSystemFacade FileSystemFacade { get; set; } = new FileSystemFacade();

		/// <summary>
		/// Property injection for IDateTimeFacade.
		/// </summary>
		public IClock DateTimeFacade { get; set; } = new SystemClock();

		public RollingLogFile(string logPath, string fileNamePattern, long rollSize)
			: this(logPath, fileNamePattern, fileNamePattern, "log", rollSize)
		{
		}

		public RollingLogFile(string logPath, string fileNamePattern, string fileNameExtension, long rollSize)
			: this(logPath, fileNamePattern, fileNamePattern, fileNameExtension, rollSize)
		{
		}

		public RollingLogFile(string logPath, string firstFileNamePattern, string nextFileNamePattern, string fileNameExtension, long rollSize)
		{
			this.logPath = logPath;
			this.firstFileNamePattern = firstFileNamePattern;
			this.nextFileNamePattern = nextFileNamePattern;
			this.fileNameExtension = fileNameExtension;
			this.rollSize = rollSize;
		}

		public StreamWriter StreamWriter
		{
			get
			{
				OpenIfRequired();
				RollIfRequired();
				return currFile.StreamWriter;
			}
		}

		public void Write(string data)
		{
			StreamWriter.Write(data);
		}

		private void OpenIfRequired()
		{
			if (currFile == null)
			{
				currFile = OpenNextFile(true);
			}
		}

		private void RollIfRequired()
		{
			if (currFile.Length >= rollSize)
			{
				currFile.Close();
				currFile = OpenNextFile(false);
			}
		}

		private IStreamWriterFacade OpenNextFile(bool isFirstFile)
		{
			return FileSystemFacade.CreateStreamWriter(GenerateFileName(isFirstFile), true, Encoding.UTF8, true);
		}

		private string GenerateFileName(bool isFirstFile)
		{
			if (isFirstFile && !FileSystemFacade.DirectoryExists(logPath))
			{
				FileSystemFacade.CreateDirectory(logPath);
			}

			StringBuilder logFileNamePart = BuildLogFileName(isFirstFile);
			StringBuilder triedFileNames = new StringBuilder();
			for (int i = 0; i < MaxTriesForDuplicatedFileNames; ++i)
			{
				string currFileName = BuildFullLogFilePath(logFileNamePart.ToString(), i);
				if (!FileSystemFacade.FileExists(currFileName))
				{
					return currFileName;
				}

				triedFileNames.AppendLine(currFileName);
			}

			throw new InvalidOperationException($"Failed to generate unique log filename after {MaxTriesForDuplicatedFileNames} tries. Following files exist:\n{triedFileNames}");
		}

		private StringBuilder BuildLogFileName(bool firstFile)
		{
			DateTime now = DateTimeFacade.Now;
			StringBuilder logFileName = new StringBuilder(firstFile ? firstFileNamePattern : nextFileNamePattern);
			logFileName.Replace("{YYYY}", $"{now.Year:0000}");
			logFileName.Replace("{MM}", $"{now.Month:00}");
			logFileName.Replace("{DD}", $"{now.Day:00}");
			logFileName.Replace("{HH}", $"{now.Hour:00}");
			logFileName.Replace("{mm}", $"{now.Minute:00}");
			logFileName.Replace("{SS}", $"{now.Second:00}");
			logFileName.Replace("{PID}", $"{pid}");

			return logFileName;
		}

		private string BuildFullLogFilePath(string logFileNamePart, int tryNumber)
		{
			StringBuilder logFilePath = new StringBuilder(Path.Combine(logPath, logFileNamePart));
			if (tryNumber > 0)
			{
				logFilePath.Append(FormattableString.Invariant($".{tryNumber:000}"));
			}
			logFilePath.Append($".{fileNameExtension}");

			return logFilePath.ToString();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				currFile?.Dispose();
				currFile = null;
			}
		}
	}
}
