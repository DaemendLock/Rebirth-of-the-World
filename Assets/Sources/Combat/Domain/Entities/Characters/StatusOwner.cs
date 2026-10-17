using System;
using System.Collections.Generic;

using Combat.Common.Primitives;
using Combat.Local.Application.ValueObjects;

namespace Combat.Domain.Entities
{
    public readonly struct StatusOwner
    {
        private readonly List<StatusInstance> _values;

        public StatusOwner(UnitId id)
        {
            Id = id;
            _values = new();
        }

        public UnitId Id { get; }

        public int Count => _values.Count;

        public void Add(StatusInstance instance) => _values.Add(instance);

        public bool Remove(StatusId id) => _values.RemoveAll(value => value.Id == id) > 0;

        public bool TryGet(StatusId id, out StatusInstance instance)
        {
            int index = GetIndex(id);
            instance = index >= 0 ? _values[index] : default;
            return index >= 0;
        }

        public void SetDuration(StatusId id, float duration)
        {
            int index = GetIndex(id);
            _values[index] = _values[index].WithDuration(new(0, duration));
        }

        public void ExtendDuration(StatusId id, float duration)
        {
            int index = GetIndex(id);
            var current = _values[index].Duration;
            _values[index] = _values[index].WithDuration(new(current.ActiveTime, current.FullDuration + duration));
        }

        public void SetStackCount(StatusId id, int stackCount)
        {
            int index = GetIndex(id);
            _values[index] = _values[index].WithStackCount(stackCount);
        }

        public void ProgressAll(float deltaTime, Span<StatusUpdateResult> progressResults)
        {
            if (progressResults.Length < Count) throw new ArgumentException("The progress buffer is too small.", nameof(progressResults));

            for (int i = 0; i < _values.Count; i++)
            {
                StatusInstance instance = _values[i];

                if (instance.Duration.Left <= 0)
                {
                    progressResults[i] = new(instance.Id, false, true);
                    continue;
                }

                instance = instance.Progress(deltaTime);
                _values[i] = instance;
                progressResults[i] = new(instance.Id, instance.Duration.Left <= 0, false);
            }
        }

        public IReadOnlyCollection<StatusInstance> GetAll() => _values;

        private int GetIndex(StatusId id)
        {
            for (int i = 0; i < _values.Count; i++)
            {
                if (_values[i].Id == id) return i;
            }

            return -1;
        }
    }
}
