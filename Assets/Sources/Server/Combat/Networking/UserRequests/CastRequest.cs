using Core.Combat.Abilities;
using Core.Combat.Engine;
using Core.Combat.Units;
using System;

namespace Server.Combat.UserRequests
{
    internal class CastRequest : InputRequest
    {
        private readonly Unit _caster;
        private readonly Unit _target;
        private readonly byte _spellSlot;

        public CastRequest(int casterId, int targetId, byte spellSlot)
        {
            _caster = Units.GetUnitById(casterId);
            _target = Units.GetUnitById(targetId);
            _spellSlot = spellSlot;
        } 

        public bool IsValid()
        {
            //TODO: check valid cast
            return true;
        }

        public void Perform(UpdateRecord target)
        {
            if (_caster == null)
            {
                throw new InvalidOperationException();
            }

            SpellSlot slot = (SpellSlot) _spellSlot;

            target.AddAction(Units.Cast(new(_caster, _target, slot)));
        }
    }
}
