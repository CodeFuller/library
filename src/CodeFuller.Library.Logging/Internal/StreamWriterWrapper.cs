using System.IO;
using System.Text;
using CodeFuller.Library.Logging.Interfaces;

namespace CodeFuller.Library.Logging.Internal
{
	internal class StreamWriterWrapper : StreamWriter, IStreamWriterWrapper
	{
		public StreamWriter StreamWriter => this;

		public long Length => BaseStream.Length;

		public StreamWriterWrapper(string path, bool append, Encoding encoding)
			: base(path, append, encoding)
		{
		}
	}
}
