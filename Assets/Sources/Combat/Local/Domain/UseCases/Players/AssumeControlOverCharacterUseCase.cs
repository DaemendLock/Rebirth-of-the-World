using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Domain.UseCases
{
    public class AssumeControlOverCharacterUseCase
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly ITakeControllOutput _takeControllOutput;

        public AssumeControlOverCharacterUseCase(IPlayerRepository playerRepository, ITakeControllOutput takeControllOutput)
        {
            _playerRepository = playerRepository;
            _takeControllOutput = takeControllOutput;
        }

        public void Execute(PlayerId playerId, UnitId? id)
        {
            Player player = _playerRepository.Get(playerId);
            player.ControlledEntity = id;
            _playerRepository.Update(player);

            _takeControllOutput.Present(playerId, id);
        }
    }

    public interface ITakeControllOutput
    {
        void Present(PlayerId playerId, UnitId? id);
    }
}
