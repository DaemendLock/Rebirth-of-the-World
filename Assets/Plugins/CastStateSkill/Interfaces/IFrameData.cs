namespace CastStateSkill
{
    public interface IFrameData
    {
        float RecoveryEnterTime { get; }

        SkillCastState GetCastState(float time);
    }
}
