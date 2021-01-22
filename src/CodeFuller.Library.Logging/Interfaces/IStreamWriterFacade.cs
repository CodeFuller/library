﻿using System;
using System.IO;

namespace CodeFuller.Library.Logging.Interfaces
{
	internal interface IStreamWriterFacade : IDisposable
	{
		StreamWriter StreamWriter { get; }

		long Length { get; }

		void Close();
	}
}
