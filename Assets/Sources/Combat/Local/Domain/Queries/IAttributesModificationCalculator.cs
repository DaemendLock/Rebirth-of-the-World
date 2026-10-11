using Combat.Common.Primitives;
using Combat.Local.Domain.ValueObjects;

using System;

namespace Combat.Local.Application.Queries.Statuses
{
    public interface IAttributesModificationCalculator
    {
        AttributesModification Evaluate(ReadOnlySpan<StatusId> values);
    }
}
