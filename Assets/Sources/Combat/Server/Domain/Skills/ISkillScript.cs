using CastStateSkill;

using Server.Combat.Domain.Entities;
using Server.Combat.Domain.Units;

namespace Server.Combat.Domain.Skills
{
    public interface ISkillScript
    {
        public Unit Caster { get; }
        public Skill Skill { get; }

        public SkillCastState SkillCastState { get; }
        public float ActiveTime { get; set; }

        void Start();
        bool CanCast();
        bool Cancel();
    }
}
