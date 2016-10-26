using System;

namespace CF.Library.Core.Interfaces
{
	/// <summary>
	/// Interface for application bootstrapper.
	/// </summary>
	public interface IBootstrapper : IDisposable
	{
		/// <summary>
		/// Bootstraps application.
		/// </summary>
		void Run();
	}
}
