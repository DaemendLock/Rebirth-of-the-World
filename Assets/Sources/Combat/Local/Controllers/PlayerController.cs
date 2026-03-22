using Combat.Common.ValueObjects;
using Combat.Local.Domain.UseCases;

using UnityEngine;

namespace Combat.Local.Controllers
{
    public class PlayerController
    {
        private readonly CastSkillFromSlotUseCase _castSkillFromSlotUseCase;
        private readonly MoveInDirectionUseCase _moveUseCase;
        private readonly AssumeControllOverCharacterUseCase _assumeControllOverCharacterUseCase;

        public PlayerController(CastSkillFromSlotUseCase castSkillFromSlotUseCase, MoveInDirectionUseCase moveUseCase, AssumeControllOverCharacterUseCase assumeControllOverCharacterUseCase)
        {
            _castSkillFromSlotUseCase = castSkillFromSlotUseCase;
            _moveUseCase = moveUseCase;
            _assumeControllOverCharacterUseCase = assumeControllOverCharacterUseCase;
        }

        public void TakeControll(EntityId id)
        {
            _assumeControllOverCharacterUseCase.Execute(default, id);
        }

        public void MoveInDirection(EntityId target, Vector2 relativeDirection)
        {
            _moveUseCase.Execute(target, relativeDirection);
        }

        public void Cast(EntityId target, int slot)
        {
            _castSkillFromSlotUseCase.Execute(target, slot);
        }
    }
}
