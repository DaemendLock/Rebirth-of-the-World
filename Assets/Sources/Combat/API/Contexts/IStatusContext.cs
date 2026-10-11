using Combat.API.Events;
using Combat.API.Statuses;
using Combat.Common.Primitives;

namespace Combat.API.Contexts
{
    public interface IStatusContext
    {
        void StartPeriodicAction(float delay);
        void StopPeriodocAction();

        StatusState<T> GetState<T>() where T : unmanaged, IDynamicStatusData;
        void SaveState<T>(StatusState<T> value) where T : unmanaged, IDynamicStatusData;

        EventHandlerId SubscribeToEvent<TEventData>(IEventContext.EventHandler<TEventData> callback) where TEventData : IEventData;
        void Unsubscribe(EventHandlerId id);

        TQuery GetCapability<TQuery>() where TQuery : class;
    }
}
