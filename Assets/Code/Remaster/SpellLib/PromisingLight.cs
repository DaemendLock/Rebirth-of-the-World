using Remaster.Abilities;
using Remaster.Events;

namespace Remaster.SpellLib
{
    public sealed class PromisingLight : Spell
    {
        public PromisingLight(SpellData data) : base(data) { }

        public override void Cast(EventData data, SpellModification spellModification)
        {
            const byte HealingIndex = 0;
            const byte EnergyConvertionIndex = 1;
            
            float energyConvertionCoefficient = GetEffectValue(EnergyConvertionIndex, spellModification.EffectsModificationList[EnergyConvertionIndex]);
            float energy = data.Caster.GetResourceValue(ResourceType.LIGHT_POWER);

            data.Caster.SpendResource(new AbilityCost(0, energy));
            ApplyEffect(HealingIndex, spellModification.EffectsModificationList[HealingIndex] + energyConvertionCoefficient * energy, data);
        }
    }
}