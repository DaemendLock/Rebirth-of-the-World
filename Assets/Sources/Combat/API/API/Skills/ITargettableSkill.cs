namespace Combat.API.Skills
{
    public interface ITargettableSkill : ISkillProperty
    {
        bool CanTarget(Unit unit);
    }
}
