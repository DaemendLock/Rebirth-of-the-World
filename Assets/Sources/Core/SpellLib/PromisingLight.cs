using Core.Combat.Abilities;
using Core.Combat.Units;
using Core.Combat.Utils;
using Utils.DataTypes;

namespace Core.SpellLib
{
    public sealed class PromisingLight : Spell
    {
        public PromisingLight(SpellData data) : base(data) { }

        public override void Cast(CastEventData data, SpellModification spellModification)
        {
            const byte HealingIndex = 0;
            const byte EnergyConvertionIndex = 1;

            float energyConvertionCoefficient = GetEffectValue(EnergyConvertionIndex, spellModification.EffectsModifications[EnergyConvertionIndex]);
            float energy = data.Caster.GetResourceValue(ResourceType.LIGHT_POWER);

            data.Caster.SpendResource(new AbilityCost(0, energy));
            ApplyEffect(HealingIndex, spellModification.EffectsModifications[HealingIndex] + energyConvertionCoefficient * energy, data);
        }
    }
}