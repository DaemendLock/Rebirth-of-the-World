namespace Combat.API.Skills
{
    public readonly struct SkillState<T> where T : unmanaged, IDynamicSkillData
    {
        public SkillState(T dynamicState)
        {
            DynamicState = dynamicState;
        }

        public T DynamicState { get; }
    }

    public interface IDynamicSkillData { }
}
