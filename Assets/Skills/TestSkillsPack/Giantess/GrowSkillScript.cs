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

        public void OnCast()
        {
            float currentSize = Owner.Scale;
            
            _targetSize = currentSize + (_growPercent / 100);
        }
    }
}
