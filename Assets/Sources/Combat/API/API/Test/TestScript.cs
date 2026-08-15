using Combat.API.Contexts;
using Combat.API.Scripting;
using Combat.API.Skills;
using Combat.Common.Primitives;

namespace Combat.API.API.Skills
{
    public struct TestSkillData : IDynamicSkillData
    {
        public int CastCount;
    }

    public sealed class TestScript : ISkillScriptNew, ICastableNew
    {
        public bool OnCast(IActor actor, ISkillContext skillContext)
        {
            SkillState<TestSkillData> value = skillContext.GetState<TestSkillData>();
            TestSkillData data = value.DynamicState;
            data.CastCount++;
            skillContext.SaveState<TestSkillData>(new(data));

            UnityEngine.Debug.Log($"Test cast; Counter value: {data.CastCount}");
            //skillContext.Owner.SubscribeToEvent<DealDamageEvent>((@event) => { });
            return false;
        }
    }

    public sealed class GrowSelfSkillScript : ICastableNew, IActableNew
    {
        private const float GrowthRate = 10f / 100f;

        private readonly struct GrowModifier : IDynamicSkillData
        {
            public readonly ScaleEffectId? ScaleEffectId;

            public GrowModifier(ScaleEffectId id)
            {
                ScaleEffectId = id;
            }
        }

        public bool OnCast(IActor actor, ISkillContext skillContext)
        {
            skillContext.SaveState<GrowModifier>(new(new()));
            UnityEngine.Debug.Log("Grow!");
            return true;
        }

        public void OnEnterStartup(IActor actor, ISkillContext skillContext)
        {
            ScaleEffectId effectId = actor.StartScaleOverTime(GrowthRate);
            skillContext.SaveState<GrowModifier>(new(new(effectId)));
        }

        public void OnEnded(IActor actor, ISkillContext skillContext)
        {
            var data = skillContext.GetState<GrowModifier>().DynamicState;
            skillContext.SaveState<GrowModifier>(new(default));

            if (data.ScaleEffectId.HasValue == false)
            {
                return;
            }

            actor.StopScaleOverTime(data.ScaleEffectId.Value);
        }
    }
}
