﻿using System;
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
	public static class MultilayerEventManager
	{
		internal delegate void LayerEventHandler(object target, object sender, MultilayerEventArgs e);

		internal const int CallsBetweenDeadReferencesRemoving = 10;

		internal static readonly Dictionary<Type, WeakReferenceMap> Parents = new Dictionary<Type, WeakReferenceMap>();
		internal static readonly Dictionary<Type, WeakReferenceDictionary<LayerEventHandler>> Handlers = new Dictionary<Type, WeakReferenceDictionary<LayerEventHandler>>();
		internal static int CallsAfterLastDeadReferencesRemoving = 0;

		public static void RegisterLowerLayer<TEventArgs>(object currentLayer, object lowerLayer)
		{
			lock (Parents)
			{
				CollectDeadReferences();
				Parents.ProvideValue(typeof(TEventArgs))[lowerLayer] = new WeakReference(currentLayer);
			}
		}

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

		public static void RegisterHandler<TTarget, TEventArgs>(TTarget target, Action<TTarget, object, TEventArgs> handler) where TEventArgs : MultilayerEventArgs
		{
			lock (Handlers)
			{
				CollectDeadReferences();
				Handlers.ProvideValue(typeof(TEventArgs))[target] = (t, s, e) => handler((TTarget)t, s, (TEventArgs)e);
			}
		}

		public static void UnRegisterHandler<TEventArgs>(object target)
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

		public static void UnRegisterInstance(object target)
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
		
		public static void TriggerEvent<TEventArgs>(object sender, TEventArgs e) where TEventArgs : MultilayerEventArgs
		{
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
							handler(target, sender, e);
							if (e.Handled)
							{
								break;
							}
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
		}

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