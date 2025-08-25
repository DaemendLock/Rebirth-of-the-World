using System;

namespace Server.Combat.Domain.Implementations.Utils
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class ScriptSkillIdAttribute : Attribute
    {
        public ScriptSkillIdAttribute(int skillId)
        {
            SkillId = skillId;
        }

        public int SkillId { get; }
    }
}
