using System;
using System.IO;

namespace Core.Combat.Auras.AuraEffects
{
    public class ProcTriggerSpell : AuraEffect
    {
        public ProcTriggerSpell(BinaryReader source)
        {

        }

        public void ApplyEffect(Status status)
        {
            throw new NotImplementedException();
        }

        public void RemoveEffect(Status status)
        {
            throw new NotImplementedException();
        }

        public void Serialize(BinaryWriter buffer)
        {
            throw new NotImplementedException();
        }
    }
}
