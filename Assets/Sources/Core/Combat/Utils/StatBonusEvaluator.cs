using Core.Combat.Abilities;
using Core.Combat.Interfaces;
using System;
using System.Runtime.CompilerServices;
using Utils.DataStructure;

namespace Core.Combat.Utils
{
    public static class StatBonusEvaluator
    {
        private const float HasteEffiency = 0.075f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EvaluateHasteTimeDivider(float hasteValue) => (float) Math.Clamp(1 + hasteValue * HasteEffiency * 0.01f, 0.5, 2);

        public static float EvaluateCastGcd(Spell spell, StatsOwner statsOwner)
        {
            if (statsOwner == null)
            {
                return spell.GCD;
            }

            return spell.GCD * statsOwner.EvaluateStatValue(UnitStat.HASTE);
        }

        public static float EvaluateAbilityCooldown(float baseCooldown, StatsOwner statsProvider, bool useHaste)
        {
            float cooldown = baseCooldown;

            if (statsProvider != null && useHaste)
            {
                cooldown /= EvaluateHasteTimeDivider(statsProvider.EvaluateStatValue(UnitStat.HASTE));
            }

            return cooldown;
        }
    }
}
