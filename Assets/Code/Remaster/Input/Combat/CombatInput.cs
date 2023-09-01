using Core.Networking;
using Core.Combat.Units;
using Core.Combat.Abilities;

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
