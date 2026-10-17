using Combat.Common.Primitives;
using Combat.Domain.ValueObjects;

namespace Combat.Local.Application.UseCases
{
    public class AssumeControlOverCharacterUseCase
    {
        private readonly PlayerSession _playerSession;
        private readonly ITakeControllOutput _takeControllOutput;

        public AssumeControlOverCharacterUseCase(ITakeControllOutput takeControllOutput, PlayerSession playerSession)
        {
            _takeControllOutput = takeControllOutput;
            _playerSession = playerSession;
        }

        public void Execute(UnitId? id)
        {
            _playerSession.ControlledUnitId = id;

            _takeControllOutput.Present(id);
        }
    }

    public interface ITakeControllOutput
    {
        void Present(UnitId? id);
    }
}
