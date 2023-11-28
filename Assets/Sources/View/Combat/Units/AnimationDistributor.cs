using Core.Combat.Abilities;

namespace View.Combat.Units
{
    public static class AnimationDistributor
    {
        public static void PlayAnimation(int unitId, int animationId)
        {
            object unit = EntityPool.GetUnitById(unitId);
        }

        public static void CastSpell(int unitId, SpellSlot slot)
        {

        }
    }
}