using Combat.Common.Primitives;
using Combat.Local.Domain.ValueObjects;

using System;
using System.Collections.Generic;

namespace Combat.Local.Application.Queries.Statuses
{
    public interface IHealingDamageModificationCalculator : IDamageModificationCalculator, IHealingModificationCalculator
    {
    }

    public interface IDamageModificationCalculator : IHealingModificationCalculator
    {
        DamageModification GetOutgoingDamageModification(IReadOnlyCollection<StatusId> attackerModifier, in DamageInstance instance);
        DamageModification GetIncomingDamageModification(IReadOnlyCollection<StatusId> defenderModifier, in DamageInstance instance);
    }

    public interface IHealingModificationCalculator
    {
        HealingModification GetOutgoingHealingModification(IReadOnlyCollection<StatusId> modifiers, in HealingInstance instance);
        //HealingModification GetHealeeModification(ReadOnlySpan<StatusId> modifiers, in HealingInstance instance);
    }
}
