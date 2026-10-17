using Combat.Common.Primitives;
using Combat.Local.Application.ValueObjects;

using System.Collections.Generic;

namespace Combat.Local.Application.Queries.Statuses
{
    public interface IAttributesModificationCalculator
    {
        AttributesModification Evaluate(IEnumerable<StatusId> values);
    }
}
