using Combat.Common.Primitives;
using Combat.Domain.Entities.Units;
using Combat.Local.Application.ValueObjects;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Combat.Domain.Entities
{
    public readonly struct ScaleEffectOwner
    {
        private readonly List<ScaleOverTimeEffect> _values;

        public ScaleEffectOwner(UnitId id)
        {
            Id = id;
            _values = new();
        }

        public UnitId Id { get; }

        public IReadOnlyCollection<ScaleOverTimeEffect> Values => _values;

        public void Add(ScaleOverTimeEffect effect)
        {
            if (_values.Any(value => value.Id == effect.Id))
            {
                throw new InvalidOperationException();
            }

            _values.Add(effect);
        }

        public void Remove(ScaleEffectId id) => _values.RemoveAll(value => value.Id == id);
    }
}

namespace Combat.Local.Application.Entities
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
