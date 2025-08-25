using System;

namespace Combat.Local.Domain.API.Skills
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class SkillScriptNameAttribute : Attribute
    {
        public SkillScriptNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
