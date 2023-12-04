using Core.Combat.Abilities;
using Core.Combat.Abilities.SpellEffects;
using Core.Combat.Abilities.SpellScripts;
using Core.Combat.Auras.AuraEffects;
using Core.Combat.Units;
using Core.Combat.Utils.ValueSources;
using Utils.DataStructure;
using Utils.DataTypes;
using Utils.SpellIdGenerator;

namespace SpellLib.Paladin
{
    public class LifegivingLight : Spell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.PALADIN, Spec.SPEC_1, 1);

        private static SpellData _spellData = new SpellData(_id,
            new AbilityCost(10, 0),
            TargetTeam.ALLY,
            0,
            0,
            10,
            1.5f,
            GcdCategory.NORMAL,
            10,
            DispellType.NONE,
            SchoolType.LIGHT,
            Mechanic.NONE,
            new SpellEffect[]
            {
                new Heal(new MultiplyValue(new StatValue(1, UnitStat.SPELLPOWER), new CasterResourceValue(ResourceType.LIGHT_POWER))),
                new GiveResource(float.NegativeInfinity, ResourceType.LIGHT_POWER)
            },
            SpellFlags.HASTE_AFFECTS_COOLDOWN,
            typeof(Spell)
            );

        public LifegivingLight() : base(_spellData)
        {
        }
    }

    public class BladeOfFaith : SelfcastSpell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.PALADIN, Spec.SPEC_1, 2);

        private static SpellData _spellData = new SpellData(_id,
            new AbilityCost(10, 0),
            TargetTeam.ALLY,
            0,
            0,
            6,
            1.5f,
            GcdCategory.NORMAL,
            3,
            DispellType.NONE,
            SchoolType.LIGHT,
            Mechanic.NONE,
            new SpellEffect[]
            {
                new ApplyAura(new ReactionCast(SpellIdCalculator.GenerateId(Class.PALADIN, Spec.SPEC_1, 7), Core.Combat.Auras.UnitAction.AUTOATTACK))
            },
            SpellFlags.HASTE_AFFECTS_COOLDOWN,
            typeof(SelfcastSpell)
            );

        public BladeOfFaith() : base(_spellData)
        {
        }
    }

    public class BladeOfFaithProc : Spell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.PALADIN, Spec.SPEC_1, 7);

        private static SpellData _spellData = new SpellData(_id,
            new AbilityCost(0, 0),
            TargetTeam.ENEMY,
            float.PositiveInfinity,
            0,
            0,
            0f,
            GcdCategory.IGNOR,
            0,
            DispellType.NONE,
            SchoolType.LIGHT,
            Mechanic.NONE,
            new SpellEffect[]
            {
                new SchoolDamage(new StatValue(1, UnitStat.SPELLPOWER)),
                new TriggerSpell(SpellIdCalculator.GenerateId(Class.PALADIN, Spec.SPEC_1, 8))
            },
            SpellFlags.CANT_BE_EVAIDED | SpellFlags.CANT_BE_PARRIED | SpellFlags.CANT_BE_BLOCKED,
            typeof(Spell)
            );

        public BladeOfFaithProc() : base(_spellData)
        {
        }
    }

    public class BladeOfFaithProcSelf : SelfcastSpell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.PALADIN, Spec.SPEC_1, 8);

        private static SpellData _spellData = new SpellData(_id,
            new AbilityCost(0, 0),
            TargetTeam.ALLY,
            0,
            0,
            0,
            0f,
            GcdCategory.IGNOR,
            0,
            DispellType.NONE,
            SchoolType.LIGHT,
            Mechanic.NONE,
            new SpellEffect[]
            {
                new GiveResource(1, ResourceType.LIGHT_POWER)
            },
            SpellFlags.HASTE_AFFECTS_COOLDOWN,
            typeof(SelfcastSpell)
            );

        public BladeOfFaithProcSelf() : base(_spellData)
        {
        }
    }

    public class Consecration : SelfcastSpell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.PALADIN, Spec.SPEC_1, 3);

        private static SpellData _spellData = new SpellData(_id,
            new AbilityCost(100, 0),
            TargetTeam.ALLY,
            0,
            0,
            20,
            0f,
            GcdCategory.NORMAL,
            0,
            DispellType.NONE,
            SchoolType.FIRE,
            Mechanic.NONE,
            new SpellEffect[]
            {
                new TriggerSpell(SpellIdCalculator.GenerateId(Class.PALADIN, Spec.SPEC_1, 9)),
                new TriggerSpell(SpellIdCalculator.GenerateId(Class.PALADIN, Spec.SPEC_1, 10))
            },
            SpellFlags.HASTE_AFFECTS_COOLDOWN,
            typeof(SelfcastSpell)
            );

        public Consecration() : base(_spellData)
        {
        }
    }

    public class ConsecrationAllyBuff : SplashSpell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.PALADIN, Spec.SPEC_1, 9);

        private static SpellData _spellData = new SpellData(_id,
            new AbilityCost(0, 0),
            TargetTeam.ALLY,
            5,
            0,
            0,
            0f,
            GcdCategory.IGNOR,
            5,
            DispellType.NONE,
            SchoolType.FIRE,
            Mechanic.NONE,
            new SpellEffect[]
            {
                new ApplyAura(new ModStat(UnitStat.SPEED, new Constant(1), false))
            },
            SpellFlags.HASTE_AFFECTS_COOLDOWN | SpellFlags.PROC_SPELL,
            typeof(SplashSpell)
            );

        public ConsecrationAllyBuff() : base(_spellData)
        {
        }
    }

    public class ConsecrationEnemyDamage : SplashSpell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.PALADIN, Spec.SPEC_1, 10);

        private static SpellData _spellData = new SpellData(_id,
            new AbilityCost(0, 0),
            TargetTeam.ENEMY,
            50,
            0,
            0,
            0f,
            GcdCategory.IGNOR,
            0,
            DispellType.NONE,
            SchoolType.FIRE,
            Mechanic.NONE,
            new SpellEffect[]
            {
                new SchoolDamage(new StatValue(1, UnitStat.SPELLPOWER))
            },
            SpellFlags.PROC_SPELL,
            typeof(SplashSpell)
            );

        public ConsecrationEnemyDamage() : base(_spellData)
        {
        }
    }

    public class CandentArmor : SelfcastSpell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.PALADIN, Spec.SPEC_1, 4);

        private static SpellData _spellData = new SpellData(_id,
            new AbilityCost(100, 0),
            TargetTeam.ALLY,
            0,
            0,
            0,
            1f,
            GcdCategory.NORMAL,
            10,
            DispellType.NONE,
            SchoolType.FIRE,
            Mechanic.NONE,
            new SpellEffect[]
            {
                new ApplyAura(new PeriodicallyTriggerSpell(SpellIdCalculator.GenerateId(Class.PALADIN, Spec.SPEC_1, 11), 1f))
            },
            SpellFlags.HASTE_AFFECTS_COOLDOWN,
            typeof(SelfcastSpell)
            );

        public CandentArmor() : base(_spellData)
        {
        }
    }

    public class CandentArmorProc : SplashSpell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.PALADIN, Spec.SPEC_1, 11);

        private static SpellData _spellData = new SpellData(_id,
            new AbilityCost(0, 0),
            TargetTeam.ENEMY,
            50,
            0,
            0,
            0f,
            GcdCategory.IGNOR,
            0,
            DispellType.NONE,
            SchoolType.FIRE,
            Mechanic.NONE,
            new SpellEffect[]
            {
                new SchoolDamage(new StatValue(0.3f, UnitStat.SPELLPOWER)),
                new TriggerSpell(SpellIdCalculator.GenerateId(Class.PALADIN, Spec.SPEC_1, 12))
            },
            SpellFlags.PROC_SPELL,
            typeof(SplashSpell)
            );

        public CandentArmorProc() : base(_spellData)
        {
        }
    }

    public class CandentArmorProcPower : SelfcastSpell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.PALADIN, Spec.SPEC_1, 12);

        private static SpellData _spellData = new SpellData(_id,
            new AbilityCost(0, 0),
            TargetTeam.ALLY,
            5,
            0,
            0,
            0f,
            GcdCategory.IGNOR,
            0,
            DispellType.NONE,
            SchoolType.FIRE,
            Mechanic.NONE,
            new SpellEffect[]
            {
                new GiveResource(1, ResourceType.LIGHT_POWER)
            },
            SpellFlags.PROC_SPELL,
            typeof(SelfcastSpell)
            );

        public CandentArmorProcPower() : base(_spellData)
        {
        }
    }
}
