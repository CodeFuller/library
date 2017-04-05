using System;

namespace CF.Library.Patterns.Internal
{
	internal class WeakReferenceMap : WeakReferenceDictionary<WeakReference>
	{
		public bool TryGetValue(object key, out object value)
		{
			WeakReference objRef;
			if (TryGetValue(key, out objRef))
			{
				value = objRef.Target;
				return true;
			}
			else
			{
				value = null;
				return false;
			}
		}

		public void RemoveInstance(object key)
		{
			Items.RemoveAll(it => ReferenceEquals(it.Item1.Target, key) || ReferenceEquals(it.Item2.Target, key));
			RemoveDeadReferences();
		}

		public override void RemoveDeadReferences()
		{
			Items.RemoveAll(it => !it.Item1.IsAlive || !it.Item2.IsAlive);
		}
	}
}
