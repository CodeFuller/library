using System;

namespace CF.Library.Core.DI
{
	/// <summary>
	/// Interface for instance creation.
	/// </summary>
	public interface IInstanceCreator
	{
		/// <summary>
		/// Creates instance of specified type, passing given arguments to its constructor.
		/// </summary>
		object CreateInstance(Type type, params ConstructorArgument[] constructorArguments);
	}
}
