using Combat.Common.Primitives;
using Combat.Local.Scripting.Contexts;
using Combat.Local.Scripting.IDK;

namespace Combat.Local.Scripting.Runtime
{
    public readonly struct SkillRuntime
    {
        public readonly DomainSkillContext Context;
        public readonly ISkillCapabilityProvider Container;

        public SkillRuntime(DomainSkillContext context, ISkillCapabilityProvider container)
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
