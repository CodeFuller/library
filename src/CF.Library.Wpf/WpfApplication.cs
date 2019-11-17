using System;
using System.Windows;
using CF.Library.Bootstrap;

namespace CF.Library.Wpf
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

			TRootViewModel rootViewModel = bootstrapper.Bootstrap(new string[0]);
			Run(rootViewModel);
		}
	}
}
