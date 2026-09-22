using Game.Application.DTO;
using Game.Domain.Entities;
using Game.Domain.ValueObjects;

using Lobby.Common.Primitives;

namespace Game.Application.Outputs
{
    public interface IAccountAvailableScenariosOutput
    {
        void Notify(AccountId accountId, ScenarioInfo[] scenarios);
    }

    public interface IAccountScenarioJoinOutput
    {
        void Present(AccountId accountId, ScenarioId scenarioId, ScenarioMemberInfo?[] members);
    }

    public interface IScenarioCreateOutput
    {
        void Present(Scenario scenario);
    }

    public interface IScenarioCancelOutput
    {
        void Present(ScenarioId scenarioId);
    }
}
