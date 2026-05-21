using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Statuses;

using System.Collections.Generic;

namespace Combat.Local.Domain.Factories
{
    public interface IStatusPropertyContainerFactory
    {
        bool CanHandle(StatusType statusName);

        IStatusPropertyContainer Create(StatusId id, StatusType name, UnitId parent, AbilityKey? source);
    }

    public class StatusFactory
    {
        private readonly List<IStatusPropertyContainerFactory> _statusStrategyFactories;
        private int _nextId = 0;

        public StatusFactory()
        {
            _statusStrategyFactories = new();
        }

        public void RegisterStrategyFactory(IStatusPropertyContainerFactory factory)
        {
            _statusStrategyFactories.Add(factory);
        }

        public Status Create(StatusType name, UnitId parentId, float duration, int stackCount, AbilityKey? source)
        {
            StatusId nextId = new(_nextId++);
            IStatusPropertyContainerFactory factory = GetFactory(name);

            IStatusPropertyContainer container = GetFactory(name)?.Create(nextId, name, parentId, source);
            return new(nextId, parentId, name, source, stackCount, new(0, duration), container);
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
