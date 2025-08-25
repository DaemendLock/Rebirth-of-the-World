using Combat.Common.ValueObjects;
using Combat.Local.Domain.API.IDK;
using Combat.Local.Domain.API.Skills;

namespace Combat.Local.Domain.API
{
    public class Scene
    {
        private readonly UnitApiRepository _unitApiRepository;

        public Scene(UnitApiRepository unitApiRepository)
        {
            _unitApiRepository = unitApiRepository;
        }

        public object? CreateProjectile(object from, object speed, IHitHandler hitHandler)
        {
            throw new System.NotImplementedException();
        }

        public Unit CreateUnit(ModelName id) => throw new System.NotImplementedException();

        public Unit GetUnit(EntityId id) => _unitApiRepository.Get(id);

        public void RemoveUnit(EntityId id) => throw new System.NotImplementedException();
    }
}
