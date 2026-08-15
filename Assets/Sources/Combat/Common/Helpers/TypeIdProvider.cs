using System.Threading;

namespace Assets.Sources.Combat.Common.Helpers
{
    public static class TypeIdProvider
    {
        private static int _counter;

        public static int Get<T>()
        {
            return TypeIdContainer<T>.Id;
        }

        public static class TypeIdContainer<T>
        {
            public static readonly int Id = Interlocked.Increment(ref _counter);
        }
    }
}
