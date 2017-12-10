using System;
using CF.Library.Core;
using CF.Library.Core.Bootstrap;
using CF.Library.Core.Configuration;
using CF.Library.Core.Logging;
using Unity;

namespace CF.Library.Unity
{
	/// <summary>
	/// Implementation of IBootstrapper based on Unity Container.
	/// </summary>
	public abstract class UnityBootstrapper<TApplication> : IBootstrapper<TApplication>
	{
		private bool executed;

		/// <summary>
		/// Property Injection for IUnityContainer.
		/// </summary>
		protected IUnityContainer DIContainer { get; set; } = new UnityContainer();

		/// <summary>
		/// Registers all dependencies required for application object.
		/// </summary>
		protected abstract void RegisterDependencies();

		/// <summary>
		/// Bootstraps application and returns instance of application object.
		/// </summary>
		public TApplication Run()
		{
			if (executed)
			{
				throw new InvalidOperationException("Bootstrapper should be launched only once");
			}
			executed = true;

			if (DIContainer.IsRegistered<ISettingsProvider>())
			{
				AppSettings.SettingsProvider = DIContainer.Resolve<ISettingsProvider>();
			}

			OnDependenciesRegistering();
			RegisterDependencies();
			OnDependenciesRegistered();

			if (DIContainer.IsRegistered<IMessageLogger>())
			{
				Application.Logger = DIContainer.Resolve<IMessageLogger>();
			}

			return DIContainer.Resolve<TApplication>();
		}

		/// <summary>
		/// Called before dependencies are registered in DI container.
		/// </summary>
		protected virtual void OnDependenciesRegistering()
		{
		}

		/// <summary>
		/// Called after dependencies are registered in DI container.
		/// </summary>
		protected virtual void OnDependenciesRegistered()
		{
		}

		/// <summary>
		/// Implementation for IDisposable.Dispose().
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases object resources.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", Justification = "False positive. See http://stackoverflow.com/q/34583417/5740031 for details.")]
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				DIContainer.Dispose();
			}
		}
	}
}
