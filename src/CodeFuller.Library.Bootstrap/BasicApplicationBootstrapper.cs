﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CodeFuller.Library.Bootstrap
{
	public abstract class BasicApplicationBootstrapper<TRoot>
	{
		public abstract TRoot Bootstrap(string[] args);

		public abstract ILogger<TCategoryName> GetLogger<TCategoryName>();

		protected abstract void RegisterServices(IServiceCollection services, IConfiguration configuration);
	}
}