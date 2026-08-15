using Combat.API.Skills;
using Combat.API.Statuses;
using Combat.API.Events;
using Combat.Common.ValueObjects;

namespace Combat.API.Contexts
{
    public interface IStatusContext
    {
        TQuery GetCapability<TQuery>() where TQuery : class;
        StatusState<T> GetState<T>() where T : unmanaged, IDynamicStatusData;
        void SaveState<T>(StatusState<T> value) where T : unmanaged, IDynamicStatusData;

        EventHandlerId SubscribeToEvent<TEventData>(IEventContext.EventHandler<TEventData> callback) where TEventData : IEventData;
        void Unsubscribe(EventHandlerId id);
    }
}
