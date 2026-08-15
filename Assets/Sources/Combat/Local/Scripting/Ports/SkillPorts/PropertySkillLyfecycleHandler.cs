using Combat.Common.ValueObjects;
using Combat.Local.Domain.Endpoints.Skills;
using Combat.Local.Scripting.Factories;
using Combat.Local.Scripting.IDK;

using System.Collections.Generic;
using Combat.Local.Scripting.Runtime;
using Combat.Local.Scripting.Capabilities.Skills;

namespace Combat.Local.Scripting.SkillPorts
{
    public sealed class PropertySkillLyfecycleHandler : ISkillLyfecycleHandler
    {
        private readonly List<ISkillRuntimeFactory> _factories;
        private readonly ISkillRuntimeRegistry _runtimeRegistry;

        public PropertySkillLyfecycleHandler(ISkillRuntimeRegistry runtimeRegistry)
        {
            _runtimeRegistry = runtimeRegistry;
            _factories = new();
        }

        public void RegisterStrategyFactory(ISkillRuntimeFactory factory)
        {
            _factories.Add(factory);
        }

        public void Give(AbilityKey abilityKey)
        {
            SkillRuntime? properties = GetFactory(abilityKey.Skill)?.Create(abilityKey.Owner, abilityKey.Skill);

            if (properties.HasValue == false)
            {
                throw new System.InvalidOperationException($"No skill property factory can handle skill '{abilityKey.Skill}'.");
            }

            _runtimeRegistry.Create(abilityKey, properties.Value);
        }

        public void Remove(AbilityKey abilityKey)
        {
            if (_runtimeRegistry.TryGet(abilityKey, out var properties) == false)
            {
                return;
            }

            properties.Context?.Cleanup();
            _runtimeRegistry.Remove(abilityKey);
        }

        private ISkillRuntimeFactory GetFactory(SkillId id)
        {
            foreach (ISkillRuntimeFactory factory in _factories)
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
            if (_runtimeRegistry.TryGet(abilityKey, out SkillRuntime properties) == false)
            {
                return;
            }

            properties.Container.GetCapability<ISkillHandleActionStateChangeCapability>()?.Handle(properties.Context, newState);
        }
    }
}
