using Core.Combat.Abilities.SpellEffects;
using Core.Combat.Abilities;
using Utils.DataTypes;
using Utils.SpellIdGenerator;
using Core.Combat.Abilities.SpellScripts;
using Core.Combat.Auras.AuraEffects;
using Utils.DataStructure;
using Core.Combat.Utils.ValueSources;

namespace Assets.Sources.Temp.SpellLib
{
    public class ShadowSlash : SelfcastSpell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.DARK_KNIGHT, Spec.SPEC_1, 1);

        private static SpellData _spellData = new SpellData(_id,
            new AbilityCost(10, 0),
            TargetTeam.ALLY,
            0,
            0,
            0,
            1f,
            GcdCategory.NORMAL,
            10,
            DispellType.NONE,
            SchoolType.DARKNESS,
            Mechanic.NONE,
            new SpellEffect[]
            {
                new TriggerSpell(SpellIdCalculator.GenerateId(Class.DARK_KNIGHT, Spec.SPEC_1, 7))
            },
            SpellFlags.HASTE_AFFECTS_COOLDOWN,
            typeof(SelfcastSpell)
            );

        public ShadowSlash() : base(_spellData)
        {
        }
    }

    public class ShadowExplostion : AoeSpell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.DARK_KNIGHT, Spec.SPEC_1, 2);

        private static SpellData _spellData = new SpellData(_id,
            new AbilityCost(10, 0),
            TargetTeam.ALLY,
            0,
            0,
            8,
            0,
            GcdCategory.NORMAL,
            10,
            DispellType.NONE,
            SchoolType.DARKNESS,
            Mechanic.NONE,
            new SpellEffect[]
            {
                new TriggerSpell(SpellIdCalculator.GenerateId(Class.DARK_KNIGHT, Spec.SPEC_1, 7))
            },
            SpellFlags.HASTE_AFFECTS_COOLDOWN,
            typeof(SelfcastSpell)
            );

        public ShadowExplostion() : base(_spellData)
        {
        }
    }

    public class Elegy : SelfcastSpell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.DARK_KNIGHT, Spec.SPEC_1, 4);

        private static SpellData _spellData = new SpellData(_id,
            new AbilityCost(10, 0),
            TargetTeam.ALLY,
            0,
            0,
            20,
            0,
            GcdCategory.NORMAL,
            4,
            DispellType.NONE,
            SchoolType.CHAOS,
            Mechanic.NONE,
            new SpellEffect[]
            {
                new ApplyAura(new ModStat(UnitStat.HASTE, new Constant(30), true))
            },
            SpellFlags.HASTE_AFFECTS_COOLDOWN,
            typeof(SelfcastSpell)
            );

        public Elegy() : base(_spellData)
        {
        }
    }
}
