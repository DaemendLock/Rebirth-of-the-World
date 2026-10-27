using Combat.Common.Primitives;
using Combat.Local.Application.Repositories;

using System.Collections.Generic;

namespace Combat.Local.Data.Repositories
{
    public sealed class CharacterDeleteQueue : ICharacterDeleteQueue
    {
        private readonly Queue<UnitId> _values = new();

        public void Enqueue(UnitId unitId)
        {
            if (_values.Contains(unitId)) return;
            _values.Enqueue(unitId);
        }

        public bool TryDequeue(out UnitId unitId) => _values.TryDequeue(out unitId);
    }

    public class UpdateTargetRepository : ICharacterUpdateQueue
    {
        private readonly HashSet<UnitId> _values;

        public UpdateTargetRepository()
        {
            _values = new();
        }

        public void Add(UnitId value) => _values.Add(value);

        public IReadOnlyCollection<UnitId> GetAll() => _values;

        public void Delete(UnitId id) => _values.Remove(id);
    }
}
