using Combat.API.Contexts;
using Combat.API.Events;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.OutputPorts.Statuses;
using Combat.Local.Scripting.Capabilities.Statuses;
using Combat.Local.Scripting.Factories;
using Combat.Local.Scripting.IDK;
using Combat.Local.Scripting.Runtime;

using System.Collections.Generic;

namespace Combat.Local.Scripting.Ports.Statuses
{
    public sealed class PropertyStatusLifecycleHandler : IStatusLifecycleHandler
    {
        private readonly List<IStatusRuntimeFactory> _statusStrategyFactories;
        private readonly IStatusRuntimeRegistry _statusRuntimeRegistry;
        private readonly IEventContext _eventContext;

        public PropertyStatusLifecycleHandler(IStatusRuntimeRegistry statusRuntimeRegistry, IEventContext eventContext)
        {
            _statusRuntimeRegistry = statusRuntimeRegistry;
            _statusStrategyFactories = new();
            _eventContext = eventContext;
        }

        public void RegisterStrategyFactory(IStatusRuntimeFactory factory)
        {
            _statusStrategyFactories.Add(factory);
        }

        public void Apply(Status status)
        {
            IStatusRuntimeFactory factory = GetFactory(status.Name);

            if (factory == null)
            {
                UnityEngine.Debug.LogError($"No StatusScript factory found for script <{status.Name}>. Status will be skipped.");
                return;
            }

            StatusRuntime runtime = factory.Create(status.Id, status.Name, status.Parent, status.Source);
            IStatusPropertyContainer container = runtime.Container;

            if (container != null)
            {
                _statusRuntimeRegistry.Create(status.Id, runtime);

                if (container.TryGetProperty(out BaseStatusCapabilties effect))
                {
                    effect.Apply();
                }
            }

            _eventContext.Publish<StatusAppliedEventData>(new(new(status.Id, status.Parent, status.Source)));
        }

        public void Remove(StatusId id)
        {
            if (!_statusRuntimeRegistry.TryGet(id, out StatusRuntime runtime))
            {
                return;
            }

            IStatusPropertyContainer properties = runtime.Container;

            if (properties.TryGetProperty(out BaseStatusCapabilties effect))
            {
                effect.Remove();
            }

            runtime.Context.Cleanup();
            _statusRuntimeRegistry.Remove(id);
            _eventContext.Publish<StatusRemovedEventData>(new(new(id)));
        }

        public void Expire(StatusId id)
        {
            if (!_statusRuntimeRegistry.TryGet(id, out StatusRuntime runtime))
            {
                return;
            }

            IStatusPropertyContainer properties = runtime.Container;

            if (properties.TryGetProperty(out BaseStatusCapabilties effect) == false)
            {
                return;
            }

            effect.Expire();
            _eventContext.Publish<StatusExpiredEventData>(new(new(id)));
        }

        private IStatusRuntimeFactory GetFactory(StatusType name)
        {
            foreach (IStatusRuntimeFactory factory in _statusStrategyFactories)
            {
                if (factory.CanHandle(name) == false)
                {
                    continue;
                }

                return factory;
            }

            return null;
        }
    }

    public sealed class StatusPropertyTickHandler : IStatusTickHandler
    {
        private readonly IStatusRuntimeRegistry _statusRuntimeRegistry;

        public StatusPropertyTickHandler(IStatusRuntimeRegistry statusRuntimeRegistry)
        {
            _statusRuntimeRegistry = statusRuntimeRegistry;
        }

        public void Handle(StatusId statusId)
        {
            if (!_statusRuntimeRegistry.TryGet(statusId, out StatusRuntime runtime))
            {
                return;
            }

            IStatusPropertyContainer properties = runtime.Container;

            if (!properties.TryGetProperty(out BaseStatusCapabilties effect))
            {
                return;
            }

            effect.Tick();
        }
    }
}
