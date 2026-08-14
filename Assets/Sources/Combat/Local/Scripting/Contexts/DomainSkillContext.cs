using Combat.API.Contexts;
using Combat.API.Skills;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Repositories.Skill;

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Combat.Local.Scripting.Contexts
{
    public class DomainSkillContext : ISkillContext // ?? Model
    {
        private readonly IEnvironmentContext _environmentContext;
        private readonly ISkillMemoryRepository _skillMemoryRepository;
        private readonly AbilityKey _key;

        private readonly List<EventHandlerId> _eventHandlers;
        private readonly IEventContext _eventContext;

        public DomainSkillContext(ISkillMemoryRepository skillMemoryRepository, AbilityKey key)
        {
            _skillMemoryRepository = skillMemoryRepository;
            _key = key;
        }

        public SkillState<T> GetState<T>() where T : unmanaged, IDynamicSkillData
        {
            if (_skillMemoryRepository.TryGetRawData(_key, out Span<byte> value) == false)
            {
                return default;
            }

            T dynamicData = MemoryMarshal.Read<T>(value);
            return new(dynamicData);
        }

        public void SaveState<T>(SkillState<T> value) where T : unmanaged, IDynamicSkillData
        {
            _skillMemoryRepository.Save(_key, value);
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
