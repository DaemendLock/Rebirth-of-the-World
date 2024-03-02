using Client.Lobby.Core.Characters;

using Data.Utils;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

using Utils.Patterns.Adapters;
using Utils.ThrowHepler;

namespace Assets.Sources.Temp
{
    internal class DataIniter : MonoBehaviour
    {
        public event Action Loaded;

        [SerializeField] private AssetLabelReference _charactersLabel;
        [SerializeField] private AssetLabelReference _itemLabel;

        private readonly List<AsyncOperationHandle> _handlers = new();

        private void Start() => ThrowHepler.ArgumentNullException(_itemLabel, _charactersLabel);

        public async Task<List<Character>> LoadCharacters(Adapter<Character, Data.Characters.Character> adapter)
        {
            List<Character> result = new();

            AsyncOperationHandle<IList<Data.Characters.Character>> loading = Addressables.LoadAssetsAsync<Data.Characters.Character>(_charactersLabel,
                (callback) =>
                {
                    callback.OnLoad();
                    result.Add(adapter.Adapt(callback));
                });

            _ = await loading.Task;

            return result;
        }

        private void LoadItems() => _handlers.Add(Addressables.LoadAssetsAsync<Loadable>(_itemLabel, (callback)
                                                 =>
                                             {
                                                 callback.OnLoad();
                                             }));
    }
}
