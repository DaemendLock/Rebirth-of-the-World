using Combat.Common.Primitives;
using Combat.Local.Scripting.Contexts;
using Combat.Local.Scripting.IDK;
using Combat.API.Contexts;
using Combat.API;

namespace Combat.Local.Scripting.Runtime
{
    public sealed class CastContextScope
    {
        private DomainCastContext _active;
        private DomainCastContext _pending;

        public ICastContext Current => _active;

        public DomainCastContext Begin(ISkillContext skillContext, IActor caster)
        {
            _pending?.Dispose();
            _pending = new DomainCastContext(skillContext, caster);
            return _pending;
        }

        public ICastContext PromotePending()
        {
            _active?.Dispose();
            _active = _pending;
            _pending = null;
            return _active;
        }

        public void DiscardPending(ICastContext context)
        {
            if (ReferenceEquals(_pending, context) == false)
            {
                return;
            }

            _pending?.Dispose();
            _pending = null;
        }

        public void CompleteActive()
        {
            _active?.Dispose();
            _active = null;
        }

        public void Cleanup()
        {
            _active?.Dispose();
            _pending?.Dispose();
            _active = null;
            _pending = null;
        }
    }

    public readonly struct SkillRuntime
    {
        public readonly DomainSkillContext Context;
        public readonly ISkillCapabilityProvider Container;
        public readonly CastContextScope CastContexts;

        public SkillRuntime(DomainSkillContext context, ISkillCapabilityProvider container)
        {
            Context = context;
            Container = container;
            CastContexts = new();
        }
    }

    public interface ISkillRuntimeRegistry
    {
        void Create(AbilityKey id, SkillRuntime value);
        void Remove(AbilityKey id);
        bool TryGet(AbilityKey id, out SkillRuntime container);
    }
}
