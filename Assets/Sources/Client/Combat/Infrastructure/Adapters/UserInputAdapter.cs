using Adapters.Combat;
using Client.Combat.View.UI;
using Client.Combat.View.UI.Nameplates;
using UnityEngine;

namespace Client.Combat.Infrastructure.Adapters
{
    public static class UserInputAdapter
    {
        public static void ChangeCameraDistance(float delta)
        {
            if (UIRoot.Instance == null)
            {
                return;
            }

            UIRoot.Instance.CameraDistance = -delta / 100;
        }

        public static void HandleCameraAngleChange(Vector2 rotation)
        {
            if (UIRoot.Instance == null)
            {
                return;
            }

            UIRoot.Instance.CameraRotation = Quaternion.Euler(new(-rotation.y, rotation.x));
        }

        public static void HandleSelectionChange()
        {
            //TODO: UIRoot.Instance?.DisplayUnit(SelectionInfo.SelectionId);
            NameplatesRoot.Instance?.ShowSelelction(SelectionInfo.SelectionId);
        }

        public static void HandleTargetChange()
        {
            //TODO: UIRoot.Instance?.DisplayTarget(SelectionInfo.TargetId);
            NameplatesRoot.Instance?.ShowTarget(SelectionInfo.TargetId);
        }
    }
}
