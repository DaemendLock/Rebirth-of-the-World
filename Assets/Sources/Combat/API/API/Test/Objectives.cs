using Combat.API.Contexts;
using Combat.API.Events;
using Combat.Common.ValueObjects;

namespace Combat.API.Objectives
{
    public sealed class BasicKillUnitObjective : ICombatObjective
    {
        private readonly struct KillTagret : IObjectiveData
        {
            public readonly UnitId Target;

            public KillTagret(UnitId target)
            {
                Target = target;
            }
        }

        public void OnStart(IObjectiveContext context)
        {
            Unit unit = context.GetCapability<IEncounterContext>().CreateUnit(new()
            {
                ModelName = new("Katerina"),
                Team = new(2),
                Position = UnityEngine.Vector3.zero,
                BaseHealth = 2000
            });

            context.Save<KillTagret>(new(unit.Id));

            context.SubscribeToEvent<UnitDiedEventData>(value => HandleKill(value, context));
        }

        public void OnComplete(IObjectiveContext context)
        {
            UnityEngine.Debug.Log("Kill quest complete!");
        }

        private void HandleKill(GameEvent<UnitDiedEventData> @event, IObjectiveContext context)
        {
            UnitId target = context.GetInfo<KillTagret>().Data.Target;

            if (@event.Data.Victim != target)
            {
                return;
            }

            context.Complete();
        }
    }

    public sealed class DealDamageObjective : ICombatObjective
    {
        private float _targetProgress;
        private float _currentProgress;

        public void OnStart(IObjectiveContext context)
        {
            _currentProgress = 0;
            _targetProgress = 1000;
            context.Save<DealDamageObjectiveData>(new(_currentProgress, _targetProgress));

            context.SubscribeToEvent<DealDamageEventData>(@event => UpdateProgress(@event, context));

            UnityEngine.Debug.Log($"New objective started: Deal damage({_currentProgress}/{_targetProgress})");
        }

        public void OnComplete(IObjectiveContext context)
        {
            UnityEngine.Debug.Log("Quest complete!");
        }

        private void UpdateProgress(GameEvent<DealDamageEventData> @event, IObjectiveContext context)
        {
            _currentProgress += @event.Data.FinalDamage;
            context.Save(new DealDamageObjectiveData(_currentProgress, _targetProgress));

            if (_currentProgress >= _targetProgress)
            {
                context.Complete();
            }
        }
    }
}
