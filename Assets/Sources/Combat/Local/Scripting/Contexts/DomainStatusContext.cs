using Combat.API.Contexts;
using Combat.API.Events;
using Combat.API.Statuses;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Facades;
using Combat.Local.Domain.Repositories;

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Combat.Local.Scripting.Contexts
{
    public sealed class DomainStatusContext : IStatusContext
    {
        private readonly StatusId _id;
        private readonly IEnvironmentContext _environmentContext;
        private readonly IEventContext _eventContext;
        private readonly IStatusDynamicMemoryRepository _memoryRepository;
        private readonly StatusFacade _statusFacade;

        private readonly List<EventHandlerId> _eventHandlers;

        public DomainStatusContext(StatusId id, IStatusDynamicMemoryRepository memoryRepository, IEventContext eventContext, StatusFacade statusFacade)
        {
            _id = id;
            _memoryRepository = memoryRepository;
            _eventContext = eventContext;
            _environmentContext = null;
            _statusFacade = statusFacade;

            _eventHandlers = new();
        }

        public void StartPeriodicAction(float interval) => _statusFacade.StartPeriodicAction(_id, interval, 0);

        public void StopPeriodocAction() => _statusFacade.StopPeriodicAction(_id);

        public StatusState<T> GetState<T>() where T : unmanaged, IDynamicStatusData
        {
            if (_memoryRepository.TryGetRawData(_id, out ReadOnlySpan<byte> value) == false)
            {
                return default;
            }

            T dynamicData = MemoryMarshal.Read<T>(value);
            return new(dynamicData);
        }

        public void SaveState<T>(StatusState<T> value) where T : unmanaged, IDynamicStatusData
        {
            _memoryRepository.Save(_id, value);
        }

        public EventHandlerId SubscribeToEvent<TEventData>(IEventContext.EventHandler<TEventData> callback) where TEventData : IEventData
        {
            EventHandlerId id = _eventContext.Subscribe(callback);
            _eventHandlers.Add(id);
            return id;
        }

        public void Unsubscribe(EventHandlerId eventHandlerId)
        {
            if (_eventHandlers.Remove(eventHandlerId) == false)
            {
                return;
            }

            _eventContext.Unsubscribe(eventHandlerId);
        }

        public TQuery GetCapability<TQuery>() where TQuery : class
        {
            if (typeof(TQuery) == typeof(IEnvironmentContext))
            {
                return _environmentContext as TQuery;
            }

            return null;
        }

        public void Cleanup()
        {
            foreach (EventHandlerId id in _eventHandlers)
            {
                _eventContext.Unsubscribe(id);
            }

            _eventHandlers.Clear();
            _memoryRepository.Delete(_id);
        }
    }
}
