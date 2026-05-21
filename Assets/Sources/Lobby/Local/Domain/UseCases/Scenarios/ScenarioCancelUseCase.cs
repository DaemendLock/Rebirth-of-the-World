using Lobby.Common.Primitives;
using Lobby.Local.Domain.Repositories;

namespace Lobby.Local.Domain.UseCases.Scenarios
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
            _scenarioRepository.Delete(scenarioId);
            _scenarioCancelOutput.Present(scenarioId);
        }
    }

    public interface IScenarioCancelOutput
    {
        void Present(ScenarioId scenarioId);
    }
}
