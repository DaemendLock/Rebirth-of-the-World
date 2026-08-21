using Combat.API.Contexts;
using Combat.Common.Primitives;
using Combat.Local.Domain.Endpoints.Skills;
using Combat.Local.Domain.ValueObjects;
using Combat.Local.Scripting.Capabilities.Skills;
using Combat.Local.Scripting.Runtime;

using System.Collections.Generic;

namespace Combat.Local.Scripting.SkillPorts
{
    public sealed class PropertySkillHitHandler : ISkillHitHandler
    {
        private readonly ISkillRuntimeRegistry _runtimeRegistry;
        private readonly Dictionary<AbilityKey, List<UnitId>> _hittedTargets;

        public PropertySkillHitHandler(ISkillRuntimeRegistry runtimeRegistry)
        {
            _runtimeRegistry = runtimeRegistry;
            _hittedTargets = new();
        }

        public void HandleHit(AbilityKey abilityKey, HitRecord hitRecord)
        {
            if (_runtimeRegistry.TryGet(abilityKey, out var skillRuntime) == false)
            {
                return;
            }

            var handler = skillRuntime.Container.GetCapability<ISkillHitCapability>();

            if (handler == null)
            {
                return;
            }

            if (_hittedTargets.TryGetValue(abilityKey, out List<UnitId> targets) == false)
            {
                targets = new();
                _hittedTargets[abilityKey] = targets;
            }

            HandleHit(skillRuntime.Context, skillRuntime.CastContexts.Current, handler, targets, hitRecord);
        }

        public void Reset(AbilityKey abilityKey)
        {
            if (_hittedTargets.TryGetValue(abilityKey, out List<UnitId> values))
            {
                values.Clear();
            }
        }

        private static void HandleHit(ISkillContext context, ICastContext castContext, ISkillHitCapability handler, List<UnitId> hittedTargets, in HitRecord record)
        {
            try
            {
                if (hittedTargets.Contains(record.HurtboxOwner))
                {
                    return;
                }

                hittedTargets.Add(record.HurtboxOwner);
                handler.Handle(context, castContext, record);
            }
            catch (System.Exception exception)
            {
                UnityEngine.Debug.LogException(exception);
            }
        }
    }
}
