using System.Collections.Generic;
using System.Collections;
using System;

namespace parserlog.dotnet.ui.view_model
{

    class CompareReverse<T> : IComparer<T>
    {
        public int Compare(T left, T right)
        {
            return Comparer<T>.Default.Compare(right, left);
        }
    }

    public class MultiSortedList<TKey, TValue>
		: IEnumerable<KeyValuePair<TKey, TValue>>, 
		IEnumerator<KeyValuePair<TKey, TValue>>
    {
        public MultiSortedList()
		{
			container = new SortedList<TKey, List<TValue>>();
			rewind = true;
		}
        public MultiSortedList(IComparer<TKey> comparer)
        {
            container = new SortedList<TKey, List<TValue>>(comparer);
            rewind = true;
        }

        public void Dispose()
		{ }

        public bool ContainsKey(TKey key)
        {
            return container.ContainsKey(key);
        }

        public List<TValue> this[TKey key]
        {
            get { return container[key]; }
            set
            {
                container[key] = value;
            }
        }

        public void Add(TKey key, TValue value)
		{
			if (container.ContainsKey(key))
				container[key].Add(value);
			else
				container.Add(key, new List<TValue> { value });
		}

		//IEnumerable
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			Reset();
			return this;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		//IEnumerator
		public bool MoveNext()
		{
			if (rewind)
				return FindFirst();
			return FindNext();
		}

		public void Reset()
		{
			rewind = true;
		}

		public KeyValuePair<TKey, TValue> Current
		{
			get
			{
				return new KeyValuePair<TKey, TValue>(enumerator_items.Current.Key, enumerator_values.Current);
			}
		}
		object IEnumerator.Current
		{
			get { return Current; }
		}

		//private
		private bool FindFirst()
		{
			rewind = false;
			enumerator_items = container.GetEnumerator();
			if (enumerator_items.MoveNext())
				enumerator_values = enumerator_items.Current.Value.GetEnumerator();
			else
				return false;

			return FindNext();
		}

		private bool FindNext()
		{
			while (!enumerator_values.MoveNext())
				if (!enumerator_items.MoveNext())
					return false;
				else
					enumerator_values = enumerator_items.Current.Value.GetEnumerator();
			return true;
		}

		private IEnumerator<KeyValuePair<TKey, List<TValue>>> enumerator_items;
		private IEnumerator<TValue> enumerator_values;
		private bool rewind;
		private SortedList<TKey, List<TValue>> container;
	}
}
