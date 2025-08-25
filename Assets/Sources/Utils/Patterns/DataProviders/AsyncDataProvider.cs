using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Utils.Patterns.DataProviders
{
    public abstract class AsyncDataProvider<TKey, TValue>
    {
        private readonly Dictionary<TKey, TaskCompletionSource<TValue>> _requiredData;

        protected AsyncDataProvider()
        {
            _requiredData = new();
        }

        public void ProvideData(TKey key, TValue value)
        {
            ResponeAwaits(key, value);
        }

        public async Task<TValue> GetValue(TKey key)
        {
            if (_requiredData.TryGetValue(key, out TaskCompletionSource<TValue> requirement))
            {
                return await requirement.Task;
            }

            requirement = new();
            _requiredData.Add(key, requirement);

            OnValueRequested(key);

            return await requirement.Task;
        }

        protected abstract void OnValueRequested(TKey key);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ResponeAwaits(TKey key, TValue value)
        {
            if (_requiredData.TryGetValue(key, out TaskCompletionSource<TValue> task) == false)
            {
                return;
            }

            _requiredData.Remove(key);
            task.SetResult(value);
        }
    }
}
