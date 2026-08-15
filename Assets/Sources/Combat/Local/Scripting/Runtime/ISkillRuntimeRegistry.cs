using Combat.API.Contexts;
using Combat.Common.ValueObjects;
using Combat.Local.Scripting.IDK;

namespace Combat.Local.Scripting.Runtime
{
    public readonly struct SkillRuntime
    {
        public readonly ISkillContext Context;
        public readonly ISkillCapabilityProvider Container;

        public SkillRuntime(ISkillContext context, ISkillCapabilityProvider container)
        {
            Context = context;
            Container = container;
        }
    }

    public interface ISkillRuntimeRegistry
    {
        void Create(AbilityKey id, SkillRuntime value);
        void Remove(AbilityKey id);
        bool TryGet(AbilityKey id, out SkillRuntime container);
    }
}
