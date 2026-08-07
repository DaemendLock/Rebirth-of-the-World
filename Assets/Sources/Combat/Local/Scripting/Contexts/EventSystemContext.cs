using Combat.API.Contexts;
using Combat.Common.ValueObjects;

using System;
using System.Collections.Generic;

namespace Combat.Local.Scripting.Contexts
{
    public sealed class EventHandler
    {
        public readonly EventHandlerId Id;
        public readonly object Callback;

        public EventHandler(EventHandlerId id, object callback)
        {
            Id = id;
            Callback = callback;
        }

        public bool Dead { get; set; } = false;
    }

    public sealed class EventSystemContext : IEventContext
    {
        private readonly Dictionary<Type, List<EventHandler>> _callbacks;
        private readonly Dictionary<EventHandlerId, List<EventHandler>> _cleanupTargets;

        private int _nextId = 0;

        public EventSystemContext()
        {
            _callbacks = new();
            _cleanupTargets = new();
        }

        public void Publish<T>(GameEvent<T> @event) where T : unmanaged, IEventData
        {
            if (_callbacks.TryGetValue(typeof(T), out var callbacks) == false)
            {
                return;
            }

            foreach (EventHandler item in callbacks)
            {
                if (item.Dead)
                {
                    continue;
                }

                if (item.Callback is not IEventContext.EventHandler<T> callback)
                {
                    continue;
                }

                callback.Invoke(@event);
            }

            callbacks.RemoveAll(value => value.Dead);
        }

        public EventHandlerId Subscribe<T>(IEventContext.EventHandler<T> callback) where T : unmanaged, IEventData
        {
            EventHandlerId id = new(_nextId++);

            if (_callbacks.TryGetValue(typeof(T), out var values) == false)
            {
                values = new();
                _callbacks.Add(typeof(T), values);
            }

            values.Add(new(id, callback));
            _cleanupTargets.Add(id, values);
            return id;
        }

        public bool Unsubscribe(EventHandlerId handler)
        {
            if (_cleanupTargets.Remove(handler, out var target) == false)
            {
                return false;
            }

            for (int i = 0; i < target.Count; i++)
            {
                var oldValue = target[i];

                if (oldValue.Dead)
                {
                    continue;
                }

                if (oldValue.Id != handler)
                {
                    continue;
                }

                oldValue.Dead = true;
                return true;
            }

            return false;
        }
    }
}
