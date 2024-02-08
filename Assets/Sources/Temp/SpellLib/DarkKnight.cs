using Core.Combat.Abilities;
using Core.Combat.Abilities.SpellEffects;
using Core.Combat.Abilities.SpellScripts;
using Core.Combat.Utils.Serialization;
using Utils.DataTypes;
using Utils.SpellIdGenerator;

namespace Assets.Sources.Temp.SpellLib
{
    public class ShadowSlash : SelfcastSpell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.DARK_KNIGHT, Spec.SPEC_1, 1);

        private static SpellData _spellData = new SpellData(_id,
            new AbilityCost(10, 0),
            TargetTeam.Ally,
            0,
            0,
            0,
            1f,
            GcdCategory.Normal,
            SchoolType.Darkness,
            Mechanic.None,
            new SpellEffect[]
            {
                new TriggerSpell(SpellIdCalculator.GenerateId(Class.DARK_KNIGHT, Spec.SPEC_1, 7))
            },
            SpellFlags.HASTE_AFFECTS_COOLDOWN,
            SpellType.Selfcast
            );

        public ShadowSlash() : base(_spellData)
        {
        }
    }

    public class ShadowExplostion : SelfcastSpell
    {
        private static readonly int _id = SpellIdCalculator.GenerateId(Class.DARK_KNIGHT, Spec.SPEC_1, 2);

        private static SpellData _spellData = new SpellData(_id,
            new AbilityCost(10, 0),
            TargetTeam.Ally,
            0,
            0,
            8,
            0,
            GcdCategory.Normal,
            SchoolType.Darkness,
            Mechanic.None,
            new SpellEffect[]
            {
                new TriggerSpell(SpellIdCalculator.GenerateId(Class.DARK_KNIGHT, Spec.SPEC_1, 7))
            },
            SpellFlags.HASTE_AFFECTS_COOLDOWN,
            SpellType.Selfcast
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
            TargetTeam.Ally,
            0,
            0,
            20,
            0,
            GcdCategory.Normal,
            SchoolType.Chaos,
            Mechanic.None,
            new SpellEffect[]
            {
                //new ApplyAura(new ModStat(UnitStat.HASTE, new Constant(30), true))
            },
            SpellFlags.HASTE_AFFECTS_COOLDOWN,
            SpellType.Selfcast
            );

        public Elegy() : base(_spellData)
        {
        }
    }
}
