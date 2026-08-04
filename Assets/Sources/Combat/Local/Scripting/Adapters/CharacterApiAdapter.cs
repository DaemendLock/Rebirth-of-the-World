using Combat.Common.ValueObjects;
using Combat.Local.Domain.Facades;
using Combat.Local.Scripting;

using System.Collections.Generic;

namespace Combat.API.Adapters
{
    public sealed class UnitNewAdapter
    {
        private readonly HealthOwnerFacade _healthOwnerFacade;

        public UnitNewAdapter(HealthOwnerFacade healthOwnerFacade)
        {
            _healthOwnerFacade = healthOwnerFacade;
        }

        public UnitNew Adaptee(UnitId id)
        {
            return new UnitNew(id, _healthOwnerFacade);
        }
    } 

    public readonly struct CharacterApiAdapter : ICharacterApiAdapter
    {
        private readonly CharacterFacade _characterFacade;
        private readonly HealthOwnerFacade _healthOwnerFacade;
        private readonly AttributeOwnerFacade _attributeOwnerFacade;

        private readonly Dictionary<UnitId, OldDomainUnitContext> _cache;

        public CharacterApiAdapter(CharacterFacade characterFacade, HealthOwnerFacade healthFacade, AttributeOwnerFacade attributeOwnerFacade)
        {
            _characterFacade = characterFacade;
            _healthOwnerFacade = healthFacade;
            _attributeOwnerFacade = attributeOwnerFacade;

            _cache = new();
        }

        public Unit Adaptee(UnitId id)
        {
            if (_cache.TryGetValue(id, out OldDomainUnitContext result))
            {
                return new(result);
            }

            result = new(id,
            _characterFacade,
            _healthOwnerFacade,
            _attributeOwnerFacade);
            _cache.Add(id, result);
            return new(result);
        }
    }
}
