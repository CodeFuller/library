using System;

namespace CF.Library.Core.Attributes
{
	/// <summary>
	/// Specifies that attributed code should be excluded from coverage results, gathered for Unit and Integration tests.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event,
		Inherited = false, AllowMultiple = false)]
	public sealed class ExcludeFromTestCoverageAttribute : ExcludeFromTestCoverageBasicAttribute
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public ExcludeFromTestCoverageAttribute(string justification)
			: base(justification)
		{
		}
	}
}
