using Core.Combat.Abilities.SpellEffects;
using System;
using Utils.DataTypes;

namespace Core.Combat.Abilities
{
    [Serializable]
    public readonly struct SpellData
    {
        public readonly SpellId Id;
        public readonly string Icon;

        public readonly AbilityCost Cost;
        public readonly TargetTeam TargetTeam;
        public readonly float Range;
        public readonly float CastTime;
        public readonly float Cooldown;
        public readonly float GCD;
        public readonly GcdCategory GcdCategory;
        public readonly float Duration;
        public readonly SchoolType School;
        public readonly Mechanic Mechanic;
        public readonly DispellType DispellType;
        public readonly SpellFlags Flags;
        public readonly SpellEffect[] Effects;
        public readonly string Script;

        public SpellData(int id, string icon, AbilityCost cost, TargetTeam targetTeam, float range, float castTime, float cooldown, float gcd, GcdCategory gcdCategory, float duration, DispellType dispell, SchoolType school, Mechanic mechanic, SpellEffect[] effects, SpellFlags flags, Type script)
        {
            Id = (SpellId) id;
            Icon = icon;
            Cost = cost;
            TargetTeam = targetTeam;
            Range = range;
            CastTime = castTime;
            Cooldown = cooldown;
            GCD = gcd;
            GcdCategory = gcdCategory;
            Duration = duration;
            School = school;
            Mechanic = mechanic;
            DispellType = dispell;
            Effects = effects;
            Flags = flags;
            Script = script.ToString();
        }
    }
}
