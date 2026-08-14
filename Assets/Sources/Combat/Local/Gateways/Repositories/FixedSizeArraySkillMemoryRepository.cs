using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories.Objectives;

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Combat.Local.Gateways.Repositories
{
    public readonly struct ObjectiveModel
    {
        public readonly string Name;
        public readonly ObjectiveState State;

        public ObjectiveModel(Objective objective)
        {
            Name = objective.Name;
            State = objective.State;
        }
    }

    public sealed class ObjectiveRepository : IObjectiveRepository
    {
        private readonly Dictionary<ObjectiveId, ObjectiveModel> _values = new();

        public void Create(Objective value) => _values.Add(value.Id, new(value));

        public void Delete(ObjectiveId id) => _values.Remove(id);

        public bool TryGet(ObjectiveId id, out Objective objective)
        {
            if (_values.TryGetValue(id, out var data) == false)
            {
                objective = default;
                return false;
            }

            objective = new(id, data.Name, data.State);
            return true;
        }

        public IReadOnlyCollection<ObjectiveId> GetAllIds() => _values.Keys;

        public void Update(Objective value) => _values[value.Id] = new(value);
    }

    public sealed class FixedSizeArrayObjectiveMemoryRepository : IObjectiveMemoryRepository
    {
        private readonly Dictionary<ObjectiveId, byte[]> _values = new();

        public void Create(ObjectiveId id, int size) => _values.Add(id, new byte[size]);

        public void Delete(ObjectiveId id) => _values.Remove(id);

        public void Save<T>(ObjectiveId id, T value) where T : unmanaged
        {
            if (_values.TryGetValue(id, out var memory) == false)
            {
                memory = new byte[128];
                _values.Add(id, memory);
            }

            MemoryMarshal.Write(memory, ref value);
        }

        public bool TryGetRawData(ObjectiveId id, out ReadOnlySpan<byte> result)
        {
            if (_values.TryGetValue(id, out var val) == false)
            {
                result = Span<byte>.Empty;
                return false;
            }

            result = val;
            return true;
        }
    }
}
