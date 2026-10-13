using Combat.Common.Primitives;
using Combat.Local.Domain.ValueObjects;

using System;
using System.Collections.Generic;

namespace Combat.Local.Application.Queries.Statuses
{
    public interface IAttributesModificationCalculator
    {
        AttributesModification Evaluate(IReadOnlyCollection<StatusId> values);
    }
}
