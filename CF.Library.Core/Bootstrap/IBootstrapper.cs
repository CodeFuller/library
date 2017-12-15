using System;

namespace CF.Library.Core.Bootstrap
{
	/// <summary>
	/// Interface for application bootstrapper.
	/// </summary>
	public interface IBootstrapper<out TApplication> : IDisposable
	{
		/// <summary>
		/// Bootstraps application and returns instance of application object.
		/// </summary>
		TApplication Run(params string[] commandLineArgs);
	}
}
