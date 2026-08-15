using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Domain.UseCases;
using Combat.Local.Domain.ValueObjects;

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

        void IHealthOutput.Present(Health value)
        {
            Debug.Log($"Health updated for {value.Id}: {value.CurrentValue}/{value.MaxHealth}");
        }

        void ICharacterConsciousStateOutput.Present(UnitId value, ConsciousState state)
        {
            Debug.Log($"Conscious State updated for {value}: {state}");
        }

        void IHealthOutput.Present(DamageInstance value)
        {
            Debug.Log($"Damage taken by {value.Target} {value.Damage}; With {value.Source?.Skill} from {value.Source?.Owner}");
        }
    }
}