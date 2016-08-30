using System;

namespace CF.Library.Testing
{
	/// <summary>
	/// Helper for temporary directory.
	/// Directory is created in constructor and deleted (with all content) on disposing.
	/// </summary>
	public class TempDirectory : IDisposable
	{
		/// <summary>
		/// Path of temporary directory created.
		/// </summary>
		public string Path { get; }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <remarks>
		/// Generates temporary path and create the directory.
		/// </remarks>
		public TempDirectory()
		{
			Path = GetTempDirectoryPath();
			System.IO.Directory.CreateDirectory(Path);
		}

		private static string GetTempDirectoryPath()
		{
			return System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName());
		}

		/// <summary>
		/// Returns a string that represents the current object, i.e. directory path.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return Path;
		}

		/// <summary>
		/// Implementation for IDisposable.Dispose().
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases object resources.
		/// </summary>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				System.IO.Directory.Delete(Path, true);
			}
		}
	}
}
