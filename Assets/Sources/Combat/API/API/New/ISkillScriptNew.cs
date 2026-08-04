using Combat.API.Contexts;

namespace Combat.API.API.Skills
{
    public interface ISkillScriptNew // AbstractUseCase
    {
        SkillTimeline OnCast(IActor actor, ISkillContext skillContext);
        void OnEnterStartup(IActor actor, ISkillContext skillContext) { }
        void OnEnterActive(IActor actor, ISkillContext skillContext) { }
        void OnEnterRecovery(IActor actor, ISkillContext skillContext) { }

        void OnCancel(IActor actor, ISkillContext skillContext) { }
        void OnInterrupt(IActor actor, ISkillContext skillContext) { }
    }

    public readonly struct SkillTimeline
    {
        public readonly float Startup;
        public readonly float ActiveTime;
        public readonly float Recovery;
    }

    public readonly struct DealDamageEventData : IEventData
    {

    }
}
