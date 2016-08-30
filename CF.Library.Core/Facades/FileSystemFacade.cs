using System;
using System.IO;
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
		/// Returns directory where assembly of executing process is placed.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "The method is not suitable for converting into property")]
		string GetProcessDirectory();

		/// <summary>
		/// Creates instance of IStreamWriterFacade for given path.
		/// </summary>
		IStreamWriterFacade CreateStreamWriter(string path, bool append, Encoding encoding, bool autoFlush);
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
		/// Implementation for IFileSystemFacade.GetProcessDirectory().
		/// </summary>
		public string GetProcessDirectory()
		{
			Assembly assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
			return Path.GetDirectoryName(assembly.Location);
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
	}
}
