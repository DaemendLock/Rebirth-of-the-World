using Combat.Common.ValueObjects;
using Combat.Local.Domain.Repositories.Player;

namespace Combat.Local.Domain.UseCases
{
    public class AssumeControllOverCharacterUseCase
    {
        private readonly IPlayerControlRepository _playerControlRepository;
        private readonly ITakeControllOutput _takeControllOutput;

        public AssumeControllOverCharacterUseCase(/*IPlayerControlRepository playerControlRepository,*/ ITakeControllOutput takeControllOutput)
        {
            //_playerControlRepository = playerControlRepository;
            _takeControllOutput = takeControllOutput;
        }

        public void Execute(PlayerId playerId, EntityId id)
        {
            //_playerControlRepository.Update(playerId, id);
            _takeControllOutput.Present(id);
        }
    }

    public interface ITakeControllOutput
    {
        void Present(EntityId id);
    }
}
