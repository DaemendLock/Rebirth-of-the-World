using Combat.Common.ValueObjects;
using Combat.Local.Domain.ValueObjects;

using System.Collections.Generic;

namespace Combat.Local.Domain.Entities
{
    public readonly ref struct MovementEffectOwner
    {
        public MovementEffectOwner(UnitId id, ICollection<MoveInDirectionEffect> effects)
        {
            Id = id;
            Effects = effects;
        }

        public UnitId Id { get; }

        public ICollection<MoveInDirectionEffect> Effects { get; }
    }
}
