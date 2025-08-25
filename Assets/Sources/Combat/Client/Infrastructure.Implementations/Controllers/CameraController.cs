using Client.Combat.Presentation;

using UnityEngine;

namespace Client.Combat.Infrastructure.Implementations.Controllers
{
    public class CameraController : Infrastructure.Controllers.ICameraController
    {
        private readonly ICameraView _view;
        //private readonly CombatInput.CameraActions _cameraInput;
        //private readonly ConfigProvider _configProvider;

        //private bool _enabled;

        public CameraController(ICameraView view)
        {
            _view = view;

            //_cameraInput = input;
            //_configProvider = config;
        }

        public void Follow(Transform target) => _view.Bind(target);

        public void Rotate(Vector2 rotation) => _view.Rotation += rotation;

        //public void Enable()
        //{
        //    if (_enabled)
        //    {
        //        return;
        //    }

        //    _enabled = true;

        //    _cameraInput.MoveCamera.started += ChangeCameraAngle;
        //    _cameraInput.MoveCamera.canceled += ChangeCameraAngle;
        //    _cameraInput.DistantCamera.performed += ChangeCameraDistance;
        //}

        //public void Disable()
        //{
        //    if (_enabled == false)
        //    {
        //        return;
        //    }

        //    _enabled = false;

        //    _cameraInput.MoveCamera.started -= ChangeCameraAngle;
        //    _cameraInput.MoveCamera.canceled -= ChangeCameraAngle;
        //    _cameraInput.DistantCamera.performed -= ChangeCameraDistance;
        //}

        //public void SetDistance(float value)
        //{
        //    //_model.Distance = Math.Clamp(value, _configProvider.MinCameraDistance, _configProvider.MaxCameraDistance);
        //}

        //public void SetAngle(UnityEngine.Vector2 value)
        //{
        //    value.x %= 360;
        //    value.y = Math.Clamp(value.y, -90, 90);
        //    //_model.Rotation = value;
        //}

        //private void ChangeCameraDistance(InputAction.CallbackContext ctx)
        //{
        //    //SetDistance(_model.Distance + ctx.ReadValue<UnityEngine.Vector2>().y * _configProvider.CameraDistanceSensativity);
        //}

        //private void ChangeCameraAngle(InputAction.CallbackContext ctx)
        //{
        //    UnityEngine.Vector2 delta = ctx.ReadValue<UnityEngine.Vector2>() * _configProvider.CameraSensativity;
        //    //SetAngle(_model.Rotation + delta);
        //}
    }
}
