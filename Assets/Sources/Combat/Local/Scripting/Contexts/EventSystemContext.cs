using Combat.API.Contexts;
using Combat.Common.ValueObjects;

using System;
using System.Collections.Generic;

namespace Combat.Local.Scripting.Contexts
{
    public sealed class EventSystemContext : IEventContext
    {
        private readonly struct EventHandler<T> : IEquatable<EventHandler<T>> where T : unmanaged, IEventData
        {
            public readonly EventHandlerId Id;
            public readonly IEventContext.EventHandler<T> Callback;

            public EventHandler(EventHandlerId id, IEventContext.EventHandler<T> callback)
            {
                Callback = callback;
                Id = id;
            }

            public override bool Equals(object obj) => obj is EventHandler<T> handler && Equals(handler);
            public bool Equals(EventHandler<T> other) => Id.Equals(other.Id);
            public override int GetHashCode() => Id.GetHashCode();

            public static bool operator ==(EventHandler<T> left, EventHandler<T> right) => left.Equals(right);

            public static bool operator !=(EventHandler<T> left, EventHandler<T> right) => !(left == right);
        }

        private interface IEventBucket
        {
            bool Remove(EventHandlerId id);
        }

        private sealed class EventBucket<T> : IEventBucket where T : unmanaged, IEventData
        {
            private EventHandler<T>[] _handlers;

            public EventBucket()
            {
                _handlers = Array.Empty<EventHandler<T>>();
            }

            public void Publish(GameEvent<T> @event)
            {
                ReadOnlySpan<EventHandler<T>> subscriptions = _handlers.AsSpan();

                foreach (var subscription in subscriptions)
                {
                    subscription.Callback(@event);
                }
            }

            public void AddHandler(EventHandlerId id, IEventContext.EventHandler<T> callback)
            {
                EventHandler<T> subscription = new(id, callback);

                Array.Resize(ref _handlers, _handlers.Length + 1);
                _handlers[^1] = subscription;
            }

            public bool Remove(EventHandlerId id)
            {
                for (int i = 0; i < _handlers.Length; i++)
                {
                    if (_handlers[i].Id != id)
                    {
                        continue;
                    }

                    EventHandler<T>[] buffer = new EventHandler<T>[_handlers.Length - 1];
                    Array.Copy(_handlers, 0, buffer, 0, i);

                    if (i != buffer.Length)
                        Array.Copy(_handlers, i + 1, buffer, i, buffer.Length - i);
                    _handlers = buffer;
                    return true;
                }

                return false;
            }
        }

        private readonly Dictionary<Type, IEventBucket> _buckets = new();
        private readonly Dictionary<EventHandlerId, IEventBucket> _idToBucket = new();

        private int _nextId;

        public void Publish<T>(GameEvent<T> @event) where T : unmanaged, IEventData
        {
            if (_buckets.TryGetValue(typeof(T), out IEventBucket bucket) == false)
            {
                return;
            }

            ((EventBucket<T>)bucket).Publish(@event);
        }

        public EventHandlerId Subscribe<T>(IEventContext.EventHandler<T> callback) where T : unmanaged, IEventData
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            EventHandlerId id = new(_nextId++);

            if (_buckets.TryGetValue(typeof(T), out IEventBucket bucket) == false)
            {
                bucket = new EventBucket<T>();
                _buckets.Add(typeof(T), bucket);
            }

            ((EventBucket<T>)bucket).AddHandler(id, callback);
            _idToBucket.Add(id, bucket);

            return id;
        }

        public bool Unsubscribe(EventHandlerId handlerId)
        {
            if (_idToBucket.Remove(handlerId, out IEventBucket bucket) == false)
            {
                return false;
            }

            if (bucket.Remove(handlerId) == false)
            {
                return false;
            }

            return true;
        }
    }
}
