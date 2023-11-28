using Core.Combat.Units;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace View.Combat.Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float _cameraSensativity;
        [SerializeField] private float _cameraDistance;
        [SerializeField] private float _cameraHeight;

        [SerializeField] private float _minCameraDistance = 0;
        [SerializeField] private float _maxCameraDistance = 10;
        [SerializeField] private float _cameraDistanceSensativity;

        private Unit _followTarget;

        private UnityEngine.Camera _camera;

        public Quaternion CameraRotation
        {
            get => transform.rotation;
            set => transform.rotation = value;
        }

        public float CameraDistance
        {
            get { return _cameraDistance; }
        }

        private void Start()
        {
            _camera = GetComponent<UnityEngine.Camera>();
        }

        private void LateUpdate()
        {
            if (_followTarget == null)
            {
                return;
            }

            Vector3 position = new Vector3(_followTarget.Position.x, _followTarget.Position.y, _followTarget.Position.z);
            transform.position = position - transform.forward * _cameraDistance + _cameraHeight * Vector3.up;
        }

        public void FollowTarget(Unit target)
        {
            _followTarget = target;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetDistance(float delta)
        {
            _cameraDistance = Math.Clamp(_cameraDistance + delta * _cameraDistanceSensativity, _minCameraDistance, _maxCameraDistance);
        }
    }
}
