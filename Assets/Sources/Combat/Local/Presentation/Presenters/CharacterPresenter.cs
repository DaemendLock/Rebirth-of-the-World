using Combat.Common;
using Combat.Common.Primitives;
using Combat.Local.Application.DTO;
using Combat.Local.Application.OutputPorts;
using Combat.Local.Application.UseCases;
using Combat.Local.Application.ValueObjects;

using UnityEngine;

namespace Combat.Local.Presentation.Presenters
{
    public class CharacterPresenter : IHealthOutput, IGiveResourceOutput, ISpendResourceOutput, ICharacterConsciousStateOutput
    {
        public CharacterPresenter()
        {
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
    }
}
