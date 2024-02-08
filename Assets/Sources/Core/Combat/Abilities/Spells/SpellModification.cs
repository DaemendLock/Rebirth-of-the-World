using Utils.DataTypes;

namespace Core.Combat.Abilities
{
    public class SpellModification
    {
        public AbilityCost BonusCost;
        public PercentModifiedValue BonusRange;
        public PercentModifiedValue BonusCastTime;
        public PercentModifiedValue CooldownReduction;
        public readonly float[] EffectsModifications;

        public SpellModification(Spell spell)
        {
            EffectsModifications = new float[spell.EffectsCount];
        }
    }

    public class SpellValueProvider
    {
        private SpellModification _modification;
        private Spell _spell;

        public SpellValueProvider(Spell spell, SpellModification modification)
        {
            _modification = modification;
            _spell = spell;
        }

        public SpellId SpellId => _spell.Id;
        public AbilityCost Cost => _spell.Cost + _modification.BonusCost;
        public float Range => (_modification.BonusRange + new PercentModifiedValue(_spell.Range, 100)).CalculatedValue;
        public float CastTime => (_modification.BonusCastTime + new PercentModifiedValue(_spell.CastTime, 100)).CalculatedValue;
        public float Cooldown => (new PercentModifiedValue(_spell.CastTime, 100) - _modification.CooldownReduction).CalculatedValue;

        public float EffectBonus(int effect) => _modification.EffectsModifications[effect];

    }
}
