using Game.Application.Outputs;
using Game.Domain.Repositories;

using Lobby.Common.Primitives;

namespace Game.Application.UseCases.Scenarios
{
    public sealed class ScenarioCancelUseCase
    {
        private readonly IScenarioRepository _scenarioRepository;
        private readonly IScenarioCancelOutput _scenarioCancelOutput;

        public ScenarioCancelUseCase(IScenarioRepository scenarioRepository, IScenarioCancelOutput scenarioCancelOutput)
        {
            _scenarioRepository = scenarioRepository;
            _scenarioCancelOutput = scenarioCancelOutput;
        }

        public void Execute(ScenarioId scenarioId)
        {
            _scenarioRepository.Remove(scenarioId);
            _scenarioCancelOutput.Present(scenarioId);
        }
    }
}
