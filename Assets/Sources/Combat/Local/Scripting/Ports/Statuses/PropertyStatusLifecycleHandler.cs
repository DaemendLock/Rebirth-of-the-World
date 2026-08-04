using Combat.API.API.IDK;
using Combat.Common.ValueObjects;
using Combat.Local.API.IDK;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.OutputPorts.Statuses;
using Combat.Local.Scripting.Factories;

using System.Collections.Generic;

namespace Combat.Local.Scripting.Ports.Statuses
{
    public sealed class PropertyStatusLifecycleHandler : IStatusLifecycleHandler
    {
        private readonly List<IStatusPropertyContainerFactory> _statusStrategyFactories;
        private readonly IStatusRuntimeRegistry _statusRuntimeRegistry;

        public void RegisterStrategyFactory(IStatusPropertyContainerFactory factory)
        {
            _statusStrategyFactories.Add(factory);
        }

        public void Apply(Status status)
        {
            IStatusPropertyContainerFactory factory = GetFactory(status.Name);
            IStatusPropertyContainer container = factory?.Create(status.Id, status.Name, status.Parent, status.Source);
            _statusRuntimeRegistry.Create(status.Id, container);
            container.Apply();
        }

        public void Cleanup(StatusId id)
        {
            if (_statusRuntimeRegistry.TryGet(id, out var properties))
            {
                properties.Remove();
            }

            _statusRuntimeRegistry.Remove(id);
        }

        public void Expire(StatusId status)
        {
            if (_statusRuntimeRegistry.TryGet(status, out var properties))
            {
                properties.Expire();
            }
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
}
