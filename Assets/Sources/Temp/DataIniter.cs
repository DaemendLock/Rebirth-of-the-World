using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

using Data.Utils;
using Data.Characters;
using Utils.ThrowHepler;

namespace Assets.Sources.Temp
{
    internal class DataIniter : MonoBehaviour
    {
        public event Action Loaded;

        [SerializeField] private AssetLabelReference _charactersLabel;
        [SerializeField] private AssetLabelReference _itemLabel;

        private readonly List<AsyncOperationHandle> _handlers = new();

        private void Start()
        {
            ThrowHepler.ArgumentNullException(_itemLabel, _charactersLabel);
            LoadCharacters();
        }

        public Task<IList<Character>> LoadCharacters()
        {
            AsyncOperationHandle<IList<Character>> loading = Addressables.LoadAssetsAsync<Character>(_charactersLabel,
                (callback) =>
                {
                    callback.OnLoad();
                });
            return loading.Task;
        }

        private void LoadItems() => _handlers.Add(Addressables.LoadAssetsAsync<Loadable>(_itemLabel, (callback)
                                                 =>
                                             {
                                                 callback.OnLoad();
                                             }));
    }
}
