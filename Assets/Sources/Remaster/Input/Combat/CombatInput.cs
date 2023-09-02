using Core.Combat.Abilities;
using Core.Combat.Units;
using Networking;

namespace Core.Input.Combat
{
    public class CombatInput
    {
        public Unit SelectedUnit;
        public Unit Target;

        public void Cast(SpellSlot slot)
        {
            Server.SendCastRequest(SelectedUnit, Target, slot);
        }
    }
}
