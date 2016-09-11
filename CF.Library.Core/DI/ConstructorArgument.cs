namespace CF.Library.Core.DI
{
	/// <summary>
	/// Holder for argument value passed to instance constructor.
	/// </summary>
	public class ConstructorArgument
	{
		/// <summary>
		/// Parameter name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Parameter value.
		/// </summary>
		public object Value { get; set; }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">Parameter name.</param>
		/// <param name="value">Parameter value.</param>
		public ConstructorArgument(string name, object value)
		{
			Name = name;
			Value = value;
		}
	}
}
