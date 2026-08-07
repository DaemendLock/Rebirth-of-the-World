using Combat.API.Contexts;
using Combat.API.Events;
using Combat.API.Objectives;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Repositories.Objectives;

using System;
using System.Runtime.InteropServices;

namespace Combat.Local.Scripting.Contexts
{
    public sealed class ObjectiveContext : IObjectiveContext
    {
        private readonly ObjectiveId _id;
        private readonly IObjectiveMemoryRepository _memoryRepository;
        private readonly IEventContext _eventContext;

        public ObjectiveContext(ObjectiveId id, IObjectiveMemoryRepository memoryRepository, IEventContext eventContext)
        {
            _id = id;
            _memoryRepository = memoryRepository;
            _eventContext = eventContext;
        }

        public ObjectiveId Id => _id;

        public bool IsCompleted { get; private set; } = false;

        public void Complete()
        {
            if (IsCompleted) return;

            IsCompleted = true;
            _eventContext.Publish(new GameEvent<ObjectiveCompletedEventData>(new(_id)));
        }

        public TQuery GetCapability<TQuery>() where TQuery : class
        {
            if (typeof(TQuery) == typeof(IEventContext))
            {
                return _eventContext as TQuery;
            }

            return null;
        }

        public void Save<T>(T value) where T : unmanaged, IObjectiveData
        {
            _memoryRepository.Save(_id, value);
        }

        public ObjectiveInfo<T> GetInfo<T>() where T : unmanaged, IObjectiveData
        {
            if (_memoryRepository.TryGetRawData(_id, out ReadOnlySpan<byte> value) == false)
            {
                return default;
            }

            T dynamicData = MemoryMarshal.Read<T>(value);
            return new("todo", dynamicData);
        }

        public void Dispose() => throw new NotImplementedException();
    }
}
