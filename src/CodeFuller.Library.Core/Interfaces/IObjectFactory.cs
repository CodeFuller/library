namespace CodeFuller.Library.Core.Interfaces
{
	/// <summary>
	/// Interface for Object Factory.
	/// </summary>
	public interface IObjectFactory<out TType> where TType : class
	{
		/// <summary>
		/// Creates instance of TType.
		/// </summary>
		TType CreateInstance();
	}
}
