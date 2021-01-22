using System.IO;

namespace CodeFuller.Library.Logging.Interfaces
{
	internal interface IRollingLogFile
	{
		StreamWriter StreamWriter { get; }

		void Write(string data);
	}
}
