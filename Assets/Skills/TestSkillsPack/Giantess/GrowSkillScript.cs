using Combat.Local.Domain.API;
using Combat.Local.Domain.API.DTO;
using Combat.Local.Domain.API.Skills;

namespace Server.Combat.Domain.Implementations.Actions
{
    [SkillScriptName("growself")]
    public class GrowSkillScript : ScriptedSkill
    {
        private readonly float _growPercent;
        private readonly float _duration;

        private float _growRate;
        private float _targetSize;

        public GrowSkillScript(ScriptedSkillContext context) : this(context, 40, 10)
        {
        }

        public GrowSkillScript(ScriptedSkillContext context, float growPercent, float duration) : base()
        {
            _duration = duration;
            _growPercent = growPercent;
            _targetSize = Caster.Scale * (1 + _growPercent / 100f);
            _growRate = (_targetSize - Caster.Scale) / _duration;
        }

        public override bool CanCast() => GetCooldownRemain() <= 0;
    }
}
