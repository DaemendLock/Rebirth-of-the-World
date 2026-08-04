using Combat.API;
using Combat.API.Skills;
using Combat.Common.ValueObjects;

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

        public bool TryGet(out IHitHandler result)
        {
            result = default;
            return false;
        }

        public bool TryGet(out ICastStateChangeHandler result)
        {
            result = default;
            return false;
        }

        public bool TryGet(out ITargettableSkill result)
        {
            result = default;
            return false;
        }
    }
}
