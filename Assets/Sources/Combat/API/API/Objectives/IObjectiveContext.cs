using Combat.API.Contexts;
using Combat.API.Events;
using Combat.Common.ValueObjects;

using System;

namespace Combat.API.Objectives
{
    public interface IObjectiveContext : IDisposable
    {
        ObjectiveId Id { get; }

        void Complete();
        void Cancel();
        void Fail();

        void Save<T>(T value) where T : unmanaged, IObjectiveData;
        ObjectiveInfo<T> GetInfo<T>() where T : unmanaged, IObjectiveData;

        EventHandlerId SubscribeToEvent<T>(IEventContext.EventHandler<T> handler) where T : unmanaged, IEventData;
        void Unsubscribe(EventHandlerId id);

        T GetCapability<T>() where T : class;
    }
}
