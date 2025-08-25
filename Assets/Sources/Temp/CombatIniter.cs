using Assets.Sources.Common;

using Client.Combat.Infrastructure.Providers;
using Client.Combat.Presentation;
using Client.Combat.Presentation.UI;
using Client.Combat.Presentation.UI.Nameplates;

using UnityEngine;

namespace Temp.Testing
{
    internal class CombatIniter : MonoBehaviour
    {
        private const int Port = 27123;

        [SerializeField] private CameraView _mainCamera;
        [SerializeField] private UIRoot _uiRoot;
        [SerializeField] private NameplatesRoot _nameplatesRoot;
        [SerializeField] private ConfigProvider _configProvider;
        [SerializeField] private AssetProvider _assetProvider;

        private void Start()
        {
        }
    }
}
