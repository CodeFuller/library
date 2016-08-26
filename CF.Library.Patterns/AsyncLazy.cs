using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CF.Library.Patterns
{
	/// <summary>
	/// Analogue of Lazy&lt;T&gt; that initializes value asynchronously
	/// </summary>
	/// <remarks>
	/// http://blogs.msdn.com/b/pfxteam/archive/2011/01/15/asynclazy-lt-t-gt.aspx
	/// </remarks>
	public class AsyncLazy<T> : Lazy<Task<T>>
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public AsyncLazy(Func<T> valueFactory) :
			base(() => Task.Factory.StartNew(valueFactory))
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need possibility to have asynchronous value factory")]
		public AsyncLazy(Func<Task<T>> taskFactory) :
			base(() => Task.Factory.StartNew(taskFactory).Unwrap())
		{
		}

		/// <summary>
		/// Accessor for TaskAwaiter of kept task
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Mimic of Task.GetAwaiter()")]
		public TaskAwaiter<T> GetAwaiter()
		{
			return Value.GetAwaiter();
		}
	}
}
