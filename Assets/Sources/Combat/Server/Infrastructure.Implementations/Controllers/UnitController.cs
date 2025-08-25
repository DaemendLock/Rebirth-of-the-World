using System.Collections.Generic;

using DaeHitbox;

using Server.Combat.Domain.Entities;
using Server.Combat.Infrastructure.Controllers;

using UnityEngine;

namespace Server.Combat.Infrastructure.Implementations.Controllers
{
    public class UnitController : IUnitController
    {
        //private readonly Dictionary<int, IAction> _actions;
        private readonly Unit _model;

        public UnitController(Unit model)
        {
            _model = model;

            //_actions = new();
        }

        public void PerformAction(int slot)
        {
            //if (_actions.TryGetValue(slot, out IAction action) == false)
            //{
            //    return;
            //}

            //if (action.CanPerformBy(_model) == false)
            //{
            //    return;
            //}

            //action.PerformBy(_model);
        }

        //public void SetAction(int actionSlot, IAction action) => _actions[actionSlot] = action;

        public void TryMove(Vector3 position, float rotation, Vector3 moveDirection)
        {
            //if (_model.CanMove() == false)
            //{
            //    return;
            //}

            //if (moveDirection.sqrMagnitude > 1)
            //{
            //    moveDirection = moveDirection.normalized;
            //}

            //_model.Position = position;
            //_model.Velocity = moveDirection * _model.GetAttributeValue(Domain.Attributes.Attribute.Speed) * _model.Scale + _model.Velocity.y * Vector3.up;
            //_model.Rotation = rotation;
        }

        public void Equip()
        {

        }

        public void HandleHit(HitEvent @event)
        {
            //Model.OnHit(@event);
        }
    }
}
