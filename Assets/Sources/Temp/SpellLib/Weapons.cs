using Core.Combat.Abilities;
using Core.Combat.Abilities.SpellEffects;
using Utils.DataStructure;
using Utils.DataTypes;
using Utils.SpellIdGenerator;

namespace SpellLib.Weapons
{
    public class DefaultSwordAttack : Spell
    {
        private static readonly int _id = SpellIdCalculator.GenerateGearSpellId(new(0));

        private static SpellData _spellData = new SpellData(_id,
            new AbilityCost(0, 0),
            TargetTeam.ENEMY,
            2,
            0,
            1,
            0,
            GcdCategory.IGNOR,
            0,
            DispellType.NONE,
            SchoolType.PHYSICAL,
            Mechanic.ATTACK,
            new SpellEffect[]
            {
                new SchoolDamage(new StatValue(1, UnitStat.ATK))
            },
            SpellFlags.HASTE_AFFECTS_COOLDOWN | SpellFlags.AUTOATTACK,
            typeof(Spell)
            );

        public DefaultSwordAttack() : base(_spellData)
        {
        }
    }
}