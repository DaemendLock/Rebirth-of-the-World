using Combat.Common.ValueObjects;
using Combat.Local.Domain.ValueObjects;

using System;

namespace Combat.Local.Domain.OutputPorts.Statuses
{
    public interface IDamageModifierCalculator
    {
        DamageModification GetAttackerDamageModification(ReadOnlySpan<StatusId> attackerModifier, in DamageInstance instance);
        DamageModification GetDefenderDamageModification(ReadOnlySpan<StatusId> defenderModifier, in DamageInstance instance);
    }
}
