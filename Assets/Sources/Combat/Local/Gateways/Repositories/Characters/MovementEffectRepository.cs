using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Repositories;
using Combat.Local.Gateways.DataSources;
using Combat.Local.Gateways.Models;

namespace Combat.Local.Gateways.Repositories.Characters
{
    public class MovementEffectRepository : IMovementEffectRepository
    {
        private readonly ICharacterModelDataSource _characterModelDataSource;

        public MovementEffectRepository(ICharacterModelDataSource characterModelDataSource)
        {
            _characterModelDataSource = characterModelDataSource;
        }

        public void Create(MoveInDirectionEffect value)
        {
            if (_characterModelDataSource.TryGetMovementContainer(value.Target, out IMovementEffectContainer container) == false)
            {
                throw new System.InvalidOperationException();
            }

            container.AddEffect(value);
        }

        public void Delete(EntityId target, TransformEffectId id)
        {
            if (_characterModelDataSource.TryGetMovementContainer(target, out IMovementEffectContainer container) == false)
            {
                throw new System.InvalidOperationException();
            }

            container.RemoveEffect(id);
        }

        public void Get(TransformEffectId id) => throw new System.NotImplementedException();
    }
}
