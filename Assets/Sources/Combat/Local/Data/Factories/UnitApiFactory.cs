using Combat.API;
using Combat.API.Controllers.Factories;
using Combat.Common.ValueObjects;
using Combat.Local.Controllers;

namespace Combat.Local.Data.Factories
{
    public class UnitApiFactory : IUnitApiFactory
    {
        private readonly CharacterController _characterController;
        private readonly HealthOwnerController _healthOwnerController;
        private readonly AttributeOwnerController _attributeOwnerController;

        public UnitApiFactory(CharacterController characterController, HealthOwnerController healthOwnerController, AttributeOwnerController attributeOwnerController)
        {
            _characterController = characterController;
            _healthOwnerController = healthOwnerController;
            _attributeOwnerController = attributeOwnerController;
        }

        public Unit Create(EntityId id)
        {
            return new(id,
            _characterController,
            _healthOwnerController,
            _attributeOwnerController);
        }
    }
}
