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

        void Save<T>(T value) where T : unmanaged, IObjectiveData;
        ObjectiveInfo<T> GetInfo<T>() where T : unmanaged, IObjectiveData;

        void Complete();

        EventHandlerId SubscribeToEvent<T>(IEventContext.EventHandler<T> handler) where T : unmanaged, IEventData;
        void Unsubscribe(EventHandlerId id);
    }

    public interface ICombatObjective
    {
        void OnStart(ICombatObjective parent, IObjectiveContext context);
        void OnComplete(IObjectiveContext context);
        void OnCancel(IObjectiveContext context);
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
            _targetProgress = context.GetInfo<DealDamageObjectiveData>().Data.Target;

            context.SubscribeToEvent<DealDamageEventData>(@event => UpdateProgress(@event, context));

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
            var info = context.GetInfo<DealDamageObjectiveData>();

            var data = info.Data;
            data = new(data.Current + @event.Data.FinalDamage, data.Target);
            info.Data = data;
            _currentProgress += @event.Data.FinalDamage;

            if (_currentProgress >= _targetProgress)
            {
                context.Complete();
            }
        }

        private void Cleanup(IObjectiveContext context)
        {
            
        }
    }
}
