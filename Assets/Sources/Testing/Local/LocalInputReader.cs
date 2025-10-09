using Combat.Common.ValueObjects;
using Combat.Local.Controllers;

using UnityEngine;

using Zenject;

namespace Assets.Sources.Testing.Local
{
    public class LocalInputReader : MonoBehaviour
    {
        [Inject] private PlayerController _playerController;

        [SerializeField] private int _unitId;

        private void Update()
        {
            EntityId id = new(_unitId);

            if (_playerController == null)
            {
                return;
            }

            Vector2 movement = Vector2.zero;

            if (Input.GetKey(KeyCode.W))
            {
                movement += (Vector2.up);
            }
            if (Input.GetKey(KeyCode.S))
            {
                movement += (Vector2.down);
            }
            if (Input.GetKey(KeyCode.Q))
            {
                movement += (Vector2.left);
            }
            if (Input.GetKey(KeyCode.E))
            {
                movement += (Vector2.right);
            }

            _playerController.MoveInDirection(id, movement);

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _playerController.Cast(id, 0);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _playerController.Cast(id, 1);
            }
        }
    }
}
