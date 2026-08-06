using Combat.API.API.IDK;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Endpoints.Skills;
using Combat.Local.Scripting.Factories;
using Combat.Local.Scripting.IDK;
using Combat.Local.Scripting.Idk.Capabilities.Skills;

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

            if (properties == null)
            {
                throw new System.InvalidOperationException($"No skill property factory can handle skill '{abilityKey.Skill}'.");
            }

            _runtimeRegistry.Create(abilityKey, properties);
        }

        public void Remove(AbilityKey abilityKey)
        {
            if (_runtimeRegistry.TryGet(abilityKey, out var properties) == false)
            {
                return;
            }

            _runtimeRegistry.Remove(abilityKey);
        }

        private ISkillPropertyContainerFactory GetFactory(SkillId id)
        {
            foreach (ISkillPropertyContainerFactory factory in _factories)
            {
                if (factory.CanHandle(id))
                {
                    return factory;
                }
            }

            return null;
        }
    }

    public sealed class PropertySkillActionStateChangeHandler : ISkillActionStateChangeHandler
    {
        private readonly ISkillRuntimeRegistry _runtimeRegistry;

        public PropertySkillActionStateChangeHandler(ISkillRuntimeRegistry runtimeRegistry)
        {
            _runtimeRegistry = runtimeRegistry;
        }

        public void Handle(AbilityKey abilityKey, ActionState newState)
        {
            if (_runtimeRegistry.TryGet(abilityKey, out IAbilityPropertyContainer properties) == false ||
                properties.TryGet(out HandleSkillActionStateChangeCapability handler) == false)
            {
                return;
            }

            handler.Handle(newState);
        }
    }
}
