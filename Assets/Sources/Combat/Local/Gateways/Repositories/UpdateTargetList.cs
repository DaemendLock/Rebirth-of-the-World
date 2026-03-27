using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Repositories;
using Combat.Local.Gateways.DataSources;

using System.Collections.Generic;

namespace Combat.Local.Data.Repositories
{
    public class UpdateTargetList : ICharacterUpdateList
    {
        private readonly Dictionary<EntityId, Updatable> _values;
        private readonly ICharacterModelDataSource _sceneCharacterModelDataSource;

        public UpdateTargetList(ICharacterModelDataSource sceneCharacterModelDataSource)
        {
            _values = new();
            _sceneCharacterModelDataSource = sceneCharacterModelDataSource;
        }

        public void Create(Updatable value)
        {
            _values.Add(value.Id, value);

            if (_sceneCharacterModelDataSource.TryGetCharacterModel(value.Id, out var model))
            {
                model.TimeScale = value.TimeScale;
            }
        }

        public void Update(Updatable value)
        {
            _values[value.Id] = value;

            if (_sceneCharacterModelDataSource.TryGetCharacterModel(value.Id, out var model))
            {
                model.TimeScale = value.TimeScale;
            }
        }

        public Updatable Get(EntityId id) => _values[id];

        public IReadOnlyCollection<Updatable> GetAll() => _values.Values;

        public void Delete(EntityId id) => _values.Remove(id);
    }
}
