using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
