using Combat.Common.ValueObjects;

using System.Collections.Generic;

namespace Combat.Local.Scripting.Runtime
{
    public sealed class ObjectiveRuntimeRegistry
    {
        private readonly Dictionary<ObjectiveId, RuntimeObjectiveContainer> _values = new();

        public void Create(ObjectiveId id, RuntimeObjectiveContainer value) => _values.Add(id, value);

        public void Remove(ObjectiveId id) => _values.Remove(id);

        public bool TryGet(ObjectiveId id, out RuntimeObjectiveContainer objective) => _values.TryGetValue(id, out objective);
    }
}
