using System;

using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.API.Statuses
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class StatusScriptNameAttribute : System.Attribute
    {
        public StatusScriptNameAttribute(string name)
        {
            Name = new(name);
        }

        public StatusName Name { get; }
    }
}
