using Combat.API.API.Skills;
using Combat.API.Skills;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Repositories.Skill;

using System;
using System.Runtime.InteropServices;

namespace Combat.Local.Scripting
{
    public class CoreUnitSkillContext : ISkillContext // ?? Model
    {
        private readonly IEnvironmentContext _environmentContext;

        private readonly ISkillMemoryRepository _skillMemoryRepository;
        private readonly AbilityKey _key;

        public CoreUnitSkillContext(ISkillMemoryRepository skillMemoryRepository, AbilityKey key)
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

        public void SubscribeToEvent<TEventData>(Action<GameEvent<TEventData>> callback) where TEventData : unmanaged, IEventData
        {
            throw new NotImplementedException();
        }
    }
}
