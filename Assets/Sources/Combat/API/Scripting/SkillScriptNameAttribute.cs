using System;

namespace Combat.API.Skills
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class SkillScriptNameAttribute : Attribute
    {
        public SkillScriptNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
