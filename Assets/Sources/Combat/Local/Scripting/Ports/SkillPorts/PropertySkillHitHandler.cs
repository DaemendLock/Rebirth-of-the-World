using Combat.API.Contexts;
using Combat.Common.Primitives;
using Combat.Local.Application.DTO;
using Combat.Local.Application.Endpoints.Skills;
using Combat.Local.Scripting.Capabilities.Skills;
using Combat.Local.Scripting.Runtime;

using System.Collections.Generic;

namespace Combat.Local.Scripting.SkillPorts
{
    public sealed class PropertySkillHitHandler : ISkillHitHandler
    {
        private readonly ISkillRuntimeRegistry _runtimeRegistry;
        private readonly ICastRuntimeRegistry _castRegistry;
        private readonly Dictionary<AbilityKey, List<UnitId>> _hittedTargets;

        public PropertySkillHitHandler(ISkillRuntimeRegistry runtimeRegistry, ICastRuntimeRegistry castRegistry)
        {
            _runtimeRegistry = runtimeRegistry;
            _hittedTargets = new();
            _castRegistry = castRegistry;
        }

        public void HandleHit(AbilityKey abilityKey, HitRecord hitRecord)
        {
            if (_castRegistry.TryGet(abilityKey, out var castRuntime) == false)
            {
                return;
            }

            if (_runtimeRegistry.TryGet(abilityKey, out var skillRuntime) == false)
            {
                return;
            }

            var handler = castRuntime.Container.GetCapability<ISkillHitCapability>();

            if (handler == null)
            {
                return;
            }

            if (_hittedTargets.TryGetValue(abilityKey, out List<UnitId> targets) == false)
            {
                targets = new();
                _hittedTargets[abilityKey] = targets;
            }

            HandleHit(skillRuntime.Context, castRuntime.Context, handler, targets, hitRecord);
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
