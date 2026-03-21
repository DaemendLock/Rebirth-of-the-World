using Combat.Common.ValueObjects;
using Combat.Local.Domain.Facades;

using System.Collections.Generic;

namespace Combat.API.Controllers
{
    public class ChracterApiAdapter
    {
        private readonly CharacterFacade _characterFacade;
        private readonly HealthOwnerFacade _healthOwnerFacade;
        private readonly AttributeOwnerFacade _attributeOwnerFacade;

        private readonly Dictionary<EntityId, Unit> _cache;

        public ChracterApiAdapter(CharacterFacade characterFacade, HealthOwnerFacade healthFacade, AttributeOwnerFacade attributeOwnerFacade)
        {
            _characterFacade = characterFacade;
            _healthOwnerFacade = healthFacade;
            _attributeOwnerFacade = attributeOwnerFacade;

            _cache = new();
        }

        public Unit Adaptee(EntityId id)
        {
            if (_cache.TryGetValue(id, out Unit result))
            {
                return result;
            }

            result = new(id,
            _characterFacade,
            _healthOwnerFacade,
            _attributeOwnerFacade);
            _cache.Add(id, result);
            return result;
        }
    }
}
