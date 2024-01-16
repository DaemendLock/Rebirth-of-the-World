using Core.Combat.Abilities;
using System;
using System.IO;

namespace Core.Combat.Statuses.AuraEffects
{
    public class Immunity : AuraEffect
    {
        private Mechanic _mechanic;

        public Immunity(BinaryReader source)
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
