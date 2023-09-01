using Core.Combat.Units.Components;
using Core.Combat.Utils;
using System.IO;
using UnityEngine;

namespace Core.Combat.Abilities.SpellEffects
{
    public class CreateProjectile : SpellEffect
    {
        private Spell _spell;
        private PositionComponent _position;
        private Vector3 _direction;
        private float _speed;

        public void ApplyEffect(EventData data, float modifyValue)
        {
            throw new System.NotImplementedException();
        }

        public float GetValue(float modifyValue)
        {
            throw new System.NotImplementedException();
        }
        public void Serialize(BinaryWriter buffer)
        {
            throw new System.NotImplementedException();
        }
    }
}