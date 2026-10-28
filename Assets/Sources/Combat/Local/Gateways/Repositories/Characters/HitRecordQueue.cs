using Combat.Domain.Events;
using Combat.Local.Application.Queries;
using Combat.Local.Application.ValueObjects;

using System.Collections.Generic;

namespace Combat.Local.Gateways.Repositories.Characters
{
    public sealed class HitRecordQueue : IHitRecordQueue
    {
        private readonly Queue<HitEvent> _values = new();

        public void Enqueue(HitEvent record) => _values.Enqueue(record);

        public bool TryDequeue(out HitEvent record) => _values.TryDequeue(out record);
    }

    public sealed class InputBuffer : IInputQueue
    {
        private readonly Queue<IInputCommand> _values = new();

        public void Enqueue(IInputCommand value) => _values.Enqueue(value);

        public bool TryDequeue(out IInputCommand record) => _values.TryDequeue(out record);
    }
}
