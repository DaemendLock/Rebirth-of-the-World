using Combat.API.API.Skills;
using Combat.API.Contexts;

namespace Combat.API.API.Tasks
{
    public interface IObjectiveData { }

    public interface IObjectiveContext
    {
        T GetData<T>() where T : unmanaged, IObjectiveData;
        EncounterApi Encounter { get; }
        TQuery GetCapability<TQuery>() where TQuery : class;
    }

    public interface ICombatObjective
    {
        int Id { get; }
        bool IsComplited { get; }
        void Start(ICombatObjective parent, IObjectiveContext startCombatContext);
    }

    public readonly struct DealDamageObjectiveData : IObjectiveData
    {
        public readonly float Target;
    }

    public sealed class DealDamageObjective : ICombatObjective
    {
        private float _targetProgress;
        private float _progress;

        public int Id { get; }

        public bool IsComplited => throw new System.NotImplementedException();

        public void Start(ICombatObjective parent, IObjectiveContext context)
        {
            _targetProgress = context.GetData<DealDamageObjectiveData>().Target;

            var eventSystem = context.GetCapability<IEventContext>();
            eventSystem.SubscribeToEvent<DealDamageEventData>(UpdateProgress);
        }

        private void UpdateProgress(GameEvent<DealDamageEventData> @event)
        {
            _progress += @event.Data.FinalDamage;
        }
    }
}
