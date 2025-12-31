using System.Text;

namespace CodeFuller.Library.Logging.Interfaces
{
	internal interface IFileSystem
	{
		bool FileExists(string path);

		void CreateDirectory(string path);

		IStreamWriterWrapper CreateStreamWriter(string path, bool append, Encoding encoding, bool autoFlush);
	}
}
