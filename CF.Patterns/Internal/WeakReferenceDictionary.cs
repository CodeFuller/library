using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Patterns.Internal
{
	internal class WeakReferenceDictionary<TValue>
	{
		protected readonly List<Tuple<WeakReference, TValue>> Items = new List<Tuple<WeakReference, TValue>>();

		public TValue this[object key]
		{
			get
			{
				TValue result;
				if (!TryGetValue(key, out result))
				{
					throw new KeyNotFoundException();
				}

				return result;
			}

			set
			{
				var itemIndex = FindItem(key);
				if (itemIndex == -1)
				{
					Items.Add(new Tuple<WeakReference, TValue>(new WeakReference(key), value));
				}
				else
				{
					Items[itemIndex] = new Tuple<WeakReference, TValue>(Items[itemIndex].Item1, value);
				}
			}
		}

		public int Count => Items.Count;

		public bool TryGetValue(object key, out TValue value)
		{
			var itemIndex = FindItem(key);
			if (itemIndex == -1)
			{
				value = default(TValue);
				return false;
			}
			else
			{
				value = Items[itemIndex].Item2;
				return true;
			}
		}

		public bool Remove(object key)
		{
			return Items.RemoveAll(it => EqualItems(it, key) || !it.Item1.IsAlive) > 0;
		}

		public virtual void RemoveDeadReferences()
		{
			Items.RemoveAll(it => !it.Item1.IsAlive);
		}

		private int FindItem(object obj)
		{
			return Items.FindIndex(it => EqualItems(it, obj));
		}

		private bool EqualItems(Tuple<WeakReference, TValue> item, object obj)
		{
			return ReferenceEquals(item.Item1.Target, obj);
		}
	}
}
