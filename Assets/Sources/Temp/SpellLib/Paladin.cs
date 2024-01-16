using Core.Combat.Abilities;
using Core.Combat.Abilities.SpellEffects;
using Core.Combat.Abilities.SpellScripts;
using Core.Combat.Statuses.AuraEffects;
using Core.Combat.Utils.Serialization;
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
            TargetTeam.Ally,
            0,
            0,
            7,
            1.5f,
            GcdCategory.Normal,
            SchoolType.Light,
            Mechanic.None,
            new SpellEffect[]
            {
                new Heal(new MultiplyValue(new StatValue(0.5f, UnitStat.SPELLPOWER), new CasterResourceValue(ResourceType.LIGHT_POWER))),
                new GiveResource(float.NegativeInfinity, ResourceType.LIGHT_POWER)
            },
            SpellFlags.HASTE_AFFECTS_COOLDOWN,
            SpellType.Default
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
            TargetTeam.Ally,
            0,
            0,
            6,
            1.5f,
            GcdCategory.Normal,
            SchoolType.Light,
            Mechanic.None,
            new SpellEffect[]
            {
                new ApplyAura(new ReactionCast(SpellIdCalculator.GenerateId(Class.PALADIN, Spec.SPEC_1, 7), Core.Combat.Statuses.UnitAction.AUTOATTACK))
            },
            SpellFlags.HASTE_AFFECTS_COOLDOWN,
            SpellType.Selfcast
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
            TargetTeam.Enemy,
            float.PositiveInfinity,
            0,
            0,
            0f,
            GcdCategory.Ignor,
            SchoolType.Light,
            Mechanic.None,
            new SpellEffect[]
            {
                new SchoolDamage(new StatValue(1, UnitStat.SPELLPOWER)),
                new TriggerSpell(SpellIdCalculator.GenerateId(Class.PALADIN, Spec.SPEC_1, 8))
            },
            SpellFlags.CANT_BE_EVAIDED | SpellFlags.CANT_BE_PARRIED | SpellFlags.CANT_BE_BLOCKED | SpellFlags.PROC_SPELL,
            SpellType.Default
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
            TargetTeam.Ally,
            float.PositiveInfinity,
            0,
            0,
            0f,
            GcdCategory.Ignor,
            SchoolType.Light,
            Mechanic.None,
            new SpellEffect[]
            {
                new GiveResource(1, ResourceType.LIGHT_POWER)
            },
            SpellFlags.PROC_SPELL,
            SpellType.Selfcast
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
            TargetTeam.Ally,
            0,
            0,
            20,
            0f,
            GcdCategory.Normal,
            SchoolType.Fire,
            Mechanic.None,
            new SpellEffect[]
            {
                new TriggerSpell(SpellIdCalculator.GenerateId(Class.PALADIN, Spec.SPEC_1, 9)),
                new TriggerSpell(SpellIdCalculator.GenerateId(Class.PALADIN, Spec.SPEC_1, 10))
            },
            SpellFlags.HASTE_AFFECTS_COOLDOWN,
            SpellType.Selfcast
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
            TargetTeam.Ally,
            5,
            0,
            0,
            0f,
            GcdCategory.Ignor,
            SchoolType.Fire,
            Mechanic.None,
            new SpellEffect[]
            {
                new ApplyAura(new ModStat(UnitStat.SPEED, new Constant(1), false))
            },
            SpellFlags.HASTE_AFFECTS_COOLDOWN | SpellFlags.PROC_SPELL,
            SpellType.Splash
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
            TargetTeam.Enemy,
            5,
            0,
            0,
            0f,
            GcdCategory.Ignor,
            SchoolType.Fire,
            Mechanic.None,
            new SpellEffect[]
            {
                new SchoolDamage(new StatValue(1, UnitStat.SPELLPOWER)),
                new TriggerSpell(SpellIdCalculator.GenerateId(Class.PALADIN, Spec.SPEC_1, 11))
            },
            SpellFlags.PROC_SPELL,
            SpellType.Splash
            );

        public ConsecrationEnemyDamage() : base(_spellData)
        {
        }
    }

    public class ConsecrationEnemyHit : SelfcastSpell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.PALADIN, Spec.SPEC_1, 11);

        private static SpellData _spellData = new SpellData(_id,
            new AbilityCost(0, 0),
            TargetTeam.Ally,
            0,
            0,
            0,
            0f,
            GcdCategory.Ignor,
            SchoolType.Fire,
            Mechanic.None,
            new SpellEffect[]
            {
                new GiveResource(10, ResourceType.LIGHT_POWER)
            },
            SpellFlags.HASTE_AFFECTS_COOLDOWN,
            SpellType.Selfcast
            );

        public ConsecrationEnemyHit() : base(_spellData)
        {
        }
    }

    public class CandentArmor : SelfcastSpell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.PALADIN, Spec.SPEC_1, 4);

        private static SpellData _spellData = new SpellData(_id,
            new AbilityCost(100, 0),
            TargetTeam.Ally,
            0,
            0,
            25,
            1f,
            GcdCategory.Normal,
            SchoolType.Fire,
            Mechanic.None,
            new SpellEffect[]
            {
                new ApplyAura(new PeriodicallyTriggerSpell(SpellIdCalculator.GenerateId(Class.PALADIN, Spec.SPEC_1, 12), 1f))
            },
            SpellFlags.HASTE_AFFECTS_COOLDOWN,
            SpellType.Selfcast
            );

        public CandentArmor() : base(_spellData)
        {
        }
    }

    public class CandentArmorProc : SplashSpell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.PALADIN, Spec.SPEC_1, 12);

        private static SpellData _spellData = new SpellData(_id,
            new AbilityCost(0, 0),
            TargetTeam.Enemy,
            5,
            0,
            0,
            0f,
            GcdCategory.Ignor,
            SchoolType.Fire,
            Mechanic.None,
            new SpellEffect[]
            {
                new SchoolDamage(new StatValue(0.5f, UnitStat.SPELLPOWER)),
                new TriggerSpell(SpellIdCalculator.GenerateId(Class.PALADIN, Spec.SPEC_1, 13))
            },
            SpellFlags.PROC_SPELL,
            SpellType.Splash
            );

        public CandentArmorProc() : base(_spellData)
        {
        }
    }

    public class CandentArmorProcPower : SelfcastSpell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.PALADIN, Spec.SPEC_1, 13);

        private static SpellData _spellData = new SpellData(_id,
            new AbilityCost(0, 0),
            TargetTeam.Ally,
            0,
            0,
            0,
            0f,
            GcdCategory.Ignor,
            SchoolType.Fire,
            Mechanic.None,
            new SpellEffect[]
            {
                new GiveResource(3, ResourceType.LIGHT_POWER)
            },
            SpellFlags.PROC_SPELL,
            SpellType.Selfcast
            );

        public CandentArmorProcPower() : base(_spellData)
        {
        }
    }
}
