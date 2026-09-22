using Assets.Sources.Testing.Local.Lobby.Temp;

using Game.Application.Outputs;
using Game.Application.Repositories;
using Game.Application.UseCases;
using Game.Application.UseCases.Scenarios;
using Game.Domain.Repositories;

using Lobby.Local.Application.Outputs;
using Lobby.Local.Application.UseCases.Scenarios;
using Lobby.Local.Data.Repositories;
using Lobby.Local.Domain.Entities;
using Lobby.Local.Domain.UseCases.Accounts;
using Lobby.Local.Domain.UseCases.CharacterGallery;
using Lobby.Local.Domain.UseCases.Scenarios;
using Lobby.Local.Gateways;
using Lobby.Local.Gateways.Repositories;
using Lobby.Local.Presentation.Controllers;
using Lobby.Local.Presentation.Misc;
using Lobby.Local.Presentation.Presenters;
using Lobby.Local.Presentation.View;
using Lobby.Local.Presentation.View.MainMenu;

using Zenject;

namespace Testing.Local.Lobby
{
    public sealed class LobbySceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<LobbySession>().AsSingle();
            Container.Bind<LocalEncounterCreateGateway>().AsSingle();
            Container.Bind<LocalScenarioRepository>().AsSingle();

            BindRepositories();

            BindUseCases();
            BindPresenters();

            Container.Bind<IAssetProvider>().To<LazyAssetProvider>().FromComponentInHierarchy().AsSingle();

            Container.Bind<UiNavigationService>().FromComponentInHierarchy().AsSingle();
            Container.Bind<CharacterGalleryController>().AsSingle();
            Container.Bind<CharacterSheetController>().AsSingle();
            Container.Bind<TeamSetupController>().AsSingle();
            Container.Bind<CharacterSheetView>().FromComponentInHierarchy(true).AsSingle();
            Container.Bind<ProfileView>().FromComponentInHierarchy(true).AsSingle();
            Container.Bind<TeamSetupView>().FromComponentInHierarchy(true).AsSingle();
        }

        private void BindPresenters()
        {
            Container.Bind<ScenarioWindowPresenter>().FromNew().AsSingle();
            Container.Bind<IScenarioCreateOutput>().To<ScenarioWindowPresenter>().FromResolve();
            Container.Bind<IScenarioCancelOutput>().To<ScenarioWindowPresenter>().FromResolve();
            Container.Bind<IScenarioCommandGateway>().To<LocalScenarioCommandGateway>().AsSingle();

            Container.Bind<IEncounterCommandGateway>().To<LocalEncounterCreateGateway>().FromResolve();
            Container.Bind<IScenarioQueryGateway>().To<LocalScenarioRepository>().FromResolve();

            Container.Bind<AccountPresenter>().FromNew().AsSingle();
            Container.Bind<IAccountJoinEncounterOutput>().To<LocalEncounterCreateGateway>().FromResolve();
            Container.Bind<IAccountScenarioJoinOutput>().To<TeamSetupView>().FromResolve();
            Container.Bind<IAccountCreateOutput>().To<AccountPresenter>().FromResolve();
        }

        private void BindRepositories()
        {
            Container.Bind<IScenarioRepository>().To<LocalScenarioRepository>().FromResolve();

            Container.Bind<IAccountRepository>().To<AccountRepository>().AsSingle();

            Container.Bind<ICharacterRepository>().To<CharacterRepository>().AsSingle();
        }

        private void BindUseCases()
        {
            //Scenarios
            Container.Bind<ScenarioRequestJoinUseCase>().FromNew().AsSingle();
            Container.Bind<ScenarioRequestLeaveUseCase>().FromNew().AsSingle();
            Container.Bind<ScenarioRequestSelectCharacterUseCase>().FromNew().AsSingle();
            Container.Bind<ScenarioRequestStartUseCase>().FromNew().AsSingle();

            Container.Bind<ScenarioCreateUseCase>().FromNew().AsSingle();
            Container.Bind<ScenarioCancelUseCase>().FromNew().AsSingle();
            Container.Bind<ScenarioJoinUseCase>().FromNew().AsSingle();
            Container.Bind<ScenarioLeaveUseCase>().FromNew().AsSingle();
            Container.Bind<ScenarioSelectCharacterUseCase>().FromNew().AsSingle();
            Container.Bind<ScenarioStartUseCase>().FromNew().AsSingle();

            //Accounts
            Container.Bind<AccountCreateUseCase>().FromNew().AsSingle();
            Container.Bind<FindCharactersUseCase>().FromNew().AsSingle();
        }
    }
}
