using Combat.Common.ValueObjects;
using Combat.Local.Domain.Endpoints.Skills;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Scripting.Idk
{
    public sealed class NewScriptAbilityPropertyContainer : IAbilityPropertyContainer
    {
        private readonly AbilityKey _id;

        public NewScriptAbilityPropertyContainer(AbilityKey id)
        {
            _id = id;
        }

        public void Give() { }

        public void Remove() { }

        public bool TryGet(out ISkillHitHandler result)
        {
            result = default;
            return false;
        }

        public bool TryGet(out ISkillActionStateChangeHandler result)
        {
            result = default;
            return false;
        }

        public bool TryGet(out ILockTargetBehaviour result)
        {
            result = default;
            return false;
        }
    }
}
