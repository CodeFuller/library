using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CF.Library.Core.Facades
{
	/// <summary>
	/// Facade interface for StreamWriter.
	/// </summary>
	public interface IStreamWriterFacade : IDisposable
	{
		/// <summary>
		/// Facade method for StreamWriter.Write(string value).
		/// </summary>
		void Write(string value);

		/// <summary>
		/// Facade method for StreamWriter.Close().
		/// </summary>
		void Close();

		/// <summary>
		/// Facade property for StreamWriter.BaseStream.Length.
		/// </summary>
		long Length { get; }
	}

	/// <summary>
	/// Facade for StreamWriter.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Justification = "False positive. See http://stackoverflow.com/a/8926598/5740031 for details.")]
	public class StreamWriterFacade : StreamWriter, IStreamWriterFacade
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public StreamWriterFacade(string path, bool append, Encoding encoding)
			: base(path, append, encoding)
		{
		}

		/// <summary>
		/// Implementation for IStreamWriterFacade.Length.
		/// </summary>
		public long Length => BaseStream.Length;
	}

	/// <summary>
	/// Facade interface for System.IO calls.
	/// </summary>
	public interface IFileSystemFacade
	{
		/// <summary>
		/// Determines whether the specified file exists.
		/// </summary>
		bool FileExists(string path);

		/// <summary>
		/// Determines whether the given path refers to an existing directory on disk.
		/// </summary>
		bool DirectoryExists(string path);

		/// <summary>
		/// Creates all directories and subdirectories in the specified path unless they already exist.
		/// </summary>
		void CreateDirectory(string path);

		/// <summary>
		/// Returns an enumerable collection of directory names in a specified path.
		/// </summary>
		IEnumerable<string> EnumerateDirectories(string path);

		/// <summary>
		/// Returns an enumerable collection of file names in a specified path.
		/// </summary>
		IEnumerable<string> EnumerateFiles(string path);

		/// <summary>
		/// Checks whether specified directory is empty.
		/// </summary>
		bool DirectoryIsEmpty(string path);

		/// <summary>
		/// Moves a directory to new location.
		/// </summary>
		void MoveDirectory(string sourceDirName, string destDirName);

		/// <summary>
		/// Deletes an empty directory from a specified path.
		/// </summary>
		void DeleteDirectory(string path);

		/// <summary>
		/// Deletes the specified directory and, if indicated, any subdirectories and files in the directory.
		/// </summary>
		void DeleteDirectory(string path, bool recursive);

		/// <summary>
		/// Copies an existing file to a new file. Overwriting a file of the same name is not allowed.
		/// </summary>
		void CopyFile(string sourceFileName, string destFileName);

		/// <summary>
		/// Moves a specified file to a new location.
		/// </summary>
		void MoveFile(string sourceFileName, string destFileName);

		/// <summary>
		/// Deletes specified file.
		/// </summary>
		void DeleteFile(string fileName);

		/// <summary>
		/// Returns executable file name of current process.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "The method is not suitable for converting into property")]
		string GetProcessExecutableFileName();

		/// <summary>
		/// Returns directory where assembly of executing process is placed.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "The method is not suitable for converting into property")]
		string GetProcessDirectory();

		/// <summary>
		/// Creates instance of IStreamWriterFacade for given path.
		/// </summary>
		IStreamWriterFacade CreateStreamWriter(string path, bool append, Encoding encoding, bool autoFlush);

		/// <summary>
		/// Clears Read-only attribute for the file.
		/// </summary>
		void ClearReadOnlyAttribute(string fileName);

		/// <summary>
		/// Sets Read-only attribute for the file.
		/// </summary>
		void SetReadOnlyAttribute(string fileName);

		/// <summary>
		/// Returns value of Read-only attribute for the file.
		/// </summary>
		bool GetReadOnlyAttribute(string fileName);
	}

	/// <summary>
	/// Facade for System.IO calls.
	/// </summary>
	public class FileSystemFacade : IFileSystemFacade
	{
		/// <summary>
		/// Implementation for IFileSystemFacade.FileExists(string path).
		/// </summary>
		public bool FileExists(string path)
		{
			return File.Exists(path);
		}

		/// <summary>
		/// Implementation for IFileSystemFacade.DirectoryExists(string path).
		/// </summary>
		public bool DirectoryExists(string path)
		{
			return Directory.Exists(path);
		}

		/// <summary>
		/// Implementation for IFileSystemFacade.CreateDirectory(string path).
		/// </summary>
		public void CreateDirectory(string path)
		{
			Directory.CreateDirectory(path);
		}

		/// <summary>
		/// Returns an enumerable collection of directory names in a specified path.
		/// </summary>
		public IEnumerable<string> EnumerateDirectories(string path)
		{
			return Directory.EnumerateDirectories(path);
		}

		/// <summary>
		/// Returns an enumerable collection of file names in a specified path.
		/// </summary>
		public IEnumerable<string> EnumerateFiles(string path)
		{
			return Directory.EnumerateFiles(path);
		}

		/// <summary>
		/// Checks whether specified directory is empty.
		/// </summary>
		public bool DirectoryIsEmpty(string path)
		{
			return !Directory.EnumerateFileSystemEntries(path).Any();
		}

		/// <summary>
		/// Moves a directory to new location.
		/// </summary>
		public void MoveDirectory(string sourceDirName, string destDirName)
		{
			Directory.Move(sourceDirName, destDirName);
		}

		/// <summary>
		/// Deletes an empty directory from a specified path.
		/// </summary>
		public void DeleteDirectory(string path)
		{
			Directory.Delete(path);
		}

		/// <summary>
		/// Deletes the specified directory and, if indicated, any subdirectories and files in the directory.
		/// </summary>
		public void DeleteDirectory(string path, bool recursive)
		{
			Directory.Delete(path, recursive);
		}

		/// <summary>
		/// Copies an existing file to a new file. Overwriting a file of the same name is not allowed.
		/// </summary>
		public void CopyFile(string sourceFileName, string destFileName)
		{
			File.Copy(sourceFileName, destFileName);
		}

		/// <summary>
		/// Moves a specified file to a new location.
		/// </summary>
		public void MoveFile(string sourceFileName, string destFileName)
		{
			File.Move(sourceFileName, destFileName);
		}

		/// <summary>
		/// Deletes specified file.
		/// </summary>
		public void DeleteFile(string fileName)
		{
			File.Delete(fileName);
		}

		/// <summary>
		/// Implementation for IFileSystemFacade.GetProcessExecutableFilename().
		/// </summary>
		public string GetProcessExecutableFileName()
		{
			Assembly assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
			return assembly.Location;
		}

		/// <summary>
		/// Implementation for IFileSystemFacade.GetProcessDirectory().
		/// </summary>
		public string GetProcessDirectory()
		{
			return Path.GetDirectoryName(GetProcessExecutableFileName());
		}

		/// <summary>
		/// Implementation for IFileSystemFacade.CreateStreamWriter(string path, bool append, Encoding encoding, bool autoFlush).
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope", Justification = "Object is disposed by the caller")]
		public IStreamWriterFacade CreateStreamWriter(string path, bool append, Encoding encoding, bool autoFlush)
		{
			return new StreamWriterFacade(path, append, encoding)
			{
				AutoFlush = autoFlush
			};
		}

		/// <summary>
		/// Clears Read-only attribute for the file.
		/// </summary>
		public void ClearReadOnlyAttribute(string fileName)
		{
			FileInfo fileInfo = new FileInfo(fileName);
			fileInfo.IsReadOnly = false;
		}

		/// <summary>
		/// Sets Read-only attribute for the file.
		/// </summary>
		public void SetReadOnlyAttribute(string fileName)
		{
			FileInfo fileInfo = new FileInfo(fileName);
			fileInfo.IsReadOnly = true;
		}

		/// <summary>
		/// Returns value of Read-only attribute for the file.
		/// </summary>
		public bool GetReadOnlyAttribute(string fileName)
		{
			FileInfo fileInfo = new FileInfo(fileName);
			return fileInfo.IsReadOnly;
		}
	}
}
