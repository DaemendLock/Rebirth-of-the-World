using UnityEngine;
using View.Combat.UI;
using View.Combat.UI.Nameplates;

namespace Adapters.Combat
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
            UIRoot.Instance?.DisplayUnit(SelectionInfo.SelectionId);
            NameplatesRoot.Instance?.ShowSelelction(SelectionInfo.SelectionId);
        }

        public static void HandleTargetChange()
        {
            UIRoot.Instance?.DisplayTarget(SelectionInfo.SelectionId);
            NameplatesRoot.Instance?.ShowTarget(SelectionInfo.SelectionId);
        }
    }
}
