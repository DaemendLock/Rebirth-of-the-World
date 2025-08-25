using Client.Combat.Domain.Units.Components;
using Client.Combat.Presentation.Units.Components;

using UnityEngine;

namespace Client.Combat.Presentation.Implementations.Units
{
    [RequireComponent(typeof(Animator), typeof(CasterView), typeof(Rigidbody))]
    public class MovableView : BindableViewComponent<IPositionOwner>, IMovableView
    {
        private const string AnimatorSpeedName = "Movespeed";
        private Rigidbody _rigidbody;
        private float _baseMass;

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody>();
            _baseMass = _rigidbody.mass;
        }

        private void LateUpdate()
        {
            if (Model == null)
            {
                return;
            }

            transform.localScale = Vector3.one * Model.Scale;
            _rigidbody.linearVelocity = Model.Velocity;
            _rigidbody.mass = _baseMass * Model.Scale * Model.Scale;
        }
    }
}
