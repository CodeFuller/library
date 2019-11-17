namespace CF.Library.Core.Enums
{
	/// <summary>
	/// Enumeration of possible buttons that are displayed in MessageBox.
	/// </summary>
	public enum ShowMessageBoxButton
	{
		/// <summary>
		/// Undefined default value.
		/// </summary>
		None,
		
		/// <summary>
		/// Message box contains OK button.
		/// </summary>
		Ok,

		/// <summary>
		/// Message box contains OK and Cancel buttons.
		/// </summary>
		OkCancel,

		/// <summary>
		/// Message box contains Yes, No and Cancel buttons.
		/// </summary>
		YesNoCancel,

		/// <summary>
		/// Message box contains Yes and No buttons.
		/// </summary>
		YesNo,
	}
}
