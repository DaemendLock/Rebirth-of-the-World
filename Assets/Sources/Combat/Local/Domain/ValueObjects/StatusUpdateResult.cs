using Combat.Common.Primitives;

namespace Combat.Local.Domain.ValueObjects
{
    public readonly struct StatusUpdateResult
    {
        internal StatusUpdateResult(StatusId id, bool expired, bool remove)
        {
            Id = id;
            Expired = expired;
            Remove = remove;
        }

        public StatusId Id { get; }
        public bool Expired { get; }
        public bool Remove { get; }
    }
}
