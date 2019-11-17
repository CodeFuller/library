using System;

namespace CF.Library.Core.Attributes
{
	/// <summary>
	/// Specifies that attributed code should be excluded from coverage results, gathered for Unit tests.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event,
		Inherited = false, AllowMultiple = false)]
	public sealed class ExcludeFromUnitTestCoverageAttribute : ExcludeFromTestCoverageBasicAttribute
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public ExcludeFromUnitTestCoverageAttribute(string justification)
			: base(justification)
		{
		}
	}
}
