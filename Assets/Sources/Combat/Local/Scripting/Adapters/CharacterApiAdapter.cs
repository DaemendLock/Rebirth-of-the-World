using Combat.API;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Facades;

namespace Combat.Local.Scripting.Adapters
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

    public sealed class CharacterApiAdapter
    {
        private readonly CharacterFacade _characterFacade;
        private readonly HealthOwnerFacade _healthOwnerFacade;
        private readonly AttributeOwnerFacade _attributeOwnerFacade;

        public CharacterApiAdapter(CharacterFacade characterFacade, HealthOwnerFacade healthFacade, AttributeOwnerFacade attributeOwnerFacade)
        {
            _characterFacade = characterFacade;
            _healthOwnerFacade = healthFacade;
            _attributeOwnerFacade = attributeOwnerFacade;
        }

        public Unit Adaptee(UnitId id)
        {
            OldDomainUnitContext result = new(id,
            _characterFacade,
            _healthOwnerFacade,
            _attributeOwnerFacade);
            return new(result);
        }
    }
}
