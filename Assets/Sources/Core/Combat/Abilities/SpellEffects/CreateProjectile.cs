using Core.Combat.Units.Components;
using Core.Combat.Utils;
using System.IO;
using Utils.DataTypes;

namespace Core.Combat.Abilities.SpellEffects
{
    public class CreateProjectile : SpellEffect
    {
        private readonly SpellId _spell;
        private readonly PositionComponent _position;

        private readonly float _speed;

        public void ApplyEffect(CastEventData data, float modifyValue)
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