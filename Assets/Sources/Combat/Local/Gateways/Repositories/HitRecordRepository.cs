using Combat.Local.Data.Models;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

using System.Collections.Generic;

namespace Combat.Local.Data.Repositories
{
    public class HitRecordRepository : IHitRecordRepository
    {
        private readonly Queue<HitData> _values = new();

        public void Register(HitRecord value) => _values.Enqueue(new(value.HitboxId, value.HurtboxId, value.Location));

        public bool TryPop(out HitRecord result)
        {
            if (_values.TryDequeue(out HitData data) == false)
            {
                result = default;
                return false;
            }

            result = new(data.HitboxId, data.HurtboxId, data.Location);
            return true;
        }
    }
}
