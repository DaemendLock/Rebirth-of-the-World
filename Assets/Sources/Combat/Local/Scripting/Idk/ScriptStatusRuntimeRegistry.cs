using Combat.API.API.IDK;
using Combat.Common.ValueObjects;
using Combat.Local.Scripting.IDK;

using System.Collections.Generic;

namespace Combat.Local.Scripting.Idk
{

    public sealed class ScriptStatusRuntimeRegistry : IStatusRuntimeRegistry
    {
        private readonly Dictionary<StatusId, IStatusPropertyContainer> _values = new();

        public void Create(StatusId id, IStatusPropertyContainer value) => _values.Add(id, value);

        public void Remove(StatusId id) => _values.Remove(id);

        public bool TryGet(StatusId id, out IStatusPropertyContainer container)
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
