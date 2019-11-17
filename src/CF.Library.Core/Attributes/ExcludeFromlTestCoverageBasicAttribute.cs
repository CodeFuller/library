using System;

namespace CF.Library.Core.Attributes
{
	/// <summary>
	/// Base abstract attribute that all other coverage exclusion attributes should inherit.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event,
		Inherited = false, AllowMultiple = false)]
	public abstract class ExcludeFromTestCoverageBasicAttribute : Attribute
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		protected ExcludeFromTestCoverageBasicAttribute(string justification)
		{
			Justification = justification;
		}

		/// <summary>
		/// Justification for excluding the code from test coverage.
		/// </summary>
		public string Justification { get; }
	}
}
