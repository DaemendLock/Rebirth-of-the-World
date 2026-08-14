using Combat.Common.ValueObjects;
using Combat.Common.Flags;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Gateways.DataSources;
using Combat.Local.Gateways.Models;

using DaeAnimator;

using System.Collections.Generic;

using UnityEngine;

namespace Combat.Local.Gateways.Repositories.Characters
{
    public sealed class ActorRepository : IActorRepository
    {
        private readonly Dictionary<UnitId, ActorModelComponent> _values;
        private readonly IActionDataContainer _dataSource;
        private readonly ISceneObjectDataSource _sceneObjectDataSource;

        public ActorRepository(IActionDataContainer dataSource, ISceneObjectDataSource sceneObjectDataSource)
        {
            _values = new();
            _dataSource = dataSource;
            _sceneObjectDataSource = sceneObjectDataSource;
        }

        public void Create(Actor value)
        {
            if (_sceneObjectDataSource.TryGet(value.Id, out Transform parent) == false)
            {
                throw new System.InvalidOperationException();
            }

            ActorModelComponent result = parent.gameObject.AddComponent<ActorModelComponent>();
            result.State = value.State;
            result.ConsciousState = value.ConsciousState;
            result.Action = value.CurrentAction;
            result.DesiredActions = value.DesiredActions;

            _values[value.Id] = result;
        }

        public void Update(Actor value)
        {
            if (_values.TryGetValue(value.Id, out var model) == false)
            {
                throw new System.InvalidOperationException();
            }

            model.State = value.State;
            model.DesiredActions = value.DesiredActions;

            if (value.ConsciousState != model.ConsciousState)
                model.ConsciousState = value.ConsciousState;

            if (ReferenceEquals(value.CurrentAction, model.Action) == false)
                UpdateAction(value.Id, value.CurrentAction);

            UpdateActionState(value.Id, value.CurrentAction);
            model.Action = value.CurrentAction;
        }

        public bool TryGet(UnitId id, out Actor result)
        {
            if (_values.TryGetValue(id, out ActorModelComponent data) == false)
            {
                result = default;
                return false;
            }

            result = new(id, data.State, data.Action, data.ConsciousState, data.DesiredActions);
            return true;
        }

        public void Delete(UnitId id) => _values.Remove(id);

        private void UpdateAction(UnitId actor, Action action)
        {
            if (_sceneObjectDataSource.TryGet(actor, out var view) == false)
            {
                return;
            }

            CharacterAnimator casterView = view.GetComponent<CharacterAnimator>();

            if (action == null)
            {
                casterView.StopCastAnimation();
                return;
            }

            ActionData data = _dataSource.Get(action.Id);

            if (data.Animation == null)
            {
                return;
            }

            bool isHoldable = action.Flags.HasFlag(ActionFlags.Holdable);
            CharacterAnimation animation = isHoldable
                ? new(
                    data.Animation,
                    0,
                    0,
                    true,
                    data.FrameData.LastActiveEnterTime,
                    data.FrameData.LastActiveExitTime)
                : new(data.Animation, 0, 0, false);

            casterView.PlayCastAnimation(animation, action.ActiveTime);
        }

        private void UpdateActionState(UnitId actor, Action action)
        {
            if (action == null || action.Flags.HasFlag(ActionFlags.Holdable) == false)
            {
                return;
            }

            if (action.TryGet(out IAbilityAction abilityAction) == false ||
                abilityAction.State != ActionState.Recovery)
            {
                return;
            }

            if (_sceneObjectDataSource.TryGet(actor, out var view))
            {
                view.GetComponent<CharacterAnimator>().ReleaseCastAnimation();
            }
        }
    }
}
