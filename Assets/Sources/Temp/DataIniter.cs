using Data.Utils;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utils.ThrowHepler;

namespace Assets.Sources.Temp
{
    internal class DataIniter : MonoBehaviour
    {
        [SerializeField] private AssetLabelReference _charactersLabel;
        [SerializeField] private AssetLabelReference _itemLabel;

        private void Start()
        {
            ThrowHepler.ArgumentNullException(_itemLabel, _charactersLabel);

            LoadSpellLib();
            LoadCharacters();
            LoadItems();
            //ItemLib.LoadAllData();
            //ModelLib.LoadAllData();
            //SpriteLib.LoadAllData();
        }

        private static void LoadSpellLib()
        {
        }

        private void LoadCharacters()
        {
            Addressables.LoadAssetsAsync<Loadable>(_charactersLabel, (callback)
                =>
            {
                callback.OnLoad();
            });

        }

        private void LoadItems()
        {
            Addressables.LoadAssetsAsync<Loadable>(_itemLabel, (callback)
                =>
            {
                callback.OnLoad();
            });

        }
    }
}
