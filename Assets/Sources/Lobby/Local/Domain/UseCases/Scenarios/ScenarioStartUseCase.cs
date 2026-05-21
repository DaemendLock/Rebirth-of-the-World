using Lobby.Common.Primitives;
using Lobby.Local.Domain.Entities;
using Lobby.Local.Domain.Entities.Characters;
using Lobby.Local.Domain.Repositories;
using Lobby.Local.Domain.UseCases.CharacterGallery;

namespace Lobby.Local.Domain.UseCases.Scenarios
{
    public sealed class ScenarioStartUseCase
    {
        private readonly IScenarioRepository _scenarioRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ICharacterRepository _characterRepository;

        public ScenarioStartUseCase(IScenarioRepository scenarioRepository, IAccountRepository accountRepository, ICharacterRepository characterRepository)
        {
            _scenarioRepository = scenarioRepository;
            _accountRepository = accountRepository;
            _characterRepository = characterRepository;
        }

        public void Execute(ScenarioId scenarioId)
        {
            Scenario scenario = _scenarioRepository.Get(scenarioId);

            foreach (var val in scenario.SelectedCharacters)
            {
                if (val.HasValue == false)
                {
                    continue;
                }

                if (val.Value.CharacterId.HasValue)
                {
                    continue;
                }

                UnityEngine.Debug.Log("Can't start: Player didnt select character.");
                //return;
            }

            CreateEncounter(scenario);

            throw new System.NotImplementedException();
        }

        private void CreateEncounter(Scenario scenario)
        {
            //Encounter encounter = _encounterFacotry.Create(new(0));

            //foreach (var val in scenario.SelectedCharacters)
            //{
            //    if (val.HasValue == false)
            //    {
            //        continue;
            //    }

            //    AccountId accountId = val.Value.Player;

            //    Account account = _accountRepository.Get(accountId);
            //    account.CurrentEncounter = encounter.Id;
            //    _accountRepository.Update(account);

            //    if (val.Value.CharacterId.HasValue == false)
            //    {
            //        continue;
            //    }

            //    Character character = _characterRepository.Get(accountId, val.Value.CharacterId.Value);

            //    if (character.IsAvailable == false)
            //    {
            //        throw new System.InvalidOperationException($"Character {character.Id} is not available for player {accountId}");
            //    }

            //    encounter.PlayerCharacters.Add((accountId, character.Id));
            //}

            //_encounterRepository.Create(encounter);
        }
    }
}
