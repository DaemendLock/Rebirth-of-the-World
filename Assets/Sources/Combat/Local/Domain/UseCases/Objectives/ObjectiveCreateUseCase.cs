using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Factories;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Domain.Repositories.Objectives;

namespace Combat.Local.Domain.UseCases.Objectives
{
    public sealed class ObjectiveCreateUseCase
    {
        private readonly IObjectiveFactory _factory;
        private readonly IObjectiveRepository _objectiveRepository;
        private readonly IObjectiveCreateHandler _handler;

        public ObjectiveCreateUseCase(IObjectiveFactory factory, IObjectiveCreateHandler handler, IObjectiveRepository objectiveRepository)
        {
            _factory = factory;
            _handler = handler;
            _objectiveRepository = objectiveRepository;
        }

        public ObjectiveId Execute(string objectiveName)
        {
            Objective objective = _factory.Create(objectiveName);
            _objectiveRepository.Create(objective);
            _handler.Create(objective);
            return objective.Id;
        }
    }
}
