using Combat.Local.Data.Databases;
using Combat.Local.Domain.UseCases;
using Combat.Local.Domain.UseCases.Scene;

using Zenject;

namespace Testing.Local
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
    public class UpdateController : ITickable
    {
        private readonly PrecacheAttributersUseCase _updateCombatUseCase;
        private readonly UpdateStatusTimersUseCase _updateStatusTimersUseCase;
        private readonly UpdateStatusesUseCases _updateStatusesUseCase;
        private readonly UpdateActorsUseCase _updateActorsUseCase;
        private readonly HandleHitUseCase _handleHitUseCase;

        private readonly StatusModificationProvider _statusModificationProvider;

        public UpdateController(PrecacheAttributersUseCase updateCombatUseCase, UpdateStatusTimersUseCase updateStatusTimersUseCase, StatusModificationProvider statusModificationProvider, UpdateStatusesUseCases updateStatusesUseCase, UpdateActorsUseCase updateActionUseCase, HandleHitUseCase handleHitUseCase)
        {
            _updateCombatUseCase = updateCombatUseCase;
            _updateStatusTimersUseCase = updateStatusTimersUseCase;
            _statusModificationProvider = statusModificationProvider;
            _updateStatusesUseCase = updateStatusesUseCase;
            _updateActorsUseCase = updateActionUseCase;
            _handleHitUseCase = handleHitUseCase;
        }

        public void Tick()
        {
            float deltaTime = UnityEngine.Time.deltaTime;

            _updateCombatUseCase.Execute(deltaTime);
            _updateStatusesUseCase.Execute(deltaTime);
            _updateStatusTimersUseCase.Execute(deltaTime);
            _updateActorsUseCase.Execute(deltaTime);
            _handleHitUseCase.Execute();

            _statusModificationProvider.ClearCache();
        }
    }
}
