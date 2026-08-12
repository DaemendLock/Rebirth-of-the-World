using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Factories;
using Combat.Local.Domain.OutputPorts;

namespace Combat.Local.Domain.UseCases.Objectives
{
    public sealed class ObjectiveCreateUseCase
    {
        private readonly IObjectiveFactory _factory;
        private readonly IObjectiveCreateHandler _handler;

        public ObjectiveCreateUseCase(IObjectiveFactory factory, IObjectiveCreateHandler handler)
        {
            _factory = factory;
            _handler = handler;
        }

        public void Execute(string objectiveName)
        {
            Objective objective = _factory.Create(objectiveName);
            _handler.Create(objective);
        }
    }
}
