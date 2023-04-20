namespace BlazorBLE.Data
{
    public sealed class ShiftList<T>
    {
        public IReadOnlyList<T> List => list;

        private readonly List<T> list = new();
        private readonly int capacity;

        public ShiftList(int capacity)
        {
            this.capacity = capacity;
        }

        public void Add(T value)
        {
            list.Add(value);

            if (list.Count > capacity)
            {
                list.RemoveAt(0);
            }
        }
    }
}
