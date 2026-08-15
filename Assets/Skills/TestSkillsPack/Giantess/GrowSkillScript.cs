using Combat.API;
using Combat.API.Contexts;
using Combat.API.Scripting;
using Combat.API.Skills;

namespace Server.Combat.Domain.Implementations.Actions
{
    [SkillScriptName("growself")]
    public class GrowSkillScript : SkillScript, ICastableSkill, ICastStateChangeHandler
    {
        private float _growPercent;
        private float _duration;

        private float _growRate;
        private float _targetSize;

        protected override void OnInit()
        {
            _duration = 5;
            _growPercent = 40;
            //_targetSize = Owner.Scale * (1 + _growPercent / 100f);
            //_growRate = (_targetSize - Owner.Scale) / _duration;
        }

        public bool OnCast()
        {
            float currentSize = Owner.Scale;

            _targetSize = currentSize + (_growPercent / 100);
            return true;
        }
    }

    public sealed class GrowSelfSkillScript : ICastableNew, IActableNew
    {
        public bool OnCast(IActor actor, ISkillContext skillContext) => throw new System.NotImplementedException();
    }
}
