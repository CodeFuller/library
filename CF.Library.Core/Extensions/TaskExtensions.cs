using System;
using System.Threading.Tasks;

namespace CF.Library.Core.Extensions
{
	/// <summary>
	/// Holder for Task extension methods
	/// </summary>
	/// <remarks>
	/// Copy/Paste from IM project (that is copy/pasted from http://blogs.msdn.com/b/pfxteam/archive/2010/11/21/10094564.aspx)
	/// </remarks>
	public static class TaskExtensions
	{
		/// <summary>Creates a task that represents the completion of a follow-up function when a task completes.</summary>
		/// <param name="task">The task.</param>
		/// <param name="next">The function to run when the task completes.</param>
		/// <returns>The task that represents the completion of both the task and the function.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "All exceptions are caught by design")]
		public static Task<TResult> Then<TResult>(this Task task, Func<TResult> next)
		{
			if (task == null) throw new ArgumentNullException("task");
			if (next == null) throw new ArgumentNullException("next");

			var tcs = new TaskCompletionSource<TResult>();
			task.ContinueWith(delegate
			{
				if (task.IsFaulted) tcs.TrySetException(task.Exception.InnerExceptions);
				else if (task.IsCanceled) tcs.TrySetCanceled();
				else
				{
					try
					{
						var result = next();
						tcs.TrySetResult(result);
					}
					catch (Exception exc) { tcs.TrySetException(exc); }
				}
			}, TaskScheduler.Default);
			return tcs.Task;
		}

		/// <summary>Creates a task that represents the completion of a follow-up function when a task completes.</summary>
		/// <param name="task">The task.</param>
		/// <param name="next">The function to run when the task completes.</param>
		/// <returns>The task that represents the completion of both the task and the function.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "All exceptions are caught by design")]
		public static Task<TNewResult> Then<TResult, TNewResult>(this Task<TResult> task, Func<TResult, TNewResult> next)
		{
			if (task == null) throw new ArgumentNullException("task");
			if (next == null) throw new ArgumentNullException("next");

			var tcs = new TaskCompletionSource<TNewResult>();
			task.ContinueWith(delegate
			{
				if (task.IsFaulted) tcs.TrySetException(task.Exception.InnerExceptions);
				else if (task.IsCanceled) tcs.TrySetCanceled();
				else
				{
					try
					{
						var result = next(task.Result);
						tcs.TrySetResult(result);
					}
					catch (Exception exc) { tcs.TrySetException(exc); }
				}
			}, TaskScheduler.Default);
			return tcs.Task;
		}
	}
}
