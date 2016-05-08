using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CF.Patterns
{
	/// <summary>
	/// Analogue of Lazy<T> that initializes value asynchronously
	/// </summary>
	/// <remarks>
	/// http://blogs.msdn.com/b/pfxteam/archive/2011/01/15/asynclazy-lt-t-gt.aspx
	/// </remarks>
	public class AsyncLazy<T> : Lazy<Task<T>>
	{
		public AsyncLazy(Func<T> valueFactory) :
			base(() => Task.Factory.StartNew(valueFactory))
		{
		}

		public AsyncLazy(Func<Task<T>> taskFactory) :
			base(() => Task.Factory.StartNew(taskFactory).Unwrap())
		{
		}

		public TaskAwaiter<T> GetAwaiter()
		{
			return Value.GetAwaiter();
		}
	}
}
