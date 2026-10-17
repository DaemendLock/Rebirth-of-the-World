using Combat.Common.Primitives;
using Combat.Local.Application.ValueObjects;

using System;
using System.Collections.Generic;

namespace Combat.Local.Application.Queries.Statuses
{
    public interface IHealingDamageModificationCalculator : IDamageModificationCalculator, IHealingModificationCalculator
    {
    }

    public interface IDamageModificationCalculator : IHealingModificationCalculator
    {
        DamageModification GetOutgoingDamageModification(IEnumerable<StatusId> attackerModifier, in DamageInstance instance);
        DamageModification GetIncomingDamageModification(IEnumerable<StatusId> defenderModifier, in DamageInstance instance);
    }

    public interface IHealingModificationCalculator
    {
        HealingModification GetOutgoingHealingModification(IEnumerable<StatusId> modifiers, in HealingInstance instance);
        //HealingModification GetHealeeModification(ReadOnlySpan<StatusId> modifiers, in HealingInstance instance);
    }
}
