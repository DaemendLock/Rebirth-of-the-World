using Input;
using Input.Combat;
using System;
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

        [SerializeField] private Transform _targetPostion;
        
        private UnityEngine.Camera _camera;

        public static Quaternion CameraRotation { get; private set; }

        private void Start()
        {
            _camera = GetComponent<UnityEngine.Camera>();
            SellectionInfo.SelectionChanged += FollowTarget;

            if(SellectionInfo.ContolledUnitId != -1)
            {
                FollowTarget(SellectionInfo.ContolledUnitId);
            }
        }

        private void OnEnable()
        {
            MovementInputHandler.CameraRotationChanged += Rotate;
            MovementInputHandler.CameraDistanceChanged += Distant;
        }

        private void OnDisable()
        {
            MovementInputHandler.CameraRotationChanged -= Rotate;
        }

        private void LateUpdate()
        {
            transform.position = _targetPostion.position - transform.forward * _cameraDistance + _cameraHeight * Vector3.up;
        }

        private void Rotate(Vector2 rotation)
        {
            CameraRotation = Quaternion.Euler(new(-rotation.y, rotation.x));
            transform.rotation = CameraRotation;
        }

        private void Distant(float delta)
        {
            _cameraDistance = Math.Clamp(_cameraDistance + delta * _cameraDistanceSensativity, _minCameraDistance, _maxCameraDistance);
        }

        private void FollowTarget(int unitId)
        {
            _targetPostion = Units.Unit.GetUnit(unitId).transform;
        }
    }
}
