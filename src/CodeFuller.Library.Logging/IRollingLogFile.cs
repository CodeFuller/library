using System.IO;

namespace CodeFuller.Library.Logging
{
	internal interface IRollingLogFile
	{
		StreamWriter StreamWriter { get; }

		void Write(string data);
	}
}
