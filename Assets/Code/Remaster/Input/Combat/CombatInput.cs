using Remaster.Networking;

namespace Remaster.Input.Combat
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
