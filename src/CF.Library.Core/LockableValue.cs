using System;

namespace CF.Library.Core
{
	/// <summary>
	/// Wrapper for value that could be locked for any modifications.
	/// </summary>
	/// <remarks>
	/// This class is useful when implementing Property Injection pattern.
	/// </remarks>
	public class LockableValue<TValue>
	{
		private bool valueIsLocked;

		private TValue value;

		/// <summary>
		/// Constructor.
		/// </summary>
		public LockableValue(TValue value)
		{
			this.value = value;
		}

		/// <summary>
		/// Property for stored value.
		/// </summary>
		public TValue Value
		{
			get
			{
				return value;
			}

			[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1500:VariableNamesShouldNotMatchFieldNames", Justification = "Used name is the most suitable for this case.")]
			set
			{
				if (valueIsLocked)
				{
					throw new InvalidOperationException("Value could be modified after lock");
				}

				this.value = value;
			}
		}

		/// <summary>
		/// Prohibits modification of stored value.
		/// </summary>
		public void Lock()
		{
			valueIsLocked = true;
		}
	}
}
