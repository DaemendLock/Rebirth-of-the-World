using Combat.API.Events;
using Combat.Common.Primitives;

namespace Combat.API.Contexts
{
    public interface ICastContext
    {
        IActor Caster { get; }

        EventHandlerId SubscribeToEvent<TEventData>(IEventContext.EventHandler<TEventData> callback) where TEventData : IEventData;
        void Unsubscribe(EventHandlerId id);

        TQuery GetCapability<TQuery>() where TQuery : class;
    }
}
