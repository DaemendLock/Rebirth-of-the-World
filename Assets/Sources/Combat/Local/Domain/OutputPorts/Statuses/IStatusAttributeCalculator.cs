using Combat.Common.ValueObjects;
using Combat.Local.Domain.ValueObjects;

using System;

namespace Combat.Local.Domain.OutputPorts.Statuses
{
    public interface IStatusAttributeCalculator
    {
        AttributesModification Evaluate(ReadOnlySpan<StatusId> values);
    }
}
