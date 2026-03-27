using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Domain.UseCases;
using Combat.Local.Presentation.Components;
using Combat.Local.Presentation.Units.ViewModels;

using UnityEngine;

namespace Combat.Local.Presentation.Presenters
{
    public interface IActionAnimationProvider
    {
        AnimationClip GetAnimation(ActionId id);
    }

    public interface ICharacterViewContainer
    {
        bool TryGetValue(EntityId entityId, out Transform result);
    }

    public class CharacterPresenter : IHealthOutput, IGiveResourceOutput, ISpendResourceOutput, IMovementOutput, IActionOutput, ICharacterConsciousStateOutput
    {
        private readonly ICharacterViewContainer _container;
        private readonly IActionAnimationProvider _actionAnimationProvider;

        public CharacterPresenter(ICharacterViewContainer container, IActionAnimationProvider skillAnimationRepository)
        {
            _container = container;
            _actionAnimationProvider = skillAnimationRepository;
        }

        void IActionOutput.Present(Actor actor)
        {
            if (_container.TryGetValue(actor.Id, out var view) == false)
            {
                return;
            }

            if (actor.CurrentAction == null)
            {
                view.GetComponent<CasterView>().StopCast();
                return;
            }

            AnimationClip clip = _actionAnimationProvider.GetAnimation(actor.CurrentAction.Id);

            ActivityViewModel skillViewModel = new(clip, 0, 0);

            //TODO: wth is this?
            view.GetComponent<CasterView>().DisplayAction(skillViewModel);
        }

        void IGiveResourceOutput.Present(GiveResourceResult value)
        {
            Debug.Log($"Resource update for {value.Target}: {value.CurrentValue}/{value.MaxValue}");
        }

        void ISpendResourceOutput.Present(Resource value)
        {

            Debug.Log($"Resource update for {value.Id}: {value.CurrentValue}/{value.MaxValue}");
        }

        void IHealthOutput.Present(Health value)
        {
            Debug.Log($"Health updated for {value.Id}: {value.CurrentHealth}/{value.MaxHealth}");
        }

        public void SetHorizontalVelocity(EntityId target, Vector3 velocity)
        {
            if (_container.TryGetValue(target, out var view) == false)
            {
                return;
            }

            Rigidbody rigidbody = view.GetComponent<Rigidbody>();
            rigidbody.linearVelocity = new(velocity.x, rigidbody.linearVelocity.y, velocity.z);
        }

        void ICharacterConsciousStateOutput.Present(EntityId value, ConsciousState state)
        {
            if (_container.TryGetValue(value, out var view) == false)
            {
                return;
            }

            if (state == ConsciousState.Alive)
            {
                view.GetComponent<KillableView>().Revive();
                return;
            }

            if (state == ConsciousState.Dead)
            {
                view.GetComponent<KillableView>().Kill();
                return;
            }
        }
    }
}