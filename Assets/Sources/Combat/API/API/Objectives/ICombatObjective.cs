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
        ObjectiveState State { get; }

        void Save<T>(T value) where T : unmanaged, IObjectiveData;
        ObjectiveInfo<T> GetInfo<T>() where T : unmanaged, IObjectiveData;

        void Complete();
        void Cancel();
        void Fail();

        EventHandlerId SubscribeToEvent<T>(IEventContext.EventHandler<T> handler) where T : unmanaged, IEventData;
        void Unsubscribe(EventHandlerId id);
    }

    public interface ICombatObjective
    {
        void OnStart(ICombatObjective parent, IObjectiveContext context);
        void OnComplete(IObjectiveContext context) { }
        void OnCancel(IObjectiveContext context) { }
        void OnFail(IObjectiveContext context) { }
    }

    public readonly struct DealDamageObjectiveData : IObjectiveData
    {
        public readonly float Current;
        public readonly float Target;

        public DealDamageObjectiveData(float current, float target)
        {
            Target = target;
            Current = current;
        }
    }

    public sealed class DealDamageObjective : ICombatObjective
    {
        private float _targetProgress;
        private float _currentProgress;

        public void OnStart(ICombatObjective parent, IObjectiveContext context)
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
