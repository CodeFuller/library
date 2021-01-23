using System;
using System.Windows;
using CodeFuller.Library.Bootstrap;

namespace CodeFuller.Library.Wpf
{
	/// <summary>
	/// Abstract class for WPF application.
	/// </summary>
	/// <typeparam name="TRootViewModel">The type of application root view model.</typeparam>
	public abstract class WpfApplication<TRootViewModel> : Application
	{
		private readonly BasicApplicationBootstrapper<TRootViewModel> bootstrapper;

		/// <summary>
		/// Initializes a new instance of the <see cref="WpfApplication{TRootViewModel}"/> class with provided <see cref="BasicApplicationBootstrapper{TRoot}"/>.
		/// </summary>
		/// <param name="bootstrapper">Application bootstrapper.</param>
		protected WpfApplication(BasicApplicationBootstrapper<TRootViewModel> bootstrapper)
		{
			this.bootstrapper = bootstrapper ?? throw new ArgumentNullException(nameof(bootstrapper));
		}

		/// <summary>
		/// Runs WPF application.
		/// </summary>
		/// <param name="rootViewModel">The application root view model.</param>
		protected abstract void Run(TRootViewModel rootViewModel);

		/// <summary>
		/// Initializes and runs WPF application.
		/// </summary>
		/// <param name="e"><see cref="StartupEventArgs"/> event.</param>
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			var rootViewModel = bootstrapper.Bootstrap(e.Args);
			Run(rootViewModel);
		}
	}
}
