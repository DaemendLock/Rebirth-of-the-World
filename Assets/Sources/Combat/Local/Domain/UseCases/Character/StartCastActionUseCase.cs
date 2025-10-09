using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Domain.UseCases
{
    public interface IActionRepository
    {

    }

    public class StartCastActionUseCase
    {
        private readonly IActorRepository _actorRepository;

        public void Execute(EntityId target, SkillId skillId)
        {
            Actor actor = _actorRepository.Get(target);

            ActionData actionData = new(); 

        }
    }
}
