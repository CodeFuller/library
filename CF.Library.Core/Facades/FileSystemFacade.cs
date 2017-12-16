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
		StreamWriter StreamWriter { get; }

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
		public StreamWriter StreamWriter => this;

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
		/// Returns an enumerable collection of directory names that match a search pattern in a specified path, and optionally searches subdirectories.
		/// </summary>
		IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption);

		/// <summary>
		/// Returns an enumerable collection of file names in a specified path.
		/// </summary>
		IEnumerable<string> EnumerateFiles(string path);

		/// <summary>
		/// Returns an enumerable collection of file names that match a search pattern in a specified path, and optionally searches subdirectories.
		/// </summary>
		IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption);

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
		/// Returns the parent directory of the specified path.
		/// </summary>
		string GetParentDirectory(string path);

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

		/// <summary>
		/// Opens a text file, reads all lines of the file, and then closes the file.
		/// </summary>
		string ReadAllText(string path);

		/// <summary>
		/// Opens a file, reads all lines of the file with the specified encoding, and then closes the file.
		/// </summary>
		string ReadAllText(string path, Encoding encoding);

		/// <summary>
		/// Creates a new file, writes the specified string to the file, and then closes the file. If the target file already exists, it is overwritten.
		/// </summary>
		void WriteAllText(string path, string contents);

		/// <summary>
		/// Creates a new file, writes the specified string to the file using the specified encoding, and then closes the file. If the target file already exists, it is overwritten.
		/// </summary>
		void WriteAllText(string path, string contents, Encoding encoding);

		/// <summary>
		/// Opens a binary file, reads the contents of the file into a byte array, and then closes the file.
		/// </summary>
		byte[] ReadAllBytes(string path);

		/// <summary>
		/// Creates a new file, writes the specified byte array to the file, and then closes the file. If the target file already exists, it is overwritten.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "bytes", Justification = "Using the same parameter name as in original method")]
		void WriteAllBytes(string path, byte[] bytes);

		/// <summary>
		/// Creates a uniquely named, zero-byte temporary file on disk and returns the full path of that file.
		/// </summary>
		string GetTempFileName();

		/// <summary>
		/// Returns file size, in bytes.
		/// </summary>
		long GetFileSize(string fileName);

		/// <summary>
		/// Returns the absolute path for the specified path string.
		/// </summary>
		string GetFullPath(string path);
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
		/// Returns an enumerable collection of directory names that match a search pattern in a specified path, and optionally searches subdirectories.
		/// </summary>
		public IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption)
		{
			return Directory.EnumerateDirectories(path, searchPattern, searchOption);
		}

		/// <summary>
		/// Returns an enumerable collection of file names in a specified path.
		/// </summary>
		public IEnumerable<string> EnumerateFiles(string path)
		{
			return Directory.EnumerateFiles(path);
		}

		/// <summary>
		/// Returns an enumerable collection of file names that match a search pattern in a specified path, and optionally searches subdirectories.
		/// </summary>
		public IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption)
		{
			return Directory.EnumerateFiles(path, searchPattern, searchOption);
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
		/// Returns the parent directory of the specified path.
		/// </summary>
		public string GetParentDirectory(string path)
		{
			return Directory.GetParent(path)?.FullName;
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

		/// <summary>
		/// Opens a text file, reads all lines of the file, and then closes the file.
		/// </summary>
		public string ReadAllText(string path)
		{
			return File.ReadAllText(path);
		}

		/// <summary>
		/// Opens a file, reads all lines of the file with the specified encoding, and then closes the file.
		/// </summary>
		public string ReadAllText(string path, Encoding encoding)
		{
			return File.ReadAllText(path, encoding);
		}

		/// <summary>
		/// Creates a new file, writes the specified string to the file, and then closes the file. If the target file already exists, it is overwritten.
		/// </summary>
		public void WriteAllText(string path, string contents)
		{
			File.WriteAllText(path, contents);
		}

		/// <summary>
		/// Creates a new file, writes the specified string to the file using the specified encoding, and then closes the file. If the target file already exists, it is overwritten.
		/// </summary>
		public void WriteAllText(string path, string contents, Encoding encoding)
		{
			File.WriteAllText(path, contents, encoding);
		}

		/// <summary>
		/// Opens a binary file, reads the contents of the file into a byte array, and then closes the file.
		/// </summary>
		public byte[] ReadAllBytes(string path)
		{
			return File.ReadAllBytes(path);
		}

		/// <summary>
		/// Creates a new file, writes the specified byte array to the file, and then closes the file. If the target file already exists, it is overwritten.
		/// </summary>
		public void WriteAllBytes(string path, byte[] bytes)
		{
			File.WriteAllBytes(path, bytes);
		}

		/// <summary>
		/// Creates a uniquely named, zero-byte temporary file on disk and returns the full path of that file.
		/// </summary>
		public string GetTempFileName()
		{
			return Path.GetTempFileName();
		}

		/// <summary>
		/// Returns file size, in bytes.
		/// </summary>
		public long GetFileSize(string fileName)
		{
			return new FileInfo(fileName).Length;
		}

		/// <summary>
		/// Returns the absolute path for the specified path string.
		/// </summary>
		public string GetFullPath(string path)
		{
			return Path.GetFullPath(path);
		}
	}
}
