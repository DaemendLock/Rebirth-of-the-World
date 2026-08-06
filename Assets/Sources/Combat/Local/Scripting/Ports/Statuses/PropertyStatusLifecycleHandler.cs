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
        private readonly List<IStatusPropertyContainerFactory> _statusStrategyFactories;
        private readonly IStatusRuntimeRegistry _statusRuntimeRegistry;

        public PropertyStatusLifecycleHandler(IStatusRuntimeRegistry statusRuntimeRegistry)
        {
            _statusRuntimeRegistry = statusRuntimeRegistry;
            _statusStrategyFactories = new();
        }

        public void RegisterStrategyFactory(IStatusPropertyContainerFactory factory)
        {
            _statusStrategyFactories.Add(factory);
        }

        public void Apply(Status status)
        {
            IStatusPropertyContainerFactory factory = GetFactory(status.Name);

            if (factory == null)
            {
                UnityEngine.Debug.LogError($"No StatusScript factory found for script <{status.Name}>. Status will be skipped.");
                return;
            }

            IStatusPropertyContainer container = factory.Create(status.Id, status.Name, status.Parent, status.Source);
            _statusRuntimeRegistry.Create(status.Id, container);

            if (!container.TryGetProperty(out BaseStatusCapabilties effect))
            {
                return;
            }

            effect.Apply();
        }

        public void Cleanup(StatusId id)
        {
            if (!_statusRuntimeRegistry.TryGet(id, out IStatusPropertyContainer properties))
            {
                return;
            }

            if (properties.TryGetProperty(out BaseStatusCapabilties effect))
            {
                effect.Remove();
            }

            _statusRuntimeRegistry.Remove(id);
        }

        public void Expire(StatusId id)
        {
            if (!_statusRuntimeRegistry.TryGet(id, out IStatusPropertyContainer properties))
            {
                return;
            }

            if (!properties.TryGetProperty(out BaseStatusCapabilties effect))
            {
                return;
            }

            effect.Expire();
        }

        private IStatusPropertyContainerFactory GetFactory(StatusType name)
        {
            foreach (IStatusPropertyContainerFactory factory in _statusStrategyFactories)
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
            if (!_statusRuntimeRegistry.TryGet(statusId, out IStatusPropertyContainer properties))
            {
                return;
            }

            if (!properties.TryGetProperty(out BaseStatusCapabilties effect))
            {
                return;
            }

            effect.Tick();
        }
    }
}
