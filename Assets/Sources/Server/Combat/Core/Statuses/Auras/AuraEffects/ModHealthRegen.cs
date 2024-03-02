using Core.Combat.Statuses.Auras.AuraEffects;
using Core.Combat.Units;
using System;
using System.IO;

namespace Core.Combat.Statuses.AuraEffects
{
    public class ModHealthRegen : AuraEffect
    {
        public ModHealthRegen(BinaryReader source)
        {

        }

        public void ApplyEffect(Unit target)
        {
            throw new NotImplementedException();
        }

        public void RemoveEffect(Unit target)
        {
            throw new NotImplementedException();
        }

        public void Serialize(BinaryWriter buffer)
        {
            throw new NotImplementedException();
        }
    }
}
