using Combat.Common.Primitives;
using Combat.Local.Application.DTO;

using System.Collections.Generic;

namespace Combat.Local.Application.Queries.Statuses
{
    public interface IAttributesModificationCalculator
    {
        StatusBonuses Evaluate(IEnumerable<StatusId> values);
    }
}
