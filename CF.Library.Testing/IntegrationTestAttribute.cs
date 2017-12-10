using System;

namespace CF.Library.Testing
{
	[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
	public sealed class IntegrationTestAttribute : Attribute
	{
	}
}
