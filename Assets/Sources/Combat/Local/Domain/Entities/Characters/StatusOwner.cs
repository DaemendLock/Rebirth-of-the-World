using Combat.Common.ValueObjects;

using System;

namespace Combat.Local.Domain.Entities
{
    public readonly ref struct StatusOwner
    {
        private readonly ReadOnlySpan<StatusId> _statuses;

        public StatusOwner(UnitId id, ReadOnlySpan<StatusId> statuses)
        {
            Id = id;
            _statuses = statuses;
        }

        public UnitId Id { get; }

        public ReadOnlySpan<StatusId> GetAll() => _statuses;

        public bool HasStatus(StatusId id)
        {
            foreach (StatusId status in _statuses)
            {
                if (status != id)
                    continue;

                return true;
            }

            return false;
        }
    }
}
