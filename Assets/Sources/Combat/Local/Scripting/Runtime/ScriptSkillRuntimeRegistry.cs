using Combat.Common.Primitives;

using System.Collections.Generic;

namespace Combat.Local.Scripting.Runtime
{
    public sealed class ScriptSkillRuntimeRegistry : ISkillRuntimeRegistry
    {
        private readonly Dictionary<AbilityKey, SkillRuntime> _values = new();

        public void Create(AbilityKey id, SkillRuntime value) => _values.Add(id, value);

        public void Remove(AbilityKey id) => _values.Remove(id);

        public bool TryGet(AbilityKey id, out SkillRuntime container) => _values.TryGetValue(id, out container);
    }

    public sealed class UnitRuntimeRegistry : IUnitRuntimeRegistry
    {
        private readonly Dictionary<UnitId, UnitNew> _values = new();

        public void Create(UnitNew unitNew) => _values.Add(unitNew.Id, unitNew);

        public void Remove(UnitId id) => _values.Remove(id);

        public bool TryGet(UnitId id, out UnitNew unitNew) => _values.TryGetValue(id, out unitNew);
    }
}
