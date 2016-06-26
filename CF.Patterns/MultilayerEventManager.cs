using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CF.Core;
using CF.Patterns.Internal;

namespace CF.Patterns
{
	/// <summary>
	/// Class for convenient subscribing and processing of events in deep hierarchies
	/// </summary>
	public static class MultilayerEventManager
	{
		internal delegate void LayerEventHandler(object target, object sender, MultilayerEventArgs e);

		internal const int CallsBetweenDeadReferencesRemoving = 10;

		internal static readonly Dictionary<Type, WeakReferenceMap> Parents = new Dictionary<Type, WeakReferenceMap>();
		internal static readonly Dictionary<Type, WeakReferenceDictionary<LayerEventHandler>> Handlers = new Dictionary<Type, WeakReferenceDictionary<LayerEventHandler>>();
		internal static int CallsAfterLastDeadReferencesRemoving = 0;

		/// <summary>
		/// Registers the relationship between upper and lower layer
		/// </summary>
		public static void RegisterLowerLayer<TEventArgs>(object currentLayer, object lowerLayer)
		{
			lock (Parents)
			{
				CollectDeadReferences();
				Parents.ProvideValue(typeof(TEventArgs))[lowerLayer] = new WeakReference(currentLayer);
			}
		}

		/// <summary>
		/// Serves for batch relationshiop registration between two layers for set of events
		/// </summary>
		public static void RegisterLowerLayerForEvents(object currentLayer, object lowerLayer, params Type[] eventTypes)
		{
			lock (Parents)
			{
				CollectDeadReferences();
				foreach (var eventType in eventTypes)
				{
					Parents.ProvideValue(eventType)[lowerLayer] = new WeakReference(currentLayer);
				}
			}
		}

		/// <summary>
		/// Registers the handler for the specific event
		/// </summary>
		public static void RegisterHandler<TTarget, TEventArgs>(TTarget target, Action<TTarget, object, TEventArgs> handler) where TEventArgs : MultilayerEventArgs
		{
			lock (Handlers)
			{
				CollectDeadReferences();
				Handlers.ProvideValue(typeof(TEventArgs))[target] = (t, s, e) => handler((TTarget)t, s, (TEventArgs)e);
			}
		}

		/// <summary>
		/// Unregisters the handler for the specific event
		/// </summary>
		public static void UnregisterHandler<TEventArgs>(object target)
		{
			lock (Handlers)
			{
				CollectDeadReferences();

				WeakReferenceDictionary<LayerEventHandler> eventHandlers;
				if (!Handlers.TryGetValue(typeof(TEventArgs), out eventHandlers))
				{
					return;
				}

				eventHandlers.Remove(target);
				if (eventHandlers.Count == 0)
				{
					Handlers.Remove(typeof(TEventArgs));
				}
			}
		}

		/// <summary>
		/// Unregisters all relationships and handlers for specific object
		/// </summary>
		public static void UnregisterInstance(object target)
		{
			lock (Parents)
			lock (Handlers)
			{
				CollectDeadReferences();
				
				foreach (var typeData in Handlers.ToList())
				{
					typeData.Value.Remove(target);
				}

				foreach (var typeData in Parents.ToList())
				{
					typeData.Value.RemoveInstance(target);
				}

				TrimDictionaries();
			}
		}

		/// <summary>
		/// Launches the chain of handlers calls for specific event, from lower to upper layer
		/// </summary>
		public static void TriggerEvent<TEventArgs>(object sender, TEventArgs e) where TEventArgs : MultilayerEventArgs
		{
			//	Handlers should not be called under lock
			//	Otherwise deadlock could happen because some handlers could switch to UI thread and trigger other events
			//	That's why handlers sequence is built under lock but handlers are called outside the lock
			var handlers = new List<Tuple<object, LayerEventHandler>>();

			lock (Parents)
			lock (Handlers)
			{
				CollectDeadReferences();

				WeakReferenceDictionary<LayerEventHandler> eventHandlers;
				if (Handlers.TryGetValue(e.GetType(), out eventHandlers))
				{
					WeakReferenceMap eventParents;
					Parents.TryGetValue(e.GetType(), out eventParents);

					object target = sender;
					while (target != null)
					{
						LayerEventHandler handler;
						if (eventHandlers.TryGetValue(target, out handler) && handler != null)
						{
							handlers.Add(new Tuple<object, LayerEventHandler>(target, handler));
						}

						if (eventParents == null)
						{
							break;
						}
						else
						{
							var targetType = target.GetType();
							if (!(eventParents.TryGetValue(target, out target) || eventParents.TryGetValue(targetType, out target)))
							{
								break;
							}
						}
					}
				}
			}

			foreach (var handler in handlers)
			{
				handler.Item2(handler.Item1, sender, e);
				if (e.Handled)
				{
					break;
				}
			}
		}

		/// <summary>
		/// Clears all relationship and handlers information
		/// </summary>
		public static void Clear()
		{
			lock (Parents)
			lock (Handlers)
			{
				Parents.Clear();
				Handlers.Clear();
			}
		}

		private static void CollectDeadReferences()
		{
			if (Interlocked.Increment(ref CallsAfterLastDeadReferencesRemoving) >= CallsBetweenDeadReferencesRemoving)
			{
				CallsAfterLastDeadReferencesRemoving = 0;
				RemoveDeadReferences();
			}
		}

		internal static void RemoveDeadReferences()
		{
			lock (Parents)
			lock (Handlers)
			{
				foreach (var typeData in Parents.ToList())
				{
					typeData.Value.RemoveDeadReferences();
				}

				foreach (var typeData in Handlers.ToList())
				{
					typeData.Value.RemoveDeadReferences();
				}

				TrimDictionaries();
			}
		}

		private static void TrimDictionaries()
		{
			foreach (var typeData in Parents.Where(dict => dict.Value.Count == 0).ToList())
			{
				Parents.Remove(typeData.Key);
			}

			foreach (var typeData in Handlers.Where(dict => dict.Value.Count == 0).ToList())
			{
				Handlers.Remove(typeData.Key);
			}
		}
	}
}
