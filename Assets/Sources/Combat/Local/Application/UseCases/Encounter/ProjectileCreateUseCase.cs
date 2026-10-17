using Combat.Common.Primitives;
using Combat.Local.Application.Entities;
using Combat.Local.Application.Repositories;

using UnityEngine;

namespace Combat.Local.Application.UseCases.Encounter
{
    public readonly struct CreateCharacterDTO
    {
        public readonly ModelName ModelName;
        public readonly Vector3 InitialPosition;

    }

    public sealed class ProjectileCreateUseCase
    {
        private readonly IProjectileRepository _projectileRepository;

        public void Execute(CreateCharacterDTO dto)
        {
            Projectile projectile = new();
            _projectileRepository.Create(projectile);
        }
    }

    public interface IProjectileCreateOutput
    {
        void Present(Projectile projectile);
    }
}
