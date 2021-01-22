using System.Text;

namespace CodeFuller.Library.Logging.Interfaces
{
	internal interface IFileSystemFacade
	{
		bool FileExists(string path);

		bool DirectoryExists(string path);

		void CreateDirectory(string path);

		IStreamWriterFacade CreateStreamWriter(string path, bool append, Encoding encoding, bool autoFlush);
	}
}
