using Core.Combat.Abilities.SpellEffects;
using Core.Combat.Utils.Serialization;
using System;
using Utils.DataTypes;

namespace Core.Combat.Abilities
{
    [Serializable]
    public readonly struct SpellData
    {
        public readonly SpellId Id;

        public readonly AbilityCost Cost;
        public readonly TargetTeam TargetTeam;
        public readonly float Range;
        public readonly float CastTime;
        public readonly float Cooldown;
        public readonly float GCD;
        public readonly GcdCategory GcdCategory;
        public readonly SchoolType School;
        public readonly Mechanic Mechanic;
        public readonly SpellFlags Flags;
        public readonly SpellEffect[] Effects;
        public readonly SpellType Script;

        public SpellData(int id, AbilityCost cost, TargetTeam targetTeam, float range, float castTime, float cooldown, float gcd, GcdCategory gcdCategory, SchoolType school, Mechanic mechanic, SpellEffect[] effects, SpellFlags flags, SpellType script)
        {
            Id = (SpellId) id;
            Cost = cost;
            TargetTeam = targetTeam;
            Range = range;
            CastTime = castTime;
            Cooldown = cooldown;
            GCD = gcd;
            GcdCategory = gcdCategory;
            School = school;
            Mechanic = mechanic;
            Effects = effects;
            Flags = flags;
            Script = script;
        }
    }
}
