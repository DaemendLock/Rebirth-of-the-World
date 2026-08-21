using Combat.Common.Primitives;
using Combat.Local.Domain.ValueObjects;

using System;

namespace Combat.Local.Domain.OutputPorts.Statuses
{
    public interface IHealingDamageModifierCalculator
    {
        DamageModification GetAttackerDamageModification(ReadOnlySpan<StatusInstance> attackerModifier, in DamageInstance instance);
        DamageModification GetDefenderDamageModification(ReadOnlySpan<StatusInstance> defenderModifier, in DamageInstance instance);
        HealingModification GetHealingModification(ReadOnlySpan<StatusInstance> modifiers, in HealingInstance instance);
    }
}
