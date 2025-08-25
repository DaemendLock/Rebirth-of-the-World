using Client.Combat.Presentation.Units;

using UnityEngine;

namespace Client.Combat.Infrastructure.Controllers
{
    public interface ICameraController
    {
        void Follow(Transform target);

        void Rotate(Vector2 rotation);

        //void Enable();

        //void Disable();

        //void SetDistance(float distance);

        //void SetAngle(UnityEngine.Vector2 rotation);
    }
}
