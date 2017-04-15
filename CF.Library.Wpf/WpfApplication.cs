using System;
using System.Windows;
using CF.Library.Core.Bootstrap;

namespace CF.Library.Wpf
{
	/// <summary>
	/// Abstract class for WPF application.
	/// </summary>
	public abstract class WpfApplication<TRootViewModel> : Application
	{
		/// <summary>
		/// Application bootstrapper.
		/// </summary>
		protected IBootstrapper<TRootViewModel> Bootstrapper { get; }

		/// <summary>
		/// Constructor.
		/// </summary>
		protected WpfApplication(IBootstrapper<TRootViewModel> bootstrapper)
		{
			if (bootstrapper == null)
			{
				throw new ArgumentNullException(nameof(bootstrapper));
			}

			Bootstrapper = bootstrapper;
		}

		/// <summary>
		/// Runs WPF application.
		/// </summary>
		protected abstract void Run(TRootViewModel rootViewModel);

		/// <summary>
		/// Initializes and runs WPF application.
		/// </summary>
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			TRootViewModel rootViewModel = Bootstrapper.Run();
			Run(rootViewModel);
		}
	}
}
