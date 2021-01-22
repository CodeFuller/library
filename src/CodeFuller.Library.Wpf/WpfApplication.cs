using System;
using System.Windows;
using CodeFuller.Library.Bootstrap;

namespace CodeFuller.Library.Wpf
{
	/// <summary>
	/// Abstract class for WPF application.
	/// </summary>
	public abstract class WpfApplication<TRootViewModel> : Application
	{
		private readonly BasicApplicationBootstrapper<TRootViewModel> bootstrapper;

		protected WpfApplication(BasicApplicationBootstrapper<TRootViewModel> bootstrapper)
		{
			this.bootstrapper = bootstrapper ?? throw new ArgumentNullException(nameof(bootstrapper));
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

			var rootViewModel = bootstrapper.Bootstrap(e.Args);
			Run(rootViewModel);
		}
	}
}
