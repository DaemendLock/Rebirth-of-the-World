using Combat.API.Contexts;

namespace Combat.API.Scripting
{
    public interface ISkillScriptNew
    {
    }

    public interface ICastableNew : ISkillScriptNew
    {
        bool OnCast(ISkillContext skillContext, ICastContext castContext) => false;
    }

    public interface IActableNew : ISkillScriptNew
    {
        void OnEnterStartup(ISkillContext skillContext, ICastContext castContext) { }
        void OnEnterActive(ISkillContext skillContext, ICastContext castContext) { }
        void OnEnterGap(ISkillContext skillContext, ICastContext castContext) { }
        void OnEnterRecovery(ISkillContext skillContext, ICastContext castContext) { }

        void OnEnded(ISkillContext skillContext, ICastContext castContext) { }
        void OnCancel(ISkillContext skillContext, ICastContext castContext) { }
        void OnInterrupt(ISkillContext skillContext, ICastContext castContext) { }
    }
}
