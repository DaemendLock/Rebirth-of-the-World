using Combat.Local.Domain.Entities;
using Combat.Local.Gateways.DataSources;
using Combat.Local.Gateways.Models.Encounter;

using Lobby.Common.Primitives;

using UnityEngine.SceneManagement;

namespace Combat.Local.Gateways.Repositories.Encounter
{
    public interface ILocationDataSource
    {
        Scene Load(LocationId location);
    }

    public sealed class EncounterProvider
    {
        private readonly ILocationDataSource _locationDataSource;
        private readonly ISceneObjectDataSource _sceneObjectDataSource;

        private EncounterModel _model;

        public void Create(Domain.Entities.Encounter encounter)
        {
            if (_model != null)
            {
                throw new System.InvalidOperationException();
            }

            Scene scene = _locationDataSource.Load(encounter.Location.Id);
            Spawnpoint[] spawnpoints;

            _sceneObjectDataSource.Scene = scene;
            //_model = new(encounter, scene);

            //_model.Scene = scene;
        }

        public void Delete()
        {
            if (_model == null)
            {
                return;
            }

            EncounterModel model = _model;

            _model = null;

            SceneManager.UnloadSceneAsync(model.Scene);
        }

        private async void CreateInternal()
        {

        }
    }
}
