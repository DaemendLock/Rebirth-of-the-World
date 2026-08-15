using Combat.Common.Primitives;
using Combat.Local.Scripting.IDK;

using System.Collections.Generic;

namespace Combat.Local.Scripting.Runtime
{
    public sealed class ScriptStatusRuntimeRegistry : IStatusRuntimeRegistry
    {
        private readonly Dictionary<StatusId, StatusRuntime> _values = new();

        public void Create(StatusId id, StatusRuntime value) => _values.Add(id, value);

        public void Remove(StatusId id) => _values.Remove(id);

        public bool TryGet(StatusId id, out StatusRuntime runtime)
        {
            if (_values.TryGetValue(id, out var result) == false)
            {
                runtime = default;
                return false;
            }

            runtime = result;
            return true;
        }
    }
}
