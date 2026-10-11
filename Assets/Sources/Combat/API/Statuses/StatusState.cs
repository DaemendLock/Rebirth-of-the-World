using Combat.API.Skills;

namespace Combat.API.Statuses
{
    public readonly struct StatusState<T> where T : unmanaged, IDynamicStatusData
    {
        public StatusState(T dynamicState)
        {
            DynamicState = dynamicState;
        }

        public T DynamicState { get; }
    }

    public interface IDynamicStatusData { }
}
