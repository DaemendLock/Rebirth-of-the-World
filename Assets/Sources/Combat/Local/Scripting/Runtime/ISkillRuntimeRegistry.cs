using Combat.Common.Primitives;
using Combat.Local.Scripting.Contexts;
using Combat.Local.Scripting.IDK;

namespace Combat.Local.Scripting.Runtime
{
    public readonly struct SkillRuntime
    {
        public readonly ISkillCapabilityProvider Container;

        public SkillRuntime(DomainSkillContext context, ISkillCapabilityProvider container)
        {
            Context = context;
            Container = container;
        }

        public DomainSkillContext Context { get; }
    }

    public readonly struct CastRuntime
    {
        public readonly ISkillCapabilityProvider Container;

        public CastRuntime(DomainCastContext context, ISkillCapabilityProvider container)
        {
            Context = context;
            Container = container;
        }

        public DomainCastContext Context { get; }
    }

    public interface ICastRuntimeRegistry
    {
        void Create(AbilityKey key, CastRuntime castRuntime);
        void Remove(AbilityKey key);
        bool TryGet(AbilityKey key, out CastRuntime castRuntime);
    }

    public interface ISkillRuntimeRegistry
    {
        void Create(AbilityKey id, SkillRuntime value);
        void Remove(AbilityKey id);
        bool TryGet(AbilityKey id, out SkillRuntime container);
    }
}
