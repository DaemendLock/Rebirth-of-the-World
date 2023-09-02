using Core.Combat.Abilities;
using Core.Combat.Abilities.SpellEffects;
using Core.Combat.Abilities.SpellScripts;
using Core.Combat.Auras.AuraEffects;
using Core.Combat.Stats;
using Core.Combat.Units;
using Core.Combat.Utils.ValueSources;
using Utils.DataTypes;
using Utils.SpellIdGenerator;

namespace Core.SpellLib.Warrior
{
    public class DirectHit : Spell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.WARRIOR, Spec.SPEC_1, 1);

        private static SpellData _spellData = new SpellData(_id, "",
            new AbilityCost(30, 0),
            TargetTeam.ENEMY,
            2,
            0,
            0,
            1.5f,
            GcdCategory.NORMAL,
            3,
            DispellType.BLEED,
            SchoolType.PHYSICAL,
            Mechanic.NONE,
            new SpellEffect[]
            {
                new SchoolDamage(new AttackpowerValue(0.3f)),
                new TriggerSpell((SpellId)2),
                new ApplyAura(new ModStat(UnitStat.SPEED, new Constant(-0.15f), true))
            },
            SpellFlags.HASTE_AFFECTS_COOLDOWN,
            typeof(Spell)
            );

        public DirectHit() : base(_spellData)
        {
        }
    }

    public class СoncentratedDefense : Spell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.WARRIOR, Spec.SPEC_1, 2);

        private static SpellData _spellData = new SpellData(_id, "",
            new AbilityCost(0, 70),
            TargetTeam.ALLY,
            0,
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
                new ApplyAura(new ModStat(UnitStat.BLOCK, new Constant(0.3f), false)),
                new ApplyAura(new ModStat(UnitStat.PARRY, new Constant(0.1f), true))
            },
            SpellFlags.HASTE_AFFECTS_COOLDOWN,
            typeof(Spell)
            );

        public СoncentratedDefense() : base(_spellData)
        {
        }
    }

    public class Slash : Spell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.WARRIOR, Spec.SPEC_1, 3);

        private static SpellData _spellData = new SpellData(_id, "",
            new AbilityCost(60, 0),
            TargetTeam.ENEMY,
            3,
            0,
            10,
            1.5f,
            GcdCategory.NORMAL,
            0,
            DispellType.NONE,
            SchoolType.PHYSICAL,
            Mechanic.NONE,
            new SpellEffect[]
            {
                new Dummy(30), //cleave angle from view axis
                new SchoolDamage(new AttackpowerValue(1.2f)),
                new TriggerSpell((SpellId)1),
                new TriggerSpell((SpellId)2),
                new TriggerSpell((SpellId)4),
            },
            SpellFlags.HASTE_AFFECTS_COOLDOWN,
            typeof(CleaveSpell)
            );

        public Slash() : base(_spellData)
        {
        }
    }

    //TODO: Ignor pain spell
    public class IgnorPain : Spell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.WARRIOR, Spec.SPEC_1, 4);

        private static SpellData _spellData = new SpellData(_id, "",
            new AbilityCost(0, 0),
            TargetTeam.ALLY,
            0,
            0,
            50,
            1.5f,
            GcdCategory.NORMAL,
            10,
            DispellType.NONE,
            SchoolType.PHYSICAL,
            Mechanic.NONE,
            new SpellEffect[]
            {
                //TODO: Current resource value source, <spell effect modification> value source
                //new ApplyAura(new AbsorbDamage(new ValueMultiplication())),
                new ApplyAura(new ModStat(UnitStat.ATK, new Constant(0.15f), true))
            },
            SpellFlags.NONE,
            typeof(Spell)
            );

        public IgnorPain() : base(_spellData)
        {
        }
    }

    public class WillForVictory : Spell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.WARRIOR, Spec.SPEC_1, 5);

        private static SpellData _spellData = new SpellData(_id, "",
            new AbilityCost(0, 0),
            TargetTeam.ALLY,
            10,
            0,
            150,
            1.5f,
            GcdCategory.NORMAL,
            20,
            DispellType.NONE,
            SchoolType.PHYSICAL,
            Mechanic.NONE,
            new SpellEffect[]
            {
                new ApplyAura(new ModStat(UnitStat.DAMAGE_TAKEN, new Constant(-0.3f), true)),
                new GiveResource(100, ResourceType.CONCENTRATION)
            },
            SpellFlags.NONE,
            typeof(SplashSpell)
            );

        public WillForVictory() : base(_spellData)
        {
        }
    }

    public class Charge : Spell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.WARRIOR, Spec.SPEC_1, 6);

        private static SpellData _spellData = new SpellData(_id, "",
            new AbilityCost(0, 0),
            TargetTeam.ENEMY,
            15,
            0,
            20,
            1.5f,
            GcdCategory.NORMAL,
            3,
            DispellType.NONE,
            SchoolType.PHYSICAL,
            Mechanic.NONE,
            new SpellEffect[]
            {
                new Taunt()
            },
            SpellFlags.NONE,
            typeof(Spell)
            );

        public Charge() : base(_spellData)
        {
        }
    }

    public class DirectHit2 : Spell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.WARRIOR, Spec.SPEC_2, 1);

        private static SpellData _spellData = new SpellData(_id, "",
            new AbilityCost(40, 0),
            TargetTeam.ENEMY,
            2,
            0,
            5,
            1.5f,
            GcdCategory.NORMAL,
            3,
            DispellType.NONE,
            SchoolType.PHYSICAL,
            Mechanic.NONE,
            new SpellEffect[]
            {
                new SchoolDamage(new AttackpowerValue(1.3f)),
                //TODO: Apply periodic damage
                new TriggerSpell((SpellId)0)
            },
            SpellFlags.HASTE_AFFECTS_COOLDOWN,
            typeof(Spell)
            );

        public DirectHit2() : base(_spellData)
        {
        }
    }

    public class СoncentratedWeaknesses : Spell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.WARRIOR, Spec.SPEC_2, 1);

        private static SpellData _spellData = new SpellData(_id, "",
            new AbilityCost(0, 70),
            TargetTeam.ALLY,
            0,
            0,
            30,
            1.5f,
            GcdCategory.NORMAL,
            10,
            DispellType.NONE,
            SchoolType.PHYSICAL,
            Mechanic.NONE,
            new SpellEffect[]
            {
                new ApplyAura(new ModStat(UnitStat.CRIT, new Constant(500), false)),
                new GiveResource(25, ResourceType.CONCENTRATION)
                //TODO: disable blocks
            },
            SpellFlags.HASTE_AFFECTS_COOLDOWN | SpellFlags.STATUS_STACK_COUNT_AFFECTS_BONUSES,
            typeof(Spell)
            );

        public СoncentratedWeaknesses() : base(_spellData)
        {
        }
    }

    //TODO: Cleave
    public class Slash2 : CleaveSpell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.WARRIOR, Spec.SPEC_2, 3);

        private static SpellData _spellData = new SpellData(_id, "",
            new AbilityCost(0, 50),
            TargetTeam.ENEMY,
            3,
            0,
            10,
            1.5f,
            GcdCategory.NORMAL,
            0,
            DispellType.NONE,
            SchoolType.PHYSICAL,
            Mechanic.NONE,
            new SpellEffect[]
            {
                new Dummy(15),
                new SchoolDamage(new AttackpowerValue(1.2f)),
                //TODO
                new TriggerSpell((SpellId)1),
                new TriggerSpell((SpellId)2),
                new TriggerSpell((SpellId)4),
            },
            SpellFlags.HASTE_AFFECTS_COOLDOWN,
            typeof(CleaveSpell)
            );

        public Slash2() : base(_spellData)
        {
        }
    }

    //TODO: spend all rage
    public class BloodyRage : Spell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.WARRIOR, Spec.SPEC_2, 4);

        private static SpellData _spellData = new SpellData(_id, "",
            new AbilityCost(0, 0),
            TargetTeam.ENEMY,
            3,
            0,
            10,
            1.5f,
            GcdCategory.NORMAL,
            0,
            DispellType.NONE,
            SchoolType.PHYSICAL,
            Mechanic.NONE,
            new SpellEffect[]
            {
                //TODO: get rage new ApplyAura(new ModStat(Stats.UnitStat.HASTE, )),
            },
            SpellFlags.HASTE_AFFECTS_COOLDOWN,
            typeof(Spell)
            );

        public BloodyRage() : base(_spellData)
        {
        }
    }

    public class Charge2 : Spell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.WARRIOR, Spec.SPEC_1, 6);

        private static SpellData _spellData = new SpellData(_id, "",
            new AbilityCost(0, 0),
            TargetTeam.ENEMY,
            15,
            0,
            20,
            1.5f,
            GcdCategory.NORMAL,
            3,
            DispellType.NONE,
            SchoolType.PHYSICAL,
            Mechanic.NONE,
            new SpellEffect[]
            {
                new ApplyAura(new ModStat(UnitStat.SPEED, new Constant(-0.2f), true))
            },
            SpellFlags.NONE,
            typeof(Spell)
            );

        public Charge2() : base(_spellData)
        {
        }
    }
}
