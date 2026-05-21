using Combat.Common.ValueObjects;
using Combat.Local.Data.Models;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Repositories;

using System.Collections.Generic;

namespace Combat.Local.Gateways.Repositories.Characters
{
    public sealed class AligmentRepository : IAligmentRepository
    {
        private readonly Dictionary<UnitId, AligmentData> _values = new();

        public void Create(Aligment value) => _values.Add(value.Id, new(value));

        public void Delete(UnitId id) => _values.Remove(id);

        public Aligment Get(UnitId id) => new(id, _values[id].Team);

        public void Update(Aligment value) => _values[value.Id] = new(value);
    }
}
