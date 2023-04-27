using System.Collections;

namespace BlazorBLE.Data
{
    public sealed class ShiftList<T> : IList<T>
    {
        public int Count => list.Count;
        
        public bool IsReadOnly => false;

        private readonly List<T> list = new();
        private readonly int capacity;
        
        public T this[int index]
        {
            get => list[index];
            set => list[index] = value;
        }
        
        public ShiftList(int capacity) => this.capacity = capacity;

        public void Add(T value)
        {
            list.Add(value);

            if (list.Count > capacity)
            {
                list.RemoveAt(0);
            }
        }

        public void Clear() => list.Clear();

        public bool Contains(T item) => list.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);

        public bool Remove(T item) => list.Remove(item);

        public IEnumerator<T> GetEnumerator() => list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int IndexOf(T item) => list.IndexOf(item);

        public void Insert(int index, T item) => list.Insert(index, item);

        public void RemoveAt(int index) => list.RemoveAt(index);
    }
}
