using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Domain.Repositories;
using Combat.Local.Application.Factories;
using Combat.Local.Application.OutputPorts;

namespace Combat.Local.Application.UseCases.Objectives
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
