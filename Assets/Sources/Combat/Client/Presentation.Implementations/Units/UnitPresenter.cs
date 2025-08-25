using Client.Combat.Presentation.Units;

using Client.Combat.Domain.Units;

using UnityEngine;

namespace Client.Combat.Presentation.Implementations.Units
{
    public class UnitPresenter : BindableViewComponent<IUnit>, IUnitPresenter
    {
        private Rigidbody _rigitbody;

        private void Awake()
        {
            _rigitbody = GetComponent<Rigidbody>();
        }

        public bool OnGround => throw new System.NotImplementedException();

        private void Update()
        {
            if (Model == null)
            {
                return;
            }

            Model.Position = _rigitbody.position;
            Model.Rotation = _rigitbody.rotation.eulerAngles.y;
            Model.Velocity = _rigitbody.linearVelocity;
        }
    }
}