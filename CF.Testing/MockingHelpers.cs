using System;
using System.Linq;
using System.Reflection;

namespace CF.Testing
{
	/// <summary>
	/// Holder for mocking helper methods.
	/// </summary>
	/// <remarks>
	/// Copy/paste from https://github.com/nsubstitute/NSubstitute/issues/222
	/// </remarks>
	public static class MockingHelpers
	{
		/// <summary>
		/// Calls non-puplic method on target.
		/// </summary>
		public static object Protected(this object target, string name, params object[] args)
		{
			if (target == null)
			{
				throw new ArgumentNullException(nameof(target));
			}

			var type = target.GetType();
			var method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).Single(x => x.Name == name);

			return method.Invoke(target, args);
		}
	}
}
