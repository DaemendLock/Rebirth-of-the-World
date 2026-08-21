using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Gateways.DataSources;

using System;

namespace Combat.Local.Gateways.Repositories.Characters
{
    public class StatusOwnerRepository : IStatusOwnerRepository
    {
        private readonly ComponentPool<StatusOwner> _values;

        public StatusOwnerRepository()
        {
            _values = new();
        }

        public void Create(StatusOwner statusOwner) => _values.Add(statusOwner);

        public void Delete(UnitId id) => _values.Remove(id);

        public ref StatusOwner Get(UnitId id) => ref _values.Get(id);

        public Span<StatusOwner> GetAll() => _values.GetAll();
    }
}
