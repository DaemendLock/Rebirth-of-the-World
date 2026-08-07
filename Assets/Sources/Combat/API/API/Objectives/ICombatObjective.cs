using Combat.API.Contexts;
using Combat.API.Events;
using Combat.Common.ValueObjects;

using System;

namespace Combat.API.Objectives
{
    public interface IObjectiveData { }

    public struct ObjectiveInfo<T> where T : unmanaged, IObjectiveData
    {
        public ObjectiveInfo(string name, T data)
        {
            Name = name;
            Data = data;
        }

        public string Name { get; }
        public T Data { get; set; }
    }

    public interface IObjectiveContext : IDisposable
    {
        ObjectiveId Id { get; }
        bool IsCompleted { get; }
        ObjectiveInfo<T> GetInfo<T>() where T : unmanaged, IObjectiveData;

        TQuery GetCapability<TQuery>() where TQuery : class;

        void Complete();
        void Save<T>(T value) where T : unmanaged, IObjectiveData;
    }

    public interface ICombatObjective
    {
        void Start(ICombatObjective parent, IObjectiveContext context);
        void OnComplete(IObjectiveContext context);
        void OnCancel(IObjectiveContext context);
    }

    public readonly struct DealDamageObjectiveData : IObjectiveData
    {
        public readonly float Current;
        public readonly float Target;
        public readonly EventHandlerId HandlerId;

        public DealDamageObjectiveData(float current, float target, EventHandlerId handlerId)
        {
            Target = target;
            HandlerId = handlerId;
            Current = current;
        }
    }

    public sealed class DealDamageObjective : ICombatObjective
    {
        private float _targetProgress;
        private float _currentProgress;
        private EventHandlerId _handlerId;

        public void Start(ICombatObjective parent, IObjectiveContext context)
        {
            _targetProgress = context.GetInfo<DealDamageObjectiveData>().Data.Target;

            IEventContext eventSystem = context.GetCapability<IEventContext>();
            _handlerId = eventSystem.Subscribe<DealDamageEventData>(@event => UpdateProgress(@event, context));

            UnityEngine.Debug.Log($"New objective started: Deal damage({_currentProgress}/{_targetProgress})");
        }

        public void OnComplete(IObjectiveContext context)
        {
            UnityEngine.Debug.Log("Quest complete!");
            Cleanup(context);
        }

        public void OnCancel(IObjectiveContext context)
        {
            Cleanup(context);
        }

        private void UpdateProgress(GameEvent<DealDamageEventData> @event, IObjectiveContext context)
        {
            _currentProgress += @event.Data.FinalDamage;

            if (_currentProgress >= _targetProgress)
            {
                context.Complete();
            }
        }

        private void Cleanup(IObjectiveContext context)
        {
            IEventContext eventSystem = context.GetCapability<IEventContext>();
            eventSystem.Unsubscribe(_handlerId);
        }
    }
}
