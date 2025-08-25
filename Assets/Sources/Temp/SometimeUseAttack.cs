using Combat.Common.ValueObjects;
using Combat.Local.Presentation.Units.ViewModels;

using Server.Combat.Networking;

using UnityEngine;

using Zenject;

namespace Assets.Sources.Temp
{
    [RequireComponent(typeof(UnitViewModel))]
    public class SometimeUseAttack : MonoBehaviour
    {
        [SerializeField] private float _cooldown;

        [Inject] private ClientInputReader _clientInputReader;

        private UnitViewModel _unitViewModel;
        private float _cooldownLeft;

        private void Awake()
        {
            _unitViewModel = GetComponent<UnitViewModel>();
            _cooldownLeft = _cooldown;
        }

        private void Update()
        {
            _cooldownLeft -= Time.deltaTime;

            UserInput playerInput = new UserInput(default, default, new ActionInputs(0));

            if (_cooldownLeft <= 0)
            {
                _cooldownLeft += _cooldown;
                playerInput = new UserInput(default, default, new ActionInputs(1));
            }

            if (_unitViewModel == null)
            {
                return;
            }

            EntityId id = _unitViewModel.EntityId;
            Vector3 position = transform.position;
            float rotation = transform.rotation.eulerAngles.y + playerInput.MouseMovement.x;
            Vector3 moveDirection = Quaternion.AngleAxis(rotation, Vector3.up) * new Vector3(playerInput.MoveDirection.x, 0, playerInput.MoveDirection.y);

            InputData inputData = new(new(id.Value), position, rotation, moveDirection, playerInput.Actions);

            _clientInputReader.HandleInputUpdate(inputData);
        }
    }
}
