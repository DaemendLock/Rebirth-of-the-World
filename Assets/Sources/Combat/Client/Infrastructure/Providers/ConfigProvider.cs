using UnityEngine;

namespace Client.Combat.Infrastructure.Providers
{
    public class ConfigProvider : MonoBehaviour
    {
        private float _cameraSensativity;

        private float _minCameraDistance = 0;
        private float _maxCameraDistance = 10;

        [field: SerializeField] public float CameraDistanceSensativity { get; private set; }
        [field: SerializeField] public float CameraSensativity { get; private set; }

        [field: SerializeField] public float MinCameraDistance { get; private set; }
        [field: SerializeField] public float MaxCameraDistance { get; private set; }
    }
}
