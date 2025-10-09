using Combat.API.DTO;
using Combat.Common.Flags;

namespace Combat.API
{
    public class SkillScript : ISkillProperty
    {
        private SkillApi _skill;
        private Unit _owner;
        private SceneApi _scene;

        public SkillApi Skill => _skill;

        public SceneApi Enviroment => _scene;

        public Unit Owner => _owner;

        public SkillFlags Flags => Skill.Flags;

        public virtual bool HasFlag(SkillFlags flag) => Flags.HasFlag(flag);

        internal void Init(ScriptedSkillContext context)
        {
            _skill = context.Skill;
            _owner = context.Owner;
            _scene = context.Enviroment;
            OnInit();
        }

        protected virtual void OnInit() { }
    }

    public static class ScriptedSkillExtension
    {
        public static float GetCooldownRemain(this SkillScript script) => script.Owner.GetCooldown(script.Skill.SkillId);
    }
}
