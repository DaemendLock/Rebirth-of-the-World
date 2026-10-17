using Combat.Domain.Entities;

namespace Combat.Local.Application.Factories
{
    public interface IObjectiveFactory
    {
        Objective Create(string name);
    }

    public sealed class ObjectiveFactory : IObjectiveFactory
    {
        private int _nextId;

        public Objective Create(string name) => new(new(_nextId++), name);
    }
}
