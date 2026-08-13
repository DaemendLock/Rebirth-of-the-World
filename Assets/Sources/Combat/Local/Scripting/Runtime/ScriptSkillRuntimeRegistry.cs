using Combat.Common.ValueObjects;
using Combat.Local.Scripting.IDK;

using System.Collections.Generic;

namespace Combat.Local.Scripting.Runtime
{

    public sealed class ScriptSkillRuntimeRegistry : ISkillRuntimeRegistry
    {
        private readonly Dictionary<AbilityKey, IAbilityPropertyContainer> _values = new();

        public void Create(AbilityKey id, IAbilityPropertyContainer value) => _values.Add(id, value);

        public void Remove(AbilityKey id) => _values.Remove(id);

        public bool TryGet(AbilityKey id, out IAbilityPropertyContainer container)
        {
            if (_values.TryGetValue(id, out var result) == false)
            {
                container = null;
                return false;
            }

            container = result;
            return true;
        }
    }
}
