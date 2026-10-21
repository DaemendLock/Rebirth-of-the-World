using Combat.API;
using Combat.API.DTO;
using Combat.API.ValueObjects;
using Combat.Local.Application.DTO;
using Combat.Local.Application.ValueObjects;
using Combat.Local.Scripting.Adapters;

namespace Combat.Local.Scripting.Capabilities.Statuses
{

    internal static class StatusCapabilityMapper
    {
        public static DamageInstanceApi Adapt(in DamageInstance instance, CharacterApiAdapter units, AbilityApiAdapter abilities)
        {
            Unit target = units.Adaptee(instance.Target);
            Unit attacker = instance.Attacker.HasValue ? units.Adaptee(instance.Attacker.Value) : null;
            AbilityApi source = instance.Source.HasValue ? abilities.Adaptee(instance.Source.Value) : null;

            return new(target, attacker, source, instance.OriginalHealthDamage, instance.OriginalComposureDamage, instance.Flags);
        }

        public static HealingInstanceApi Adapt(in HealingInstance instance, CharacterApiAdapter units, AbilityApiAdapter abilities)
        {
            Unit target = units.Adaptee(instance.Target);
            Unit healer = instance.Healer.HasValue ? units.Adaptee(instance.Healer.Value) : null;
            AbilityApi source = instance.Source.HasValue ? abilities.Adaptee(instance.Source.Value) : null;

            return new(target, healer, source, instance.OriginalHealing, instance.Flags);
        }

        public static API.DTO.DamageRecord Adapt(in Application.DTO.DamageRecord result, CharacterApiAdapter units, AbilityApiAdapter abilities)
        {
            Unit target = units.Adaptee(result.Target);
            Unit attacker = result.Attacker.HasValue ? units.Adaptee(result.Attacker.Value) : null;
            AbilityApi source = result.Skill.HasValue ? abilities.Adaptee(result.Skill.Value) : null;

            return new(target, result.OriginalHealthDamage, result.OriginalComposureDamage,
                result.FinalHealthDamage, result.FinalComposureDamage, result.Flags, attacker, source);
        }
    }
}
