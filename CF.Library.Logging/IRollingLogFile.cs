using System.IO;

namespace CF.Library.Logging
{
	internal interface IRollingLogFile
	{
		StreamWriter StreamWriter { get; }

		void Write(string data);
	}
}
