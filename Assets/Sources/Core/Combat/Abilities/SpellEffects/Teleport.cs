using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Units;
using System.IO;
using Utils.DataStructure;

namespace Core.Combat.Abilities.SpellEffects
{
    public class Teleport : SpellEffect
    {
        public Teleport(ByteReader source)
        {
        }

        public ActionRecord ApplyEffect(Unit caster, Unit target, float modification)
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