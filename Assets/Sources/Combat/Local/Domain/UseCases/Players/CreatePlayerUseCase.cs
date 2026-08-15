using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Domain.UseCases.Players
{
    public sealed class CreatePlayerUseCase
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IPlayerCreateOutput _playerCreateOutput;

        public CreatePlayerUseCase(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
            _playerCreateOutput = new UnityDebugLogPlayerOutput();
        }

        public void Execute(PlayerId playerId)
        {
            Player player = new(playerId);
            _playerRepository.Create(player);
            _playerCreateOutput.Present(player);
        }
    }

    public class UnityDebugLogPlayerOutput : IPlayerCreateOutput
    {
        public void Present(Player player) =>
            UnityEngine.Debug.Log("New player for input readed: " + player.Id);
    }

    public interface IPlayerCreateOutput
    {
        void Present(Player player);
    }
}
