using Combat.Common.ValueObjects;

namespace Combat.API.Contexts
{
    public interface IEventContext //TODO: rename
    {
        delegate void EventHandler<T>(GameEvent<T> @event) where T : IEventData;

        EventHandlerId Subscribe<T>(EventHandler<T> callback) where T : IEventData;
        void Publish<T>(GameEvent<T> @event) where T : IEventData;
        bool Unsubscribe(EventHandlerId handler);
    }
}
