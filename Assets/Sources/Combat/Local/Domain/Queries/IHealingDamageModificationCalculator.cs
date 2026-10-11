using Combat.Common.Primitives;
using Combat.Local.Domain.ValueObjects;

using System;

namespace Combat.Local.Application.Queries.Statuses
{
    public interface IHealingDamageModificationCalculator : IDamageModificationCalculator, IHealingModificationCalculator
    {
    }

    public interface IDamageModificationCalculator : IHealingModificationCalculator
    {
        DamageModification GetAttackerDamageModification(ReadOnlySpan<StatusId> attackerModifier, in DamageInstance instance);
        DamageModification GetDefenderDamageModification(ReadOnlySpan<StatusId> defenderModifier, in DamageInstance instance);
    }

    public interface IHealingModificationCalculator
    {
        HealingModification GetHealerModification(ReadOnlySpan<StatusId> modifiers, in HealingInstance instance);
        //HealingModification GetHealeeModification(ReadOnlySpan<StatusId> modifiers, in HealingInstance instance);
    }
}
