using System.Threading;
using System.Threading.Tasks;

namespace CF.Library.ConsoleApplication
{
	public interface IApplicationLogic
	{
		Task<int> Run(string[] args, CancellationToken cancellationToken);
	}
}
