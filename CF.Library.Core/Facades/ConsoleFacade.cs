using System;

namespace CF.Library.Core.Facades
{
	/// <summary>
	/// Facade interface for System.Console.
	/// </summary>
	public interface IConsoleFacade
	{
		/// <summary>
		/// Facade method for Console.WriteLine(string message).
		/// </summary>
		void WriteLine(string message);

		/// <summary>
		/// Facade method for Console.Read().
		/// </summary>
		int Read();
	}

	/// <summary>
	/// Facade for System.Console.
	/// </summary>
	public class ConsoleFacade : IConsoleFacade
	{
		/// <summary>
		/// Facade method for Console.WriteLine(string message).
		/// </summary>
		public void WriteLine(string message)
		{
			Console.WriteLine(message);
		}

		/// <summary>
		/// Facade method for Console.Read().
		/// </summary>
		public int Read()
		{
			return Console.Read();
		}
	}
}
