using System.Threading;

namespace CF.Library.Core.Bootstrap
{
	/// <summary>
	/// Interface for running application logic.
	/// </summary>
	public interface IApplicationLogic
	{
		/// <summary>
		/// Runs application logic.
		/// </summary>
		int Run(string[] args, CancellationToken cancellationToken);
	}
}
