using Combat.API.Contexts;
using Combat.API.Skills;
using Combat.Common.Primitives;
using Combat.Domain.Repositories;

using System;
using System.Runtime.InteropServices;

namespace Combat.Local.Scripting.Contexts
{
    public readonly struct DomainSkillContext : ISkillContext
    {
        private readonly AbilityKey _key;
        private readonly IEncounterContext _environmentContext;
        private readonly ISkillDynamicMemoryRepository _memoryRepository;

        public DomainSkillContext(AbilityKey key, ISkillDynamicMemoryRepository skillMemoryRepository, IEncounterContext environmentContext)
        {
            _key = key;
            _memoryRepository = skillMemoryRepository;
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

        public void SaveState<T>(SkillState<T> value) where T : unmanaged, IDynamicSkillData => _memoryRepository.Save(_key, value);

        public TQuery GetCapability<TQuery>() where TQuery : class
        {
            if (typeof(TQuery) == typeof(IEnvironmentContext))
            {
                return _environmentContext as TQuery;
            }

            return null;
        }
    }
}
