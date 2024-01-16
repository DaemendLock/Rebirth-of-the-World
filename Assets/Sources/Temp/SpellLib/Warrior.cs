using Core.Combat.Abilities;
using Core.Combat.Abilities.SpellEffects;
using Core.Combat.Abilities.SpellScripts;
using Core.Combat.Statuses.AuraEffects;
using Core.Combat.Utils.Serialization;
using Core.Combat.Utils.ValueSources;
using Utils.DataStructure;
using Utils.DataTypes;
using Utils.SpellIdGenerator;

namespace Core.SpellLib.Warrior
{
    public class DirectHit : Spell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.WARRIOR, Spec.SPEC_1, 1);

        private static SpellData _spellData = new SpellData(_id,
            new AbilityCost(30, 0),
            TargetTeam.Enemy,
            2,
            0,
            0,
            0.5f,
            GcdCategory.Normal,
            SchoolType.Physical,
            Mechanic.None,
            new SpellEffect[]
            {
                new SchoolDamage(new StatValue(0.3f, UnitStat.ATK)),
                //new TriggerSpell((SpellId)2),
                new ApplyAura(new ModStat(UnitStat.SPEED, new Constant(0f), true))
            },
            SpellFlags.HASTE_AFFECTS_COOLDOWN,
            SpellType.Default
            );

        public DirectHit() : base(_spellData)
        {
        }
    }

    public class СoncentratedDefense : Spell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.WARRIOR, Spec.SPEC_1, 2);

        private static SpellData _spellData = new SpellData(_id,
            new AbilityCost(0, 70),
            TargetTeam.Ally,
            0,
            0,
            20,
            1.5f,
            GcdCategory.Normal,
            SchoolType.Physical,
            Mechanic.None,
            new SpellEffect[]
            {
                new ApplyAura(new ModStat(UnitStat.BLOCK, new Constant(0.3f), false)),
                new ApplyAura(new ModStat(UnitStat.PARRY, new Constant(0f), true))
            },
            SpellFlags.HASTE_AFFECTS_COOLDOWN,
            SpellType.Default
            );

        public СoncentratedDefense() : base(_spellData)
        {
        }
    }

    public class Slash : CleaveSpell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.WARRIOR, Spec.SPEC_1, 3);

        private static SpellData _spellData = new SpellData(_id,
            new AbilityCost(60, 0),
            TargetTeam.Enemy,
            3,
            0,
            10,
            1.5f,
            GcdCategory.Normal,
            SchoolType.Physical,
            Mechanic.None,
            new SpellEffect[]
            {
                new Dummy(30),
                new SchoolDamage(new StatValue(1.2f, UnitStat.ATK)),
                new GiveResource(0, ResourceType.CONCENTRATION),
            },
            SpellFlags.HASTE_AFFECTS_COOLDOWN,
            SpellType.Cleave
            );

        public Slash() : base(_spellData)
        {
        }
    }

    public class IgnorPain : Spell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.WARRIOR, Spec.SPEC_1, 4);

        private static SpellData _spellData = new SpellData(_id,
            new AbilityCost(0, 0),
            TargetTeam.Ally,
            0,
            0,
            50,
            1.5f,
            GcdCategory.Normal,
            SchoolType.Physical,
            Mechanic.None,
            new SpellEffect[]
            {
                new AbsorbDamage(new MultiplyValue(new StatValue(0.01f, UnitStat.MAX_HEALTH), new CasterResourceValue(ResourceType.ENERGY)), SchoolType.Any),
                new ApplyAura(new ModStat(UnitStat.ATK, new Constant(0), true)),
                new GiveResource(float.NegativeInfinity, ResourceType.RAGE)
            },
            SpellFlags.NONE,
            SpellType.Default
            );

        public IgnorPain() : base(_spellData)
        {
        }
    }

    public class WillForVictory : SplashSpell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.WARRIOR, Spec.SPEC_1, 5);

        private static SpellData _spellData = new SpellData(_id,
            new AbilityCost(0, 0),
            TargetTeam.Ally,
            10,
            0,
            150,
            1.5f,
            GcdCategory.Normal,
            SchoolType.Physical,
            Mechanic.None,
            new SpellEffect[]
            {
                new ApplyAura(new ModStat(UnitStat.DAMAGE_TAKEN, new Constant(-0.3f), true)),
                new GiveResource(0, ResourceType.CONCENTRATION)
            },
            SpellFlags.NONE,
            SpellType.Splash
            );

        public WillForVictory() : base(_spellData)
        {
        }
    }

    public class Charge : Spell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.WARRIOR, Spec.SPEC_1, 6);

        private static SpellData _spellData = new SpellData(_id,
            new AbilityCost(0, 0),
            TargetTeam.Enemy,
            15,
            0,
            20,
            1.5f,
            GcdCategory.Normal,
            SchoolType.Physical,
            Mechanic.None,
            new SpellEffect[]
            {
                new Taunt()
            },
            SpellFlags.NONE,
            SpellType.Default
            );

        public Charge() : base(_spellData)
        {
        }
    }

    public class DirectHit2 : Spell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.WARRIOR, Spec.SPEC_2, 1);

        private static SpellData _spellData = new SpellData(_id,
            new AbilityCost(40, 0),
            TargetTeam.Enemy,
            2,
            0,
            5,
            1.5f,
            GcdCategory.Normal,
            SchoolType.Physical,
            Mechanic.None,
            new SpellEffect[]
            {
                new SchoolDamage(new StatValue(1.3f, UnitStat.ATK)),
                //TODO: Apply periodic damage
                new TriggerSpell((SpellId)0)
            },
            SpellFlags.HASTE_AFFECTS_COOLDOWN,
            SpellType.Default
            );

        public DirectHit2() : base(_spellData)
        {
        }
    }

    public class СoncentratedWeaknesses : Spell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.WARRIOR, Spec.SPEC_2, 1);

        private static SpellData _spellData = new SpellData(_id,
            new AbilityCost(0, 70),
            TargetTeam.Ally,
            0,
            0,
            30,
            1.5f,
            GcdCategory.Normal,
            SchoolType.Physical,
            Mechanic.None,
            new SpellEffect[]
            {
                new ApplyAura(new ModStat(UnitStat.CRIT, new Constant(500), false)),
                new GiveResource(25, ResourceType.CONCENTRATION)
                //TODO: disable blocks
            },
            SpellFlags.HASTE_AFFECTS_COOLDOWN | SpellFlags.STATUS_STACK_COUNT_AFFECTS_BONUSES,
            SpellType.Default
            );

        public СoncentratedWeaknesses() : base(_spellData)
        {
        }
    }

    public class Slash2 : CleaveSpell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.WARRIOR, Spec.SPEC_2, 3);

        private static SpellData _spellData = new SpellData(_id,
            new AbilityCost(0, 50),
            TargetTeam.Enemy,
            3,
            0,
            10,
            1.5f,
            GcdCategory.Normal,
            SchoolType.Physical,
            Mechanic.None,
            new SpellEffect[]
            {
                new Dummy(15),
                new SchoolDamage(new StatValue(1.2f, UnitStat.ATK)),
                //new TriggerSpell((SpellId)1),
                //new TriggerSpell((SpellId)2),
                //new TriggerSpell((SpellId)4),
            },
            SpellFlags.HASTE_AFFECTS_COOLDOWN,
            SpellType.Cleave
            );

        public Slash2() : base(_spellData)
        {
        }
    }

    //TODO: spend all rage
    public class BloodyRage : SelfcastSpell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.WARRIOR, Spec.SPEC_2, 4);

        private static SpellData _spellData = new SpellData(_id,
            new AbilityCost(0, 0),
            TargetTeam.Ally,
            0,
            0,
            10,
            1.5f,
            GcdCategory.Normal,
            SchoolType.Physical,
            Mechanic.None,
            new SpellEffect[]
            {
                //new ApplyAura(new ModStat(new ValueMultiplication(new StatProvider(Stats.UnitStat.HASTE, 0.3f), new )),
            },
            SpellFlags.HASTE_AFFECTS_COOLDOWN | SpellFlags.SEPARATED_STATUS,
            SpellType.Default
            );

        public BloodyRage() : base(_spellData)
        {
        }
    }

    public class Charge2 : Spell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.WARRIOR, Spec.SPEC_1, 6);

        private static SpellData _spellData = new SpellData(_id,
            new AbilityCost(0, 0),
            TargetTeam.Enemy,
            15,
            0,
            20,
            1.5f,
            GcdCategory.Normal,
            SchoolType.Physical,
            Mechanic.None,
            new SpellEffect[]
            {
                new ApplyAura(new ModStat(UnitStat.SPEED, new Constant(-0.2f), true))
            },
            SpellFlags.NONE,
            SpellType.Default
            );

        public Charge2() : base(_spellData)
        {
        }
    }
}
