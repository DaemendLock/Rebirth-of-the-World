using Combat.Local.Domain.OutputPorts;

namespace Combat.Local.Domain.UseCases.Scene
{
    public sealed class EncounterEndUseCase
    {
        private readonly IEncounterEndOutput _output;

        public EncounterEndUseCase(IEncounterEndOutput output)
        {
            _output = output;
        }

        public void Execute()
        {
            _output.Present();
        }
    }
}
