using Combat.Common.Primitives;

using System;

namespace Combat.Local.Scripting.Idk
{
    public interface ISkillScriptTypeProvider
    {
        Type GetScriptType(SkillId skillId);
    }

    public interface IStatusScriptTypeProvider
    {
        void Register(Type type);
        bool TryGet(StatusType type, out Type result);
    }
}
