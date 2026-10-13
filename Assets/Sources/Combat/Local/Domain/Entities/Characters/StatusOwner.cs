using System.Collections.Generic;

using Combat.Common.Primitives;

namespace Combat.Local.Domain.Entities
{
    public readonly struct StatusOwner
    {
        private readonly List<StatusId> _values;

        public StatusOwner(UnitId id)
        {
            Id = id;
            _values = new();
        }

        public UnitId Id { get; }

        public void AddStatus(StatusId statusId) => _values.Add(statusId);

        public bool RemoveStatus(StatusId statusId) => _values.Remove(statusId);

        public bool HasStatus(StatusId id)
        {
            foreach (StatusId status in _values)
            {
                if (status != id)
                    continue;

                return true;
            }

            return false;
        }

        public IReadOnlyCollection<StatusId> GetAll() => _values;
    }
}
