using Combat.Common.Primitives;
using Combat.Local.Domain.ValueObjects;

using System;

namespace Combat.Local.Domain.Entities
{
    public readonly struct StatusOwner : IUnitComponent
    {
        private readonly StatusInstance[] _statuses;

        public StatusOwner(UnitId id, StatusInstance[] statuses, bool needCleanup = false)
        {
            Id = id;
            _statuses = statuses;
            NeedCleanup = needCleanup;
        }

        public UnitId Id { get; }
        public bool NeedCleanup { get; }

        public Span<StatusInstance> GetAll() => _statuses;

        public bool HasStatus(StatusId id)
        {
            foreach (var status in _statuses)
            {
                if (status.StatusId != id)
                    continue;

                return true;
            }

            return false;
        }

        public StatusOwner MarkDirty() => new(Id, _statuses, true);

        public StatusOwner Cleared() => new(Id, _statuses, false);
    }
}
