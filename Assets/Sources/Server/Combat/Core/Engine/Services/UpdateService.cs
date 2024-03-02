using System;
using System.Collections.Generic;
using System.IO;

namespace Core.Combat.Engine.Services
{
    internal interface UpdateService
    {
        long LastUpdate { get; }

        long UpdateTime { get; }

        UpdateRecord Update(long deltaTime, MemoryStream memory);
    }

    internal class FixedTimeUpdateService : UpdateService
    {
        private readonly long _updateTime;
        private readonly List<PositionUpdateRecord> _memory;

        public FixedTimeUpdateService(long updateTime)
        {
            if (_updateTime <= 0)
            {
                throw new ArgumentException(nameof(updateTime) + " can't be less or equeal than zero.");
            }

            _updateTime = updateTime;
            _memory = new List<PositionUpdateRecord>();
        }

        public long LastUpdate { get; private set; }

        public long UpdateTime => _updateTime;

        public UpdateRecord Update(long deltaTime, MemoryStream memory)
        {
            LastUpdate = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            UpdateRecord record = new UpdateRecord(LastUpdate, _memory);

            ModelUpdate.UpdateInput(record);

            Units.UpdatePosition(deltaTime, record);
            Projectiles.Update(record);
            Units.Update(record);
            Units.UpdateDeathState(record);

            return record;
        }
    }

    internal class DynamicTimeUpdateService : UpdateService
    {
        private readonly List<PositionUpdateRecord> _memory;

        public DynamicTimeUpdateService()
        {
            _memory = new List<PositionUpdateRecord>();
        }

        public long LastUpdate { get; private set; }

        public long UpdateTime { get; private set; }

        public UpdateRecord Update(long deltaTime, MemoryStream memory)
        {
            LastUpdate = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            UpdateTime = deltaTime;

            UpdateRecord record = new UpdateRecord(LastUpdate, _memory);

            ModelUpdate.UpdateInput(record);

            Units.UpdatePosition(deltaTime, record);

            Projectiles.Update(record);
            Units.Update(record);
            Units.UpdateDeathState(record);

            return record;
        }
    }
}
