using Combat.API.Contexts;
using Combat.API.Events;
using Combat.API.Objectives;
using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Facades;
using Combat.Local.Domain.Repositories.Objectives;

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Combat.Local.Scripting.Contexts
{
    public sealed class ObjectiveContext : IObjectiveContext
    {
        private readonly ObjectiveId _id;
        private readonly IObjectiveMemoryRepository _memoryRepository;
        private readonly IEventContext _eventContext;
        private readonly ObjectiveCompleteFacade _objectiveFacade;

        private readonly IEncounterContext _encounterContext;

        private readonly List<EventHandlerId> _subscriptions;

        private bool _disposed;

        public ObjectiveContext(ObjectiveId id, IObjectiveMemoryRepository memoryRepository, IEventContext eventContext, IEncounterContext context, ObjectiveCompleteFacade objectiveFacade)
        {
            _id = id;
            _disposed = false;
            _memoryRepository = memoryRepository;
            _eventContext = eventContext;
            _objectiveFacade = objectiveFacade;
            _encounterContext = context;

            _subscriptions = new List<EventHandlerId>();
            State = ObjectiveState.Running;
        }

        public ObjectiveId Id => _id;

        public ObjectiveState State { get; private set; }

        public void Complete()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(ObjectiveContext));
            }

            _objectiveFacade.Finalize(_id, ObjectiveState.Completed);
            State = ObjectiveState.Completed;
        }

        public void Cancel()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(ObjectiveContext));
            }

            _objectiveFacade.Finalize(_id, ObjectiveState.Cancelled);
            State = ObjectiveState.Cancelled;
        }

        public void Fail()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(ObjectiveContext));
            }

            _objectiveFacade.Finalize(_id, ObjectiveState.Failed);
            State = ObjectiveState.Failed;
        }

        public void Save<T>(T value) where T : unmanaged, IObjectiveData
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(ObjectiveContext));
            }

            _memoryRepository.Save(_id, value);
        }

        public ObjectiveInfo<T> GetInfo<T>() where T : unmanaged, IObjectiveData
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(ObjectiveContext));
            }

            if (_memoryRepository.TryGetRawData(_id, out ReadOnlySpan<byte> value) == false)
            {
                return default;
            }

            T dynamicData = MemoryMarshal.Read<T>(value);
            return new(dynamicData);
        }

        public T GetCapability<T>() where T : class
        {
            if (typeof(T) == typeof(IEncounterContext))
            {
                return (T)_encounterContext;
            }

            return null;
        }

        public EventHandlerId SubscribeToEvent<T>(IEventContext.EventHandler<T> handler) where T : unmanaged, IEventData
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(ObjectiveContext));
            }

            if (State != ObjectiveState.Running)
            {
                throw new System.InvalidOperationException();
            }

            var id = _eventContext.Subscribe(handler);
            _subscriptions.Add(id);
            return id;
        }

        public void Unsubscribe(EventHandlerId id)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(ObjectiveContext));
            }

            if (_eventContext.Unsubscribe(id))
            {
                _subscriptions.Remove(id);
            }
        }

        public void Dispose()
        {
            if (_disposed) return;

            foreach (EventHandlerId item in _subscriptions)
            {
                _eventContext.Unsubscribe(item);
            }

            _subscriptions.Clear();
            _disposed = true;
        }
    }
}
