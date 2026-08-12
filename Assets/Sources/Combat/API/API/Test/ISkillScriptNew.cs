using Combat.API.Contexts;

namespace Combat.API.API.Skills
{
    public interface ISkillScriptNew // AbstractUseCase
    {
        bool OnCast(IActor actor, ISkillContext skillContext);
        void OnEnterStartup(IActor actor, ISkillContext skillContext) { }
        void OnEnterActive(IActor actor, ISkillContext skillContext) { }
        void OnEnterRecovery(IActor actor, ISkillContext skillContext) { }

        void OnCancel(IActor actor, ISkillContext skillContext) { }
        void OnInterrupt(IActor actor, ISkillContext skillContext) { }
    }
}
