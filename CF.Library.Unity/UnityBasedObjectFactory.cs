using System;
using CF.Library.Core.Interfaces;
using Unity;

namespace CF.Library.Unity
{
	/// <summary>
	/// Implementation of IObjectFactory based on Unity container.
	/// </summary>
	public class UnityBasedObjectFactory<TType> : IObjectFactory<TType> where TType : class
	{
		private readonly IUnityContainer unityContainer;

		/// <summary>
		/// Constructor.
		/// </summary>
		public UnityBasedObjectFactory(IUnityContainer unityContainer)
		{
			if (unityContainer == null)
			{
				throw new ArgumentNullException(nameof(unityContainer));
			}

			this.unityContainer = unityContainer;
		}

		/// <summary>
		/// Creates object instance by resolving type from Unity container.
		/// </summary>
		/// <returns></returns>
		public TType CreateInstance()
		{
			return unityContainer.Resolve<TType>();
		}
	}
}
