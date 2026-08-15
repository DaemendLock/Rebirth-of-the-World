using Combat.API.Objectives;

using System;

namespace Combat.API.Scripting
{
    public interface ICombatObjective
    {
        void OnStart(IObjectiveContext context);
        void OnComplete(IObjectiveContext context) { }
        void OnCancel(IObjectiveContext context) { }
        void OnFail(IObjectiveContext context) { }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ObjectiveNameAttribute : Attribute
    {
        public ObjectiveNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
