using Combat.API.Contexts;
using Combat.API.Events;
using Combat.API.Skills;
using Combat.Common.Primitives;
using Combat.Local.Domain.Repositories.Skill;

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Combat.Local.Scripting.Contexts
{

    public sealed class DomainSkillContext : ISkillContext
    {
        private readonly AbilityKey _key;
        private readonly IEncounterContext _environmentContext;
        private readonly IEventContext _eventContext;
        private readonly ISkillDynamicMemoryRepository _memoryRepository;

        private readonly List<EventHandlerId> _eventHandlers;

        public DomainSkillContext(AbilityKey key, ISkillDynamicMemoryRepository skillMemoryRepository, IEventContext eventContext, IEncounterContext environmentContext)
        {
            _key = key;

            _memoryRepository = skillMemoryRepository;
            _eventContext = eventContext;

            _eventHandlers = new();
            _environmentContext = environmentContext;
        }

        public AbilityKey Key => _key;

        public void StartCooldown(float value) { }

        public SkillState<T> GetState<T>() where T : unmanaged, IDynamicSkillData
        {
            if (_memoryRepository.TryGetRawData(_key, out ReadOnlySpan<byte> value) == false)
            {
                return default;
            }

            T dynamicData = MemoryMarshal.Read<T>(value);
            return new(dynamicData);
        }

        public void SaveState<T>(SkillState<T> value) where T : unmanaged, IDynamicSkillData
        {
            _memoryRepository.Save(_key, value);
        }

        public TQuery GetCapability<TQuery>() where TQuery : class
        {
            if (typeof(TQuery) == typeof(IEnvironmentContext))
            {
                return _environmentContext as TQuery;
            }

            return null;
        }

        public EventHandlerId SubscribeToEvent<TEventData>(IEventContext.EventHandler<TEventData> callback) where TEventData : IEventData
        {
            var id = _eventContext.Subscribe(callback);
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

        public void Cleanup()
        {
            foreach (var id in _eventHandlers)
            {
                _eventContext.Unsubscribe(id);
            }

            _eventHandlers.Clear();
        }
    }
}
