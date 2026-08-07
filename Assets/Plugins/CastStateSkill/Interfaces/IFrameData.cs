namespace CastStateSkill
{
    public interface IFrameData
    {
        float LastActiveEnterTime { get; }
        float LastActiveExitTime { get; }
        float RecoveryEnterTime { get; }

        SkillCastState GetCastState(float time);
    }
}
