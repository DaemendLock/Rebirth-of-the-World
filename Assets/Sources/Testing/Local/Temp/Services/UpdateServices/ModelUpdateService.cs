using System.Collections.Generic;
using System.Linq;

using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.Services;

using UnityEngine;

namespace Testing.Local.Temp.Services
{
    //public class AttributesEvaluationService : IAttributeEvaluationService
    //{
    //    private readonly IStatusRepository _effectRepository;
    //    private readonly IAttributesRepository _attributesRepository;

    //    private readonly Dictionary<EntityId, CachedAttributes> _cachedValues;

    //    private byte _version;

    //    public AttributesEvaluationService(IStatusRepository effectRepository, IAttributesRepository attributesRepository)
    //    {
    //        _effectRepository = effectRepository;
    //        _attributesRepository = attributesRepository;

    //        _cachedValues = new();
    //        _version = 0;
    //    }

    //    public float GetAttributeValue(EntityId id, Attribute attribute)
    //    {
    //        if (_cachedValues.TryGetValue(id, out CachedAttributes cachedValue) == false)
    //        {
    //            StatsTable memory = new(_attributesRepository.Get(id));
    //            cachedValue = new(memory, _version);
    //        }

    //        if (cachedValue.Version != _version)
    //        {
    //            cachedValue.Values.Clear();
    //            cachedValue.Values.Add(_attributesRepository.Get(id));
    //            _cachedValues.Add(id, new(cachedValue.Values, _version));

    //            IEnumerable<StatusEffect> effects = _effectRepository.FindStatusEffects(id);

    //            foreach (StatusEffect effect in effects)
    //            {
    //                effect.ModifyAttributes(cachedValue.Values);
    //            }
    //        }

    //        return cachedValue.Values[attribute].CalculatedValue;
    //    }

    //    public void ClearCache()
    //    {
    //        _version++;
    //    }

    //    private readonly struct CachedAttributes
    //    {
    //        public CachedAttributes(StatsTable values, int version)
    //        {
    //            Values = values;
    //            Version = version;
    //        }

    //        public StatsTable Values { get; }
    //        public int Version { get; }
    //    }
    //}

    public class ModelUpdateService : IModelUpdateService
    {
        private readonly IHitHandlingService _hitService;
        private readonly IActionRepository _actionRepository;

        private readonly AttributeUpdateService _attributeUpdateService;
        private readonly HealthUpdateService _healthUpdateService;
        private readonly StatusUpdateService _statusUpdateService;
        private readonly StatusApiUpdateService _statusApiUpdateService;

        private readonly Queue<(HitboxId source, HurtboxId target, Vector3 position)> _hits;

        public ModelUpdateService(IActionRepository castActionRepository, AttributeUpdateService attributeUpdateService, HealthUpdateService healthUpdateService, IHitHandlingService hitService, StatusUpdateService statusUpdateService, StatusApiUpdateService statusApiUpdateService)
        {
            _actionRepository = castActionRepository;
            _attributeUpdateService = attributeUpdateService;
            _healthUpdateService = healthUpdateService;

            _hits = new();
            _hitService = hitService;
            _statusUpdateService = statusUpdateService;
            _statusApiUpdateService = statusApiUpdateService;
        }

        public void RegisterHit(HitboxId source, HurtboxId target, Vector3 position) => _hits.Enqueue((source, target, position));

        public void Update(float deltaTime)
        {
            _statusUpdateService.Update(deltaTime);
            _attributeUpdateService.Update();
            //_api.UpdateAttributes();

            _statusApiUpdateService.Update();
            _healthUpdateService.Update();

            foreach (IAction value in _actionRepository.GetAll().ToArray())
            {
                if (value.IsActive == false)
                {
                    _actionRepository.Delete(value.Actor);
                    continue;
                }

                value.ActiveTime += deltaTime;
                //_api.OnActionUpdate(value);
                value.Update();
            }

            lock (_hits)
            {
                while (_hits.Count > 0)
                {
                    var value = _hits.Dequeue();
                    //_api.HandleHit();
                    _hitService.HandleHit(value.source, value.target, value.position);
                }
            }
        }
    }
}
