using Combat.API;
using Combat.API.Contexts;
using Combat.API.Events;
using Combat.Common.Primitives;

using System;
using System.Collections.Generic;

namespace Combat.Local.Scripting.Contexts
{
    public sealed class DomainCastContext : ICastContext, IDisposable
    {
        private readonly IEventContext _eventContext;
        private readonly IActor _caster;
        private readonly List<EventHandlerId> _eventHandlers;

        private bool _isDisposed;

        public DomainCastContext(IEventContext eventContext, IActor caster)
        {
            _caster = caster;
            _eventContext = eventContext;
            _eventHandlers = new();
        }

        public IActor Caster => _caster;

        public EventHandlerId SubscribeToEvent<TEventData>(IEventContext.EventHandler<TEventData> callback) where TEventData : IEventData
        {
            ThrowIfDisposed();

            EventHandlerId id = _eventContext.Subscribe(callback);
            _eventHandlers.Add(id);
            return id;
        }

        public void Unsubscribe(EventHandlerId id)
        {
            ThrowIfDisposed();

            if (_eventHandlers.Remove(id))
            {
                _eventContext.Unsubscribe(id);
            }
        }

        public TQuery GetCapability<TQuery>() where TQuery : class
        {
            ThrowIfDisposed();
            return null;
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            foreach (EventHandlerId id in _eventHandlers)
            {
                _eventContext.Unsubscribe(id);
            }

            _eventHandlers.Clear();
            _isDisposed = true;
        }

        private void ThrowIfDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(DomainCastContext));
            }
        }
    }
}
