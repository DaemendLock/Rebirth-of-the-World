using System.Collections.Generic;

using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

namespace Temp.Repositories.Implementations
{
    public class StatusRepository : IStatusRepository
    {
        private readonly Dictionary<StatusId, Status> _values;

        public StatusRepository()
        {
            _values = new Dictionary<StatusId, Status>();
        }

        public void Create(Status status) => _values.Add(status.Id, status);

        public bool TryGet(StatusId id, out Status effect) => _values.TryGetValue(id, out effect);

        public void Update(Status effect) => _values[effect.Id] = effect;

        public void Delete(StatusId id) => _values.Remove(id);

        public ICollection<Status> GetAll() => _values.Values;
    }
}
