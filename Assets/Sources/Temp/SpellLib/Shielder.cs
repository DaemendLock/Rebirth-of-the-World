﻿using Core.Combat.Abilities;
using Core.Combat.Abilities.SpellEffects;
using Core.Combat.Abilities.SpellScripts;
using Core.Combat.Statuses;
using Core.Combat.Statuses.AuraEffects;
using Core.Combat.Utils.Serialization;
using Core.Combat.Utils.ValueSources;
using Utils.DataStructure;
using Utils.DataTypes;
using Utils.SpellIdGenerator;

namespace Core.SpellLib.Shielder
{
    public class ShieldStrike : CleaveSpell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.SHIELDER, Spec.SPEC_1, 1);

        private static SpellData _spellData = new SpellData(_id,
            new AbilityCost(0, 90),
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
                new ApplyAura(new ModStat(UnitStat.SPEED, new Constant(-0.1f), true)),
                //periodic damage
            },
            SpellFlags.HASTE_AFFECTS_COOLDOWN,
            SpellType.Cleave
            );

        public ShieldStrike() : base(_spellData)
        {
        }
    }

    //TODO: charge
    public class Breakthrough : SelfcastSpell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.SHIELDER, Spec.SPEC_1, 2);

        private static SpellData _spellData = new SpellData(_id,
            new AbilityCost(0, 90),
            TargetTeam.Ally,
            0,
            0,
            0,
            1.5f,
            GcdCategory.Normal,
            SchoolType.Physical,
            Mechanic.None,
            new SpellEffect[]
            {
                //charge
                new ReduceCooldown(SpellIdCalculator.GenerateId(Class.SHIELDER, Spec.SPEC_1, 1), float.PositiveInfinity),
                //give overshield
                new ApplyAura(new ModifySpellEffect(SpellIdCalculator.GenerateId(Class.SHIELDER, Spec.SPEC_1, 1), 1, 1))
            },
            SpellFlags.HASTE_AFFECTS_COOLDOWN,
            SpellType.Selfcast
            );

        public Breakthrough() : base(_spellData)
        {
        }
    }

    public class Shelter : SplashSpell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.SHIELDER, Spec.SPEC_1, 3);

        private static SpellData _spellData = new SpellData(_id,
            new AbilityCost(0, 90),
            TargetTeam.Ally,
            10,
            0,
            20,
            1.5f,
            GcdCategory.Normal,
            SchoolType.Physical,
            Mechanic.None,
            new SpellEffect[]
            {
                new ApplyAura(new ModStat(UnitStat.DAMAGE_TAKEN, new Constant(-0.2f), true)),
            },
            SpellFlags.NONE,
            SpellType.Splash
            );

        public Shelter() : base(_spellData)
        {
        }
    }

    public class SacrificialProtection : Spell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.SHIELDER, Spec.SPEC_1, 4);

        private static SpellData _spellData = new SpellData(_id,
            new AbilityCost(0, 90),
            TargetTeam.Ally,
            10,
            0,
            30,
            1.5f,
            //<duration>,
            //DispellType,
            GcdCategory.Normal,
            SchoolType.Physical,
            Mechanic.None,
            new SpellEffect[]
            {
                //TODO: Apply overshield new ApplyAura(new Overshield(UnitStat.ATK, new Constant(0.1f), true)),
                new ApplyAura(new ModStat(UnitStat.ATK, new Constant(0.1f), true)),
                new ApplyAura(new ModStat(UnitStat.HEALING_TAKEN, new Constant(0.1f), true)),
            },
            SpellFlags.HASTE_AFFECTS_COOLDOWN,
            SpellType.Default
            );

        public SacrificialProtection() : base(_spellData)
        {
        }
    }

    public class AnchoringHowl : SplashSpell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.SHIELDER, Spec.SPEC_1, 5);

        private static SpellData _spellData = new SpellData(_id,
            new AbilityCost(0, 90),
            TargetTeam.Enemy,
            5,
            0,
            15,
            1.5f,
            GcdCategory.Normal,
            SchoolType.Physical,
            Mechanic.None,
            new SpellEffect[]
            {

            },
            SpellFlags.NONE,
            SpellType.Splash
            );

        public AnchoringHowl() : base(_spellData)
        {
        }
    }

    public class SkinOfStone : SelfcastSpell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.SHIELDER, Spec.SPEC_1, 6);

        private static SpellData _spellData = new SpellData(_id,
            new AbilityCost(0, 90),
            TargetTeam.Ally,
            0,
            0,
            180,
            1.5f,
            GcdCategory.Normal,
            SchoolType.Physical,
            Mechanic.None,
            new SpellEffect[]
            {
                //disable controll
                new ApplyAura(new ModStat(UnitStat.DAMAGE_TAKEN, new Constant(-0.9f), true))
                //count damage to 200% max hp, then trugger spell
                //create aoe damage aura
            },
            SpellFlags.NONE,
            SpellType.Selfcast
            );

        public SkinOfStone() : base(_spellData)
        {
        }
    }
}
