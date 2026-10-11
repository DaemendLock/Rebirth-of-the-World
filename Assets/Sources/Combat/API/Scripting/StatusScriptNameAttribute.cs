using Combat.Common.Primitives;

using System;

namespace Combat.API.Statuses
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class StatusScriptNameAttribute : System.Attribute
    {
        public StatusScriptNameAttribute(string name)
        {
            Name = new(name);
        }

        public StatusType Name { get; }
    }
}
