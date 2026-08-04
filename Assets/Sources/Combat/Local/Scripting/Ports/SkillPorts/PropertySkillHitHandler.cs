using Combat.API;
using Combat.API.Adapters;
using Combat.API.API.IDK;
using Combat.API.Skills;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Endpoints.Skills;
using Combat.Local.Domain.ValueObjects;

using System.Collections.Generic;

namespace Combat.Local.Scripting.SkillPorts
{
    public sealed class PropertySkillHitHandler : ISkillHitHandler
    {
        private readonly ISkillRuntimeRegistry _runtimeRegistry;
        private readonly ICharacterApiAdapter _characterApiAdapter;

        private readonly Dictionary<AbilityKey, List<UnitId>> _hittedTargets;

        public PropertySkillHitHandler(ISkillRuntimeRegistry runtimeRegistry, ICharacterApiAdapter characterApiAdapter)
        {
            _runtimeRegistry = runtimeRegistry;
            _characterApiAdapter = characterApiAdapter;

            _hittedTargets = new();
        }

        public void HandleHits(AbilityKey abilityKey, Queue<HitRecord> hitRecords)
        {
            if (_runtimeRegistry.TryGet(abilityKey, out var properties) == false)
            {
                return;
            }

            if (properties.TryGet(out IHitHandler handler) == false)
            {
                return;
            }

            while (hitRecords.TryDequeue(out HitRecord hitRecord))
            {
                if (_hittedTargets.TryGetValue(abilityKey, out var values) == false)
                {
                    values = new();
                    _hittedTargets[abilityKey] = values;
                }

                HandleHit(handler, values, hitRecord);
            }
        }

        public void Reset(AbilityKey abilityKey)
        {
            if (_hittedTargets.TryGetValue(abilityKey, out var values))
            {
                values.Clear();
            }
        }

        private void HandleHit(IHitHandler handler, List<UnitId> hittedTargets, HitRecord record)
        {
            try
            {
                if (hittedTargets.Contains(record.HurtboxOwner))
                {
                    return;
                }

                hittedTargets.Add(record.HurtboxOwner);
                Combat.API.DTO.HitRecord @event = Parse(record);
                handler.OnHit(@event);
            }
            catch (System.Exception exception)
            {
                UnityEngine.Debug.LogException(exception);
            }
        }

        private Combat.API.DTO.HitRecord Parse(HitRecord hitRecord)
        {
            Unit source = _characterApiAdapter.Adaptee(hitRecord.HitboxOwner);
            Unit target = _characterApiAdapter.Adaptee(hitRecord.HurtboxOwner);
            return new(source, hitRecord.HitboxType, target, hitRecord.HurtboxType, hitRecord.Location);
        }
    }
}
