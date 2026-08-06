using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Gateways.Models;

using System.Collections.Generic;

namespace Combat.Local.Gateways.Repositories
{
    public class StatusRepository : IStatusRepository
    {
        private readonly Dictionary<StatusId, StatusData> _values;

        public StatusRepository()
        {
            _values = new();
        }

        public void Create(Status status)
        {
            _values[status.Id] = new(status);
        }

        public bool TryGet(StatusId id, out Status effect)
        {
            if (_values.TryGetValue(id, out StatusData data) == false)
            {
                effect = default;
                return false;
            }

            effect = data.ToStatus(id);
            return true;
        }

        public void Update(Status effect)
        {
            _values[effect.Id] = new(effect);
        }

        public void Delete(StatusId id)
        {
            _values.Remove(id);
        }
    }
}
