using Core.Combat.Abilities;
using Core.Combat.Abilities.SpellEffects;
using Core.Combat.Abilities.SpellScripts;
using Core.Combat.Utils.Serialization;

using Utils.DataStructure;
using Utils.DataTypes;
using Utils.SpellIdGenerator;

namespace SpellLib.Weapons
{
    public class DefaultSwordAttack : TargetSpell
    {
        private static readonly int _id = SpellIdCalculator.GenerateGearSpellId(new(0));

        private static readonly SpellData _spellData = new(_id,
            new AbilityCost(0, 0),
            TargetTeam.Enemy,
            2,
            0,
            1,
            0,
            GcdCategory.Ignor,
            SchoolType.Physical,
            Mechanic.Attack,
            new SpellEffect[]
            {
                new SchoolDamage(new StatValue(1, UnitStat.ATK))
            },
            SpellFlags.HASTE_AFFECTS_COOLDOWN | SpellFlags.AUTOATTACK,
            SpellType.Target
            );

        public DefaultSwordAttack() : base(_spellData)
        {
        }
    }
}