using Combat.API.Contexts;
using Combat.API.Skills;

namespace Combat.API.API.Skills
{
    public struct TestSkillData : IDynamicSkillData
    {
        public int CastCount;
    }

    public sealed class TestScript : ISkillScriptNew
    {
        public SkillTimeline OnCast(IActor actor, ISkillContext skillContext)
        {
            SkillState<TestSkillData> value = skillContext.GetState<TestSkillData>();
            TestSkillData data = value.DynamicState;
            data.CastCount++;
            skillContext.SaveState<TestSkillData>(new(data));

            //skillContext.Owner.SubscribeToEvent<DealDamageEvent>((@event) => { });
            return default;
        }
    }
}
