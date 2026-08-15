using Combat.Local.Gateways.Repositories.Encounter;

using Data.Levels;

using Lobby.Common.Primitives;

using System;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Local.Combat.LazyData
{
    public sealed class LazyEnviromentDataSource : MonoBehaviour, ILocationDataSource
    {
        [SerializeField] private UnitySceneData[] _values;

        public Scene Load(LocationId locationId)
        {
            string sceneName = GetName(locationId);

            if (string.IsNullOrEmpty(sceneName))
            {
                throw new InvalidOperationException();
            }

            Scene result = SceneManager.LoadScene(sceneName, new LoadSceneParameters(LoadSceneMode.Additive));
            var val = FindObjectsByType<PlayerSpawnpoint>(FindObjectsSortMode.None);
            return result;
        }

        private string GetName(LocationId unitySceneId)
        {
            foreach (var value in _values)
            {
                if (value.Id != unitySceneId.Value)
                {
                    continue;
                }

                return value.Scene;
            }

            return null;
        }
    }

    [Serializable]
    public sealed class UnitySceneData
    {
        public int Id;
        public string Scene;
    }
}
