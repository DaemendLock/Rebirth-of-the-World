using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Statuses;
using Combat.Local.Domain.ValueObjects;

using System.Collections.Generic;

namespace Combat.Local.Domain.Factories
{
    public interface IStatusStrategyFactory
    {
        bool CanHandle(StatusName statusName);

        IStatusStrategy Create(StatusId id, StatusName name, EntityId parent);
    }

    public interface IStatusFactory
    {
        void RegisterStrategyFactory(IStatusStrategyFactory factory);
        Status Create(StatusName name, EntityId parent, float duration, int stackCount, EventSource source);
    }

    public class StatusFactory : IStatusFactory
    {
        private readonly List<IStatusStrategyFactory> _statusStrategyFactories;
        private int _nextId = 0;

        public StatusFactory()
        {
            _statusStrategyFactories = new();
        }

        public void RegisterStrategyFactory(IStatusStrategyFactory factory)
        {
            _statusStrategyFactories.Add(factory);
        }

        public Status Create(StatusName name, EntityId parentId, float duration, int stackCount, EventSource source)
        {
            StatusId nextId = new(_nextId++);
            IStatusStrategy statusStrategy = GetFactory(name)?.Create(nextId, name, parentId);
            return new(nextId, parentId, name, source, stackCount, new(0, duration), statusStrategy);
        }

        private IStatusStrategyFactory GetFactory(StatusName name)
        {
            foreach (IStatusStrategyFactory factory in _statusStrategyFactories)
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
