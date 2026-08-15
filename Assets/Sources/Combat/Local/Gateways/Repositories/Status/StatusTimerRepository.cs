using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

using System.Collections.Generic;

namespace Combat.Local.Gateways.Repositories
{
    public class StatusTimerRepository : IStatusTimerRepository
    {
        private readonly Dictionary<StatusId, StatusTimer> _values;

        public StatusTimerRepository()
        {
            _values = new();
        }

        public bool Contain(StatusId statusId) => _values.ContainsKey(statusId);

        public void Create(StatusTimer value) => _values[value.StatusId] = value;

        public void Delete(StatusId statusId) => _values.Remove(statusId);

        public bool TryGet(StatusId id, out StatusTimer value) => _values.TryGetValue(id, out value);

        public ICollection<StatusTimer> GetAll() => _values.Values;

        public void Update(StatusTimer value) => _values[value.StatusId] = value;
    }
}
