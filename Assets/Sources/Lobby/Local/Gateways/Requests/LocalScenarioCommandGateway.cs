using Game.Application.DTO;
using Game.Application.UseCases;
using Game.Application.UseCases.Scenarios;
using Game.Domain.ValueObjects;

using Lobby.Local.Application.DTO;
using Lobby.Local.Application.Outputs;

namespace Lobby.Local.Gateways
{
    public sealed class LocalScenarioCommandGateway : IScenarioCommandGateway
    {
        private readonly ScenarioStartUseCase _startHandler;
        private readonly ScenarioJoinUseCase _joinHandler;
        private readonly ScenarioSelectCharacterUseCase _selecetCharacterHandler;
        private readonly ScenarioLeaveUseCase _leaveHandler;

        public LocalScenarioCommandGateway(ScenarioStartUseCase startHandler, ScenarioJoinUseCase joinHandler, ScenarioSelectCharacterUseCase selecetCharacterHandler, ScenarioLeaveUseCase leaveHandler)
        {
            _startHandler = startHandler;
            _joinHandler = joinHandler;
            _selecetCharacterHandler = selecetCharacterHandler;
            _leaveHandler = leaveHandler;
        }

        public void Send(ScenarioStartRequest request)
        {
            StartScenarioInfo command = new(request.ScenarioId, request.RequestedBy);
            _startHandler.Execute(command);
        }

        public void Send(ScenarioJoinRequest request) => _joinHandler.Execute(new(request.ScenarioId, request.RequestedBy));

        public bool Send(ScenarioSelectCharacterRequest request) =>
            _selecetCharacterHandler.Execute(new(request.RequestedBy, Adapt(request.CharacterPickInfo)));

        public void Send(ScenarioLeaveRequest request) => _leaveHandler.Execute(request.RequestedBy);

        private CharacterSelection? Adapt(CharacterSelectInfo? selection)
        {
            if (selection.HasValue == false)
            {
                return null;
            }

            return new(selection.Value.CharacterKey);
        }
    }
}
