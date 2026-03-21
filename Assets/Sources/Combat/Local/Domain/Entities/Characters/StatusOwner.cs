using Combat.Common.ValueObjects;

using System;

namespace Combat.Local.Domain.Entities
{
    public readonly ref struct StatusOwner
    {
        private readonly ReadOnlySpan<StatusId> _statuses;

        public StatusOwner(EntityId id, ReadOnlySpan<StatusId> statuses)
        {
            Id = id;
            _statuses = statuses;
        }

        public EntityId Id { get; }

        public ReadOnlySpan<StatusId> GetAll() => _statuses;
    }
}
