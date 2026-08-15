using Combat.API.Contexts;

namespace Combat.API.Scripting
{
    public interface ISkillScriptNew
    {
    }

    public interface ICastableNew : ISkillScriptNew
    {
        bool OnCast(IActor actor, ISkillContext skillContext);
    }

    public interface IActableNew : ISkillScriptNew
    {
        void OnEnterStartup(IActor actor, ISkillContext skillContext) { }
        void OnEnterActive(IActor actor, ISkillContext skillContext) { }
        void OnEnterGap(IActor actor, ISkillContext context) { }
        void OnEnterRecovery(IActor actor, ISkillContext skillContext) { }

        void OnEnded(IActor actor, ISkillContext skillContext) { }
        void OnCancel(IActor actor, ISkillContext skillContext) { }
        void OnInterrupt(IActor actor, ISkillContext skillContext) { }
    }
}
