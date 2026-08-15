using Combat.API.Contexts;

namespace Combat.API.Scripting
{
    public interface ISkillScriptNew
    {
        bool OnCast(IActor actor, ISkillContext skillContext);
        void OnEnterStartup(IActor actor, ISkillContext skillContext) { }
        void OnEnterActive(IActor actor, ISkillContext skillContext) { }
        void OnEnterRecovery(IActor actor, ISkillContext skillContext) { }

        void OnCancel(IActor actor, ISkillContext skillContext) { }
        void OnInterrupt(IActor actor, ISkillContext skillContext) { }
    }
}
