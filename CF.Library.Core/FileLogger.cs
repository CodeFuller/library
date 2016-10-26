using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using CF.Library.Core.Facades;
using static System.FormattableString;
using static CF.Library.Core.Extensions.FormattableStringExtensions;

namespace CF.Library.Core
{
	/// <summary>
	/// Interface for generic logger.
	/// </summary>
	public interface ILogger
	{
		/// <summary>
		/// Writes raw unformatted data to the log.
		/// </summary>
		void Write(string message);
	}

	/// <summary>
	/// Interface for logger that writes messages to the file.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces", Justification = "Interface does not add new members to base interface but conceptually represents more narrow set of possible implementations.")]
	public interface IFileLogger : ILogger, IDisposable
	{
	}

	/// <summary>
	/// Simple file logger with support of rolling by size.
	/// </summary>
	public class FileLogger : IFileLogger
	{
		internal const int DefaultRollSize = 16*1024*1024;

		private const int MaxTriesForDuplicatedFileNames = 10;

		private const string FileNameDateTimePattern = "yyyy_MM_dd - HH_mm_ss";
		private const string FirstLogFileSuffix = " - START";
		private const string LogFileExtension = ".log";

		private readonly LockableValue<IFileSystemFacade> fileSystemFacade = new LockableValue<IFileSystemFacade>(new FileSystemFacade());
		private readonly LockableValue<IClock> clock = new LockableValue<IClock>(new SystemClock());

		private IStreamWriterFacade currFile;

		private string logDir;
		private string logName;
		private readonly int rollSize;

		private readonly string pid = Invariant($"{Process.GetCurrentProcess().Id}");

		/// <summary>
		/// Property injection for IFileSystemFacade.
		/// </summary>
		public IFileSystemFacade FileSystemFacade
		{
			get { return fileSystemFacade.Value; }
			set { fileSystemFacade.Value = value; }
		}

		/// <summary>
		/// Property injection for IClock.
		/// </summary>
		public IClock Clock
		{
			get { return clock.Value; }
			set { clock.Value = value; }
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public FileLogger()
		{
			rollSize = DefaultRollSize;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public FileLogger(string logName, int rollSize)
		{
			this.logName = logName;
			this.rollSize = rollSize;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public FileLogger(string logDir, string logName, int rollSize)
		{
			this.logDir = logDir;
			this.logName = logName;
			this.rollSize = rollSize;
		}

		/// <summary>
		/// Writes raw unformatted data to the log.
		/// </summary>
		public void Write(string message)
		{
			OpenIfRequired();

			RollIfRequired();

			currFile.Write(message);
		}

		private void OpenIfRequired()
		{
			//	First write?
			if (currFile == null)
			{
				fileSystemFacade.Lock();
				clock.Lock();

				if (logDir == null)
				{
					logDir = GetDefaultLogDir(FileSystemFacade);
				}

				if (logName == null)
				{
					logName = GetDefaultLogName(FileSystemFacade);
				}

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

		private IStreamWriterFacade OpenNextFile(bool firstFile)
		{
			return OpenNextFile(GenerateFileName(firstFile));
		}

		private IStreamWriterFacade OpenNextFile(string fileName)
		{
			return FileSystemFacade.CreateStreamWriter(fileName, true, Encoding.UTF8, true);
		}

		private string GenerateFileName(bool firstFile)
		{
			if (firstFile && !FileSystemFacade.DirectoryExists(logDir))
			{
				FileSystemFacade.CreateDirectory(logDir);
			}

			StringBuilder logFileNamePart = BuildLogFileNamePart(firstFile);
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

			throw new IOException(Current($"Failed to generate unique log filename after {MaxTriesForDuplicatedFileNames} tries. Following file exist:\n{triedFileNames}"));
		}

		private StringBuilder BuildLogFileNamePart(bool firstFile)
		{
			string timestampPart = Clock.Now.ToString(FileNameDateTimePattern, CultureInfo.InvariantCulture);
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

		private static string GetDefaultLogName(IFileSystemFacade fileSystemFacade)
		{
			return Path.GetFileNameWithoutExtension(fileSystemFacade.GetProcessExecutableFileName());
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
				currFile?.Dispose();
			}
		}
	}
}
