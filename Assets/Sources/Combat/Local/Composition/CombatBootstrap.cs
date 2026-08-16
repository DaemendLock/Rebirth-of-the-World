using Combat.Common.Primitives;
using Combat.Local.Controllers;
using Combat.Local.Domain.Repositories;
using Combat.Local.Gateways.DataSources;

using Global.Local.DTO;

using System;

using UnityEngine;
using UnityEngine.SceneManagement;

using Zenject;

namespace Combat.Local.Composition
{
    public interface ILocationSceneLoader
    {
        void Load(string locationName, Action<Scene> completed);
    }

    public sealed class ZenjectLocationSceneLoader : ILocationSceneLoader
    {
        private readonly ZenjectSceneLoader _sceneLoader;

        public ZenjectLocationSceneLoader(ZenjectSceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public void Load(string locationName, Action<Scene> completed)
        {
            if (string.IsNullOrWhiteSpace(locationName))
            {
                throw new ArgumentException("A location scene is required.", nameof(locationName));
            }

            AsyncOperation operation = _sceneLoader.LoadSceneAsync(locationName, LoadSceneMode.Additive);

            operation.completed += _ =>
            {
                Scene locationScene = SceneManager.GetSceneByName(locationName);

                if (locationScene.IsValid() == false || locationScene.isLoaded == false)
                {
                    throw new InvalidOperationException(
                        $"Location scene '{locationName}' did not load successfully.");
                }

                SceneManager.SetActiveScene(locationScene);
                completed?.Invoke(locationScene);
            };
        }
    }

    public sealed class CombatBootstrap : IInitializable
    {
        private readonly StartCombatRequest _request;
        private readonly ILocationSceneLoader _locationLoader;
        private readonly ISceneObjectDataSource _sceneObjects;
        private readonly EncounterController _encounterController;

        public CombatBootstrap(StartCombatRequest request, ILocationSceneLoader locationLoader, ISceneObjectDataSource sceneObjects, EncounterController encounterController)
        {
            _request = request;
            _locationLoader = locationLoader;
            _sceneObjects = sceneObjects;
            _encounterController = encounterController;
        }

        public void Initialize()
        {
            _locationLoader.Load(_request.LocationName, OnLocationLoaded);
        }

        private void OnLocationLoaded(Scene locationScene)
        {
            _sceneObjects.Scene = locationScene;
            var spawnpoint = _sceneObjects.GetSpawnpoints();
            int spawnpointCursor = 0;

            foreach (var item in _request.Characters)
            {
                UnitId character = _encounterController.CreateCharacter(item.Character, new(item.TeamId), spawnpoint[spawnpointCursor++].Position);
            }

            _encounterController.Start();
        }
    }
}
