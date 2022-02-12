using System;

namespace CodeFuller.Library.Wpf.Extensions
{
	/// <summary>
	/// Extension methods for generic objects.
	/// </summary>
	public static class ObjectExtensions
	{
		/// <summary>
		/// Gets strongly-typed ViewModel from the DataContext object.
		/// </summary>
		/// <typeparam name="TViewModel">The type of expected ViewModel.</typeparam>
		/// <param name="dataContext">DataContext object.</param>
		/// <returns>Strongly-typed ViewModel from the DataContext object.</returns>
		public static TViewModel GetViewModel<TViewModel>(this Object dataContext)
			where TViewModel : class
		{
			_ = dataContext ?? throw new ArgumentNullException($"DataContext for {typeof(TViewModel)} is null");

			if (dataContext is not TViewModel viewModel)
			{
				throw new InvalidOperationException($"Unexpected type of DataContext: Expected {typeof(TViewModel)}, actual is {dataContext.GetType()}");
			}

			return viewModel;
		}
	}
}
