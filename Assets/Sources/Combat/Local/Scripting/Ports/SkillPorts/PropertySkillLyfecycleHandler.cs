using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Local.Application.Endpoints.Skills;
using Combat.Local.Scripting.Capabilities.Skills;
using Combat.Local.Scripting.Factories;
using Combat.Local.Scripting.Runtime;

using System.Collections.Generic;

namespace Combat.Local.Scripting.SkillPorts
{
    public sealed class PropertySkillLyfecycleHandler : ISkillLyfecycleHandler
    {
        private readonly List<ISkillRuntimeFactory> _factories;
        private readonly ISkillRuntimeRegistry _skillRuntimeRegistry;
        private readonly ICastRuntimeRegistry _castRuntimeRegistry;

        public PropertySkillLyfecycleHandler(ISkillRuntimeRegistry runtimeRegistry, ICastRuntimeRegistry castRuntimeRegistry)
        {
            _skillRuntimeRegistry = runtimeRegistry;
            _factories = new();
            _castRuntimeRegistry = castRuntimeRegistry;
        }

        public void RegisterStrategyFactory(ISkillRuntimeFactory factory)
        {
            _factories.Add(factory);
        }

        public void Give(AbilityKey abilityKey)
        {
            SkillRuntime? properties = GetFactory(abilityKey.Skill)?.Create(abilityKey);

            if (properties.HasValue == false)
            {
                throw new System.InvalidOperationException($"No skill property factory can handle skill '{abilityKey.Skill}'.");
            }

            _skillRuntimeRegistry.Create(abilityKey, properties.Value);
        }

        public void Remove(AbilityKey abilityKey)
        {
            _skillRuntimeRegistry.Remove(abilityKey);
            _castRuntimeRegistry.Remove(abilityKey);
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
        private readonly ICastRuntimeRegistry _castRuntimeRegistry;
        private readonly ISkillRuntimeRegistry _skillRuntimeRegistry;

        public PropertySkillActionStateChangeHandler(ICastRuntimeRegistry runtimeRegistry, ISkillRuntimeRegistry skillRuntimeRegistry)
        {
            _castRuntimeRegistry = runtimeRegistry;
            _skillRuntimeRegistry = skillRuntimeRegistry;
        }

        public void Handle(AbilityKey abilityKey, ActionState newState)
        {
            if (_castRuntimeRegistry.TryGet(abilityKey, out CastRuntime properties) == false)
            {
                return;
            }

            if (_skillRuntimeRegistry.TryGet(abilityKey, out SkillRuntime skillRuntime) == false)
            {
                return;
            }

            properties.Container.GetCapability<ISkillHandleActionCapability>()?.HandlePhaseChange(properties.Context, skillRuntime.Context, newState);

            if (newState == ActionState.Inactive)
            {
                _castRuntimeRegistry.Remove(abilityKey);
            }
        }
    }
}
