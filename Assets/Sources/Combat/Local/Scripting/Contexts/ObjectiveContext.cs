using Combat.API.Contexts;
using Combat.API.Objectives;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Domain.Repositories.Objectives;
using Combat.Local.Scripting.Ports;

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
        private readonly IObjectiveCompletionHandler _completionHandler;

        private readonly List<EventHandlerId> _subscriptions;

        private bool _disposed;

        public ObjectiveContext(ObjectiveId id, IObjectiveMemoryRepository memoryRepository, IEventContext eventContext, IObjectiveCompletionHandler completionHandler)
        {
            _id = id;
            _disposed = false;
            _memoryRepository = memoryRepository;
            _eventContext = eventContext;
            _completionHandler = completionHandler;

            _subscriptions = new List<EventHandlerId>();
        }

        public ObjectiveId Id => _id;

        public ObjectiveState State { get; private set; }

        public void Complete()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(ObjectiveContext));
            }

            if (State != ObjectiveState.Running) return;

            State = ObjectiveState.Completed;
            _completionHandler.Complete(_id);
        }

        public void Cancel()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(ObjectiveContext));
            }

            if (State != ObjectiveState.Running) return;

            State = ObjectiveState.Cancelled;
            _completionHandler.Cancel(_id);
        }

        public void Fail()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(ObjectiveContext));
            }

            if (State != ObjectiveState.Running) return;

            State = ObjectiveState.Failed;
            _completionHandler.Fail(_id);
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
            return new("todo", dynamicData);
        }

        public void Dispose()
        {
            if (_disposed) return;

            foreach (EventHandlerId item in _subscriptions)
            {
                _eventContext.Unsubscribe(item);
            }

            _subscriptions.Clear();
            _memoryRepository.Delete(_id);
            _disposed = true;
        }

        public EventHandlerId SubscribeToEvent<T>(IEventContext.EventHandler<T> handler) where T : unmanaged, IEventData
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(ObjectiveContext));
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
    }
}
