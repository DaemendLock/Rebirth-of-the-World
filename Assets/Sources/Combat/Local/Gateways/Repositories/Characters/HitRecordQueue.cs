using Combat.Local.Application.Repositories;

using System.Collections.Generic;

namespace Combat.Local.Gateways.Repositories.Characters
{
    public sealed class HitRecordQueue : IHitRecordQueue
    {
        private readonly Queue<HitInfo> _values = new();

        public void Enqueue(HitInfo record) => _values.Enqueue(record);

        public bool TryDequeue(out HitInfo record) => _values.TryDequeue(out record);
    }
}
