using Combat.Common.Primitives;
using Combat.Domain.Entities.Units;
using Combat.Local.Application.Repositories;
using Combat.Local.Gateways.DataSources;
using Combat.Local.Gateways.Models;

using System.Collections.Generic;
using System.Linq;

namespace Combat.Local.Data.Repositories
{
    public sealed class CharacterDeleteQueue : ICharacterDeleteQueue
    {
        private readonly Queue<UnitId> _values = new();

        public void Enqueue(UnitId unitId)
        {
            if (_values.Contains(unitId)) return;
            _values.Enqueue(unitId);
        }

        public bool TryDequeue(out UnitId unitId) => _values.TryDequeue(out unitId);
    }

    public readonly struct UpdatableModel
    {
        public UpdatableModel(Updatable value)
        {
            TimeScale = value.TimeScale;
            IsDead = false;
        }

        public UpdatableModel(float timeScale, bool isDead)
        {
            TimeScale = timeScale;
            IsDead = isDead;
        }

        public float TimeScale { get; }
        public bool IsDead { get; }

        public Updatable Parse(UnitId id) => new(id, TimeScale);
    }

    public class UpdateTargetRepository : ICharacterUpdateQueue
    {
        private readonly Dictionary<UnitId, UpdatableModel> _values;
        private readonly ISceneObjectDataSource _sceneCharacterModelDataSource;

        private bool _hasPendingDeletes;

        public UpdateTargetRepository(ISceneObjectDataSource sceneCharacterModelDataSource)
        {
            _values = new();
            _sceneCharacterModelDataSource = sceneCharacterModelDataSource;
        }

        public void Create(Updatable value)
        {
            _values.Add(value.Id, new(value));
            SetViewTimescale(value.Id, value.TimeScale);
        }

        public void Update(Updatable value)
        {
            _values[value.Id] = new(value);
            SetViewTimescale(value.Id, value.TimeScale);
        }

        public IEnumerable<Updatable> GetAll()
        {
            if (_hasPendingDeletes)
            {
                var removeTargets = _values.Where(value => value.Value.IsDead).ToArray();

                foreach (var target in removeTargets)
                {
                    _values.Remove(target.Key);
                }

                _hasPendingDeletes = false;
            }

            return _values.Select(value => value.Value.Parse(value.Key));
        }

        public void Delete(UnitId id)
        {
            if (_values.TryGetValue(id, out var target) == false)
            {
                return;
            }

            target = new(target.TimeScale, true);
            _values[id] = target;
            _hasPendingDeletes = true;
        }

        private void SetViewTimescale(UnitId id, float scale)
        {
            if (_sceneCharacterModelDataSource.TryGet(id, out var model))
            {
                model.GetComponent<CharacterModelComponent>().TimeScale = scale;

                if (model.TryGetComponent(out DaeAnimator.CharacterAnimator animator))
                {
                    animator.SetTimeScale(scale);
                }
            }
        }
    }
}
