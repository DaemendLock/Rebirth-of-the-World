using Combat.API;
using Combat.API.API.IDK;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Endpoints.Skills;
using Combat.Local.Scripting.Factories;

using System.Collections.Generic;

namespace Combat.Local.Scripting.SkillPorts
{
    public sealed class PropertySkillLyfecycleHandler : ISkillLyfecycleHandler
    {
        private readonly List<ISkillPropertyContainerFactory> _factories;
        private readonly ISkillRuntimeRegistry _runtimeRegistry;

        public PropertySkillLyfecycleHandler(ISkillRuntimeRegistry runtimeRegistry)
        {
            _runtimeRegistry = runtimeRegistry;

            _factories = new();
        }

        public void RegisterStrategyFactory(ISkillPropertyContainerFactory factory)
        {
            _factories.Add(factory);
        }

        public void Give(AbilityKey abilityKey)
        {
            IAbilityPropertyContainer properties = GetFactory(abilityKey.Skill)?.Create(abilityKey.Owner, abilityKey.Skill);
            _runtimeRegistry.Create(abilityKey, properties);
            properties.Give();
        }

        public void Remove(AbilityKey abilityKey)
        {
            if (_runtimeRegistry.TryGet(abilityKey, out var properties) == false)
            {
                return;
            }

            properties.Remove();
            _runtimeRegistry.Remove(abilityKey);
        }

        private ISkillPropertyContainerFactory GetFactory(SkillId id)
        {
            foreach (var factory in _factories)
            {
                if (factory.CanHandle(id) == false)
                {
                    continue;
                }

                return factory;
            }

            return null;
        }
    }
}
