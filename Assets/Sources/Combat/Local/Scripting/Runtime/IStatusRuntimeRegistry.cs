using Combat.Common.ValueObjects;
using Combat.Local.Scripting.IDK;

namespace Combat.Local.Scripting.Runtime
{
    public interface ISkillRuntimeRegistry
    {
        void Create(AbilityKey id, IAbilityPropertyContainer value);
        void Remove(AbilityKey id);
        bool TryGet(AbilityKey id, out IAbilityPropertyContainer container);
    }

    public interface IStatusRuntimeRegistry
    {
        void Create(StatusId id, IStatusPropertyContainer value);
        void Remove(StatusId id);
        bool TryGet(StatusId id, out IStatusPropertyContainer container);
    }
}
