using Combat.Common;
using Combat.Common.Primitives;
using Combat.Local.Application.DTO;
using Combat.Local.Application.OutputPorts;
using Combat.Local.Application.UseCases;
using Combat.Local.Application.ValueObjects;
using Combat.Local.Presentation.Providers;
using Combat.Local.Presentation.Views;

using DaeAnimator;

using UnityEngine;

namespace Combat.Local.Presentation.Presenters
{
    public class CharacterPresenter : IHealthOutput, IGiveResourceOutput, ISpendResourceOutput, ICharacterConsciousStateOutput, ICharacterLocalTimeScaleOutput, ICharacterMotionOutput, ICharacterScaleOverTimeOutput
    {
        private readonly ITransformProvider _transforms;

        public CharacterPresenter(ITransformProvider transforms)
        {
            _transforms = transforms;
        }

        void IGiveResourceOutput.Present(GiveResourceResult value)
        {
            Debug.Log($"Resource update for {value.Target}: {value.CurrentValue}/{value.MaxValue}");
        }

        void ISpendResourceOutput.Present(Resource value)
        {

            Debug.Log($"Resource update for {value.Id}: {value.CurrentValue}/{value.MaxValue}");
        }

        void IHealthOutput.Present(HealthChangeRecord value)
        {
            Debug.Log($"Health updated for {value.Target}: {value.CurrentHealth}/{value.MaxHealth}");
        }

        void ICharacterConsciousStateOutput.Present(UnitId value, ConsciousState state)
        {
            if (_transforms.TryGet(value, out Transform model) && model.TryGetComponent(out CharacterAnimator animator))
            {
                switch (state)
                {
                    case ConsciousState.Alive:
                        animator.SetConsciousState(true);
                        break;
                    case ConsciousState.Dead:
                        animator.SetConsciousState(false);
                        break;
                }
            }

            Debug.Log($"Conscious State updated for {value}: {state}");
        }

        void IHealthOutput.Present(ComposureChangeRecord value)
        {
            Debug.Log($"Composure updated for {value.Target}: {value.CurrentComposure}/{value.MaxComposure}");
        }

        void IHealthOutput.Present(DamageInstance value)
        {
            Debug.Log($"Damage taken by {value.Target}: {value.HealthDamage} health, {value.ComposureDamage} composure; With {value.Source?.Skill} from {value.Source?.Owner}");
        }

        void ICharacterLocalTimeScaleOutput.Present(UnitId target, float timeScale)
        {
            if (_transforms.TryGet(target, out Transform model) && model.TryGetComponent(out CharacterView view))
            {
                view.TimeScale = timeScale;
            }
        }

        void ICharacterMotionOutput.Present(UnitId target, System.Numerics.Vector3 directionalVelocity, System.Numerics.Vector3 relativeVelocity, System.Numerics.Vector3 desiredVelocity)
        {
            if (_transforms.TryGet(target, out Transform model) && model.TryGetComponent(out PositionableView view))
            {
                view.DirectionalVelocity = new(directionalVelocity.X, directionalVelocity.Y, directionalVelocity.Z);
                view.RelativeVelocity = new(relativeVelocity.X, relativeVelocity.Y, relativeVelocity.Z);
                view.DesiredVelocity = new(desiredVelocity.X, desiredVelocity.Y, desiredVelocity.Z);
            }
        }

        void ICharacterScaleOverTimeOutput.Present(UnitId target, float scaleRate)
        {
            if (_transforms.TryGet(target, out Transform model) && model.TryGetComponent(out ScalableViewComponent view))
            {
                view.ScaleRate = scaleRate;
            }
        }
    }
}
