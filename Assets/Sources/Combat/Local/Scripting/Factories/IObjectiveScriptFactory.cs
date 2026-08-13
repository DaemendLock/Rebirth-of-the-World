using Combat.API.Scripting;
using Combat.Local.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Combat.Local.Scripting.Factories
{
    public interface IObjectiveScriptFactory
    {
        ICombatObjective Create(Objective objective);
    }

    public sealed class ObjectiveScriptFactory : IObjectiveScriptFactory
    {
        private readonly Dictionary<string, Func<ICombatObjective>> _factories = new();

        public void Register<T>() where T : ICombatObjective, new()
        {
            string name = typeof(T).GetCustomAttribute<ObjectiveNameAttribute>().Name;
            Register<T>(name);
        }

        public void Register<T>(string name) where T : ICombatObjective, new() => Register(name, static () => new T());

        public void Register(string name, Func<ICombatObjective> factory) => _factories.Add(name, factory);

        public ICombatObjective Create(Objective objective)
        {
            if (_factories.TryGetValue(objective.Name, out var factory) == false)
            {
                throw new System.InvalidOperationException();
            }

            return factory();
        }
    }
}
