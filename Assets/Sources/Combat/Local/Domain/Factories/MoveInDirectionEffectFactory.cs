using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Units;

using UnityEngine;

namespace Combat.Local.Domain.Factories
{
    public class MoveInDirectionEffectFactory
    {
        private int _nextId = 0;

        public MoveInDirectionEffect Create(EntityId target, Vector3 direction, float speed, bool isRelative, float maxDuration)
        {
            return new MoveInDirectionEffect(new(_nextId++), target, direction.normalized * speed, isRelative, maxDuration);
        }
    }
}
