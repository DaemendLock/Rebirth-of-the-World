using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Repositories;
using Combat.Local.Gateways.DataSources;
using Combat.Local.Gateways.Models;

using System.Collections.Generic;

namespace Combat.Local.Data.Repositories
{
    public class UpdateTargetRepository : ICharacterUpdateRepository
    {
        private readonly Dictionary<UnitId, Updatable> _values;
        private readonly ISceneObjectDataSource _sceneCharacterModelDataSource;

        public UpdateTargetRepository(ISceneObjectDataSource sceneCharacterModelDataSource)
        {
            _values = new();
            _sceneCharacterModelDataSource = sceneCharacterModelDataSource;
        }

        public void Create(Updatable value)
        {
            _values.Add(value.Id, value);

            if (_sceneCharacterModelDataSource.TryGet(value.Id, out var model))
            {
                model.GetComponent<CharacterModelComponent>().TimeScale = value.TimeScale;

                if (model.TryGetComponent(out DaeAnimator.CharacterAnimator animator))
                {
                    animator.SetTimeScale(value.TimeScale);
                }
            }
        }

        public void Update(Updatable value)
        {
            _values[value.Id] = value;

            if (_sceneCharacterModelDataSource.TryGet(value.Id, out var model))
            {
                model.GetComponent<CharacterModelComponent>().TimeScale = value.TimeScale;

                if (model.TryGetComponent(out DaeAnimator.CharacterAnimator animator))
                {
                    animator.SetTimeScale(value.TimeScale);
                }
            }
        }

        public Updatable Get(UnitId id) => _values[id];

        public bool TryGet(UnitId id, out Updatable updatable) => _values.TryGetValue(id, out updatable);

        public IReadOnlyCollection<Updatable> GetAll() => _values.Values;

        public void Delete(UnitId id) => _values.Remove(id);
    }
}
