using Combat.Common.ValueObjects;
using Combat.Local.Domain.ValueObjects;

using System;

namespace Combat.Local.Domain.OutputPorts.Statuses
{
    public interface IHealingDamageModifierCalculator
    {
        DamageModification GetAttackerDamageModification(ReadOnlySpan<StatusId> attackerModifier, in DamageInstance instance);
        DamageModification GetDefenderDamageModification(ReadOnlySpan<StatusId> defenderModifier, in DamageInstance instance);

        HealingModification GetHealingModification(ReadOnlySpan<StatusId> modifiers, in HealingInstance instance);
    }
}
