using System.Threading;
using System.Threading.Tasks;

namespace CodeFuller.Library.Bootstrap
{
	public interface IApplicationLogic
	{
		Task<int> Run(string[] args, CancellationToken cancellationToken);
	}
}
