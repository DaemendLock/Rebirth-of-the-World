using Game.Application.DTO;

using Lobby.Common.Primitives;

namespace Game.Application.Repositories
{
    public interface IScenarioQueryGateway
    {
        ScenarioId? GetScenarioByAccount(AccountId accountId);
    }

    public interface IEncounterCommandGateway
    {
        EncounterId CreateCombat(CreateCombatRequest request);
    }
}
