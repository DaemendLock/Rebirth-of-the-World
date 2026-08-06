using Combat.API.API.IDK;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Endpoints.Skills;
using Combat.Local.Domain.ValueObjects;
using Combat.Local.Scripting.Idk.Capabilities.Skills;

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

        public void HandleHits(AbilityKey abilityKey, Queue<HitRecord> hitRecords)
        {
            if (_runtimeRegistry.TryGet(abilityKey, out var properties) == false)
            {
                return;
            }

            if (properties.TryGet(out HandleSkillHitCapability handler) == false)
            {
                return;
            }

            if (_hittedTargets.TryGetValue(abilityKey, out List<UnitId> targets) == false)
            {
                targets = new();
                _hittedTargets[abilityKey] = targets;
            }

            while (hitRecords.TryDequeue(out HitRecord hitRecord))
            {
                HandleHit(handler, targets, hitRecord);
            }
        }

        public void Reset(AbilityKey abilityKey)
        {
            if (_hittedTargets.TryGetValue(abilityKey, out List<UnitId> values))
            {
                values.Clear();
            }
        }

        private static void HandleHit(HandleSkillHitCapability handler, List<UnitId> hittedTargets, in HitRecord record)
        {
            try
            {
                if (hittedTargets.Contains(record.HurtboxOwner))
                {
                    return;
                }

                hittedTargets.Add(record.HurtboxOwner);
                handler.Handle(record);
            }
            catch (System.Exception exception)
            {
                UnityEngine.Debug.LogException(exception);
            }
        }
    }
}
