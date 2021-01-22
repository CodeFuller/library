using CodeFuller.Library.Wpf.Interfaces;
using CodeFuller.Library.Wpf.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace CodeFuller.Library.Wpf
{
	/// <summary>
	/// Extension methods for adding WPF window service to the DI container.
	/// </summary>
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		/// Adds services required for using window service.
		/// </summary>
		/// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
		/// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
		public static IServiceCollection AddWpfWindowService(this IServiceCollection services)
		{
			services.AddSingleton<IWindowService, WpfWindowService>();

			return services;
		}
	}
}
