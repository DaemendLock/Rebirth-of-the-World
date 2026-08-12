using Combat.API.Contexts;
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

        void Complete();
        void Cancel();
        void Fail();

        void Save<T>(T value) where T : unmanaged, IObjectiveData;
        ObjectiveInfo<T> GetInfo<T>() where T : unmanaged, IObjectiveData;

        EventHandlerId SubscribeToEvent<T>(IEventContext.EventHandler<T> handler) where T : unmanaged, IEventData;
        void Unsubscribe(EventHandlerId id);

        T GetCapability<T>() where T : class;
    }

    public interface ICombatObjective
    {
        void OnStart(IObjectiveContext context);
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
}
