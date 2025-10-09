using Client.Testing.View;

using Combat.API;
using Combat.Common.Flags;

using DaeHitbox;

using UnityEngine;

namespace Assets.Sources.Common
{
    public partial class SceneInstaller
    {
        private class TestMenuStrategy : ITestMenuStrategy
        {
            //private readonly ICameraController _cameraController;

            private Unit _model;
            //private Temp.UnitViewInputReaderCompenent _readerCompenent;

            public TestMenuStrategy(/*ICameraController cameraController*/)
            {
                //_cameraController = cameraController;
            }

            public void Kill() => _model?.Kill(default);

            public void Resurrect() => _model?.Revive(default);

            public void TakeDamage() => _model?.ApplyDamage(new(null, null, 1, DamageFlags.None));

            public void HalfHealth() => _model?.ApplyDamage(new(null, null, _model.CurrentHealth / 2, DamageFlags.NonReactable));

            public void HealHealth() => _model?.ApplyHealing(new(null, null, _model.MaxHealth, HealingFlags.NonReactable | HealingFlags.CanRevive));

            public void Select()
            {
                UnityEngine.Transform cameraTransform = Camera.main.transform;

                if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, float.PositiveInfinity, (1 << 3), QueryTriggerInteraction.Collide) == false)
                {
                    return;
                }

                //if (hit.collider.TryGetComponent(out Hurtbox hurtbox) == false)
                //{
                //    return;
                //}

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
}
