using Core.Combat.Utils;
using System.IO;

namespace Core.Combat.Abilities.SpellEffects
{
    public class Taunt : SpellEffect
    {
        public Taunt()
        {

        }

        public Taunt(BinaryReader source)
        {
        }

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