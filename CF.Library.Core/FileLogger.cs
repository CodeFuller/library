using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using CF.Library.Core.Facades;
using static System.FormattableString;
using static CF.Library.Core.FormattableStringExtensions;

namespace CF.Library.Core
{
	/// <summary>
	/// 
	/// </summary>
	public interface ILogger
	{
		/// <summary>
		/// Writes raw unformatted data to the log.
		/// </summary>
		void Write(string message);
	}

	/// <summary>
	/// Simple file logger with support of rolling by size.
	/// </summary>
	public class FileLogger : ILogger, IDisposable
	{
		private const int MaxTriesForDuplicatedFileNames = 10;

		private const string FileNameDateTimePattern = "yyyy_MM_dd - HH_mm_ss";
		private const string FirstLogFileSuffix = " - START";
		private const string LogFileExtension = ".log";

		private readonly IFileSystemFacade fileSystemFacade;
		private IStreamWriterFacade currFile;

		private readonly IClock clock;

		private readonly string logDir;
		private readonly string logName;
		private readonly int rollSize;

		private readonly string pid = Invariant($"{Process.GetCurrentProcess().Id}");

		/// <summary>
		/// Constructor.
		/// </summary>
		public FileLogger(string logName, int rollSize)
			: this(GetDefaultLogDir(new FileSystemFacade()), logName, rollSize)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public FileLogger(string logDir, string logName, int rollSize)
			: this(new FileSystemFacade(), new SystemClock(), logDir, logName, rollSize)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public FileLogger(IFileSystemFacade fileSystemFacade, IClock clock, string logName, int rollSize)
			: this(fileSystemFacade, clock, GetDefaultLogDir(fileSystemFacade), logName, rollSize)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public FileLogger(IFileSystemFacade fileSystemFacade, IClock clock, string logDir, string logName, int rollSize)
		{
			this.fileSystemFacade = fileSystemFacade;
			this.clock = clock;
			this.logDir = logDir;
			this.logName = logName;
			this.rollSize = rollSize;

			currFile = OpenNextFile(true);
		}

		/// <summary>
		/// Writes raw unformatted data to the log.
		/// </summary>
		public void Write(string message)
		{
			RollIfRequired();

			currFile.Write(message);
		}

		private void RollIfRequired()
		{
			if (currFile.Length >= rollSize)
			{
				currFile.Close();
				currFile = OpenNextFile(false);
			}
		}

		private IStreamWriterFacade OpenNextFile(bool firstFile)
		{
			return OpenNextFile(GenerateFileName(firstFile));
		}

		private IStreamWriterFacade OpenNextFile(string fileName)
		{
			return fileSystemFacade.CreateStreamWriter(fileName, true, Encoding.UTF8, true);
		}

		private string GenerateFileName(bool firstFile)
		{
			if (firstFile && !fileSystemFacade.DirectoryExists(logDir))
			{
				fileSystemFacade.CreateDirectory(logDir);
			}

			StringBuilder logFileNamePart = BuildLogFileNamePart(firstFile);
			StringBuilder triedFileNames = new StringBuilder();
			for (int i = 0; i < MaxTriesForDuplicatedFileNames; ++i)
			{
				string currFileName = BuildFullLogFilePath(logFileNamePart.ToString(), i);
				if (!fileSystemFacade.FileExists(currFileName))
				{
					return currFileName;
				}

				triedFileNames.AppendLine(currFileName);
			}

			throw new IOException(Current($"Failed to generate unique log filename after {MaxTriesForDuplicatedFileNames} tries. Following file exist:\n{triedFileNames}"));
		}

		private StringBuilder BuildLogFileNamePart(bool firstFile)
		{
			string timestampPart = clock.Now.ToString(FileNameDateTimePattern, CultureInfo.InvariantCulture);
			StringBuilder logFileNamePart = new StringBuilder(Invariant($"{logName} - {timestampPart} - {pid}"));
			if (firstFile)
			{
				logFileNamePart.Append(FirstLogFileSuffix);
			}

			return logFileNamePart;
		}

		private string BuildFullLogFilePath(string logFileNamePart, int tryNumber)
		{
			StringBuilder logFilePath = new StringBuilder(Path.Combine(logDir, logFileNamePart));
			if (tryNumber > 0)
			{
				logFilePath.Append(Invariant($".{tryNumber:000}"));
			}
			logFilePath.Append(LogFileExtension);

			return logFilePath.ToString();
		}

		private static string GetDefaultLogDir(IFileSystemFacade fileSystemFacade)
		{
			return Path.Combine(fileSystemFacade.GetProcessDirectory(), "logs");
		}

		/// <summary>
		/// Implementation for IDisposable.Dispose().
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases object resources.
		/// </summary>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				(currFile as IDisposable)?.Dispose();
			}
		}
	}
}
