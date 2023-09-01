using Core.Combat.Abilities;
using Core.Combat.Abilities.SpellEffects;
using Core.Combat.Abilities.SpellScripts;
using Core.Combat.Auras.AuraEffects;
using Core.Combat.Utils.ValueSources;
using Utils.SpellIdGenerator;

namespace Core.SpellLib.Shielder
{
    //TODO: Cleave
    public class ShieldStrike : CleaveSpell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.SHIELDER, Spec.SPEC_1, 1);

        private static SpellData _spellData = new SpellData(_id, "",
            new AbilityCost(0, 90),
            TargetTeam.ENEMY,
            3,
            0,
            10,
            1.5f,
            GcdCategory.NORMAL,
            5,
            DispellType.NONE,
            SchoolType.PHYSICAL,
            Mechanic.NONE,
            new SpellEffect[]
            {
                new Dummy(30),
                new SchoolDamage(new AttackpowerValue(1.2f)),
                new ApplyAura(new ModStat(Stats.UnitStat.SPEED, new Constant(-0.1f), true)),
                //periodic damage
            },
            SpellFlags.HASTE_AFFECTS_COOLDOWN,
            typeof(CleaveSpell)
            );

        public ShieldStrike() : base(_spellData)
        {
        }
    }

    //TODO: charge
    public class Breakthrough : SelfcastSpell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.SHIELDER, Spec.SPEC_1, 2);

        private static SpellData _spellData = new SpellData(_id, "",
            new AbilityCost(0, 90),
            TargetTeam.ALLY,
            0,
            0,
            0,
            1.5f,
            GcdCategory.NORMAL,
            5,
            DispellType.NONE,
            SchoolType.PHYSICAL,
            Mechanic.NONE,
            new SpellEffect[]
            {
                //charge
                new ReduceCooldown(SpellIdCalculator.GenerateId(Class.SHIELDER, Spec.SPEC_1, 1), float.PositiveInfinity),
                //give overshield
                new ApplyAura(new ModifySpellEffect(SpellIdCalculator.GenerateId(Class.SHIELDER, Spec.SPEC_1, 1), 1, 1))
            },
            SpellFlags.HASTE_AFFECTS_COOLDOWN,
            typeof(SelfcastSpell)
            );

        public Breakthrough() : base(_spellData)
        {
        }
    }

    //TODO: AoE spell
    public class Shelter : SplashSpell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.SHIELDER, Spec.SPEC_1, 3);

        private static SpellData _spellData = new SpellData(_id, "",
            new AbilityCost(0, 90),
            TargetTeam.ALLY,
            10,
            0,
            20,
            1.5f,
            GcdCategory.NORMAL,
            5,
            DispellType.NONE,
            SchoolType.PHYSICAL,
            Mechanic.NONE,
            new SpellEffect[]
            {
                new ApplyAura(new ModStat(Stats.UnitStat.DAMAGE_TAKEN, new Constant(-0.2f), true)),
            },
            SpellFlags.NONE,
            typeof(SplashSpell)
            );

        public Shelter() : base(_spellData)
        {
        }
    }

    public class SacrificialProtection : Spell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.SHIELDER, Spec.SPEC_1, 4);

        private static SpellData _spellData = new SpellData(_id, "",
            new AbilityCost(0, 90),
            TargetTeam.ALLY,
            10,
            0,
            30,
            1.5f,
            GcdCategory.NORMAL,
            5,
            DispellType.NONE,
            SchoolType.PHYSICAL,
            Mechanic.NONE,
            new SpellEffect[]
            {
                //TODO: Apply overshield
                new ApplyAura(new ModStat(Stats.UnitStat.ATK, new Constant(0.1f), true)),
                new ApplyAura(new ModStat(Stats.UnitStat.HEALING_TAKEN, new Constant(0.1f), true)),
            },
            SpellFlags.HASTE_AFFECTS_COOLDOWN,
            typeof(Spell)
            );

        public SacrificialProtection() : base(_spellData)
        {
        }
    }

    public class AnchoringHowl : SplashSpell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.SHIELDER, Spec.SPEC_1, 5);

        private static SpellData _spellData = new SpellData(_id, "",
            new AbilityCost(0, 90),
            TargetTeam.ENEMY,
            5,
            0,
            15,
            1.5f,
            GcdCategory.NORMAL,
            5,
            DispellType.MAGIC,
            SchoolType.PHYSICAL,
            Mechanic.NONE,
            new SpellEffect[]
            {
                //TODO: Apply overshield
                
            },
            SpellFlags.NONE,
            typeof(SplashSpell)
            );

        public AnchoringHowl() : base(_spellData)
        {
        }
    }

    public class SkinOfStone : SelfcastSpell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.SHIELDER, Spec.SPEC_1, 6);

        private static SpellData _spellData = new SpellData(_id, "",
            new AbilityCost(0, 90),
            TargetTeam.ALLY,
            0,
            0,
            180,
            1.5f,
            GcdCategory.NORMAL,
            15,
            DispellType.MAGIC,
            SchoolType.PHYSICAL,
            Mechanic.NONE,
            new SpellEffect[]
            {
                //TODO: disable controll
                new ApplyAura(new ModStat(Stats.UnitStat.DAMAGE_TAKEN, new Constant(-0.9f), true))
                //count damage to 200% max hp, then trugger spell
                //create aoe damage aura
            },
            SpellFlags.NONE,
            typeof(SelfcastSpell)
            );

        public SkinOfStone() : base(_spellData)
        {
        }
    }
}
