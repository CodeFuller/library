using System.IO;
using System.Text;
using CodeFuller.Library.Logging.Interfaces;

namespace CodeFuller.Library.Logging.Internal
{
	internal class FileSystemFacade : IFileSystemFacade
	{
		public bool FileExists(string path)
		{
			return File.Exists(path);
		}

		public bool DirectoryExists(string path)
		{
			return Directory.Exists(path);
		}

		public void CreateDirectory(string path)
		{
			Directory.CreateDirectory(path);
		}

		public IStreamWriterFacade CreateStreamWriter(string path, bool append, Encoding encoding, bool autoFlush)
		{
			return new StreamWriterFacade(path, append, encoding)
			{
				AutoFlush = autoFlush
			};
		}
	}
}
