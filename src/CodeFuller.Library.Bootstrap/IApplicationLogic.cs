using System.Threading;
using System.Threading.Tasks;

namespace CodeFuller.Library.Bootstrap
{
	/// <summary>
	/// Interface for generic application logic.
	/// </summary>
	public interface IApplicationLogic
	{
		/// <summary>
		/// Executes application logic.
		/// </summary>
		/// <param name="args">Command line arguments.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>Exit code for the process.</returns>
		Task<int> Run(string[] args, CancellationToken cancellationToken);
	}
}
