using Client.Combat.Domain.Units;
using Client.Combat.Infrastructure.Controllers;
using Client.Combat.Presentation.Implementations.Units;
using Client.Combat.Presentation.Units;

using UnityEngine;

namespace Client.Combat.Infrastructure.Implementations.Controllers
{
    public class UnitController : IUnitController
    {
        private readonly IUnit _model;
        private readonly IUnitPresenter _unitPresenter;
        private readonly Rigidbody _rigidbody;

        public UnitController(IUnit model, GameObject parent)
        {
            _model = model;
            _rigidbody = parent.GetComponent<Rigidbody>();

            (parent.GetComponent<UnitPresenter>() ?? parent.AddComponent<UnitPresenter>()).Bind(_model);
            (parent.GetComponent<CasterView>() ?? parent.AddComponent<CasterView>()).Bind(_model);
            (parent.GetComponent<KillableView>() ?? parent.AddComponent<KillableView>()).Bind(_model);
            (parent.GetComponent<MovableView>() ?? parent.AddComponent<MovableView>()).Bind(_model);
            (parent.GetComponent<HurtboxOwner>() ?? parent.AddComponent<HurtboxOwner>()).Bind(_model);
        }

        public void Update()
        {
            _model.Velocity = _rigidbody.linearVelocity;
            _model.Position = _rigidbody.position;
            _model.Rotation = _rigidbody.rotation.eulerAngles.y;
        }
    }
}
