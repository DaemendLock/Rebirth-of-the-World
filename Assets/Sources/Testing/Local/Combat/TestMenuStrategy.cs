using Assets.Sources.Testing.Local;

using Client.Testing.View;

using Combat.API;
using Combat.API.Adapters;
using Combat.Common.Flags;
using Combat.Local.Controllers;
using Combat.Local.Gateways.Models;

using UnityEngine;

namespace Testing.Local
{
    public class TestMenuStrategy : ITestMenuStrategy
    {
        //private readonly ICameraController _cameraController;

        private readonly LocalInputReader _localInputReader;
        private readonly PlayerController _playerController;
        private readonly ICharacterApiAdapter _chracterApiAdapter;
        private Unit _model;
        //private Temp.UnitViewInputReaderCompenent _readerCompenent;

        public TestMenuStrategy( /*ICameraController cameraController, */PlayerController playerController, ICharacterApiAdapter chracterApiAdapter, LocalInputReader localInputReader)
        {
            _playerController = playerController;
            _chracterApiAdapter = chracterApiAdapter;
            _localInputReader = localInputReader;
            //_cameraController = cameraController;
        }

        public void Kill() => _model?.Kill(default);

        public void Resurrect() => _model?.Revive(default);

        public void TakeDamage() => _model?.ApplyDamage(new(null, null, 1, DamageFlags.None));

        public void HalfHealth() => _model?.ApplyDamage(new(null, null, _model.CurrentHealth / 2, DamageFlags.NonReactable));

        public void HealHealth() => _model?.ApplyHealing(new(_model.MaxHealth, HealingFlags.NonReactable | HealingFlags.CanRevive, null, null));

        public void Select()
        {
            UnityEngine.Transform cameraTransform = Camera.main.transform;

            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, float.PositiveInfinity, (1 << 3), QueryTriggerInteraction.Collide) == false)
            {
                UnityEngine.Debug.Log("Not found");
                return;
            }

            CharacterModelComponent model = hit.collider.GetComponentInParent<CharacterModelComponent>();

            if (model == null)
            {
                UnityEngine.Debug.Log($"Not character: {model}; {hit.collider}");
                return;
            }

            UnityEngine.Debug.Log("Let the darkness take control");
            _playerController.TakeControll(model.Id);
            _model = _chracterApiAdapter.Adaptee(model.Id);

            //if (hurtbox.Owner is not IHurtboxOwner<Unit> hurtboxOwner || hurtboxOwner.Owner == _model)
            //{
            //    return;
            //}

            //if (_readerCompenent != null)
            //{
            //    _readerCompenent.enabled = false;
            //}

            //_model = hurtboxOwner.Owner;

            //if (hit.collider.attachedRigidbody.TryGetComponent(out _readerCompenent))
            //{
            //    _readerCompenent.enabled = true;
            //}

            //_cameraController.Follow(hit.collider.attachedRigidbody.transform);
        }
    }
}
