using CastStateSkill;

using Server.Combat.Domain.Actions;
using Server.Combat.Domain.Implementations.Utils.Extenstions;
using Server.Combat.Domain.Skills;
using Server.Combat.Domain.Units;

namespace Server.Combat.Domain.Implementations.Actions
{
    //public class CastHandler : IActionHandler
    //{
    //    public CastHandler(ISkillScript script, IFrameData frameData)
    //    {
    //        _script = script ?? throw new System.ArgumentNullException(nameof(script));
    //        _frameData = frameData ?? throw new System.ArgumentNullException(nameof(script));
    //        _hasteMultiplier = Actor.EvaluateHasteMultiplier();
    //    }

    //    public IUnit Actor => _script.Caster;
    //    public ISkill Skill => _script.Skill;

    //    public bool IsActive => _script.SkillCastState != SkillCastState.Inactive;

    //    public float ActiveTime
    //    {
    //        get => _script.ActiveTime;
    //        set
    //        {
    //            _script.ActiveTime = value;

    //            if (value == 0)
    //            {
    //                _script.SkillCastState = SkillCastState.Startup;
    //                return;
    //            }

    //            if (_frameData == null)
    //            {
    //                _script.SkillCastState = SkillCastState.Inactive;
    //                return;
    //            }

    //            _script.SkillCastState = _frameData.GetCastState(value * _hasteMultiplier);
    //        }
    //    }
    //}
}
