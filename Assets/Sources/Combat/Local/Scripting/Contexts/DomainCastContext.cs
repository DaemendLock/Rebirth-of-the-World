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
        private readonly ISkillContext _skillContext;
        private readonly IActor _caster;
        private readonly List<EventHandlerId> _eventHandlers;

        private bool _isDisposed;

        public DomainCastContext(ISkillContext skillContext, IActor caster)
        {
            _skillContext = skillContext;
            _caster = caster;
            _eventHandlers = new();
        }

        public IActor Caster => _caster;
        public AbilityKey Key => _skillContext.Key;

        public EventHandlerId SubscribeToEvent<TEventData>(IEventContext.EventHandler<TEventData> callback) where TEventData : IEventData
        {
            ThrowIfDisposed();

            EventHandlerId id = _skillContext.SubscribeToEvent(callback);
            _eventHandlers.Add(id);
            return id;
        }

        public void Unsubscribe(EventHandlerId id)
        {
            ThrowIfDisposed();

            if (_eventHandlers.Remove(id))
            {
                _skillContext.Unsubscribe(id);
            }
        }

        public TQuery GetCapability<TQuery>() where TQuery : class
        {
            ThrowIfDisposed();
            return _skillContext.GetCapability<TQuery>();
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            foreach (EventHandlerId id in _eventHandlers)
            {
                _skillContext.Unsubscribe(id);
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
