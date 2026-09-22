using Combat.Common.Primitives;

using Game.Application.DTO;
using Game.Application.Repositories;
using Game.Domain.Entities;
using Game.Domain.Repositories;
using Game.Domain.ValueObjects;

using Lobby.Common.Primitives;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Application.UseCases
{
    public interface IAccountJoinEncounterOutput
    {
        void Present(AccountId accountId, EncounterId encounterId);
    }

    public sealed class ScenarioStartUseCase
    {
        private readonly IScenarioRepository _scenarioRepository;
        private readonly IEncounterCommandGateway _encounterCreateGateway;
        //private readonly IAccountJoinEncounterOutput _accountJoinCombatOutput;

        public ScenarioStartUseCase(IScenarioRepository scenarioRepository, IEncounterCommandGateway encounterCreateGateway)
        {
            _scenarioRepository = scenarioRepository;
            _encounterCreateGateway = encounterCreateGateway;
        }

        public void Execute(StartScenarioInfo command)
        {
            ScenarioId scenarioId = command.ScenarioId;

            Scenario scenario = _scenarioRepository.Get(scenarioId)
                ?? throw new InvalidOperationException($"Scenario '{scenarioId}' was not found.");

            if (CanStart(command.RequestedBy, scenario) == false)
            {
                throw new InvalidOperationException($"Account '{command.RequestedBy}' can't start scenario '{scenarioId}'.");
            }

            if (string.IsNullOrWhiteSpace(scenario.LocationName))
            {
                throw new InvalidOperationException($"Scenario '{scenarioId}' has no combat location.");
            }

            List<CombatParticipant> participants = new(scenario._members.Length);
            HashSet<CharacterKey> selectedCharacters = new();

            foreach (ScenarioMemberInfo member in scenario._members.Where(value => value.HasValue)
                                                                  .Select(value => value.Value))
            {
                if (member.Character.HasValue == false)
                {
                    throw new InvalidOperationException($"Player '{member.AccountId}' has not selected a character.");
                }

                CharacterKey character = member.Character.Value;

                if (selectedCharacters.Add(character) == false)
                {
                    throw new InvalidOperationException($"Character '{character.Value}' is selected more than once.");
                }

                participants.Add(new(member.AccountId, character, member.TeamId, member.Skills));
            }

            if (participants.Count == 0)
            {
                throw new InvalidOperationException($"Scenario '{scenarioId}' has no members.");
            }

            CreateCombatRequest request = new(command.ScenarioId, command.RequestedBy, scenario.LocationName, participants);
            var encounterId = _encounterCreateGateway.CreateCombat(request);
            //_accountJoinCombatOutput.Present(command.RequestedBy, encounterId);
        }

        private bool CanStart(AccountId player, Scenario scenario) => scenario._members.Any(value => value?.AccountId == player);
    }
}
