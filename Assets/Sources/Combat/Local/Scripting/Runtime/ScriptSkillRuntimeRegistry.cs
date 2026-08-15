using Combat.Common.Primitives;

using System.Collections.Generic;

namespace Combat.Local.Scripting.Runtime
{
    public sealed class ScriptSkillRuntimeRegistry : ISkillRuntimeRegistry
    {
        private readonly Dictionary<AbilityKey, SkillRuntime> _values = new();

        public void Create(AbilityKey id, SkillRuntime value) => _values.Add(id, value);

        public void Remove(AbilityKey id) => _values.Remove(id);

        public bool TryGet(AbilityKey id, out SkillRuntime container)
        {
            if (_values.TryGetValue(id, out var result) == false)
            {
                container = default;
                return false;
            }

            container = result;
            return true;
        }
    }
}
