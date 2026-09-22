using Game.Application.DTO;
using Game.Application.Repositories;
using Game.Application.UseCases;

using Lobby.Common.Primitives;

using System;

using UnityEngine.SceneManagement;

using Zenject;

namespace Testing.Local.Lobby
{
    public sealed class LocalEncounterCreateGateway : IEncounterCommandGateway, IAccountJoinEncounterOutput
    {
        private const string CombatSceneName = "Combat";

        private readonly ZenjectSceneLoader _sceneLoader;
        private bool _isLoading;

        public LocalEncounterCreateGateway(ZenjectSceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public EncounterId CreateCombat(CreateCombatRequest request)
        {
            if (_isLoading)
            {
                throw new System.InvalidOperationException();
            }

            _isLoading = true;
            CombatCharacterInfo[] characters = new CombatCharacterInfo[request.Participants.Count];

            for (int i = 0; i < characters.Length; i++)
            {
                CombatParticipant participant = request.Participants[i];
                characters[i] = new(
                    participant.Character.Value,
                    participant.TeamId,
                    participant.OwnerId == request.RequestedBy,
                    participant.Skills);
            }

            StartCombatRequest startCombatRequest = new(request.LocationName, characters);

            _sceneLoader.LoadSceneAsync(
                CombatSceneName,
                LoadSceneMode.Single,
                container => container.BindInstance(startCombatRequest).AsSingle());

            return new(Guid.NewGuid());
        }

        public void Present(AccountId accountId, EncounterId encounterId)
        {

        }
    }
}
